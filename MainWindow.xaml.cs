using System.Diagnostics;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Threading.Tasks;
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
        int firstClickedIndex = -1;
        int secondClickedIndex = -1;
        bool isChecking = false;
        bool Notdrawn = true;

        public MainWindow()
        {
            InitializeComponent();
            listBoxTableSize.ItemsSource = TableSize;
            listBoxTableType.ItemsSource = TableType;
        }
        //ideas for later
        //Replay and exit button at the end of the game
        //some button for resetting the game mid game
        private void ButtonClick_TableDraw(object sender, RoutedEventArgs e)
        {
            if (pickedSize && pickedType && Notdrawn)
            {
                Notdrawn = false;
                PlacingTheCards();
                GenerateList();
            }
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
            // create grid rows and columns
            for (int i = 0; i < SelectedSize; i++)
            {
                ButtonGrid.RowDefinitions.Add(new RowDefinition());
                ButtonGrid.ColumnDefinitions.Add(new ColumnDefinition());
            }

            // place buttons face-down (empty content). Buttons are created in the same order as the cards list.
            for (int i = 0; i < SelectedSize; i++)
            {
                for (int j = 0; j < SelectedSize; j++)
                {
                    Button btn = new Button
                    {
                        Content = "",
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

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            if (isChecking) return;

            Button clickedButton = sender as Button;
            int index = ButtonGrid.Children.IndexOf(clickedButton);

            // if this card is already revealed or disabled, ignore
            if (clickedButton == null || clickedButton.IsEnabled == false) return;
            if (clickedButton.Content?.ToString() != "") return;

            // reveal
            clickedButton.Content = cards[index];

            if (firstClickedIndex == -1)
            {
                firstClickedIndex = index;
                return;
            }

            if (secondClickedIndex == -1)
            {
                // no clicking the same card twice
                if (index == firstClickedIndex) return;
                secondClickedIndex = index;
            }

            // check
            Button firstButton = ButtonGrid.Children[firstClickedIndex] as Button;
            Button secondButton = ButtonGrid.Children[secondClickedIndex] as Button;
            if (cards[firstClickedIndex] == cards[secondClickedIndex])
            {
                // leave them revealed and disable
                firstButton.IsEnabled = false;
                secondButton.IsEnabled = false;
                firstClickedIndex = -1;
                secondClickedIndex = -1;
            }
            else
            {
                // non-matching
                isChecking = true;
                await Task.Delay(800); //this is why we need async
                if (firstButton != null) firstButton.Content = "";
                if (secondButton != null) secondButton.Content = "";
                isChecking = false;
                firstClickedIndex = -1;
                secondClickedIndex = -1;
            }
        }

        private void GenerateList()
        {
            cards.Clear();
            firstClickedIndex = -1;
            secondClickedIndex = -1;
            isChecking = false;

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
                for (int i = 0; i < halfCards; i++)
                {
                    char c = (char)('A' + i);
                    cards.Add(c.ToString());
                    cards.Add(c.ToString());
                }
            }
            else if (nowSelectedType == "Emojis")
            {
                string[] emojis = {"😀", "😂", "😍", "😎", "🤔", "😢", "😡", "🎉", "🥳", "🤯", "🥶", "😱", "😴", "🤓", "😕", "🙃", "🙄", "😭"};
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