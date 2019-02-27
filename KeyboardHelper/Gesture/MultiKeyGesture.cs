namespace KeyboardHelper
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Globalization;
    using System.Windows.Input;
    using System.Windows.Markup;

    /// <summary>
    /// Implements support for a gesture made of a sequence of multiple keys.
    /// </summary>
    public class MultiKeyGesture : KeyGesture
    {
        #region Init
        /// <summary>
        /// Initializes a new instance of the <see cref="MultiKeyGesture"/> class.
        /// </summary>
        /// <param name="sequence">The list of keys.</param>
        public MultiKeyGesture(List<string> sequence)
            : base(Key.None)
        {
            // Reject an invalid sequence.
            if (sequence == null)
                throw new ArgumentNullException(nameof(sequence));
            if (sequence.Count == 0)
                throw new XamlParseException($"The key sequence for a {nameof(MultiKeyGesture)} must not be empty.");

            KeySequence = new List<object>();
            KeyGestureConverter KeyGestureConverter = KeyGestureConverter = new KeyGestureConverter();
            KeyConverter KeyConverter = KeyConverter = new KeyConverter();

            // Create a list of gestures or keys.
            foreach (string text in sequence)
            {
                if (text == null)
                    throw new XamlParseException($"Null string encountered in {nameof(MultiKeyGesture)}.");

                // Try to parse the text as a gesture.
                KeyGesture Gesture;
                try
                {
                    Gesture = (KeyGesture)KeyGestureConverter.ConvertFrom(text);
                }
                catch
                {
                    Gesture = null;
                }

                if (Gesture != null)
                    KeySequence.Add(Gesture);

                // Otherwise it's just a key.
                else
                {
                    try
                    {
                        Key Key = (Key)KeyConverter.ConvertFrom(text);
                        KeySequence.Add(Key);
                    }
                    catch
                    {
                        throw new XamlParseException($"'{text}' for a {nameof(MultiKeyGesture)} could not be parsed");
                    }
                }
            }

            Debug.Assert(KeySequence.Count == sequence.Count);
            Debug.Assert(KeySequence.Count > 0);

            // The sequence must start with a gesture. Only subsequent items are allowed to be simple keys.
            if (!(KeySequence[0] is KeyGesture))
                throw new XamlParseException($"A {nameof(MultiKeyGesture)} must start with a gesture, not a simple key.");

            SequenceIndex = 0;
        }
        #endregion

        #region Implementation
        /// <summary>
        /// Checks if accumualted key events match the sequence.
        /// </summary>
        /// <param name="targetElement">The target.</param>
        /// <param name="inputEventArgs">The input event data to compare this gesture to.</param>
        /// <returns>True if the event data matches the sequence; otherwise, false.</returns>
        public override bool Matches(object targetElement, InputEventArgs inputEventArgs)
        {
            bool IsMatching;

            // Try to match the even with the current item in the sequence.
            if (KeySequence[SequenceIndex] is KeyGesture Gesture)
                IsMatching = Gesture.Matches(targetElement, inputEventArgs);
            else if (inputEventArgs is KeyEventArgs AsKeyEventArgs)
                IsMatching = (Key)KeySequence[SequenceIndex] == AsKeyEventArgs.Key;
            else
                IsMatching = false;

            if (!IsMatching)
            {
                // No match, restart from the beginning.
                SequenceIndex = 0;
                return false;
            }
            else
            {
                SequenceIndex++;

                // Match, but the sequence is not complete.
                if (SequenceIndex < KeySequence.Count)
                {
                    inputEventArgs.Handled = true;
                    return false;
                }
                else
                {
                    // The full sequence is a match.
                    SequenceIndex = 0;
                    return true;
                }
            }
        }

        /// <summary>
        /// Returns a string that represents the current object.
        /// </summary>
        public override string ToString()
        {
            string Result = string.Empty;

            foreach (object Item in KeySequence)
            {
                if (Result.Length > 0)
                    Result += ", ";

                bool IsHandled = false;

                if (Item is KeyGesture Gesture)
                {
                    Result += Gesture.GetDisplayStringForCulture(CultureInfo.CurrentCulture);
                    IsHandled = true;
                }
                else if (Item is Key)
                {
                    Result += Item.ToString();
                    IsHandled = true;
                }

                Debug.Assert(IsHandled);
            }

            return Result;
        }

        // The sequence of gestures and keys, translated from the original string list.
        private List<object> KeySequence;

        // The item in KeySequence to match against the last key event.
        private int SequenceIndex;
        #endregion
    }
}
