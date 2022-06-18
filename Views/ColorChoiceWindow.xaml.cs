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
using System.Windows.Shapes;

namespace ChessAI.Views
{
    /// <summary>
    /// Interaction logic for ColorChoiceWindow.xaml
    /// </summary>
    public partial class ColorChoiceWindow : Window
    {

        public char Color { get; set; }

        public char Difficulty { get; set; }
        public ColorChoiceWindow()
        {
            InitializeComponent();

            BlackButton.Content = ImageGenerator.GeneratePieceImage('k', 'b');
            WhiteButton.Content = ImageGenerator.GeneratePieceImage('k', 'w');
        }

        /*private void BlackButton_Click(object sender, RoutedEventArgs e)
        {
            Color = 'b';
            //Close();
        }

        private void WhiteButton_Click(object sender, RoutedEventArgs e)
        {
            Color = 'w';
            //Close();
        }*/

        private void BlackKing_Checked(object sender, RoutedEventArgs e)
        {
            Color = 'b';
            if (Difficulty == 'h' || Difficulty == 'm')
            {
                Close();
            }

        }

        private void WhiteKing_Checked(object sender, RoutedEventArgs e)
        {
            Color = 'w';
            if (Difficulty == 'h' || Difficulty == 'm')
            {
                Close();
            }
        }

        private void mediumBox_Checked(object sender, RoutedEventArgs e)
        {
            Difficulty = 'm';
            if (Color == 'b' || Color == 'w')
            {
                Close();
            }
        }

        private void hardBox_Checked(object sender, RoutedEventArgs e)
        {
            Difficulty = 'h';
            if (Color == 'b' || Color == 'w')
            {
                Close();
            }
        }
    }
}
