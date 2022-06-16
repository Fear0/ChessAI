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

namespace ChessAI.ViewModels.Commands
{
    internal class SquareClickedCommand : CommandBase
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
            List<Move> possibleMoves = new();
            Button clickedButton = GetButtonAtPosition(clickedPosition);




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
                if (PieceOnTurn(clickedPiece))
                {
                    //update valid moves of current state
                    _chessViewModel.ValidMovesAtCurrentState = _chessViewModel.GameState.GetValidMoves();

                    possibleMoves = GetPieceMoves(clickedPiece, clickedPosition);
                    SelectPossibleMovesSquares(possibleMoves);
                }

            }
            else
            {

                // if empty square or square with other piece clicked
                if (!_chessViewModel.selected_squares.Contains(clickedPosition))
                {
                    if (((Grid)clickedButton.Content).Children.Count == 0)
                    {
                        DeselectSquares();
                        return;
                    }
                    foreach (var square in _chessViewModel.selected_squares)
                    {
                        DeselectSquare(square);

                    }
                    _chessViewModel.selected_squares.Clear();

                    //if other piece square clicked
                    clickedPiece = _chessViewModel.GameState.GetPieceAtLocation(clickedPosition);
                    possibleMoves = GetPieceMoves(clickedPiece, clickedPosition);

                    SelectPossibleMovesSquares(possibleMoves);
                }
                // if one of the selected squares has been clicked. So make move
                else
                {

                    // if moving piece square clicked, deselect all squares.
                    if (_chessViewModel.selected_squares[0].Equals(clickedPosition))
                    {
                        DeselectSquares();
                        _chessViewModel.selected_squares.Clear();
                        return;
                    }

                    //if a selected square other than moving piece is selected, then execute the move, both logically and graphically 
                    MakeMove(_chessViewModel.selected_squares[0], clickedPosition, _chessViewModel.ValidMovesAtCurrentState);

                    //After we make the move, we check the valid moves for the other player, so we can see if he is in check. Thus updating the game status
                    _chessViewModel.ValidMovesAtCurrentState = _chessViewModel.GameState.GetValidMoves();

                    // update gamestatus accordingly
                    UpdateGameStatus();

                    // deselect possible moves after move has been executed
                    DeselectSquares();


                    _chessViewModel.selected_squares.Clear();

                    if (_chessViewModel.GameVsEngine)
                    {
                        ExecuteAIMove();
                    }
                }
            }

        }



        public void ExecuteAIMove()
        {
            if (_chessViewModel.AIOnTurn())
            {
                Task AITask = Task<Move>.Run(() =>
                {
                    return _chessViewModel.GetAIMove();

                }).ContinueWith(task =>
                {
                    Move AIMove = task.Result;

                    if (AIMove != null)
                    {
                        System.Media.SoundPlayer player = new System.Media.SoundPlayer(@"C:\Users\ASUS\Uni\MyProjects\ChessAI\SoundEffects\chess_move.wav");
             
                        MakeMove(AIMove);
                 
                    }

                    //After we make the move, we check the valid moves for the other player, so we can see if he is in check. Thus updating the game status
                    _chessViewModel.ValidMovesAtCurrentState = _chessViewModel.GameState.GetValidMoves();

                    // update gamestatus accordingly
                    UpdateGameStatus();

                }, TaskScheduler.FromCurrentSynchronizationContext());
            }
        }
        /// <summary>
        /// Both logical and graphical execution of move 
        /// </summary>
        /// <param name="pieceSquare"></param>
        /// <param name="clickedPosition"></param>
        private void MakeMove(Tuple<int, int> pieceSquare, Tuple<int, int> clickedPosition, List<Move> moves)
        {

            //graphical execution of move
            String location = System.IO.Path.GetFullPath("SoundEffects\\chess_move.wav");
            location = location.Replace("\\","\\\\");
            System.Media.SoundPlayer player = new System.Media.SoundPlayer();
            player.SoundLocation = location;
            player.Play();
            Move move = moves.Where(e => e.startPosition.Equals(pieceSquare) && e.endPosition.Equals(clickedPosition)).First();
            Button targetSquare = GetButtonAtPosition(clickedPosition);
            Button sourceSquare = GetButtonAtPosition(pieceSquare);

            ((Grid)targetSquare.Content).Children.Clear();

            var pieceImage = ((Grid)sourceSquare.Content).Children[0];

            ((Grid)sourceSquare.Content).Children.Clear();

            ((Grid)targetSquare.Content).Children.Add(pieceImage);

            if (move.is_castle_move)
            {
                //king side castle
                if (clickedPosition.Item2 - pieceSquare.Item2 == 2)
                {
                    Button rookSquare = GetButtonAtPosition(new Tuple<int, int>(pieceSquare.Item1, clickedPosition.Item2 + 1));
                    Image rookImage = (Image)(UIElement)((Grid)rookSquare.Content).Children[0];
                    ((Grid)rookSquare.Content).Children.Clear();
                    Button newRookSquare = GetButtonAtPosition(new Tuple<int, int>(pieceSquare.Item1, clickedPosition.Item2 - 1));
                    ((Grid)newRookSquare.Content).Children.Add(rookImage);
                }

                //queen side castle
                else
                {
                    Button rookSquare = GetButtonAtPosition(new Tuple<int, int>(pieceSquare.Item1, clickedPosition.Item2 - 2));
                    Image rookImage = (Image)(UIElement)((Grid)rookSquare.Content).Children[0];
                    ((Grid)rookSquare.Content).Children.Clear();
                    Button newRookSquare = GetButtonAtPosition(new Tuple<int, int>(pieceSquare.Item1, clickedPosition.Item2 + 1));
                    ((Grid)newRookSquare.Content).Children.Add(rookImage);
                }

            }

            if (move.is_enpassant_move)
            {
                Button deadPawnSquare;
                if (_chessViewModel.GameState.whiteToPlay)
                {
                    deadPawnSquare = GetButtonAtPosition(new Tuple<int, int>(clickedPosition.Item1 + 1, clickedPosition.Item2));

                }
                else
                {
                    deadPawnSquare = GetButtonAtPosition(new Tuple<int, int>(clickedPosition.Item1 - 1, clickedPosition.Item2));
                }
                ((Grid)deadPawnSquare.Content).Children.Clear();
            }

            if (move.is_pawn_promotion)
            {
                ((Grid)targetSquare.Content).Children.Clear();
                if (_chessViewModel.GameState.whiteToPlay)
                {
                    ((Grid)targetSquare.Content).Children.Add(ImageGenerator.GeneratePieceImage('q', 'w'));
                }
                else
                {
                    ((Grid)targetSquare.Content).Children.Add(ImageGenerator.GeneratePieceImage('q', 'b'));
                }

            }

            //logical execution of move
            _chessViewModel.GameState.MakeMove(move);


        }

        private void MakeMove(Move move)
        {

            //graphical execution of move

            String location = System.IO.Path.GetFullPath("SoundEffects\\chess_move.wav");
            location = location.Replace("\\", "\\\\");
            System.Media.SoundPlayer player = new System.Media.SoundPlayer();
            player.SoundLocation = location;
            player.Play();
            //Move move = moves.Where(e => e.startPosition.Equals(pieceSquare) && e.endPosition.Equals(clickedPosition)).First();
            Tuple<int, int> clickedPosition = move.endPosition;
            Tuple<int, int> pieceSquare = move.startPosition;
            Button targetSquare = GetButtonAtPosition(clickedPosition);
            Button sourceSquare = GetButtonAtPosition(pieceSquare);

            ((Grid)targetSquare.Content).Children.Clear();

            var pieceImage = ((Grid)sourceSquare.Content).Children[0];

            ((Grid)sourceSquare.Content).Children.Clear();

            ((Grid)targetSquare.Content).Children.Add(pieceImage);

            if (move.is_castle_move)
            {
                //king side castle
                if (clickedPosition.Item2 - pieceSquare.Item2 == 2)
                {
                    Button rookSquare = GetButtonAtPosition(new Tuple<int, int>(pieceSquare.Item1, clickedPosition.Item2 + 1));
                    Image rookImage = (Image)(UIElement)((Grid)rookSquare.Content).Children[0];
                    ((Grid)rookSquare.Content).Children.Clear();
                    Button newRookSquare = GetButtonAtPosition(new Tuple<int, int>(pieceSquare.Item1, clickedPosition.Item2 - 1));
                    ((Grid)newRookSquare.Content).Children.Add(rookImage);
                }

                //queen side castle
                else
                {
                    Button rookSquare = GetButtonAtPosition(new Tuple<int, int>(pieceSquare.Item1, clickedPosition.Item2 - 2));
                    Image rookImage = (Image)(UIElement)((Grid)rookSquare.Content).Children[0];
                    ((Grid)rookSquare.Content).Children.Clear();
                    Button newRookSquare = GetButtonAtPosition(new Tuple<int, int>(pieceSquare.Item1, clickedPosition.Item2 + 1));
                    ((Grid)newRookSquare.Content).Children.Add(rookImage);
                }

            }

            if (move.is_enpassant_move)
            {
                Button deadPawnSquare;
                if (_chessViewModel.GameState.whiteToPlay)
                {
                    deadPawnSquare = GetButtonAtPosition(new Tuple<int, int>(clickedPosition.Item1 + 1, clickedPosition.Item2));

                }
                else
                {
                    deadPawnSquare = GetButtonAtPosition(new Tuple<int, int>(clickedPosition.Item1 - 1, clickedPosition.Item2));
                }
                ((Grid)deadPawnSquare.Content).Children.Clear();
            }

            if (move.is_pawn_promotion)
            {
                ((Grid)targetSquare.Content).Children.Clear();
                if (_chessViewModel.GameState.whiteToPlay)
                {
                    ((Grid)targetSquare.Content).Children.Add(ImageGenerator.GeneratePieceImage('q', 'w'));
                }
                else
                {
                    ((Grid)targetSquare.Content).Children.Add(ImageGenerator.GeneratePieceImage('q', 'b'));
                }

            }

            //logical execution of move
            _chessViewModel.GameState.MakeMove(move);


        }

        /// <summary>
        /// Check if piece has current turn color
        /// </summary>
        /// <param name="piece"></param>
        /// <returns></returns>
        private bool PieceOnTurn(Piece piece)
        {
            if ((piece.pieceColor == PieceColor.White && _chessViewModel.GameState.whiteToPlay)
                || (piece.pieceColor == PieceColor.Black && !_chessViewModel.GameState.whiteToPlay))
            {
                return true;
            }
            return false;
        }
        /// <summary>
        /// updates status (check, checkmate, stalemate)
        /// </summary>
        void UpdateGameStatus()
        {
            if (_chessViewModel.GameState.stalemate)
            {
                _chessViewModel.GameStatus = "Stalemate";

            }
            else if (_chessViewModel.GameState.checkmate)
            {
                //_chessViewModel.Checkmate = true;

                _chessViewModel.GameStatus = "Checkmate";
                return;

            }
            else if (_chessViewModel.GameState.IsInCheck())
            {
                _chessViewModel.GameStatus = "Check";
            }
            else
            {
                _chessViewModel.GameStatus = "";
            }
        }


        /// <summary>
        /// Get the valid moves for each piece at current gamestate
        /// </summary>
        /// <param name="piece"></param>
        /// <param name="position"></param>
        /// <returns></returns>
        List<Move> GetPieceMoves(Piece piece, Tuple<int, int> position)
        {
            List<Move> possibleMoves = piece.GetPossibleMoves(_chessViewModel.GameState);
            if (piece is King)
            {
                possibleMoves.AddRange(_chessViewModel.GameState.GetCastleMoves(position));
            }
            possibleMoves.RemoveAll(e => !_chessViewModel.ValidMovesAtCurrentState.Contains(e));
            return possibleMoves;

        }
        /// <summary>
        /// removes the illegal moves from the the row possible moves. For example those who dont block check, or reveal a discovered check...
        /// </summary>
        /// <param name="possibleMoves"></param>


        private Button GetButtonAtPosition(Tuple<int, int> position)
        {
            return _squaresButtons.Where(e => e.Tag.Equals(position)).First();
        }
        void SelectPossibleMovesSquares(List<Move> moves)
        {

            if (moves.Any())
            {
                _chessViewModel.selected_squares.Add(moves[0].startPosition);
                SelectSquare(moves[0].startPosition, true);
                foreach (var move in moves)
                {
                    _chessViewModel.selected_squares.Add(move.endPosition);
                    SelectSquare(move.endPosition);
                }
            }
        }

        void DeselectSquares()
        {
            foreach (var square in _chessViewModel.selected_squares)
            {
                DeselectSquare(square);

            }
        }
        /// <summary>
        /// graphically highlights the square on the chessboard
        /// </summary>
        /// <param name="position"></param>
        void SelectSquare(Tuple<int, int> position, bool pieceSquare = false)
        {
            Button button = _squaresButtons.Where(but => but.Tag.Equals(position)).First();
            if (pieceSquare)
            {
                ((Grid)button.Content).Children.Add(GenerateSelectedSquare(Brushes.Magenta));
            }
            else
            {
                ((Grid)button.Content).Children.Add(GenerateSelectedSquare(Brushes.Yellow));
            }
        }

        void DeselectSquare(Tuple<int, int> position)
        {
            Button button = _squaresButtons.Where(but => but.Tag.Equals(position)).First();
            if (((Grid)button.Content).Children.Count != 0)
            {
                foreach (UIElement element in ((Grid)button.Content).Children)
                {
                    if (element is Rectangle)
                    {
                        ((Grid)button.Content).Children.Remove(element);
                        break;
                    }
                }
            }
        }

        static Rectangle GenerateSelectedSquare(SolidColorBrush color)
        {
            Rectangle selectedSquare = new Rectangle();
            selectedSquare.Fill = color;
            selectedSquare.Opacity = 0.3;
            selectedSquare.Stretch = Stretch.UniformToFill;
            selectedSquare.HorizontalAlignment = HorizontalAlignment.Stretch;
            selectedSquare.IsHitTestVisible = false;
            return selectedSquare;
        }
    }
}
