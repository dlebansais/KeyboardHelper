namespace KeyboardHelper
{
    using System.Windows;

    /// <summary>
    /// Contains state information and event data associated with a character key event.
    /// </summary>
    public class CharacterKeyEventArgs : RoutedEventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CharacterKeyEventArgs"/> class.
        /// </summary>
        /// <param name="code">The character code.</param>
        public CharacterKeyEventArgs(int code)
            : base()
        {
            Code = code;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CharacterKeyEventArgs"/> class.
        /// </summary>
        /// <param name="code">The character code.</param>
        /// <param name="routedEvent">The routed event identifier for this instance of the <see cref="CharacterKeyEventArgs"/> class.</param>
        public CharacterKeyEventArgs(int code, RoutedEvent routedEvent)
            : base(routedEvent)
        {
            Code = code;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CharacterKeyEventArgs"/> class.
        /// </summary>
        /// <param name="code">The character code.</param>
        /// <param name="routedEvent">The routed event identifier for this instance of the <see cref="CharacterKeyEventArgs"/> class.</param>
        /// <param name="source">An alternate source that will be reported when the event is handled.</param>
        public CharacterKeyEventArgs(int code, RoutedEvent routedEvent, object source)
            : base(routedEvent)
        {
            Code = code;
        }

        /// <summary>
        /// The character code.
        /// </summary>
        public int Code { get; }
    }
}
