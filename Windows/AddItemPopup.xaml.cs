using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
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

namespace FoodWasteApp
{
    /// <summary>
    /// Interaction logic for AddItemPopup.xaml
    /// </summary>
    public partial class AddItemPopup : Window
    {
        
        public AddItemPopup()
        {
            this.IsInputValid = false;
            InitializeComponent();
        }

        public string FoodItemName
        {
            get { return Food_Name.Text; }
        
        }

        public bool IsInputValid
        {
            get; set;
        
        }

        public string ExpiryDate
        {
            get
            {
                return ExpiryChoice.Text;
            }

        }


        private void FoodButton_Click(object sender, RoutedEventArgs e)
        {
            bool dateWasChanged = false;
            bool nameNull = Food_Name.Text.Length == 0 ? true : false;
            dateWasChanged = (ExpiryChoice.SelectedDate == null) ? false : true;
            bool namehascolon = Food_Name.Text.IndexOf(":") == -1 ? false : true;
            
            if (nameNull)
            {
                MessageBox.Show("Please enter a name for the Food Item");
                return;
            }

            if (namehascolon) 
            {
                MessageBox.Show("Name Of Item Contains an Illegal Character"); 
                return;
            }

            if (DateTime.Now.Date.CompareTo(ExpiryChoice.SelectedDate) > 0 && ExpiryChoice.SelectedDate != null) 
            {
                MessageBox.Show("Date selected is in the past, please input a date in the future");
                return;
            
            }

            if (!dateWasChanged || (DateTime.Now.Date.CompareTo(ExpiryChoice.SelectedDate)) == 0)
            {
                ConfirmationWindow c = new ConfirmationWindow();
                c.ShowDialog();
                bool res = c.ConfirmedReturn;
                if (!res)
                {
                    return;
                }

                else
                {
                    ExpiryChoice.Text = DateTime.Now.ToString("MM/dd/yyyy");
                }
            }
            this.IsInputValid = true;
            this.Close();
        }

    }
}
