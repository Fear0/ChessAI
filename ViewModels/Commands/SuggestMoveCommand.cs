using ChessAI.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace ChessAI.ViewModels.Commands
{
    public class SuggestMoveCommand : CommandBase
    {

        private readonly ChessViewModel _chessViewModel;

       

        public SuggestMoveCommand(ChessViewModel chessViewModel)
        {
            _chessViewModel = chessViewModel;
        }
        public async override void Execute(object? parameter)
        {
            _chessViewModel.Suggesting = true;
            IEnumerable<Button> squaresButtons = (IEnumerable<Button>)parameter;

            // wait for engine move to be computed without blocking the UI
            Move AISuggestion = await CommandsOperations.GetAIMove(_chessViewModel);
            _chessViewModel.AlreadySuggested = true;

            CommandsOperations.SelectSquare(AISuggestion.startPosition, squaresButtons,true);
            CommandsOperations.SelectSquare(AISuggestion.endPosition, squaresButtons, true);
            _chessViewModel.AI_selected_squares.Add(AISuggestion.startPosition);
            _chessViewModel.AI_selected_squares.Add(AISuggestion.endPosition);
            _chessViewModel.Suggesting = false;
        }

        public override bool CanExecute(object? parameter)
        {
            return !_chessViewModel.AlreadySuggested && !_chessViewModel.Suggesting && !_chessViewModel.AIThinking;
        }
    }
}
