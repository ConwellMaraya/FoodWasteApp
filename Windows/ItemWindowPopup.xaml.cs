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

namespace FoodWasteApp.Windows 
{ 

    /// <summary>
    /// Interaction logic for ItemWindowPopup.xaml
    /// </summary>
    public partial class ItemWindowPopup : Window
    {
        public int retInt {  get; set; }
        public ItemWindowPopup(string foodname, string datestring)
        {
            
            InitializeComponent();
            FoodName.Text = foodname;
            DateString.Text = datestring;
            retInt = 0;
            

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.retInt = 0;
            this.Close();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            this.retInt++;
            this.Close();
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            this.retInt--;
            this.Close();
        }
    }
}
