using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace ChessAI.ViewModels.Commands
{
    internal class NewGameVsSelfCommand : CommandBase
    {

        private readonly ChessViewModel _chessViewModel;


        public NewGameVsSelfCommand(ChessViewModel chessViewModel)
        {
            _chessViewModel = chessViewModel;
        }

        public override void Execute(object? parameter)
        {
            //reinitiliaze everything
           
            _chessViewModel.GameState = new Model.GameState();
            _chessViewModel.ValidMovesAtCurrentState = new List<Model.Move>();
            _chessViewModel.SuggestMoveCommand = new SuggestMoveCommand(_chessViewModel);
            _chessViewModel.NewGameVsSelfCommand = new NewGameVsSelfCommand(_chessViewModel);
            _chessViewModel.SquareClickedCommand = new SquareClickedCommand(_chessViewModel);
            _chessViewModel.Checkmate = false;
            _chessViewModel.GameStatus = "";
            _chessViewModel.GameVsEngine = false;
            _chessViewModel.EngineIsWhite = false;
            _chessViewModel.AIThinking = false;
            _chessViewModel.AlreadySuggested = false;
            _chessViewModel.Suggesting = false;
            _chessViewModel.CapturedBlackPieces.Clear();
            _chessViewModel.CapturedWhitePieces.Clear();
            _chessViewModel.AI_selected_squares.Clear();
            _chessViewModel.selected_squares.Clear();
        }
    }
}
