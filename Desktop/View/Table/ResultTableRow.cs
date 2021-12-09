using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

namespace Desktop.View.Table
{
    public class ResultTableRow
    {
        public System.Guid Id { get; set; }

        private List<UIElement> _elements = new List<UIElement>();
        public List<UIElement> Elements => _elements;

        public UIElement this[int index] => Elements[index];

        public ResultTableRow(System.Guid id)
        {
            Id = id;
        }
        
        public void Add(UIElement element)
        {
            _elements.Add(element);
        }

        public void Enable()
        {
            foreach (var element in _elements)
            {
                if (element is TextBoxBase @textBoxBase)
                    @textBoxBase.IsReadOnly = true;
                else if (element is Button @buttonBase)
                    @buttonBase.IsEnabled = true;
                else if (element is Selector @selectorBase)
                    @selectorBase.IsEnabled = true;
                else
                    element.IsEnabled = true;
            }
        }
        
        public void Disable()
        {
            foreach (var element in _elements)
            {
                if (element is TextBoxBase @textBoxBase)
                    @textBoxBase.IsReadOnly = false;
                else if (element is Button @buttonBase)
                    @buttonBase.IsEnabled = false;
                else if (element is Selector @selectorBase)
                    @selectorBase.IsEnabled = false;
                else
                    element.IsEnabled = false;
            }
        }
    }
}