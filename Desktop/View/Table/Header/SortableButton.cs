using System.Windows.Controls;
using System.Windows.Controls.Primitives;

namespace Desktop.View.Table.Header
{
    internal static class SortableButtonExtensions
    {
        public static string ToString(this SortableButton.ButtonState state, bool my)
        {
            return state == SortableButton.ButtonState.None ? "" : state.ToString();
        }
    }
    public class SortableButton : Button
    {
        private SortableHeader _header;
        public enum ButtonState : short
        {
            None = 0, Asc = 1, Desc = 2
        }
        
        private ButtonState _state = ButtonState.None;
        public string ColumnTitle = "UUID";

        public ButtonState State
        {
            get => _state;
            set
            {
                _state = value;
                string field;
                if ((field = State.ToString(true)) != "")
                    Content = ColumnTitle + " " + field;
                else
                    Content = ColumnTitle;
            }
        }

        public SortableButton(SortableHeader header) : base()
        {
            _header = header;
            header.Add(this);
            Click += (sender, args) => State = (ButtonState)((1 + (short)State) % 3);
            Click += (sender, args) => _header.Update(this);
        }
    }
}