namespace KeyboardHelper
{
    using System.Windows.Input;

    internal struct KeyMap
    {
        static KeyMap()
        {
            Empty = new KeyMap(Key.None, KeyFlags.None);
        }

        public static KeyMap Empty;

        public KeyMap(Key key)
            : this()
        {
            Key = key;
            Flags = KeyFlags.None;
            KeyText = string.Empty;
            SecondaryKey = Key.None;
        }

        public KeyMap(Key key, KeyFlags flags)
            : this()
        {
            Key = key;
            Flags = flags;
            KeyText = string.Empty;
            SecondaryKey = Key.None;
        }

        public KeyMap(Key key, KeyFlags flags, string text)
            : this()
        {
            Key = key;
            Flags = flags;
            KeyText = text;
            SecondaryKey = Key.None;
        }

        public KeyMap(Key key, KeyFlags flags, Key secondaryKey)
            : this()
        {
            Key = key;
            Flags = flags;
            KeyText = string.Empty;
            SecondaryKey = secondaryKey;
        }

        public Key Key { get; private set; }
        public KeyFlags Flags { get; private set; }
        public Key SecondaryKey { get; private set; }
        public string KeyText { get; private set; }

        public bool IsEmpty { get { return Key == Key.None; } }

        public bool IsEqual(KeyMap other)
        {
            return Key == other.Key && Flags == other.Flags && SecondaryKey == other.SecondaryKey;
        }

        public KeyMap MainKey()
        {
            return new KeyMap(Key, Flags, KeyText);
        }

        public KeyMap WithSecondaryKey(Key combinedSecondaryKey)
        {
            return new KeyMap(Key, Flags, combinedSecondaryKey);
        }

        public override string ToString()
        {
            return (Flags.HasFlag(KeyFlags.Ctrl) ? "Ctrl-" : string.Empty) + (Flags.HasFlag(KeyFlags.Shift) ? "Shift-" : string.Empty) + (Flags.HasFlag(KeyFlags.Alt) ? "Alt-" : string.Empty) + Key.ToString() + (SecondaryKey != Key.None ? ", " + SecondaryKey.ToString() : string.Empty);
        }
    }
}
