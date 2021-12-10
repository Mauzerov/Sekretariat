using System;
using System.Collections.Generic;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using Microsoft.Win32;

namespace Desktop.View
{
    class ImageButton : Button
    {
        public ImageButton(string file, Dictionary<string, IComparable> dataRow)
        {
            if (file == "NULL")
                file = "/placeholder.png";

            MinHeight = 100;
            
            Content = new Image()
            {
                Source = new BitmapImage(new Uri(file, UriKind.RelativeOrAbsolute)),
                MaxHeight = 150,
            };

            Click += (sender, args) =>
            {
                var dialog = new OpenFileDialog();
                var result = dialog.ShowDialog();
                if (result == null)
                    return;
                if ((bool) result)
                {
                    dataRow["Photo"] = dialog.FileName;
                    ((Image) Content).Source = new BitmapImage(new Uri(dialog.FileName, UriKind.RelativeOrAbsolute));
                }
            };
        }
    }
}