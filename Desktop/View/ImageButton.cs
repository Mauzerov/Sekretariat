using System;
using System.Collections.Generic;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
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
        
        public ImageButton(string file, IDictionary<string, IComparable> dataRow)
        {
            Source = file != "NULL" ? file : Source;
            
            MinHeight = 100;
            
            Content = new Image { MaxHeight = 150, };
            UpdateImage();

            Click += (sender, args) =>
            {
                var dialog = new OpenFileDialog();
                switch (dialog.ShowDialog())
                {
                    case null:
                        return;
                    case true:
                        dataRow["Photo"] = dialog.FileName;
                        Source = dialog.FileName;
                        UpdateImage();
                        break;
                }
            };
        }
    }
}