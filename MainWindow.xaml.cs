using System.Diagnostics;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Memoriajatek
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        List<string> TableSize = new List<string>() { "2 x 2", "4 x 4", "6 x 6" };
        List<string> TableType = new List<string>() { "Numbers", "Letters", "Emojis" };
        List<string> cards = new List<string>();
        string nowSelectedSize = "";
        string nowSelectedType = "";
        int SelectedSize = 0;
        bool pickedSize = false;
        bool pickedType = false;


        public MainWindow()
        {
            InitializeComponent();
            listBoxTableSize.ItemsSource = TableSize;
            listBoxTableType.ItemsSource = TableType;
        }
        
        private void SizePick(object sender, RoutedEventArgs e)
        {
            var lb = sender as ListBox;
            if (lb?.SelectedItem == null)
            {
                Debug.WriteLine("SizePick: SelectedItem is null");
                return;
            }

            string selectedSize = lb.SelectedItem.ToString();
            nowSelectedSize = selectedSize;
            SelectedSize = int.Parse(selectedSize.Split(' ')[0]);
            pickedSize = true;
        }
        private void TypePick(object sender, RoutedEventArgs e)
        {
            var lb = sender as ListBox;
            if (lb?.SelectedItem == null)
            {
                Debug.WriteLine("TypePick: SelectedItem is null");
                return;
            }

            string selectedType = lb.SelectedItem.ToString();
            nowSelectedType = selectedType;
            pickedType = true;
        }

        private void PlacingTheCards()
        {
            ButtonGrid.Children.Clear();
            ButtonGrid.RowDefinitions.Clear();
            ButtonGrid.ColumnDefinitions.Clear();
            this.cards = new List<string>();
            string[,] cards = null;
            for (int i = 0; i < SelectedSize; i++)
            {
                ButtonGrid.RowDefinitions.Add(new RowDefinition());
                ButtonGrid.ColumnDefinitions.Add(new ColumnDefinition());
            }

            for (int i = 0; i < SelectedSize; i++)
            {
                for (int j = 0; j < SelectedSize; j++)
                {
                    cards = new string[SelectedSize, SelectedSize];
                }
            }

            for (int i = 0; i < SelectedSize; i++)
            {
                for (int j = 0; j < SelectedSize; j++)
                {
                    string label = cards[i, j];
                    Button btn = new Button
                    {
                        Content = label,
                        FontSize = 20,
                        FontWeight = FontWeights.Bold,
                        Margin = new Thickness(3),
                    };
                    btn.Click += Button_Click;
                    
                    Grid.SetRow(btn, i);
                    Grid.SetColumn(btn, j);
                    ButtonGrid.Children.Add(btn);
                }
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Button clickedButton = sender as Button;
            int index = ButtonGrid.Children.IndexOf(clickedButton);
            clickedButton.Content = cards[index];

            int firstClickedButton = 0;
            int secondClickedButton = 0;

        }

        private void ButtonClick_TableDraw(object sender, RoutedEventArgs e)
        {
            if (pickedSize && pickedType)
            {
                PlacingTheCards();
                GenerateList();
            }
        }

        private void GenerateList()
        {
            
            int totalCards = SelectedSize * SelectedSize;
            int halfCards = totalCards / 2;
            if (nowSelectedType == "Numbers")
            {
                for (int i = 1; i <= halfCards; i++)
                {
                    cards.Add(i.ToString());
                    cards.Add(i.ToString());
                }
            }
            else if (nowSelectedType == "Letters")
            {
                for (char c = 'A'; c < 'A' + halfCards; c++)
                {
                    cards.Add(c.ToString());
                    cards.Add(c.ToString());
                }
            }
            else if (nowSelectedType == "Emojis")
            {
                string[] emojis = { "😀", "😂", "😍", "😎", "🤔", "😢", "😡", "🎉" , "🥳", "🤯", "🥶", "😱", "😴", "🤓" , "😕", "🙃", "🙄", "😭" };
                for (int i = 0; i < halfCards; i++)
                {
                    string emoji = emojis[i % emojis.Length];
                    cards.Add(emoji);
                    cards.Add(emoji);
                }
            }

            Random rng = new Random();
            int n = cards.Count;
            while (n > 1)
            {
                n--;
                int k = rng.Next(n + 1);
                string value = cards[k];
                cards[k] = cards[n];
                cards[n] = value;
            }
        }
    }
}