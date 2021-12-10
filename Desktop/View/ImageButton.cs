using System;
using System.Collections.Generic;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using Microsoft.Win32;

namespace Desktop.View
{
    public class ImageButton : Button
    {
        private string _fileSource = "/placeholder.png";

        public string Source
        {
            get => _fileSource;
            set
            {
                _fileSource = value;
                UpdateImage();
            }
        }

        private void UpdateImage()
        {
            ((Image) Content).Source = new BitmapImage(new Uri(_fileSource, UriKind.RelativeOrAbsolute));
        }
        
        public ImageButton(string file, Dictionary<string, IComparable> dataRow)
        {
            if (file == "NULL")
                file = _fileSource;
            _fileSource = file;
            
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
                    _fileSource = dialog.FileName;
                    UpdateImage();
                }
            };
        }
    }
}