namespace KeyboardHelper
{
    using System.Windows.Input;

    /// <summary>
    /// A map to a sequence of keys.
    /// </summary>
    internal struct KeyMap
    {
        static KeyMap()
        {
            Empty = new KeyMap(Key.None, KeyFlags.None);
        }

        /// <summary>
        /// Gets the empty key map.
        /// </summary>
        public static KeyMap Empty { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="KeyMap"/> struct.
        /// </summary>
        /// <param name="key">The key.</param>
        public KeyMap(Key key)
            : this()
        {
            Key = key;
            Flags = KeyFlags.None;
            KeyText = string.Empty;
            SecondaryKey = Key.None;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="KeyMap"/> struct.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="flags">Flags for the key.</param>
        public KeyMap(Key key, KeyFlags flags)
            : this()
        {
            Key = key;
            Flags = flags;
            KeyText = string.Empty;
            SecondaryKey = Key.None;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="KeyMap"/> struct.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="flags">Flags for the key.</param>
        /// <param name="keyText">Text associated to the key.</param>
        public KeyMap(Key key, KeyFlags flags, string keyText)
            : this()
        {
            Key = key;
            Flags = flags;
            KeyText = keyText;
            SecondaryKey = Key.None;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="KeyMap"/> struct.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="flags">Flags for the key.</param>
        /// <param name="secondaryKey">The secondary key in the sequence.</param>
        public KeyMap(Key key, KeyFlags flags, Key secondaryKey)
            : this()
        {
            Key = key;
            Flags = flags;
            KeyText = string.Empty;
            SecondaryKey = secondaryKey;
        }

        /// <summary>
        /// Gets the key.
        /// </summary>
        public Key Key { get; private set; }

        /// <summary>
        /// Gets the flags for the key.
        /// </summary>
        public KeyFlags Flags { get; private set; }

        /// <summary>
        /// Gets the secondary key in the sequence.
        /// </summary>
        public Key SecondaryKey { get; private set; }

        /// <summary>
        /// Gets the text associated to the key.
        /// </summary>
        public string KeyText { get; private set; }

        /// <summary>
        /// Gets a value indicating whether the key is empty.
        /// </summary>
        public bool IsEmpty { get { return Key == Key.None; } }

        /// <summary>
        /// Compares two instances of the <see cref="KeyMap"/> struct.
        /// </summary>
        /// <param name="other">The other instance.</param>
        /// <returns>True if equal; otherwise, false.</returns>
        public bool IsEqual(KeyMap other)
        {
            return Key == other.Key && Flags == other.Flags && SecondaryKey == other.SecondaryKey;
        }

        /// <summary>
        /// Creates the main key map sequence.
        /// </summary>
        /// <returns>The main key map sequence.</returns>
        public KeyMap MainKey()
        {
            return new KeyMap(Key, Flags, KeyText);
        }

        /// <summary>
        /// Creates a new map by adding a secondary key.
        /// </summary>
        /// <param name="combinedSecondaryKey">The secondary key.</param>
        /// <returns>The new key map sequence.</returns>
        public KeyMap WithSecondaryKey(Key combinedSecondaryKey)
        {
            return new KeyMap(Key, Flags, combinedSecondaryKey);
        }

        /// <summary>
        /// Returns the text representation of this instance.
        /// </summary>
        /// <returns>The text representation.</returns>
        public override string ToString()
        {
            return (Flags.HasFlag(KeyFlags.Ctrl) ? "Ctrl-" : string.Empty) + (Flags.HasFlag(KeyFlags.Shift) ? "Shift-" : string.Empty) + (Flags.HasFlag(KeyFlags.Alt) ? "Alt-" : string.Empty) + Key.ToString() + (SecondaryKey != Key.None ? ", " + SecondaryKey.ToString() : string.Empty);
        }
    }
}
