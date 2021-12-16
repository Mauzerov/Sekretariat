using System.Windows.Controls;

namespace Desktop.View
{
    public partial class HintInput : TextBox
    {
        private string _hintText = "";
        public string HintText
        {
            get => _hintText;
            set
            {
                _hintText = value;
                if (Text.Length == 0)
                    Text = HintText;
            }
        }

        public string GetText() => Text != HintText?Text:"NULL";

        public HintInput()
        {
            InitializeComponent();
            
            LostFocus += (sender, args) =>
                {
                    if (Text.Length == 0)
                        Text = HintText;
                };
            GotFocus += (sender, args) =>
                {
                    if (Text == HintText)
                        Text = "";
                };
        }
    }
}