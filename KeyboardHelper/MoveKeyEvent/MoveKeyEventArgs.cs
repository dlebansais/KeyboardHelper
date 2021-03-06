﻿namespace KeyboardHelper
{
    using System.Windows;

    /// <summary>
    /// Contains state information and event data associated with a move key event.
    /// </summary>
    public class MoveKeyEventArgs : RoutedEventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MoveKeyEventArgs"/> class.
        /// </summary>
        /// <param name="direction">The move direction.</param>
        /// <param name="flags">Alt, Shift and Ctrl key flags.</param>
        public MoveKeyEventArgs(MoveDirections direction, KeyFlags flags)
            : base()
        {
            Direction = direction;
            Flags = flags;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MoveKeyEventArgs"/> class.
        /// </summary>
        /// <param name="direction">The move direction.</param>
        /// <param name="flags">Alt, Shift and Ctrl key flags.</param>
        /// <param name="routedEvent">The routed event identifier for this instance of the <see cref="MoveKeyEventArgs"/> class.</param>
        public MoveKeyEventArgs(MoveDirections direction, KeyFlags flags, RoutedEvent routedEvent)
            : base(routedEvent)
        {
            Direction = direction;
            Flags = flags;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MoveKeyEventArgs"/> class.
        /// </summary>
        /// <param name="direction">The move direction.</param>
        /// <param name="flags">Alt, Shift and Ctrl key flags.</param>
        /// <param name="routedEvent">The routed event identifier for this instance of the <see cref="MoveKeyEventArgs"/> class.</param>
        /// <param name="source">An alternate source that will be reported when the event is handled.</param>
        public MoveKeyEventArgs(MoveDirections direction, KeyFlags flags, RoutedEvent routedEvent, object source)
            : base(routedEvent)
        {
            Direction = direction;
            Flags = flags;
        }

        /// <summary>
        /// Gets the move direction.
        /// </summary>
        public MoveDirections Direction { get; }

        /// <summary>
        /// Gets the Alt, Shift and Ctrl key flags.
        /// </summary>
        public KeyFlags Flags { get; }

        /// <summary>
        /// Gets a value indicating whether the alt key is pressed.
        /// </summary>
        public bool IsAlt { get { return Flags.HasFlag(KeyFlags.Alt); } }

        /// <summary>
        /// Gets a value indicating whether the shift key is pressed.
        /// </summary>
        public bool IsShift { get { return Flags.HasFlag(KeyFlags.Shift); } }

        /// <summary>
        /// Gets a value indicating whether the ctrl key is pressed.
        /// </summary>
        public bool IsCtrl { get { return Flags.HasFlag(KeyFlags.Ctrl); } }
    }
}
