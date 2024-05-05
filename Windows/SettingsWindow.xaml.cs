using System;
using System.Collections.Generic;
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
using System.Windows.Shapes;
using Xceed.Wpf.Toolkit;

namespace FoodWasteApp.Windows
{
    /// <summary>
    /// Interaction logic for SettingsWindow.xaml
    /// </summary>
    public partial class SettingsWindow : Window
    {
        
        public SettingsWindow()
        {
            
            InitializeComponent();
            PantryBGColor.Visibility = Visibility.Hidden;
            PantryTextColor.Visibility = Visibility.Hidden;
            PantryBGColor.SelectedColorChanged += PantryBGColor_SelectedColorChanged;
            PantryBGColor.SelectedColor = (Color)ColorConverter.ConvertFromString("#FF000000");






        }

        private void PantryBGColor_SelectedColorChanged(object sender, RoutedPropertyChangedEventArgs<Color?> e)
        {
            ColorPicker s = (ColorPicker)sender;
            System.Windows.MessageBox.Show(s.SelectedColor.ToString());
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            PantryTextColor.Visibility = Visibility.Visible;
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            PantryBGColor.Visibility = Visibility.Visible;
        }


    }
}
