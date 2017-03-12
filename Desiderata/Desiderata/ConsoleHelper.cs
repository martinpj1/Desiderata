using System;
using System.Runtime.InteropServices;
using System.Threading;

namespace ConsoleExtender
{

    [StructLayout(LayoutKind.Sequential)]
    public struct COORD
    {
        public short X;
        public short Y;

        public COORD(short X, short Y)
        {
            this.X = X;
            this.Y = Y;
        }
    };

    [StructLayout(LayoutKind.Sequential)]
    public struct DEVMODE1
    {
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
        public string dmDeviceName;
        public short dmSpecVersion;
        public short dmDriverVersion;
        public short dmSize;
        public short dmDriverExtra;
        public int dmFields;

        public short dmOrientation;
        public short dmPaperSize;
        public short dmPaperLength;
        public short dmPaperWidth;

        public short dmScale;
        public short dmCopies;
        public short dmDefaultSource;
        public short dmPrintQuality;
        public short dmColor;
        public short dmDuplex;
        public short dmYResolution;
        public short dmTTOption;
        public short dmCollate;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
        public string dmFormName;
        public short dmLogPixels;
        public short dmBitsPerPel;
        public int dmPelsWidth;
        public int dmPelsHeight;

        public int dmDisplayFlags;
        public int dmDisplayFrequency;

        public int dmICMMethod;
        public int dmICMIntent;
        public int dmMediaType;
        public int dmDitherType;
        public int dmReserved1;
        public int dmReserved2;

        public int dmPanningWidth;
        public int dmPanningHeight;
    };

    [StructLayout(LayoutKind.Sequential)]
    public struct CONSOLE_FONT_INFO
    {
        public int nFont;
        public COORD dwFontSize;
    }

    public static class ConsoleHelper {

        #region StdHandle

        [DllImport("kernel32")]
        private static extern IntPtr GetStdHandle(StdHandle index);

        private enum StdHandle
        {
            InputHandle = -10,
            OutputHandle = -11, 
            ErrorHandle = -12
        }

        #endregion

        #region ICON
        //[DllImport("kernel32")]
        //public static extern bool SetConsoleIcon(IntPtr hIcon);

        //public static bool SetConsoleIcon(Icon icon) {
        //	return SetConsoleIcon(icon.Handle);
        //}
        #endregion

        #region FULLSCREEN
        public static void SetFullScreen()
        {
            var screenBuffer = GetStdHandle(StdHandle.OutputHandle);
            COORD fullscreenDimension;
            SetConsoleDisplayMode(screenBuffer, 1, out fullscreenDimension);
            
            //This is to hide the scroll bar
            Console.BufferWidth = Console.WindowWidth;
            Console.BufferHeight = Console.WindowHeight;
        }


        [DllImport("kernel32.dll", SetLastError = true)]
        static extern bool SetConsoleDisplayMode(IntPtr ConsoleOutput, uint Flags, out COORD NewScreenBufferDimensions);

        #endregion

        #region RESOLUTION
        //Change it back to original on application exit
        //Handle multiple screens

        public static int retainWidth;
        public static int retainHeight;

        static COORD CurrentFontSize; 

        static bool retainResolution = true;
        public static void ChangeResolution(int width, int height)
        {
            DEVMODE1 dm = new DEVMODE1();
            dm.dmDeviceName = new String(new char[32]);
            dm.dmFormName = new String(new char[32]);
            dm.dmSize = (short)Marshal.SizeOf(dm);
            

            if (0 != EnumDisplaySettings(null, ENUM_CURRENT_SETTINGS, ref dm))
            {
                //only grab it the first time
                if(retainResolution)
                {
                    retainWidth = dm.dmPelsWidth;
                    retainHeight = dm.dmPelsHeight;
                    retainResolution = false;
                }
       
                dm.dmPelsWidth = width;
                dm.dmPelsHeight = height;

                int iRet = ChangeDisplaySettings(ref dm, CDS_TEST);

                if (iRet == DISP_CHANGE_FAILED)
                {
                    Console.WriteLine("Unable to process your request");
                    Console.WriteLine("Description: Unable To Process Your Request. Sorry For This Inconvenience."); 
                }
                else
                {
                    iRet = ChangeDisplaySettings(ref dm, CDS_UPDATEREGISTRY);
                    //what if this is too fast for slower computers? 
                    //how would you check that the Console.WindowWidth had actually changed? 
                    //you can't loop forever until it does change, because there's the chance
                    //that the default resolution is the same as the one you're changing it to
                
                    switch (iRet)
                    {
                        case DISP_CHANGE_SUCCESSFUL:
                            {
                                while (Math.Abs((width / CurrentFontSize.X) - Console.WindowWidth) > CurrentFontSize.X)
                                {
                                    Thread.Sleep(50);
                                }
                                //wait for the screen size to actually change
                                //when the font and the screen size are right... 
                                //the console window width will be 126
                                        
                                //This is to hide the scroll bar
                                Console.BufferWidth = Console.WindowWidth;
                                Console.BufferHeight = Console.WindowHeight;
                                break;
                            }
                        case DISP_CHANGE_RESTART:
                            {
                                Console.WriteLine("Description: You Need To Reboot For The Change To Happen.\n If You Feel Any Problem After Rebooting Your Machine\nThen Try To Change Resolution In Safe Mode."); 
                                break;
                                //windows 9x series you have to restart
                            }
                        default:
                            {
                                //failed to change
                                Console.WriteLine("Description: Failed To Change The Resolution."); 
                                break;
                            }
                    }
                }

            }
        }

        [DllImport("user32.dll")]
        public static extern int EnumDisplaySettings(string deviceName, int modeNum, ref DEVMODE1 devMode);
        [DllImport("user32.dll")]
        public static extern int ChangeDisplaySettings(ref DEVMODE1 devMode, int flags);

        public const int ENUM_CURRENT_SETTINGS = -1;
        public const int CDS_UPDATEREGISTRY = 0x01;
        public const int CDS_TEST = 0x02;
        public const int DISP_CHANGE_SUCCESSFUL = 0;
        public const int DISP_CHANGE_RESTART = 1;
        public const int DISP_CHANGE_FAILED = -1;
        #endregion


        const uint ENABLE_MOUSE_INPUT = 0x0010;
        const uint ENABLE_QUICK_EDIT = 0x0040;
        public static void DisableQuickEdit()
        {
            UInt32 consoleMode;
            IntPtr consoleHandle = GetStdHandle(StdHandle.InputHandle);
            var r = GetConsoleMode(consoleHandle, out consoleMode);

            // Clear the quick edit bit in the mode flags
            consoleMode &= ~ENABLE_QUICK_EDIT;
            consoleMode &= ~ENABLE_MOUSE_INPUT;

           r = SetConsoleMode(consoleHandle, consoleMode); 
        }

        [DllImport("kernel32.dll")]
        static extern bool GetConsoleMode(IntPtr hConsoleHandle, out uint lpMode);
        [DllImport("kernel32.dll")]
        static extern bool SetConsoleMode(IntPtr hConsoleHandle, uint dwMode);

        #region FONT
        private const int TMPF_FIXED_PITCH = 1; 
        private const int TMPF_VECTOR = 2; 
        private const int TMPF_TRUETYPE = 4;
        private const int TMPF_DEVICE = 8; 
        private const int LF_FACESIZE = 32;
        private static IntPtr INVALID_HANDLE_VALUE = new IntPtr(-1);

        public static void SetConsoleFont(short fontHeight, string fontName = "Consolas")
        {
            unsafe
            {
                IntPtr hnd = GetStdHandle(StdHandle.OutputHandle);
                if (hnd != INVALID_HANDLE_VALUE)
                {
                    // Set console font to Lucida Console.
                    CONSOLE_FONT_INFO_EX newInfo = new CONSOLE_FONT_INFO_EX();
                    newInfo.cbSize = (uint)Marshal.SizeOf(newInfo);
                    newInfo.FontFamily = 32;
                    newInfo.nFont = 0; 

                    IntPtr ptr = new IntPtr(newInfo.FaceName);
                    Marshal.Copy(fontName.ToCharArray(), 0, ptr, fontName.Length);

                    CurrentFontSize = new COORD((short)(fontHeight/2), fontHeight);
                    // Get some settings from current font.
                    newInfo.dwFontSize = CurrentFontSize; 
                    newInfo.FontWeight = 400;

                    SetCurrentConsoleFontEx(hnd, false, ref newInfo);
                }
            }
            Console.BufferWidth = Console.WindowWidth;
            Console.BufferHeight = Console.WindowHeight; 
        }
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        unsafe public struct CONSOLE_FONT_INFO_EX
        {
            public uint cbSize;
            public uint nFont;
            public COORD dwFontSize;
            public int FontFamily;
            public int FontWeight;

            public fixed char FaceName[32]; // this will require the assembly to be unsafe 
        }

        public static COORD GetCurrentFontSize()
        {
            var outputHandle = GetStdHandle(StdHandle.OutputHandle);

            //Obtain the current console font index for a maximized window
            CONSOLE_FONT_INFO currentFont;
            bool success = GetCurrentConsoleFont(outputHandle, true, out currentFont);

            //Use that index to obtain font size    
            return GetConsoleFontSize(outputHandle, currentFont.nFont);
        }

        [DllImport("kernel32.dll")]
        static extern bool GetCurrentConsoleFont(IntPtr hConsoleOutput, bool bMaximumWindow, out CONSOLE_FONT_INFO lpConsoleCurrentFont);

        [DllImport("kernel32.dll", SetLastError = true)]
        static extern COORD GetConsoleFontSize(IntPtr hConsoleOutput, Int32 nFont);

        [DllImport("kernel32.dll", SetLastError = true)]
        static extern bool SetCurrentConsoleFontEx(IntPtr ConsoleOutput, bool MaximumWindow, ref CONSOLE_FONT_INFO_EX ConsoleCurrentFontEx);


        [DllImport("kernel32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        extern static bool GetCurrentConsoleFontEx(IntPtr hConsoleOutput, bool bMaximumWindow, [In, Out] ref CONSOLE_FONT_INFO_EX lpConsoleCurrentFont);
        #endregion

    }
}
