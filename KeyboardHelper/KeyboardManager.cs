namespace KeyboardHelper
{
    using System.Diagnostics;
    using System.Windows;
    using System.Windows.Input;

    /// <summary>
    /// Provide events to handle text edition.
    /// </summary>
    public class KeyboardManager
    {
        #region Init
        /// <summary>
        /// Initializes a new instance of the <see cref="KeyboardManager"/> class.
        /// </summary>
        /// <param name="control">The control that receive keyboard events.</param>
        public KeyboardManager(FrameworkElement control)
        {
            // Keyboard.AddPreviewKeyDownHandler(control, OnPreviewKeyDown);
            Keyboard.AddKeyDownHandler(control, OnKeyDown);

            // Keyboard.AddPreviewKeyUpHandler(control, OnPreviewKeyUp);
            Keyboard.AddKeyUpHandler(control, OnKeyUp);
        }
        #endregion

        #region Events
        /// <summary>
        /// Occurs when a visible character is obtained from the keyboard.
        /// </summary>
        public event CharacterKeyEventHandler CharacterKey;

        /// <summary>
        /// Notify handlers of a character key event.
        /// </summary>
        /// <param name="code">The character code.</param>
        /// <param name="sourceEvent">The routed event identifier for this instance of the <see cref="CharacterKeyEventArgs"/> class.</param>
        /// <returns>True if the event was handled; otherwise, false.</returns>
        protected bool NotifyCharacterKey(int code, RoutedEvent sourceEvent)
        {
            CharacterKeyEventArgs Args = new CharacterKeyEventArgs(code, sourceEvent);
            CharacterKey?.Invoke(this, Args);

            return Args.Handled;
        }

        /// <summary>
        /// Occurs when a key pressed indicates the caret should be moved.
        /// </summary>
        public event MoveKeyEventHandler MoveKey;

        /// <summary>
        /// Notify handlers of a move key event.
        /// </summary>
        /// <param name="direction">The move direction.</param>
        /// <param name="flags">Shift and Ctrl key flags.</param>
        /// <param name="sourceEvent">The routed event identifier for this instance of the <see cref="CharacterKeyEventArgs"/> class.</param>
        /// <returns>True if the event was handled; otherwise, false.</returns>
        protected bool NotifyMoveKey(MoveDirections direction, KeyFlags flags, RoutedEvent sourceEvent)
        {
            MoveKeyEventArgs Args = new MoveKeyEventArgs(direction, flags, sourceEvent);
            MoveKey?.Invoke(this, Args);

            return Args.Handled;
        }
        #endregion

        #region Implementation
        /// <summary>
        /// Handles <see cref="Keyboard.PreviewKeyDownEvent"/>.
        /// </summary>
        /// <param name="sender">The object where the event handler is attached.</param>
        /// <param name="e">The event data.</param>
        protected void OnPreviewKeyDown(object sender, KeyEventArgs e)
        {
            DebugPrint("OnPreviewKeyDown", e);
        }

        /// <summary>
        /// Handles <see cref="Keyboard.KeyDownEvent"/>.
        /// </summary>
        /// <param name="sender">The object where the event handler is attached.</param>
        /// <param name="e">The event data.</param>
        protected void OnKeyDown(object sender, KeyEventArgs e)
        {
            DebugPrint("OnKeyDown", e);

            RoutedEvent SourceEvent = e.RoutedEvent;
            KeyMap PressedKey = GetCurrentKey(e);
            bool IsHandled = true;

            switch (PressedKey.Key)
            {
                case Key.LeftCtrl:
                case Key.RightCtrl:
                case Key.LeftShift:
                case Key.RightShift:
                case Key.LeftAlt:
                case Key.RightAlt:
                    IsHandled = false;
                    break;

                case Key.Escape:
                    break;

                case Key.Left:
                case Key.Right:
                case Key.Up:
                case Key.Down:
                case Key.PageUp:
                case Key.PageDown:
                case Key.Home:
                case Key.End:
                    IsHandled = HandleMoveKey(PressedKey, SourceEvent);
                    break;

                default:
                    if (!PressedKey.Flags.HasFlag(KeyFlags.Ctrl) && !string.IsNullOrEmpty(PressedKey.KeyText))
                    {
                        int Code = StringHelper.StringToCode(PressedKey.KeyText);
                        IsHandled = NotifyCharacterKey(Code, SourceEvent);
                    }
                    break;
            }

            if (IsHandled)
                e.Handled = true;
        }

        /// <summary>
        /// Handles <see cref="Keyboard.PreviewKeyUpEvent"/>.
        /// </summary>
        /// <param name="sender">The object where the event handler is attached.</param>
        /// <param name="e">The event data.</param>
        protected void OnPreviewKeyUp(object sender, KeyEventArgs e)
        {
            DebugPrint("OnPreviewKeyUp", e);
        }

        /// <summary>
        /// Handles <see cref="Keyboard.KeyUpEvent"/>.
        /// </summary>
        /// <param name="sender">The object where the event handler is attached.</param>
        /// <param name="e">The event data.</param>
        protected void OnKeyUp(object sender, KeyEventArgs e)
        {
            DebugPrint("OnKeyUp", e);

            if (e.Key == Key.System && (Keyboard.IsKeyDown(Key.LeftAlt) || Keyboard.IsKeyDown(Key.RightAlt)) && NumPadText != null)
            {
                Key Key = e.SystemKey;
                if (Key >= Key.NumPad0 && Key <= Key.NumPad9)
                {
                    NumPadText += (char)((int)Key - (int)Key.NumPad0 + '0');
                    DebugPrint($"Recording numpad: {NumPadText}");
                }
                else
                {
                    NumPadText = string.Empty;
                    DebugPrint("Recording numpad interrupted. Not a numpad key.");
                }
            }

            else if ((e.Key == Key.LeftAlt || e.Key == Key.RightAlt) && !e.KeyStates.HasFlag(KeyStates.Down))
            {
                DebugPrint("Recording numpad finished");

                if (!string.IsNullOrEmpty(NumPadText))
                {
                    bool IsParsed = int.TryParse(NumPadText, out int Code);
                    Debug.Assert(IsParsed);

                    if (IsParsed)
                    {
                        if (StringHelper.IsVisible(Code))
                        {
                            DebugPrint($"Final numpad string parsed as {Code}");
                            NotifyCharacterKey(Code, e.RoutedEvent);
                        }
                        else
                            DebugPrint($"Final numpad string parsed as {Code} but this character is not visible.");
                    }
                }

                NumPadText = null;
            }
        }

        /// <summary>
        /// Handles <see cref="Keyboard.KeyDownEvent"/> for one of the move keys.
        /// </summary>
        /// <param name="pressedKey">The key.</param>
        /// <param name="sourceEvent">The routed event identifier for an instance of the <see cref="MoveKeyEventArgs"/> class.</param>
        private protected bool HandleMoveKey(KeyMap pressedKey, RoutedEvent sourceEvent)
        {
            MoveDirections Direction = (MoveDirections)(-1);
            bool IsValid = false;

            switch (pressedKey.Key)
            {
                case Key.Left:
                    Direction = MoveDirections.Left;
                    IsValid = true;
                    break;

                case Key.Right:
                    Direction = MoveDirections.Right;
                    IsValid = true;
                    break;

                case Key.Up:
                    Direction = MoveDirections.Up;
                    IsValid = true;
                    break;

                case Key.Down:
                    Direction = MoveDirections.Down;
                    IsValid = true;
                    break;

                case Key.PageUp:
                    Direction = MoveDirections.PageUp;
                    IsValid = true;
                    break;

                case Key.PageDown:
                    Direction = MoveDirections.PageDown;
                    IsValid = true;
                    break;

                case Key.Home:
                    Direction = MoveDirections.Home;
                    IsValid = true;
                    break;

                case Key.End:
                    Direction = MoveDirections.End;
                    IsValid = true;
                    break;
            }

            Debug.Assert(IsValid);

            return NotifyMoveKey(Direction, pressedKey.Flags, sourceEvent);
        }

        /// <summary>
        /// Gets the key pressed from a keyboard event.
        /// </summary>
        /// <param name="e">The event data.</param>
        private protected KeyMap GetCurrentKey(KeyEventArgs e)
        {
            Key Key = e.Key;

            if (Key == Key.LeftCtrl || Key == Key.RightShift || Key == Key.LeftShift || Key == Key.RightShift)
                return new KeyMap(e.Key);

            if (Key == Key.System)
                Key = e.SystemKey;

            bool IsCtrlDown = (Keyboard.IsKeyDown(Key.LeftCtrl) || Keyboard.IsKeyDown(Key.RightCtrl)) && !(Keyboard.IsKeyDown(Key.LeftAlt) || Keyboard.IsKeyDown(Key.RightAlt));
            bool IsShiftDown = Keyboard.IsKeyDown(Key.LeftShift) || Keyboard.IsKeyDown(Key.RightShift);

            if (Key == Key.LeftAlt)
            {
                if (e.KeyStates.HasFlag(KeyStates.Down))
                {
                    if (!e.IsRepeat)
                    {
                        DebugPrint("Recording numpad started");
                        NumPadText = string.Empty;
                    }

                    return KeyMap.Empty;
                }
            }

            KeyFlags Flags = KeyFlags.None;

            if (IsCtrlDown && IsShiftDown)
                Flags = KeyFlags.Ctrl | KeyFlags.Shift;

            else if (!IsCtrlDown && IsShiftDown)
                Flags = KeyFlags.Shift;

            else if (IsCtrlDown && !IsShiftDown)
                Flags = KeyFlags.Ctrl;

            string KeyText;
            string Text;
            if (KeyboardInterop.TryParseKey(e, out Text))
                KeyText = Text;
            else
                KeyText = string.Empty;

            KeyMap PressedKey = new KeyMap(Key, Flags, KeyText);

            return PressedKey;
        }

        private string NumPadText;
        #endregion

        #region Debugging
        /// <summary>
        /// Show debug traces.
        /// </summary>
        public static bool ShowTraces { get; private set; } = false;
        private bool LastKeyRepeated = false;

        private void DebugPrint(string s, KeyEventArgs e)
        {
            if (e.IsRepeat)
            {
                if (!LastKeyRepeated)
                {
                    LastKeyRepeated = true;
                    DebugPrint("...");
                }
            }
            else
            {
                LastKeyRepeated = false;
                DebugPrint($"{s}: {e.Key}, {e.ImeProcessedKey}, {e.SystemKey}, {e.DeadCharProcessedKey}, States: {e.KeyStates}");
            }
        }

        private void DebugPrint(string s)
        {
            if (ShowTraces)
                Debug.WriteLine(s);
        }
        #endregion
    }
}
