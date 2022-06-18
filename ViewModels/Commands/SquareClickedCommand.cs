using ChessAI.Model;
using ChessAI.Model.util.Pieces;
using System;
using System.Collections.Generic;
using System.Windows.Shapes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using ChessAI.Model.util;
using ChessAI.ViewModels.Util;
using System.Reflection;
using System.IO;
using System.Threading;

namespace ChessAI.ViewModels.Commands
{
    public class SquareClickedCommand : CommandBase
    {


        private readonly ChessViewModel _chessViewModel;

        public IEnumerable<Button> _squaresButtons { set; get; }
        public SquareClickedCommand(ChessViewModel chessViewModel)
        {
            _chessViewModel = chessViewModel;


        }

        public override void Execute(object? parameter)
        {


            Tuple<Tuple<int, int>, IEnumerable<Button>> positionAndSquares = (Tuple<Tuple<int, int>, IEnumerable<Button>>)parameter;
            _squaresButtons = positionAndSquares.Item2;

            Tuple<int, int> clickedPosition = positionAndSquares.Item1;
            Piece clickedPiece;
            List<Move> possibleMoves;
            Button clickedButton = CommandsOperations.GetButtonAtPosition(clickedPosition, _squaresButtons);







            //if no squares have been selected
            if (!_chessViewModel.selected_squares.Any())
            {


                // if the selected square is empty, return
                if (((Grid)clickedButton.Content).Children.Count == 0)
                {
                    return;
                }

                // get selected piece
                clickedPiece = _chessViewModel.GameState.GetPieceAtLocation(clickedPosition);

                // if the piece color is of the current turn, select all possible movessquares
                if (CommandsOperations.PieceOnTurn(clickedPiece, _chessViewModel))
                {
                    //update valid moves of current state
                    _chessViewModel.ValidMovesAtCurrentState = _chessViewModel.GameState.GetValidMoves();

                    possibleMoves = CommandsOperations.GetPieceMoves(clickedPiece, clickedPosition, _chessViewModel);
                    CommandsOperations.SelectPossibleMovesSquares(possibleMoves, _chessViewModel, _squaresButtons);
                }

            }
            else
            {

                // if empty square or square with other piece clicked
                if (!_chessViewModel.selected_squares.Contains(clickedPosition))
                {
                    if (((Grid)clickedButton.Content).Children.Count == 0)
                    {
                        CommandsOperations.DeselectSquares(_chessViewModel, _squaresButtons);
                        return;
                    }
                    foreach (var square in _chessViewModel.selected_squares)
                    {
                        CommandsOperations.DeselectSquare(square, _squaresButtons);

                    }
                    _chessViewModel.selected_squares.Clear();

                    //if other piece square clicked
                    clickedPiece = _chessViewModel.GameState.GetPieceAtLocation(clickedPosition);
                    possibleMoves = CommandsOperations.GetPieceMoves(clickedPiece, clickedPosition, _chessViewModel);

                    CommandsOperations.SelectPossibleMovesSquares(possibleMoves, _chessViewModel, _squaresButtons);
                }
                // if one of the selected squares has been clicked. So make move
                else
                {

                    // if moving piece square clicked, deselect all squares.
                    if (_chessViewModel.selected_squares[0].Equals(clickedPosition))
                    {
                        CommandsOperations.DeselectSquares(_chessViewModel, _squaresButtons);
                        _chessViewModel.selected_squares.Clear();
                        return;
                    }

                    //if a selected square other than moving piece is selected, then execute the move, both logically and graphically 
                    CommandsOperations.MakeMove(_chessViewModel.selected_squares[0], clickedPosition, _squaresButtons, _chessViewModel);

                    _chessViewModel.AlreadySuggested = false;
                    //After we make the move, we check the valid moves for the other player, so we can see if he is in check. Thus updating the game status
                    _chessViewModel.ValidMovesAtCurrentState = _chessViewModel.GameState.GetValidMoves();

                    // update gamestatus accordingly
                    CommandsOperations.UpdateGameStatus(_chessViewModel);

                    // deselect possible moves after move has been executed
                    CommandsOperations.DeselectSquares(_chessViewModel, _squaresButtons);
                    CommandsOperations.DeselectSquares(_chessViewModel.AI_selected_squares, _squaresButtons);


                    _chessViewModel.selected_squares.Clear();

                    if (_chessViewModel.GameVsEngine)
                    {
                        CommandsOperations.ExecuteAIMove(_chessViewModel, _squaresButtons);
                        _chessViewModel.AlreadySuggested = false;

                    }
                }
            }


        }

        public override bool CanExecute(object? parameter)
        {
            return !_chessViewModel.AIThinking && !_chessViewModel.Suggesting;
        }


    }
}
