namespace KeyboardHelper
{
    using System;
    using System.Diagnostics;
    using System.Runtime.InteropServices;
    using System.Text;
    using System.Windows.Input;

    internal class KeyboardInterop
    {
        [DllImport("user32.dll", CharSet = CharSet.Unicode, ExactSpelling = true)]
        private static extern int ToUnicodeEx(
            uint wVirtKey,
            uint wScanCode,
            System.Windows.Forms.Keys[] lpKeyState,
            StringBuilder pwszBuff,
            int cchBuff,
            uint wFlags,
            IntPtr dwhkl);

        [DllImport("user32.dll", ExactSpelling = true)]
        internal static extern IntPtr GetKeyboardLayout(uint threadId);

        [DllImport("user32.dll", ExactSpelling = true)]
        internal static extern bool GetKeyboardState(System.Windows.Forms.Keys[] keyStates);

        [DllImport("user32.dll", ExactSpelling = true)]
        internal static extern uint GetWindowThreadProcessId(IntPtr hwindow, out uint processId);

        public static bool TryParseKey(KeyEventArgs e, out string text)
        {
            int scanCode = KeyInterop.VirtualKeyFromKey(e.Key);

            uint procId;
            uint thread = GetWindowThreadProcessId(Process.GetCurrentProcess().MainWindowHandle, out procId);
            IntPtr hkl = GetKeyboardLayout(thread);

            if (hkl == IntPtr.Zero)
            {
                // Keyboard not valid.
                text = null;
                return false;
            }

            System.Windows.Forms.Keys[] keyStates = new System.Windows.Forms.Keys[256];
            if (!GetKeyboardState(keyStates))
            {
                text = null;
                return false;
            }

            StringBuilder sb = new StringBuilder(10);
            int rc = ToUnicodeEx((uint)scanCode, (uint)scanCode, keyStates, sb, sb.Capacity, 0, hkl);

            if (rc <= 0)
            {
                text = null;
                return false;
            }

            text = sb.ToString();
            return true;
        }
    }
}
