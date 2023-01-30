using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Image = System.Windows.Controls.Image;

namespace TransparentNotePad.CustomControls
{
    public class ImageBox : Control
    {
        private Border _border = null!;
        private Grid _main_grid = null!;
        private StackPanel _header_stack_panel = null!;
        private CustomButton _button_remove = null!;
        private Image _image = null!;

        private Bitmap _bitmap = null!;

        public Bitmap Bitmap
        {
            get
            {
                return _bitmap;
            }
            set
            {
                MemoryStream ms = new MemoryStream();
                value.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                ms.Position = 0;
                BitmapImage bitmapImage = new BitmapImage();
                bitmapImage.BeginInit();
                bitmapImage.StreamSource = ms;
                bitmapImage.EndInit();

                _image.Source = bitmapImage;
                this._bitmap = value;
                UpdateSize();
            }
        }

        static ImageBox()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ImageBox),
                new FrameworkPropertyMetadata(typeof(ImageBox)));
        }

        public override void OnApplyTemplate()
        {
            _border = (Border)Template.FindName("border", this);
            _main_grid = (Grid)Template.FindName("grid_main", this);
            _header_stack_panel = (StackPanel)Template.FindName("header_stack_panel", this);
            _button_remove = (CustomButton)Template.FindName("btn_remove", this);
            _image = (Image)Template.FindName("image", this);

            _image.Drop += OnImageDrop;

            _button_remove.Click += _button_remove_Click;

            base.OnApplyTemplate();
        }

        private void OnImageDrop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                var accepted_extensions = new List<string>
                {
                    ".png",
                    ".jpg"
                };

                var files_path = (string[])e.Data.GetData(DataFormats.FileDrop);
                var image_file_path = string.Empty;

                foreach (var path in files_path)
                {
                    if (accepted_extensions.Contains(Path.GetExtension(path)))
                    {
                        image_file_path = path;
                        break;
                    }
                }

                if (image_file_path == string.Empty)
                    return;


                Bitmap = new Bitmap(image_file_path);
            }
        }

        private void UpdateSize()
        {
            this.Width = this._bitmap.Width;
            this.Height = this._bitmap.Height;
        }
        private void _button_remove_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == true)
            {
                //Uri fileUri = new Uri();
                Bitmap = new Bitmap(openFileDialog.FileName);
            }
        }

        
    }
}
