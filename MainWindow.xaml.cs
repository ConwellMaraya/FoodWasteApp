using CommunityToolkit.WinUI.Notifications;
using FoodWasteApp.Windows;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
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
        private System.Windows.Media.Brush? PantryBG;
        private System.Windows.Media.Brush? PantryText;
        private string? ConsumedString;
        private string? DisposedString;



        public MainWindow()
        {
            InitializeComponent();
#pragma warning disable CS8622 // Nullability of reference types in type of parameter doesn't match the target delegate (possibly because of nullability attributes).
            Application.Current.MainWindow.Closing += new CancelEventHandler(onWindowClose);
#pragma warning restore CS8622 // Nullability of reference types in type of parameter doesn't match the target delegate (possibly because of nullability attributes).
        }


        

        private void setMainWindowLabels()
        {
            Date_Tracker.Content = "Today is: " + DateTime.Now.ToString("MM/dd/yyyy");

            using (StreamReader s = new StreamReader("Consumed_List.json5"))
            {
                string line = "";
                int ctr = 0;
                string name = "";
                int num = 0;
                while ((line = s.ReadLine()) != null)
                {
                    if (ctr == 0)
                    {
                        name = line.Substring(0, line.IndexOf(':'));
                        num = Int32.Parse(line.Substring(line.IndexOf(":")+1));
                        ctr++;
                    }

                    else
                    {
                        string tempname = line.Substring(0, line.IndexOf(':'));
                        int tempnum = Int32.Parse(line.Substring(line.IndexOf(":")+1));

                        if (tempnum > num)
                        {
                            name = tempname;
                            num = tempnum;
                        }
                        ctr++;
                    }
                    
                }

                ConsumedString = "Most Consumed Food:\n"+ name;
            }

            using (StreamReader s = new StreamReader("Disposed_List.json5"))
            {
                string line = "";
                int ctr = 0;
                string name = "";
                int num = 0;
                while ((line = s.ReadLine()) != null)
                {
                    if (ctr == 0)
                    {
                        name = line.Substring(0, line.IndexOf(':'));
                        num = Int32.Parse(line.Substring(line.IndexOf(":")+1));
                        ctr++;
                    }

                    else
                    {
                        string tempname = line.Substring(0, line.IndexOf(':'));
                        int tempnum = Int32.Parse(line.Substring(line.IndexOf(":")+1));

                        if (tempnum > num)
                        {
                            name = tempname;
                            num = tempnum;
                        }
                        ctr++;
                    }

                }

                DisposedString = "Most Disposed Food:\n" + name;
            }
            GreatestConsumed.Content = ConsumedString;
            GreatestWasted.Content = DisposedString;
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
                Foreground = PantryText,
                Background = PantryBG,
                FontFamily = new System.Windows.Media.FontFamily("Kayak Sans"),
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
                Foreground = PantryText,
                Background = PantryBG,
                HorizontalAlignment = HorizontalAlignment.Stretch,
                VerticalAlignment = VerticalAlignment.Top,
                FontFamily = new System.Windows.Media.FontFamily("Kayak Sans"),
                FontSize = 15,
            };




            Pantry_Grid.Children.Add(ExpDate);
            Grid.SetRow(ExpDate, 0);
            Grid.SetColumn(ExpDate, 1);

            using(StreamReader file = new StreamReader(fileName))
            {
                string ln = "";
                Dictionary<string, int> expiredFood = new Dictionary<string, int>();
#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
                while ((ln = file.ReadLine()) != null)
                {
                    // if line does not have ':', it's invalid and is skipped
                    if (ln.IndexOf(':') == -1)
                        continue;

                    string foodName = ln.Substring(0, ln.IndexOf(':'));
                    DateTime expDate;
                    if (DateTime.TryParse(ln.Substring(ln.IndexOf(":") + 1), out expDate)) 
                    {
                        if (expDate >= DateTime.Today.Date)
                        {
                            AddItemToGrid(foodName, expDate.ToString("MM/dd/yyyy"),true);
                        }
                        else 
                        {
                            addToList("Disposed_List.json5", foodName);
                            if (expiredFood.ContainsKey(foodName))
                                expiredFood[foodName]++;
                            else
                                expiredFood.Add(foodName, 1);
                        }
                    }

                    else 
                    {
                        continue;
                    }

                    

                    
                }
#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.

                if (expiredFood.Count > 0)
                {
                    string msg = "These foods have expired, please dispose of them immediately:\n";
                    foreach (string x in expiredFood.Keys)
                    {
                        msg += (x + ":" + expiredFood[x] + '\n');
                    }

                    System.Windows.MessageBox.Show(msg);

                }




            }

        }
        /// <summary>
        /// <para>Input: Void</para>
        /// <para>Output: Void</para>
        /// <para>Process: Sets up the grid to represent your pantry during Grid Rebuild</para>
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
                Foreground = PantryText,
                Background = PantryBG,
                FontFamily = new System.Windows.Media.FontFamily("Kayak Sans"),
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
                Foreground = PantryText,
                Background = PantryBG,
                HorizontalAlignment = HorizontalAlignment.Stretch,
                VerticalAlignment = VerticalAlignment.Top,
                FontFamily = new System.Windows.Media.FontFamily("Kayak Sans"),
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
                AddItemToGrid(s, t, true);
                RebuildGrid();
            }

        }

        /// <summary>
        /// <para>Input: String foodName String expriyDate</para>
        /// <para>Output: Void</para>
        /// <para>Process: Adds an Item into the PantryGrid, Creates a <see cref="FoodButton"/> and a Textblock and puts them into the Grid</para>
        /// </summary>
        /// <param name="foodName"></param>
        /// <param name="expiryDate"></param>
        private void AddItemToGrid(string foodName, string expiryDate, bool AddToButtonList)
        {
            FoodButton x = createFoodItemAndAddToList(foodName, expiryDate, AddToButtonList);
            TextBlock y = new TextBlock
            {
                Text = expiryDate,
                TextAlignment = TextAlignment.Center,
                Width = 300,
                Height = 30,
                Margin = new Thickness(0, 0, 0, 0),
                Foreground = PantryText,
                Background = PantryBG,
                HorizontalAlignment = HorizontalAlignment.Stretch,
                VerticalAlignment = VerticalAlignment.Center,
                FontFamily = new System.Windows.Media.FontFamily("Kayak Sans"),
                FontSize = 15,

            };

            Pantry_Grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(30) });
            Pantry_Grid.Children.Add(x);
            Grid.SetRow(x, Pantry_Grid.RowDefinitions.Count - 1);
            Grid.SetColumn(x, 0);
            Pantry_Grid.Children.Add(y);
            Grid.SetRow(y, Pantry_Grid.RowDefinitions.Count - 1);
            Grid.SetColumn(y, 1);
            
            
            
            return;
        }

        private FoodButton createFoodItemAndAddToList(string foodName, string expiryDate, bool addToList)
        {
            FoodButton x = new FoodButton
            {
                Content = foodName,
                HorizontalContentAlignment = HorizontalAlignment.Center,
                VerticalContentAlignment = VerticalAlignment.Top,
                Width = 300,
                Height = 30,
                Margin = new Thickness(0, 0, 0, 0),
                Foreground = PantryText,
                Background = PantryBG,
                HorizontalAlignment = HorizontalAlignment.Stretch,
                VerticalAlignment = VerticalAlignment.Center,
                FontFamily = new System.Windows.Media.FontFamily("Kayak Sans"),
                FontSize = 15,
                ExpDate = expiryDate,
                BorderThickness = new Thickness(0)


            };
            x.Click += new RoutedEventHandler(FoodButtonClicked);
            if (addToList)
                buttonList.Add(x);

            return x;
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
            setMainWindowLabels();

            buttonList.Sort((x, y) => (DateTime.Parse(x.ExpDate).CompareTo(DateTime.Parse(y.ExpDate))));
            foreach (FoodButton b in buttonList)
            {
                //Will never be null despite the warning
#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
                string name = b.Content.ToString();
#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.

                if (name != null)
                    AddItemToGrid(name, b.ExpDate, false);
            }

            buttonList.Clear();

            buttonList = Pantry_Grid.Children.OfType<FoodButton>().ToList();
            buttonList.Sort((x, y) => (DateTime.Parse(x.ExpDate).CompareTo(DateTime.Parse(y.ExpDate))));

        }


        /// <summary>
        /// <para>Input: object sender, RouterEventArgs 3</para>
        /// <para>Output: Void</para>
        /// <para>Process: Handles the event for when an item in the grid is clicked, Shows a window containing the Food Item's Name, Expiration Date and the options for the item.</para>
        /// </summary>
        public void FoodButtonClicked(object sender, RoutedEventArgs e)
        {
            FoodButton b = (FoodButton)sender;


            string name = b.Content.ToString();


            if (name != null )
            {
                ItemWindowPopup i = new ItemWindowPopup(name, b.ExpDate);
                i.ShowDialog();
                int retcode = i.retInt;

                switch (retcode)
                {
                    case 0:
                        addToList("Consumed_List.json5", name);
                        buttonList.Remove(b);
                        RebuildGrid();
                        break;
                    case 1:
                        addToList("Disposed_List.json5", name);
                        buttonList.Remove(b);
                        RebuildGrid();
                        break;
                    default:
                        break;

                }
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
#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
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
#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.



            }

            if (lineFlag)
            {
               if (ln != null)
                {
                    int num = Int32.Parse(ln[(ln.IndexOf(":") + 1)..]);
                    num++;
                    replaceLine = foodName + ":" + num.ToString();
                    lineChanger(replaceLine, fileName, ctr);
                }
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

        

        private void lineChanger(string newText, string fileName, int line_to_edit)
        {
            string[] arrLine = File.ReadAllLines(fileName);
            arrLine[line_to_edit] = newText;
            File.WriteAllLines(fileName, arrLine);
        }

        private void settingReader(string fileName, int lineNum, ref System.Windows.Media.Brush setting)
        {
            string[] arrLine = File.ReadAllLines(fileName);
            string s = arrLine[lineNum];
            if (s != null)
            {
                System.Windows.Media.Brush? b = new BrushConverter().ConvertFrom(s) as System.Windows.Media.Brush;
                if (b != null)
                {
                    setting = b.Clone();
                    //System.Windows.MessageBox.Show("WORKS");
                    //setupPantryGrid();
                } 
            }
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
            
            using (TextWriter txt = new StreamWriter("Settings.ini"))
            {
                if (PantryBG != null)
                    txt.WriteLine(PantryBG.ToString());
                if (PantryText != null)
                    txt.WriteLine(PantryText.ToString());
                txt.Close();
            }


            Environment.Exit(0);

        }

        private void changeColor()
        {
            foreach (TextBlock x in Pantry_Grid.Children.OfType<TextBlock>().ToList())
            {
                x.Foreground = PantryText;
                x.Background = PantryBG;
            }

            foreach (FoodButton x in Pantry_Grid.Children.OfType<FoodButton>().ToList())
            {
                x.Foreground = PantryText;
                x.Background = PantryBG;
            }
        }

        private void Settings_Click(object sender, RoutedEventArgs e)
        {
            if (PantryText != null && PantryBG != null)
            {
                SettingsWindow settingsWindow = new SettingsWindow(PantryBG, PantryText);
                bool open = true;
                settingsWindow.Closed += delegate
                {
                    open = false;
                };
                settingsWindow.ShowDialog();
                bool retcode = settingsWindow.retcode;





                if (retcode && !open)
                {
                    PantryBG = new SolidColorBrush(settingsWindow.PantryBGColorHolder);
                    PantryText = new SolidColorBrush(settingsWindow.PantryTextColorHolder);
                    changeColor();
                }
            }
        }


        private void Button_Click(object sender, RoutedEventArgs e)
        {
            for (int i = 0; i < 1000; i++) 
            {
                AddItemToGrid(Pantry_Grid.RowDefinitions.Count.ToString(), DateTime.Now.Date.ToString("MM/dd/yyyy"), true);
            }
        }

        private void ExpiryToast()
        {
            foreach (FoodButton b in buttonList)
            {
                TimeSpan diff = DateTime.Parse(b.ExpDate) - DateTime.Now;
                if (diff.Days > 1)
                {
                    new ToastContentBuilder()
                        .AddArgument("action", "viewConversation")
                        .AddArgument("conversationId", 9813)
                        .AddText("This Food Item is about to Expire in " + diff.Days.ToString() + " Days")
                        .AddText(b.Content.ToString())
                        .Show(); 
                }
                else if (diff.Days == 1)
                {
                    new ToastContentBuilder()
                        .AddArgument("action", "viewConversation")
                        .AddArgument("conversationId", 9813)
                        .AddText("This Food Item is about to Expire in " + diff.Days.ToString() + " Day")
                        .AddText(b.Content.ToString())
                        .Show();
                }
                else if (diff.Days == 0)
                {
                    new ToastContentBuilder()
                        .AddArgument("action", "viewConversation")
                        .AddArgument("conversationId", 9813)
                        .AddText("This Food Item is about to Expire Today")
                        .AddText(b.Content.ToString())
                        .Show();
                }
            }
        }

        private void WindowMain_Loaded(object sender, RoutedEventArgs e)
        {
            if (!File.Exists("Pantry_List.json5"))
                File.Create("Pantry_List.json5").Close();
            if (!File.Exists("Disposed_List.json5"))
                File.Create("Disposed_List.json5").Close();
            if (!File.Exists("Consumed_List.json5"))
                File.Create("Consumed_List.json5").Close();
            if (!File.Exists("Settings.ini"))
            {
                File.Create("Settings.ini").Close();
                using (TextWriter t = new StreamWriter("Settings.ini", true))
                {
                    t.Write("#FF000000" + '\n' + "#FFFFFFFF");
                    t.Close();
                }
            }

            PantryBG = new SolidColorBrush();
            PantryText = new SolidColorBrush();
            if (PantryBG != null)
                settingReader("Settings.ini", 0, ref PantryBG);
            if (PantryText != null)
                settingReader("Settings.ini", 1, ref PantryText);
            setupPantryGrid("Pantry_List.json5");
            setMainWindowLabels();
            ExpiryToast();
        }
    }





    public class FoodButton : Button
    {
        public string ExpDate { get; set; }

        public FoodButton()
        {
            ExpDate = DateTime.Now.Date.ToString();
        }
    }



    

}
