using FoodWasteApp.Windows;
using System.Collections;
using System.ComponentModel;
using System.IO;
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
            
            //TODO
            //ADD CONSUMED, ROTTEN AND FOOD LIST
            //INTERFACE
            if (!File.Exists("Pantry_List.json5"))
                File.Create("Pantry_List.json5").Close();
            if(!File.Exists("Dispose_List.json5"))
                File.Create("Dispose_List.json5").Close();
            if(!File.Exists("Consumed_List.json5"))
                File.Create("Consumed_List.json5").Close();


            InitializeComponent();
            Application.Current.MainWindow.Closing += new CancelEventHandler(onWindowClose);
            setupPantryGrid("Pantry_List.Json5");
            setLabelDate();
            ClearGrid.Click += new RoutedEventHandler(Clear_Click);





        }


        

        private void setLabelDate()
        {
            Date_Tracker.Content = "Today is: " + DateTime.Now.ToString("MM/dd/yyyy");
        }

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
                while ((ln = file.ReadLine()) != null)
                {
                    string foodName = ln.Substring(0, ln.IndexOf(':'));
                    DateTime expDate;
                    if (DateTime.TryParse(ln.Substring(ln.IndexOf(":") + 1), out expDate)) 
                    {
                        AddItemToGrid(foodName, expDate.ToString("MM/dd/yyyy"));
                    }


                    
                }



            }

        }

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

        private void AddItemToGrid(string s, string t)
        {
            FoodButton x = new FoodButton
            {
                Content = s,
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
                ExpDate = t,
                BorderThickness = new Thickness(0)


            };

            x.Click += new RoutedEventHandler(FoodButtonClicked);
            Pantry_Grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(30) });
            Pantry_Grid.Children.Add(x);
            x.rowIndex = Pantry_Grid.RowDefinitions.Count-1;
            buttonList.Add(x);
            Grid.SetRow(x, Pantry_Grid.RowDefinitions.Count - 1);
            Grid.SetColumn(x, 0);

            TextBlock y = new TextBlock
            {
                Text = t,
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

        private void RebuildGridHelper(string s, string t)
        {
            FoodButton x = new FoodButton
            {
                Content = s,
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
                ExpDate = t,
                BorderThickness = new Thickness(0)


            };

            x.Click += new RoutedEventHandler(FoodButtonClicked);
            Pantry_Grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(30) });
            Pantry_Grid.Children.Add(x);
            x.rowIndex = Pantry_Grid.RowDefinitions.Count - 1;
            Grid.SetRow(x, Pantry_Grid.RowDefinitions.Count - 1);
            Grid.SetColumn(x, 0);

            TextBlock y = new TextBlock
            {
                Text = t,
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

        public void RebuildAfterDelete()
        {
            Pantry_Grid.Children.Clear();
            Pantry_Grid.RowDefinitions.Clear();
            Pantry_Grid.ColumnDefinitions.Clear();


            setupPantryGrid();


            foreach (FoodButton b in buttonList)
            {
                RebuildGridHelper(b.Content.ToString(), b.ExpDate);
            }

            buttonList.Clear();

            foreach(var x in  Pantry_Grid.Children)
            {
                if (x.GetType() == typeof(FoodButton))
                {
                    FoodButton temp = (FoodButton)x;
                    buttonList.Add(temp);
                }
            }

            
            
        }
        public void FoodButtonClicked(object sender, RoutedEventArgs e)
        {
            FoodButton b = (FoodButton)sender;
            
            int x = Pantry_Grid.Children.IndexOf(b);
            TextBlock t = ((TextBlock)Pantry_Grid.Children[x+1]);
            string Datestring = ((TextBlock)Pantry_Grid.Children[x+1]).Text;
            string FoodName = ((FoodButton)Pantry_Grid.Children[x]).Content.ToString();
            ItemWindowPopup i = new ItemWindowPopup(FoodName,Datestring);   
            i.ShowDialog();
            int retcode = i.retInt;

            switch (retcode) 
            { 
                case 0:
                    addToList("Consumed_List.json5", b.Content.ToString());
                    buttonList.Remove(b);
                    var watch = System.Diagnostics.Stopwatch.StartNew();
                    RebuildAfterDelete();
                    watch.Stop();
                    var elapsedMs = watch.ElapsedMilliseconds;
                    MessageBox.Show(elapsedMs.ToString());
                    
                    
                    
                    
                    break;
                case 1:
                    buttonList.Remove(b);
                    RebuildAfterDelete();
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

        static void addToFoodList(string fileName, string foodName)
        {
            bool lineFlag = false;
            string ln = "";
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
                }
            }

            if (lineFlag)
            {
                return;
            }
            else
            {
                using (TextWriter t = new StreamWriter(fileName, true))
                { 
                    t.WriteLine(foodName);
                }
            }
        }

        static void lineChanger(string newText, string fileName, int line_to_edit)
        {
            string[] arrLine = File.ReadAllLines(fileName);
            arrLine[line_to_edit] = newText;
            File.WriteAllLines(fileName, arrLine);
        }

        private void onWindowClose(object sender, EventArgs e) 
        {
            File.Create("Pantry_List.json5").Close();
            

            foreach (var x in Pantry_Grid.Children)
            {
                if (x.GetType() == typeof(FoodButton))
                {
                    FoodButton btn = (FoodButton)x;
                    using (TextWriter txt = new StreamWriter("Pantry_List.json5", true))
                    {
                        txt.WriteLine(btn.Content.ToString() + ":" + btn.ExpDate);
                        txt.Close();
                    }
                    
                }
            }

        }
    }

    

    public class FoodButton : Button
    {
        public int rowIndex { get; set; }
        public string ExpDate { get; set; }
    }

    

}