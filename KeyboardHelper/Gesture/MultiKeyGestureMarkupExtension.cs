namespace KeyboardHelper.Xaml
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Windows.Markup;

    /// <summary>
    /// Markup extension to declare a <see cref="MultiKeyGesture"/> in Xaml.
    ///
    /// Xaml syntax:
    ///   xmlns:xaml="clr-namespace:KeyboardHelper.Xaml;assembly=KeyboardHelper"
    /// For the 'Ctrl+E' gesture followed by the 'W' key sequence:
    ///   Gesture="{xaml:MultiKeyGesture Ctrl+E, W}"
    ///
    /// Restrictions:
    /// 1. The first key must be a valid gesture. For example, 'Ctrl+E' is valid but 'E' is not.
    /// 2. Subsequent gestures and keys are separated by commas. The first gesture is mandatory, the rest is optional.
    /// 3. After the first gesture, simple keys or gestures are accepted. For example, 'Ctrl+E, W' is valid, and so is 'Ctrl+E, Ctrl+W'.
    /// 4. The max number of keys depends on constructors of this class. Check below how many are available.
    /// </summary>
    [ContentProperty("Sequence")]
    public class MultiKeyGesture : MarkupExtension
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MultiKeyGesture"/> class.
        /// </summary>
        public MultiKeyGesture()
        {
            throw new XamlParseException($"The key sequence for a {nameof(MultiKeyGesture)} must not be empty.");
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MultiKeyGesture"/> class.
        /// </summary>
        /// <param name="key1">The first key.</param>
        public MultiKeyGesture(string key1)
        {
            Debug.Assert(key1 != null);

            Sequence = new List<string>() { key1 };
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MultiKeyGesture"/> class.
        /// </summary>
        /// <param name="key1">The first key.</param>
        /// <param name="key2">The second key.</param>
        public MultiKeyGesture(string key1, string key2)
        {
            Debug.Assert(key1 != null);
            Debug.Assert(key2 != null);

            Sequence = new List<string>() { key1, key2 };
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MultiKeyGesture"/> class.
        /// </summary>
        /// <param name="key1">The first key.</param>
        /// <param name="key2">The second key.</param>
        /// <param name="key3">The third key.</param>
        public MultiKeyGesture(string key1, string key2, string key3)
        {
            Debug.Assert(key1 != null);
            Debug.Assert(key2 != null);
            Debug.Assert(key3 != null);

            Sequence = new List<string>() { key1, key2, key3 };
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MultiKeyGesture"/> class.
        /// </summary>
        /// <param name="key1">The first key.</param>
        /// <param name="key2">The second key.</param>
        /// <param name="key3">The third key.</param>
        /// <param name="key4">The fourth key.</param>
        public MultiKeyGesture(string key1, string key2, string key3, string key4)
        {
            Debug.Assert(key1 != null);
            Debug.Assert(key2 != null);
            Debug.Assert(key3 != null);
            Debug.Assert(key4 != null);

            Sequence = new List<string>() { key1, key2, key3, key4 };
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MultiKeyGesture"/> class.
        /// </summary>
        /// <param name="key1">The first key.</param>
        /// <param name="key2">The second key.</param>
        /// <param name="key3">The third key.</param>
        /// <param name="key4">The fourth key.</param>
        /// <param name="key5">The fifth key.</param>
        public MultiKeyGesture(string key1, string key2, string key3, string key4, string key5)
        {
            Debug.Assert(key1 != null);
            Debug.Assert(key2 != null);
            Debug.Assert(key3 != null);
            Debug.Assert(key4 != null);
            Debug.Assert(key5 != null);

            Sequence = new List<string>() { key1, key2, key3, key4, key5 };
        }

        /// <summary>
        /// Returns an object that is provided as the value of the target property for this markup extension.
        /// </summary>
        /// <param name="serviceProvider">A service provider helper that can provide services for the markup extension.</param>
        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            Debug.Assert(serviceProvider != null);
            Debug.Assert(Sequence != null);

            return new KeyboardHelper.MultiKeyGesture(Sequence);
        }

        /// <summary>
        /// Key sequence text.
        /// </summary>
        public List<string> Sequence { get; set; }
    }
}
