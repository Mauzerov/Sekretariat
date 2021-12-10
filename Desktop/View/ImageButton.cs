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
        
        public ImageButton(string file, Dictionary<string, IComparable> dataRow)
        {
            if (file == "NULL")
                file = Source;
            Source = file;
            
            MinHeight = 100;
            
            Content = new Image()
            {
                MaxHeight = 150,
            };
            UpdateImage();

            Click += (sender, args) =>
            {
                var dialog = new OpenFileDialog();
                var result = dialog.ShowDialog();
                if (result == null)
                    return;
                if ((bool) result)
                {
                    dataRow["Photo"] = dialog.FileName;
                    Source = dialog.FileName;
                    UpdateImage();
                }
            };
        }
    }
}