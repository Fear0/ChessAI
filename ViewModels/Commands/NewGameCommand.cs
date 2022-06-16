using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessAI.ViewModels.Commands
{
    internal class NewGameCommand : CommandBase
    {

        private readonly ChessViewModel _chessViewModel;


        public NewGameCommand(ChessViewModel chessViewModel)
        {
            _chessViewModel = chessViewModel;
        }

        public override void Execute(object? parameter)
        {
            _chessViewModel.GameState = new Model.GameState();
            _chessViewModel.ValidMovesAtCurrentState = new List<Model.Move>();
            _chessViewModel.Checkmate = false;
            _chessViewModel.GameStatus = "";
        }
    }
}
