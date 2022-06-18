using ChessAI.Model.AI;
using ChessAI.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace ChessAI.ViewModels.Commands
{
    internal class NewGameVsEngineCommand : CommandBase
    {
        private readonly ChessViewModel _chessViewModel;


        public NewGameVsEngineCommand(ChessViewModel chessViewModel)
        {
            _chessViewModel = chessViewModel;
        }

        public override void Execute(object? parameter)
        {
            IEnumerable<Button> squaresButtons = (IEnumerable<Button>)parameter;
            ColorChoiceWindow win = new ColorChoiceWindow();
            win.Owner = Application.Current.MainWindow;
            Application.Current.MainWindow.Effect = new System.Windows.Media.Effects.BlurEffect() { Radius = 5 };
            win.ShowDialog();
            Application.Current.MainWindow.Effect = null;


            _chessViewModel.EngineIsWhite = win.Color != 'w';
            _chessViewModel.AI = new NegaMaxAlphaBetaAgent(EvaluationType1.BoardEvaluationFunction, win.Difficulty == 'm'? 4 : 5);
            _chessViewModel.GameState = new Model.GameState();
            _chessViewModel.ValidMovesAtCurrentState = new List<Model.Move>();
            _chessViewModel.Checkmate = false;
            _chessViewModel.GameStatus = "";
            _chessViewModel.GameVsEngine = true;
            _chessViewModel.Suggesting = false;
            _chessViewModel.AlreadySuggested = false;
            _chessViewModel.CapturedBlackPieces.Clear();
            _chessViewModel.CapturedWhitePieces.Clear();
            _chessViewModel.AI_selected_squares.Clear();
            _chessViewModel.selected_squares.Clear();

            if (_chessViewModel.EngineIsWhite)
            {
                CommandsOperations.ExecuteAIMove(_chessViewModel, squaresButtons);
            }




        }
    }
}
