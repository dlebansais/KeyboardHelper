namespace KeyboardHelper.Xaml
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Windows.Markup;
    using Contracts;

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
            Contract.RequireNotNull(key1, out string Key1);

            Sequence = new List<string>() { Key1 };
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MultiKeyGesture"/> class.
        /// </summary>
        /// <param name="key1">The first key.</param>
        /// <param name="key2">The second key.</param>
        public MultiKeyGesture(string key1, string key2)
        {
            Contract.RequireNotNull(key1, out string Key1);
            Contract.RequireNotNull(key2, out string Key2);

            Sequence = new List<string>() { Key1, Key2 };
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MultiKeyGesture"/> class.
        /// </summary>
        /// <param name="key1">The first key.</param>
        /// <param name="key2">The second key.</param>
        /// <param name="key3">The third key.</param>
        public MultiKeyGesture(string key1, string key2, string key3)
        {
            Contract.RequireNotNull(key1, out string Key1);
            Contract.RequireNotNull(key2, out string Key2);
            Contract.RequireNotNull(key3, out string Key3);

            Sequence = new List<string>() { Key1, Key2, Key3 };
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
            Contract.RequireNotNull(key1, out string Key1);
            Contract.RequireNotNull(key2, out string Key2);
            Contract.RequireNotNull(key3, out string Key3);
            Contract.RequireNotNull(key4, out string Key4);

            Sequence = new List<string>() { Key1, Key2, Key3, Key4 };
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
            Contract.RequireNotNull(key1, out string Key1);
            Contract.RequireNotNull(key2, out string Key2);
            Contract.RequireNotNull(key3, out string Key3);
            Contract.RequireNotNull(key4, out string Key4);
            Contract.RequireNotNull(key5, out string Key5);

            Sequence = new List<string>() { Key1, Key2, Key3, Key4, Key5 };
        }

        /// <summary>
        /// Returns an object that is provided as the value of the target property for this markup extension.
        /// </summary>
        /// <param name="serviceProvider">A service provider helper that can provide services for the markup extension.</param>
        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            Debug.Assert(serviceProvider != null);
            Debug.Assert(Sequence.Count > 0);

            return new KeyboardHelper.MultiKeyGesture(Sequence);
        }

        /// <summary>
        /// Key sequence text.
        /// </summary>
        public List<string> Sequence { get; set; } = new List<string>();
    }
}
