using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp1.Models
{
    public static class ActiveProgramMonitor
    {
        [DllImport("user32.dll")]
        private static extern IntPtr GetForegroundWindow();

        [DllImport("user32.dll", EntryPoint = "GetWindowText", CharSet = CharSet.Auto)]
        private static extern int GetWindowText(IntPtr hwnd, StringBuilder lpString, int nMaxCount);
        [DllImport("user32.dll")]
        public static extern int GetWindowThreadProcessId(IntPtr hWnd, out uint lpdwProcessId);

        [DllImport("kernel32.dll")]
        private static extern IntPtr OpenProcess(uint dwDesiredAccess, bool bInheritHandle, uint dwProcessId);

        [DllImport("kernel32.dll")]
        private static extern bool CloseHandle(IntPtr handle);

        [DllImport("psapi.dll", CharSet = CharSet.Ansi)]
        private static extern uint GetModuleBaseName(IntPtr hWnd, IntPtr hModule, [MarshalAs(UnmanagedType.LPStr), Out] StringBuilder lpBaseName, uint nSize);

        public static string GetActiveProgramName()
        {
            var handleForegroundWindow = GetForegroundWindow();
            if (handleForegroundWindow != IntPtr.Zero)
            {
                StringBuilder sb = new StringBuilder(65535);
                GetWindowText(handleForegroundWindow, sb, 65535);
                var window_name = sb.ToString().ToLower();
                System.Diagnostics.Debug.WriteLine(window_name);
                _ = GetWindowThreadProcessId(handleForegroundWindow, out var processId);
                var handleProcess = OpenProcess(0x0400 | 0x0010, false, processId);

                var buffer = new StringBuilder(1024);
                GetModuleBaseName(handleProcess, IntPtr.Zero, buffer, (uint)buffer.Capacity);
                //CloseHandle(handleForegroundWindow);
                CloseHandle(handleProcess) ;
                System.Diagnostics.Debug.WriteLine(buffer.ToString());
                var program_name = buffer.ToString().ToLower();
                return program_name;

            }
            return null;
        }
    }

}
