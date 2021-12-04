using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace Desktop.View.Table.Header
{
    public class SortableHeader
    {
        private List<SortableButton> _buttons = new List<SortableButton>();

        public List<SortableButton> Buttons => _buttons;

        public SortableHeader Add(SortableButton button)
        {
            _buttons.Add(button);
            return this;
        }

        public SortableHeader Clear()
        {
            _buttons.Clear();
            return this;
        }

        public SortableHeader Update(SortableButton button)
        {
            foreach (var b in Buttons)
            {
                if (Equals(button, b))
                    continue;
                b.State = SortableButton.ButtonState.None;
            }
            return this;
        }
    }
}