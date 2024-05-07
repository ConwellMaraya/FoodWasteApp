using System;
using System.Collections.Generic;
using System.Drawing;
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
using Windows.Media.Playback;
using Xceed.Wpf.Toolkit;

namespace FoodWasteApp.Windows
{
    /// <summary>
    /// Interaction logic for SettingsWindow.xaml
    /// </summary>
    public partial class SettingsWindow : Window
    {
        public System.Windows.Media.Color PantryBGColorHolder { get; set; }
        public System.Windows.Media.Color PantryTextColorHolder { get; set; }

        public bool retcode { get; set; } = false;
        public bool IsClosed { get; private set; } = false;



        public SettingsWindow(System.Windows.Media.Brush OGPantryBG, System.Windows.Media.Brush OGPantryText)
        {
            
            InitializeComponent();
            PantryBGColor.Visibility = Visibility.Hidden;
            PantryTextColor.Visibility = Visibility.Hidden;
            PantryBGColor.SelectedColor = ((SolidColorBrush)OGPantryBG).Color;
            PantryBGColorHolder = ((SolidColorBrush)OGPantryBG).Color;
            PantryTextColor.SelectedColor = ((SolidColorBrush)OGPantryText).Color;
            PantryTextColorHolder = ((SolidColorBrush)OGPantryText).Color;






        }


        private void Button_Click(object sender, RoutedEventArgs e)
        {
            PantryTextColor.Visibility = Visibility.Visible;
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            PantryBGColor.Visibility = Visibility.Visible;
        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            if (PantryBGColor.SelectedColor != PantryBGColorHolder)
            {
#pragma warning disable CS8629 // Nullable value type may be null.
                PantryBGColorHolder = (System.Windows.Media.Color)PantryBGColor.SelectedColor;
#pragma warning restore CS8629 // Nullable value type may be null.
                retcode = true;
            }

            if (PantryTextColor.SelectedColor != PantryTextColorHolder)
            {
#pragma warning disable CS8629 // Nullable value type may be null.
                PantryTextColorHolder = (System.Windows.Media.Color)PantryTextColor.SelectedColor;
#pragma warning restore CS8629 // Nullable value type may be null.
                retcode = true;
            }

            IsClosed = true;
        }


    }
}
