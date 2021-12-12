using System.Collections.Generic;
using System.Linq;

namespace Desktop.View.Table.Header
{
    public class SortableHeader
    {
        private List<SortableButton> Buttons { get; } = new List<SortableButton>();

        // Add Button To Button List
        public void Add(SortableButton button) => Buttons.Add(button);

        public void Update(SortableButton button)
        {
            // Reset States Of Buttons != To pressed button
            foreach (var b in Buttons.Where(b => !Equals(button, b)))
            {
                b.State = SortableButton.ButtonState.None;
            }
        }
    }
}