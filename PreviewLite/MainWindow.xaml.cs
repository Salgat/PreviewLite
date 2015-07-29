using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace PreviewLite
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private DirectoryInfo directory;
        private FileInfo[] image_files;
        private int image_index = 0;
        private BitmapImage current_image;

        public MainWindow()
        {
            InitializeComponent();
        }

        public MainWindow(string image_location) : this()
        {
            DisplayNewImage(image_location);
        }

        /// <summary>
        /// Using Windows Open File dialogue, selects an image to display
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Open_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == true)
            {
                DisplayNewImage(openFileDialog.FileName);
            }
        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Left_Click(object sender, RoutedEventArgs e)
        {
            NavigateImages(NavigationType.Left);
        }

        private void Right_Click(object sender, RoutedEventArgs e)
        {
            NavigateImages(NavigationType.Right);
        }

        private enum NavigationType
        {
            Right,
            Left
        };

        /// <summary>
        /// Changes currently shown image to the next or previous image in the same directory
        /// </summary>
        /// <param name="direction">Determines whether to show the next (NavigationType.Right) or previous image (NavigationType.Left)</param>
        private void NavigateImages(NavigationType direction)
        {
            if (image_files == null) return;

            if (direction == NavigationType.Left)
            {
                if (image_index == 0)
                {
                    image_index = image_files.Length - 1;
                }
                else
                {
                    --image_index;
                }
            }
            else
            {
                if (image_index == image_files.Length - 1)
                {
                    image_index = 0;
                }
                else
                {
                    ++image_index;
                }
            }

            string image_location = directory.ToString() + "\\" + image_files[image_index].ToString();
            DisplayNewImage(image_location);
        }

        /// <summary>
        /// Displays an image and updates directory information
        /// </summary>
        /// <param name="image_location">The absolute position of the image file location</param>
        private void DisplayNewImage(string image_location)
        {
            try
            {
                // Find all images in directory
                string[] extensions = new[] { ".jpg", ".jpeg", ".bmp", ".png", ".gif" };
                directory = new DirectoryInfo(System.IO.Path.GetDirectoryName(image_location));
                image_files = directory.EnumerateFiles().Where(f => extensions.Contains(f.Extension.ToLower())).ToArray();

                // Find current position in directory
                image_index = 0;
                for (int index = 0; index < image_files.Length; ++index)
                {
                    if (image_files[index].ToString() == System.IO.Path.GetFileName(image_location))
                    {
                        image_index = index;
                        break;
                    }
                }

                // Load image
                this.Title = image_location;
                BitmapImage current_image = new BitmapImage(new Uri(image_location));
                preview_image.Source = current_image;
            }
            catch (System.NotSupportedException)
            {
                MessageBox.Show("File type not supported.", "Error");
                preview_image.Source = new BitmapImage(new Uri(@"load_failed.png", UriKind.Relative));
            }
        }

        /// <summary>
        /// Saves currently shown image to location (using File.Copy)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Save_As_Click(object sender, RoutedEventArgs e)
        {
            // Configure save file dialog box
            Microsoft.Win32.SaveFileDialog dialogue = new Microsoft.Win32.SaveFileDialog();
            //dlg.FileName = "Document"; // Default file name
            //dlg.DefaultExt = ".text"; // Default file extension
            //dlg.Filter = "Text documents (.txt)|*.txt"; // Filter files by extension 

            Nullable<bool> result = dialogue.ShowDialog();

            // Process save file dialog box results 
            if (result == true)
            {
                // Save document 
                string filename = dialogue.FileName;
                string image_location = directory.ToString() + "\\" + image_files[image_index].ToString();
                
                try
                {
                    File.Copy(image_location, filename, false);
                }
                catch (IOException)
                {
                    MessageBox.Show("Error: Image copy failed (does file still exist?)");
                }
            }
        }

        private void About_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Created by Austin Salgat");
        }
    }
}
