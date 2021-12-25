using System;
using System.Collections.Generic;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using Desktop.Annotations;
using Microsoft.Win32;

namespace Desktop.View
{
    public class ImageButton : Button
    {
        public string Source { get; private set; } = "/placeholder.png";

        private void UpdateImage()
        {
            ((Image) Content).Source = new BitmapImage(new Uri(Source, UriKind.RelativeOrAbsolute));
        }
        
        public ImageButton(string file,[CanBeNull] IDictionary<string, IComparable> dataRow)
        {
            Source = file != "NULL" ? file : Source;
            
            MinHeight = 100;
            MaxHeight = 100;
            
            Content = new Image { };
            UpdateImage();

            Click += (sender, args) =>
            {
                var dialog = new OpenFileDialog();
                switch (dialog.ShowDialog())
                {
                    case null:
                        return;
                    case true:
                        if (dataRow != null)
                            dataRow["Zdjecie"] = dialog.FileName;
                        Source = dialog.FileName;
                        UpdateImage();
                        break;
                }
            };
        }
    }
}