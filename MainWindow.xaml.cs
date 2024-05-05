using CommunityToolkit.WinUI.Notifications;
using FoodWasteApp.Windows;
using System.Collections;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Media.TextFormatting;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Xceed.Wpf.Toolkit;

namespace FoodWasteApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    /// TODO:
    public partial class MainWindow : Window
    {
        private List<FoodButton> buttonList = new List<FoodButton>();
        
        



        public MainWindow()
        {
            if (!File.Exists("Pantry_List.json5"))
                File.Create("Pantry_List.json5").Close();
            if(!File.Exists("Dispose_List.json5"))
                File.Create("Dispose_List.json5").Close();
            if(!File.Exists("Consumed_List.json5"))
                File.Create("Consumed_List.json5").Close();
            if (File.Exists("Settings.ini"))
                File.Create("Settings.ini").Close();


            InitializeComponent();
            Application.Current.MainWindow.Closing += new CancelEventHandler(onWindowClose);
            setupPantryGrid("Pantry_List.Json5");
            setLabelDate();
            // Requires Microsoft.Toolkit.Uwp.Notifications NuGet package version 7.0 or greater
            new ToastContentBuilder()
                .AddArgument("action", "viewConversation")
                .AddArgument("conversationId", 9813)
                .AddText("Andrew sent you a picture")
                .AddText("Check this out, The Enchantments in Washington!");
                 // Not seeing the Show() method? Make sure you have version 7.0, and if you're using .NET 6 (or later), then your TFM must be net6.0-windows10.0.17763.0 or greater







        }


        

        private void setLabelDate()
        {
            Date_Tracker.Content = "Today is: " + DateTime.Now.ToString("MM/dd/yyyy");
        }

        /// <summary>
        /// <para>Input: String fileName </para>
        /// <para>Output: Void</para>
        /// <para>Process: Sets up the grid to represent your pantry during application startup. uses the fileName to load up the Pantry_List file</para>
        /// </summary>
        /// <param name="fileName"></param>
        private void setupPantryGrid(string fileName)
        {

            Pantry_Grid.ShowGridLines = false;
            ColumnDefinition c0 = new ColumnDefinition { Width = new GridLength(300) };
            ColumnDefinition c1 = new ColumnDefinition { Width = new GridLength(300) };
            RowDefinition r1 = new RowDefinition
            {
                Height = new GridLength(30),

            };

            Pantry_Grid.RowDefinitions.Add(r1);
            Pantry_Grid.ColumnDefinitions.Add(c0);
            Pantry_Grid.ColumnDefinitions.Add(c1);

            //TODO: Try Textbox
            TextBlock PantryN = new TextBlock
            {
                Name = "Food_Name",
                Text = "Food Name",
                TextAlignment = TextAlignment.Center,
                Width = 300,
                Height = 30,
                Margin = new Thickness(0, 0, 0, 0),
                Foreground = new SolidColorBrush(Colors.White),
                Background = new SolidColorBrush(Colors.Black),
                FontFamily = new FontFamily("Kayak Sans"),
                FontSize = 15,
                VerticalAlignment = VerticalAlignment.Top,
                HorizontalAlignment = HorizontalAlignment.Stretch,
            };



            Pantry_Grid.Children.Add(PantryN);
            Grid.SetRow(PantryN, 0);
            Grid.SetColumn(PantryN, 0);


            TextBlock ExpDate = new TextBlock
            {
                Name = "Expiry_Date",
                Text = "Expiry Date",
                TextAlignment = TextAlignment.Center,
                Width = 300,
                Height = 30,
                Margin = new Thickness(0, 0, 0, 0),
                Foreground = new SolidColorBrush(Colors.White),
                Background = new SolidColorBrush(Colors.Black),
                HorizontalAlignment = HorizontalAlignment.Stretch,
                VerticalAlignment = VerticalAlignment.Top,
                FontFamily = new FontFamily("Kayak Sans"),
                FontSize = 15,
            };




            Pantry_Grid.Children.Add(ExpDate);
            Grid.SetRow(ExpDate, 0);
            Grid.SetColumn(ExpDate, 1);

            using(StreamReader file = new StreamReader(fileName))
            {
                string ln = "";
                Dictionary<string, int> expiredFood = new Dictionary<string, int>();
                while ((ln = file.ReadLine()) != null)
                {
                    string foodName = ln.Substring(0, ln.IndexOf(':'));
                    DateTime expDate;
                    if (DateTime.TryParse(ln.Substring(ln.IndexOf(":") + 1), out expDate)) 
                    {
                        if (expDate >= DateTime.Today.Date)
                        {
                            AddItemToGrid(foodName, expDate.ToString("MM/dd/yyyy"));
                        }
                        else 
                        {
                            addToList("Dispose_List.json5", foodName);
                            if (expiredFood.ContainsKey(foodName))
                                expiredFood[foodName]++;
                            else
                                expiredFood.Add(foodName, 1);
                        }
                    }

                    if (expiredFood.Count > 0) 
                    {
                        string msg = "These foods have expired, please dispose of them immediately:\n";
                        foreach(string x in expiredFood.Keys)
                        { 
                            msg += (x + ":" + expiredFood[x] + '\n');
                        }

                        System.Windows.MessageBox.Show(msg);
                    
                    }


                    
                }



            }

        }
        /// <summary>
        /// <para>Input: Void</para>
        /// <para>Output: Void</para>
        /// <para>Process: Sets up the grid to represent your pantry during Grid Reload</para>
        /// </summary>
        private void setupPantryGrid()
        {

            Pantry_Grid.ShowGridLines = false;
            ColumnDefinition c0 = new ColumnDefinition { Width = new GridLength(300) };
            ColumnDefinition c1 = new ColumnDefinition { Width = new GridLength(300) };
            RowDefinition r1 = new RowDefinition
            {
                Height = new GridLength(30),

            };

            Pantry_Grid.RowDefinitions.Add(r1);
            Pantry_Grid.ColumnDefinitions.Add(c0);
            Pantry_Grid.ColumnDefinitions.Add(c1);

            //TODO: Try Textbox
            TextBlock PantryN = new TextBlock
            {
                Name = "Food_Name",
                Text = "Food Name",
                TextAlignment = TextAlignment.Center,
                Width = 300,
                Height = 30,
                Margin = new Thickness(0, 0, 0, 0),
                Foreground = new SolidColorBrush(Colors.White),
                Background = new SolidColorBrush(Colors.Black),
                FontFamily = new FontFamily("Kayak Sans"),
                FontSize = 15,
                VerticalAlignment = VerticalAlignment.Top,
                HorizontalAlignment = HorizontalAlignment.Stretch,
            };



            Pantry_Grid.Children.Add(PantryN);
            Grid.SetRow(PantryN, 0);
            Grid.SetColumn(PantryN, 0);


            TextBlock ExpDate = new TextBlock
            {
                Name = "Expiry_Date",
                Text = "Expiry Date",
                TextAlignment = TextAlignment.Center,
                Width = 300,
                Height = 30,
                Margin = new Thickness(0, 0, 0, 0),
                Foreground = new SolidColorBrush(Colors.White),
                Background = new SolidColorBrush(Colors.Black),
                HorizontalAlignment = HorizontalAlignment.Stretch,
                VerticalAlignment = VerticalAlignment.Top,
                FontFamily = new FontFamily("Kayak Sans"),
                FontSize = 15,
            };




            Pantry_Grid.Children.Add(ExpDate);
            Grid.SetRow(ExpDate, 0);
            Grid.SetColumn(ExpDate, 1);

        }

        /// <summary>
        /// <para>Input: object Sender RoutedEventsArgs e</para>
        /// <para>Output: Void</para>
        /// <para>Process: Brings up the <see cref="AddItemPopup.AddItemPopup"/>, and when window closes uses logic to determine if an item will be added to the PantryGrid</para>
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AddItem_Click(object sender, RoutedEventArgs e)
        {
            AddItemPopup a = new AddItemPopup();
            bool open = true;
            a.Closed += delegate
            {
                open = false;
            };

            a.ShowDialog();

            bool b = a.IsInputValid;
            string s = a.FoodItemName;
            string t = a.ExpiryDate;



            if (!open && b)
            {
                AddItemToGrid(s, t);
                
            }

        }

        /// <summary>
        /// <para>Input: String foodName String expriyDate</para>
        /// <para>Output: Void</para>
        /// <para>Process: Adds an Item into the PantryGrid, Creates a <see cref="FoodButton"/> and a Textblock and puts them into the Grid</para>
        /// </summary>
        /// <param name="foodName"></param>
        /// <param name="expiryDate"></param>
        private void AddItemToGrid(string foodName, string expiryDate)
        {
            FoodButton x = new FoodButton
            {
                Content = foodName,
                HorizontalContentAlignment = HorizontalAlignment.Center,
                VerticalContentAlignment = VerticalAlignment.Top,
                Width = 300,
                Height = 30,
                Margin = new Thickness(0, 0, 0, 0),
                Foreground = new SolidColorBrush(Colors.White),
                Background = new SolidColorBrush(Colors.Black),
                HorizontalAlignment = HorizontalAlignment.Stretch,
                VerticalAlignment = VerticalAlignment.Center,
                FontFamily = new FontFamily("Kayak Sans"),
                FontSize = 15,
                ExpDate = expiryDate,
                BorderThickness = new Thickness(0)


            };

            x.Click += new RoutedEventHandler(FoodButtonClicked);
            Pantry_Grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(30) });
            Pantry_Grid.Children.Add(x);
            buttonList.Add(x);
            Grid.SetRow(x, Pantry_Grid.RowDefinitions.Count - 1);
            Grid.SetColumn(x, 0);

            TextBlock y = new TextBlock
            {
                Text = expiryDate,
                TextAlignment = TextAlignment.Center,
                Width = 300,
                Height = 30,
                Margin = new Thickness(0, 0, 0, 0),
                Foreground = new SolidColorBrush(Colors.White),
                Background = new SolidColorBrush(Colors.Black),
                HorizontalAlignment = HorizontalAlignment.Stretch,
                VerticalAlignment = VerticalAlignment.Center,
                FontFamily = new FontFamily("Kayak Sans"),
                FontSize = 15,

            };

            Pantry_Grid.Children.Add(y);
            Grid.SetRow(y, Pantry_Grid.RowDefinitions.Count - 1);
            Grid.SetColumn(y, 1);
            
            
            
            return;
        }

        /// <summary>
        /// <para>Input: String foodName String expriyDate</para>
        /// <para>Output: Void</para>
        /// <para>Process: Same as <see cref="AddItemToGrid(string, string)"/>, but removed the line to add items to <see cref="buttonList"/> to avoid modifying the list</para>
        /// </summary>
        /// <param name="foodName"></param>
        /// <param name="expiryDate"></param>
        private void RebuildGridHelper(string foodName, string expDate)
        {
            FoodButton x = new FoodButton
            {
                Content = foodName,
                HorizontalContentAlignment = HorizontalAlignment.Center,
                Width = 300,
                Height = 30,
                Margin = new Thickness(0, 0, 0, 0),
                Foreground = new SolidColorBrush(Colors.White),
                Background = new SolidColorBrush(Colors.Black),
                HorizontalAlignment = HorizontalAlignment.Stretch,
                VerticalAlignment = VerticalAlignment.Top,
                FontFamily = new FontFamily("Kayak Sans"),
                FontSize = 15,
                ExpDate = expDate,
                BorderThickness = new Thickness(0)


            };

            x.Click += new RoutedEventHandler(FoodButtonClicked);
            Pantry_Grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(30) });
            Pantry_Grid.Children.Add(x);
            Grid.SetRow(x, Pantry_Grid.RowDefinitions.Count - 1);
            Grid.SetColumn(x, 0);

            TextBlock y = new TextBlock
            {
                Text = expDate,
                TextAlignment = TextAlignment.Center,
                Width = 300,
                Height = 30,
                Margin = new Thickness(0, 0, 0, 0),
                Foreground = new SolidColorBrush(Colors.White),
                Background = new SolidColorBrush(Colors.Black),
                HorizontalAlignment = HorizontalAlignment.Stretch,
                VerticalAlignment = VerticalAlignment.Top,
                FontFamily = new FontFamily("Kayak Sans"),
                FontSize = 15,

            };

            Pantry_Grid.Children.Add(y);
            Grid.SetRow(y, Pantry_Grid.RowDefinitions.Count - 1);
            Grid.SetColumn(y, 1);

            return;
        }

        

        /// <summary>
        /// <para>Input:Void</para>
        /// <para>Output:Void</para>
        /// <para>Process:Called when an item is deleted from PantryGrid, remakes the Grid to where the item does not exist in it</para>
        /// </summary>
        
        public void RebuildGrid()
        {
            Pantry_Grid.Children.Clear();
            Pantry_Grid.RowDefinitions.Clear();
            Pantry_Grid.ColumnDefinitions.Clear();


            setupPantryGrid();


            foreach (FoodButton b in buttonList)
            {
                //Will never be null despite the warning
                RebuildGridHelper(b.Content.ToString(), b.ExpDate);
            }

            buttonList.Clear();

            buttonList = Pantry_Grid.Children.OfType<FoodButton>().ToList();



            


            
            
        }


        /// <summary>
        /// <para>Input: object sender, RouterEventArgs 3</para>
        /// <para>Output: Void</para>
        /// <para>Process: Handles the event for when an item in the grid is clicked, Shows a window containing the Food Item's Name, Expiration Date and the options for the item.</para>
        /// </summary>
        public void FoodButtonClicked(object sender, RoutedEventArgs e)
        {
            FoodButton b = (FoodButton)sender;
            
            ItemWindowPopup i = new ItemWindowPopup(b.Content.ToString(),b.ExpDate);   
            i.ShowDialog();
            int retcode = i.retInt;

            switch (retcode) 
            { 
                case 0:
                    addToList("Consumed_List.json5", b.Content.ToString());
                    buttonList.Remove(b);
                    RebuildGrid();
                    break;
                case 1:
                    addToList("Disposed_List.json5", b.Content.ToString());
                    buttonList.Remove(b);
                    RebuildGrid();
                    break;
                default:
                    break;
            
            }
        }

        

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            for (int i = 1; i < 1000; i++)
            {
                AddItemToGrid(Pantry_Grid.RowDefinitions.Count.ToString(), Pantry_Grid.RowDefinitions.Count.ToString());
            }
        }

        private void Clear_Click(object sender, RoutedEventArgs e)
        {
            Pantry_Grid.Children.Clear();
            Pantry_Grid.RowDefinitions.Clear();
            Pantry_Grid.ColumnDefinitions.Clear();
            setupPantryGrid();
        }


        /// <summary>
        /// <para>Input:String fileName String foodName</para>
        /// <para>Output: Void</para>
        /// <para>Process:Given a fileName and a foodName, Check if the foodName exists in a file.</para>
        /// <para>If it does not, Append the food to the end of the file, if it does then increment the value next to the foodName by one.</para>
        /// </summary>
        public void addToList(string fileName, string foodName)
        {
            string ln;
            int ctr = 0;
            bool lineFlag = false;
            string replaceLine = "";

            using (StreamReader file = new StreamReader(fileName))
            {
                while ((ln = file.ReadLine()) != null)
                {
                    string check = ln.Substring(0, ln.IndexOf(':'));
                    
                    if (check == foodName)
                    {
                        lineFlag = true;
                        break;
                    }
                    ctr++;
                }

               

            }

            if (lineFlag)
            {
                int num = Int32.Parse(ln.Substring(ln.IndexOf(":") + 1));
                num++;
                replaceLine = foodName + ":" + num.ToString();
                lineChanger(replaceLine, fileName, ctr);
            }
            else
            {
                int num = 1;
                replaceLine = foodName + ":" + num.ToString();
                using (TextWriter writer = new StreamWriter(fileName, true))
                {
                    writer.WriteLine(replaceLine);
                }
            }
        }

        

        static void lineChanger(string newText, string fileName, int line_to_edit)
        {
            string[] arrLine = File.ReadAllLines(fileName);
            arrLine[line_to_edit] = newText;
            File.WriteAllLines(fileName, arrLine);
        }

        static void settingReader(string fileName, int lineNum)
        {
            string[] arrLine = File.ReadAllLines(fileName);

        }


        /// <summary>
        /// <para>Input:object sender EventArgs e</para>
        /// <para>Output: Void</para>
        /// <para>Process: Saves all food items loaded into memory into Pantry_List.json5</para>
        /// </summary>
        
        //TODO: Backups
        private void onWindowClose(object sender, EventArgs e) 
        {
            
            File.Create("Pantry_List.json5").Close();
            

            foreach (FoodButton x in buttonList)
            {
                    using (TextWriter txt = new StreamWriter("Pantry_List.json5", true))
                    {
                        txt.WriteLine(x.Content.ToString() + ":" + x.ExpDate);
                        txt.Close();
                    }
                    
            }

            Environment.Exit(0);

        }

        private void Settings_Click(object sender, RoutedEventArgs e)
        {
            SettingsWindow settingsWindow = new SettingsWindow();
            settingsWindow.Show();
        }
    }



    public class FoodButton : Button
    {
        public string ExpDate { get; set; }
    }

    

}