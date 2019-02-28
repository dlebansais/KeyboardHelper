namespace Test
{
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;
    using System.Windows.Controls;
    using System.Windows.Documents;
    using System.Windows.Input;
    using KeyboardHelper;

    /// <summary>
    /// Interaction logic for MyControl.xaml
    /// </summary>
    public partial class MyControl : UserControl, INotifyPropertyChanged
    {
        public static readonly string InsertionCaret = "Insertion";
        public static readonly string OverwriteCaret = "Overwrite";

        public MyControl()
        {
            InitializeComponent();
            DataContext = this;

            KeyboardManager = new KeyboardManager(this);
            KeyboardManager.CharacterKey += OnCharacterKey;
            KeyboardManager.MoveKey += OnMoveKey;

            Text = "test";
            CaretType = InsertionCaret;
        }

        private KeyboardManager KeyboardManager;
        public string Text { get; private set; }
        public string CaretType { get; private set; }
        public int CaretPosition { get; private set; }

        public string DeleteSequence { get { return GetSequenceText(EditingCommands.Delete); } }
        public string ToggleInsertSequence { get { return GetSequenceText(EditingCommands.ToggleInsert); } }
        public string BackspaceSequence { get { return GetSequenceText(EditingCommands.Backspace); } }

        private void OnCharacterKey(object sender, CharacterKeyEventArgs e)
        {
            Debug.WriteLine($"OnCharacterKey: '{StringHelper.CodeToString(e.Code)}'");

            string CurrentText = Text;
            int CurrentCaretPosition = CaretPosition;

            if (StringHelper.IsVisible(e.Code))
            {
                if (CaretType == InsertionCaret)
                    StringHelper.InsertCharacter(e.Code, ref CurrentText, ref CurrentCaretPosition);
                else
                    StringHelper.ReplaceCharacter(e.Code, ref CurrentText, ref CurrentCaretPosition);

                Text = CurrentText;
                NotifyPropertyChanged(nameof(Text));
                CaretPosition = CurrentCaretPosition;
                NotifyPropertyChanged(nameof(CaretPosition));

                UpdateCaret();
            }
        }

        private void OnMoveKey(object sender, MoveKeyEventArgs e)
        {
            Debug.WriteLine($"OnMoveKey: {e.Direction}, Ctrl:{e.Flags.HasFlag(KeyFlags.Ctrl)}, Shift:{e.Flags.HasFlag(KeyFlags.Shift)}");

            if (e.Direction == MoveDirections.Left && CaretPosition > 0)
            {
                CaretPosition--;
                NotifyPropertyChanged(nameof(CaretPosition));
            }

            else if (e.Direction == MoveDirections.Right && CaretPosition < Text.Length)
            {
                CaretPosition++;
                NotifyPropertyChanged(nameof(CaretPosition));

                UpdateCaret();
            }

            else if (e.Direction == MoveDirections.Home && CaretPosition > 0)
            {
                CaretPosition = 0;
                NotifyPropertyChanged(nameof(CaretPosition));
            }

            else if (e.Direction == MoveDirections.End && CaretPosition < Text.Length)
            {
                CaretPosition = Text.Length;
                NotifyPropertyChanged(nameof(CaretPosition));

                UpdateCaret();
            }
        }

        private void OnDelete(object sender, ExecutedRoutedEventArgs e)
        {
            Debug.WriteLine("OnDelete");

            if (CaretPosition < Text.Length)
            {
                string CurrentText = Text;
                int CurrentCaretPosition = CaretPosition;

                if (StringHelper.DeleteCharacter(false, ref CurrentText, ref CurrentCaretPosition))
                {
                    Text = CurrentText;
                    NotifyPropertyChanged(nameof(Text));
                    CaretPosition = CurrentCaretPosition;
                    NotifyPropertyChanged(nameof(CaretPosition));

                    UpdateCaret();
                }
            }
        }

        private void OnToggleInsert(object sender, ExecutedRoutedEventArgs e)
        {
            Debug.WriteLine("OnToggleInsert");

            if (CaretType == InsertionCaret && CaretPosition < Text.Length)
            {
                CaretType = OverwriteCaret;
                NotifyPropertyChanged(nameof(CaretType));
            }
            else if (CaretType == OverwriteCaret)
            {
                CaretType = InsertionCaret;
                NotifyPropertyChanged(nameof(CaretType));
            }
        }

        private void OnBackspace(object sender, ExecutedRoutedEventArgs e)
        {
            Debug.WriteLine("OnBackspace");

            if (CaretPosition > 0)
            {
                string CurrentText = Text;
                int CurrentCaretPosition = CaretPosition;

                if (StringHelper.DeleteCharacter(true, ref CurrentText, ref CurrentCaretPosition))
                {
                    Text = CurrentText;
                    NotifyPropertyChanged(nameof(Text));
                    CaretPosition = CurrentCaretPosition;
                    NotifyPropertyChanged(nameof(CaretPosition));
                }
            }
        }

        private void UpdateCaret()
        {
            if (CaretPosition == Text.Length && CaretType != InsertionCaret)
            {
                CaretType = InsertionCaret;
                NotifyPropertyChanged(nameof(CaretType));
            }
        }

        private string GetSequenceText(ICommand command)
        {
            foreach (InputBinding Binding in InputBindings)
                if (Binding.Command == command && Binding.Gesture is MultiKeyGesture AsMultiKeyGesture)
                    return AsMultiKeyGesture.ToString();

            return null;
        }

        #region Implementation of INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;

        public void NotifyPropertyChanged(string PropertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(PropertyName));
        }

        public void NotifyThisPropertyChanged([CallerMemberName] string PropertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(PropertyName));
        }
        #endregion
    }
}
