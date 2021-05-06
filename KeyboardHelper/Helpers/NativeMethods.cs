namespace KeyboardHelper
{
    using System;
    using System.Diagnostics;
    using System.Runtime.InteropServices;
    using System.Text;
    using System.Windows.Input;

    /// <summary>
    /// Contains helper methods to acces the keyboard.
    /// </summary>
    internal class NativeMethods
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

        /// <summary>
        /// Gets the keyboard layout.
        /// </summary>
        /// <param name="threadId">The thread ID.</param>
        /// <returns>The layout.</returns>
        [DllImport("user32.dll", ExactSpelling = true)]
        internal static extern IntPtr GetKeyboardLayout(uint threadId);

        /// <summary>
        /// Gets the keyboard state.
        /// </summary>
        /// <param name="keyStates">The state of keys to update.</param>
        /// <returns>True if successful.</returns>
        [DllImport("user32.dll", ExactSpelling = true)]
        internal static extern bool GetKeyboardState(System.Windows.Forms.Keys[] keyStates);

        /// <summary>
        /// Gets the process ID of the window thread.
        /// </summary>
        /// <param name="hwindow">The window handle.</param>
        /// <param name="processId">The process ID upon return.</param>
        /// <returns>The thread ID.</returns>
        [DllImport("user32.dll", ExactSpelling = true)]
        internal static extern uint GetWindowThreadProcessId(IntPtr hwindow, out uint processId);

        /// <summary>
        /// Tries to parse a key from a keyboard event arguments.
        /// </summary>
        /// <param name="e">The arguments.</param>
        /// <param name="text">The key text upon return.</param>
        /// <returns>True if successful.</returns>
        public static bool TryParseKey(KeyEventArgs e, out string text)
        {
            int scanCode = KeyInterop.VirtualKeyFromKey(e.Key);

            uint procId;
            uint thread = GetWindowThreadProcessId(Process.GetCurrentProcess().MainWindowHandle, out procId);
            IntPtr hkl = GetKeyboardLayout(thread);

            if (hkl == IntPtr.Zero)
            {
                // Keyboard not valid.
                text = string.Empty;
                return false;
            }

            System.Windows.Forms.Keys[] keyStates = new System.Windows.Forms.Keys[256];
            if (!GetKeyboardState(keyStates))
            {
                text = string.Empty;
                return false;
            }

            StringBuilder sb = new StringBuilder(10);
            int rc = ToUnicodeEx((uint)scanCode, (uint)scanCode, keyStates, sb, sb.Capacity, 0, hkl);

            if (rc <= 0)
            {
                text = string.Empty;
                return false;
            }

            text = sb.ToString();
            return true;
        }
    }
}
