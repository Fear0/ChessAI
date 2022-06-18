
using ChessAI.ViewModels;
using ChessAI.ViewModels.Util;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ChessAI.Views
{
    /// <summary>
    /// Interaction logic for ChessView.xaml : Button will be handled through commands that acces the view model. First we load images
    /// </summary>
    public partial class ChessView : UserControl
    {

        IEnumerable<Button> squaresButtons;
        public ChessView()
        {
            InitializeComponent();
            //DataContext = new ChessViewModel();
            squaresButtons = FindVisualChildren<Button>(chessboardGrid);
            //DataContext = new ChessViewModel();
            //VisualTreeHelper.GetChild(chessboardGrid, 0);
            //var board = VisualTreeHelper.GetChild(chessboardGrid,0);
            
            foreach (var button in squaresButtons)
            {
                         
                int row = Grid.GetRow(button);
                int column = Grid.GetColumn(button);
                button.Tag = new Tuple<int, int>(row, column);
                Binding binding = new Binding("SquareClickedCommand");
                binding.Source = DataContext;
                button.SetBinding(Button.CommandProperty, binding);
                button.CommandParameter = new Tuple<Tuple<int,int>,IEnumerable<Button>>((Tuple<int,int>) button.Tag,squaresButtons);

                Grid g = new Grid();
                button.Content = g;

            }
            NewGameVsEngineButton.CommandParameter = squaresButtons;
            SuggestEngineMoveButton.CommandParameter = squaresButtons;
            //fear.ImageSource = new BitmapImage(new Uri("/Images/fear.png", UriKind.Relative));
        
            LoadNewGame();
           
        }

        void LoadNewGame()
        {

            foreach (var button in squaresButtons)
            {
                ((Grid)button.Content).Children.Clear();
                switch (((Tuple<int,int>)button.Tag).Item1)
                {
                    case 0:
                        switch (((Tuple<int, int>)button.Tag).Item2)
                        {
                            case 0:
                                ((Grid)button.Content).Children.Add((Image)ImageGenerator.GeneratePieceImage('r', 'b'));
                                break;
                            case 1:
                                ((Grid)button.Content).Children.Add((Image)ImageGenerator.GeneratePieceImage('n', 'b'));
                                break;
                            case 2:
                                ((Grid)button.Content).Children.Add((Image)ImageGenerator.GeneratePieceImage('b', 'b'));   
                                break;
                            case 3:
                                ((Grid)button.Content).Children.Add((Image)ImageGenerator.GeneratePieceImage('q', 'b'));
                                break;
                            case 4:
                                ((Grid)button.Content).Children.Add((Image)ImageGenerator.GeneratePieceImage('k', 'b'));
                                break;
                            case 5:
                                ((Grid)button.Content).Children.Add((Image)ImageGenerator.GeneratePieceImage('b', 'b'));
                                break;
                            case 6:
                                ((Grid)button.Content).Children.Add((Image)ImageGenerator.GeneratePieceImage('n', 'b'));
                                break;
                            case 7:
                                ((Grid)button.Content).Children.Add((Image)ImageGenerator.GeneratePieceImage('r', 'b'));
                                break;

                        }
                        break;
                    case 1:
                        ((Grid)button.Content).Children.Add((Image)ImageGenerator.GeneratePieceImage('p', 'b'));
                        break;
                    case 6:
                        ((Grid)button.Content).Children.Add((Image)ImageGenerator.GeneratePieceImage('p', 'w'));
                        break;
                    case 7:
                        switch (((Tuple<int, int>)button.Tag).Item2)
                        {
                            case 0:
                                ((Grid)button.Content).Children.Add((Image)ImageGenerator.GeneratePieceImage('r', 'w'));
                                break;
                            case 1:
                                ((Grid)button.Content).Children.Add((Image)ImageGenerator.GeneratePieceImage('n', 'w'));
                                break;
                            case 2:
                                ((Grid)button.Content).Children.Add((Image)ImageGenerator.GeneratePieceImage('b', 'w'));
                                break;
                            case 3:
                                ((Grid)button.Content).Children.Add((Image)ImageGenerator.GeneratePieceImage('q', 'w'));
                                break;
                            case 4:
                                ((Grid)button.Content).Children.Add((Image)ImageGenerator.GeneratePieceImage('k', 'w'));
                                break;
                            case 5:
                                ((Grid)button.Content).Children.Add((Image)ImageGenerator.GeneratePieceImage('b', 'w'));
                                break;
                            case 6:
                                ((Grid)button.Content).Children.Add((Image)ImageGenerator.GeneratePieceImage('n', 'w'));
                                break;
                            case 7:
                                ((Grid)button.Content).Children.Add((Image)ImageGenerator.GeneratePieceImage('r', 'w'));
                                break;

                        }
                        break;
                    default:
                        break;
                }
            }
        }


        public static IEnumerable<T> FindVisualChildren<T>(DependencyObject depObj) where T : DependencyObject
        {
            if (depObj != null)
            {
                for (int i = 0; i < VisualTreeHelper.GetChildrenCount(depObj); i++)
                {
                    DependencyObject child = VisualTreeHelper.GetChild(depObj, i);

                    if (child != null && child is T)
                    {
                        yield return (T)child;
                    }
                    foreach (T childOfChild in FindVisualChildren<T>(child))
                    {
                        yield return childOfChild;
                    }
                }
            }
        }

        void OnClickNewGame(Object sender, RoutedEventArgs e)
        {
            LoadNewGame();
        }
    }
}
