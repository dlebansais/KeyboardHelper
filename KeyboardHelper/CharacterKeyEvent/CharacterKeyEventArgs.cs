namespace KeyboardHelper
{
    using System.Windows;
    using System.Windows.Input;

    /// <summary>
    /// Contains state information and event data associated with a character key event.
    /// </summary>
    public class CharacterKeyEventArgs : RoutedEventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CharacterKeyEventArgs"/> class.
        /// </summary>
        /// <param name="code">The character code.</param>
        /// <param name="key">The key that generated the character, <see cref="Key.None"/> if not available.</param>
        public CharacterKeyEventArgs(int code, Key key)
            : base()
        {
            Code = code;
            Key = key;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CharacterKeyEventArgs"/> class.
        /// </summary>
        /// <param name="code">The character code.</param>
        /// <param name="key">The key that generated the character, <see cref="Key.None"/> if not available.</param>
        /// <param name="routedEvent">The routed event identifier for this instance of the <see cref="CharacterKeyEventArgs"/> class.</param>
        public CharacterKeyEventArgs(int code, Key key, RoutedEvent routedEvent)
            : base(routedEvent)
        {
            Code = code;
            Key = key;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CharacterKeyEventArgs"/> class.
        /// </summary>
        /// <param name="code">The character code.</param>
        /// <param name="key">The key that generated the character, <see cref="Key.None"/> if not available.</param>
        /// <param name="routedEvent">The routed event identifier for this instance of the <see cref="CharacterKeyEventArgs"/> class.</param>
        /// <param name="source">An alternate source that will be reported when the event is handled.</param>
        public CharacterKeyEventArgs(int code, Key key, RoutedEvent routedEvent, object source)
            : base(routedEvent)
        {
            Code = code;
            Key = key;
        }

        /// <summary>
        /// The character code.
        /// </summary>
        public int Code { get; }

        /// <summary>
        /// The key that generated the character, <see cref="Key.None"/> if not available.
        /// </summary>
        public Key Key { get; }
    }
}
