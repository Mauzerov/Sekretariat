using System.Windows.Controls;
using System.Windows.Controls.Primitives;

namespace Desktop.View.Table.Header
{
    public class SortableButton : Button
    {
        private SortableHeader _header;
        public enum ButtonState : short
        {
            None = 0, Asc = 1, Desc = 2
        }
        
        private ButtonState _state = ButtonState.None;
        public ButtonState State
        {
            get => _state;
            set
            {
                _state = value;
                Content = State.ToString();
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