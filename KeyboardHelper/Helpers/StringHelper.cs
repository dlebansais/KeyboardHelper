namespace KeyboardHelper
{
    using System;
    using System.Diagnostics;
    using System.Text;

    /// <summary>
    /// Provides methods to handle strings modified by keyboard events.
    /// </summary>
    public static class StringHelper
    {
        /// <summary>
        /// Gets the string corresponding to a Unicode character code.
        /// </summary>
        /// <param name="code">The character code.</param>
        public static string CodeToString(int code)
        {
            if (code < 0) throw new ArgumentException($"{nameof(code)} must be greater than zero.");

            byte[] Bytes = BitConverter.GetBytes(code);
            Debug.Assert(Bytes.Length == sizeof(int));

            return Encoding.UTF32.GetString(Bytes);
        }

        /// <summary>
        /// Gets the Unicode character code from the first character of a string.
        /// </summary>
        /// <param name="text">The string with the character.</param>
        public static int StringToCode(string text)
        {
            if (text == null) throw new ArgumentNullException(nameof(text));
            if (text.Length == 0) throw new ArgumentException($"{nameof(text)} must not be empty.");

            byte[] Bytes = Encoding.UTF32.GetBytes(text);
            Debug.Assert(Bytes.Length >= sizeof(int));
            int Code = BitConverter.ToInt32(Bytes, 0);
            Debug.Assert(Code >= 0);

            return Code;
        }

        /// <summary>
        /// Checks if a character is considered visible.
        /// </summary>
        /// <param name="code">The character code.</param>
        /// <returns>True if the character is visible; otherwise, false.</returns>
        public static bool IsVisible(int code)
        {
            if (code < 0) throw new ArgumentException($"{nameof(code)} must be greater than zero.");

            string Text = CodeToString(code);

            if (Text.Length < 1)
                return false;

            char c = Text[0];
            if (char.IsControl(c) || char.IsHighSurrogate(c) || c == 0x2028 || c == 0x2029)
                return false;

            if (char.IsLowSurrogate(c) && Text.Length >= 2)
            {
                if (!char.IsHighSurrogate(Text[1]))
                    return false;
            }

            return true;
        }

        /// <summary>
        /// Replaces a character in a string at a provided position.
        /// </summary>
        /// <param name="code">The character code.</param>
        /// <param name="text">The string.</param>
        /// <param name="position">The position in <paramref name="text"/> where to replace.</param>
        public static void ReplaceCharacter(int code, ref string text, ref int position)
        {
            if (code <= 0) throw new ArgumentException($"{nameof(code)} must be strictly greater than zero.");
            if (text == null) throw new ArgumentNullException(nameof(text));
            if (position < 0 || position >= text.Length) throw new ArgumentException($"{nameof(position)} must be a valid position in {nameof(text)}.");

            UTF32Encoding UTF32 = (UTF32Encoding)Encoding.UTF32;
            byte[] FirstBytes = UTF32.GetBytes(text.Substring(0, position));
            byte[] ReplacedBytes = BitConverter.GetBytes(code);
            byte[] LastBytes = UTF32.GetBytes(text.Substring(position + 1));

            byte[] Bytes = new byte[FirstBytes.Length + ReplacedBytes.Length + LastBytes.Length];
            Array.Copy(FirstBytes, 0, Bytes, 0, FirstBytes.Length);
            Array.Copy(ReplacedBytes, 0, Bytes, FirstBytes.Length, ReplacedBytes.Length);
            Array.Copy(LastBytes, 0, Bytes, FirstBytes.Length + ReplacedBytes.Length, LastBytes.Length);

            text = UTF32.GetString(Bytes);
            position++;
        }

        /// <summary>
        /// Inserts a character in a string at a provided position, updating the position to point after the inserted character.
        /// </summary>
        /// <param name="code">The character code.</param>
        /// <param name="text">The string.</param>
        /// <param name="position">The position in <paramref name="text"/> where to insert.</param>
        public static void InsertCharacter(int code, ref string text, ref int position)
        {
            if (code <= 0) throw new ArgumentException($"{nameof(code)} must be strictly greater than zero.");
            if (text == null) throw new ArgumentNullException(nameof(text));
            if (position < 0 || position > text.Length) throw new ArgumentException($"{nameof(position)} must be a valid position in {nameof(text)}.");

            UTF32Encoding UTF32 = (UTF32Encoding)Encoding.UTF32;
            byte[] FirstBytes = UTF32.GetBytes(text.Substring(0, position));
            byte[] InsertedBytes = BitConverter.GetBytes(code);
            byte[] LastBytes = UTF32.GetBytes(text.Substring(position));

            byte[] Bytes = new byte[FirstBytes.Length + InsertedBytes.Length + LastBytes.Length];
            Array.Copy(FirstBytes, 0, Bytes, 0, FirstBytes.Length);
            Array.Copy(InsertedBytes, 0, Bytes, FirstBytes.Length, InsertedBytes.Length);
            Array.Copy(LastBytes, 0, Bytes, FirstBytes.Length + InsertedBytes.Length, LastBytes.Length);

            text = UTF32.GetString(Bytes);
            position++;
        }

        /// <summary>
        /// Deletes a character in a string either at the provided position or on the left of the position.
        /// This method will do nothing if there is no character to delete.
        /// </summary>
        /// <param name="backward">True to delete on the left of <paramref name="position"/>; false to delete at <paramref name="position"/>.</param>
        /// <param name="text">The string.</param>
        /// <param name="position">The position in <paramref name="text"/> where to delete.</param>
        /// <returns>True if <paramref name="text"/> was changed; otherwise, false.</returns>
        public static bool DeleteCharacter(bool backward, ref string text, ref int position)
        {
            if (text == null) throw new ArgumentNullException(nameof(text));
            if (position < 0 || position > text.Length) throw new ArgumentException($"{nameof(position)} must be a valid position in {nameof(text)}.");

            if (backward)
            {
                if (position > 0)
                {
                    text = text.Substring(0, position - 1) + text.Substring(position);
                    position--;
                    return true;
                }
            }
            else
            {
                if (position < text.Length)
                {
                    text = text.Substring(0, position) + text.Substring(position + 1);
                    return true;
                }
            }

            return false;
        }
    }
}
