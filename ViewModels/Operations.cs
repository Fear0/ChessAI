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

namespace ChessAI.ViewModels
{
    /// <summary>
    /// Contains all the operations needed to perform moves logically, graphically and audibly
    /// </summary>
    public class CommandsOperations
    {
        


        /// <summary>
        /// Both logical and graphical execution of move 
        /// </summary>
        /// <param name="pieceSquare"></param>
        /// <param name="clickedPosition"></param>
        public static void MakeMove(Tuple<int, int> pieceSquare, Tuple<int, int> clickedPosition, IEnumerable<Button> _squaresButtons, ChessViewModel _chessViewModel)
        {

            //graphical execution of move
            String location = System.IO.Path.GetFullPath("SoundEffects\\chess_move.wav");
            location = location.Replace("\\", "\\\\");
            System.Media.SoundPlayer player = new System.Media.SoundPlayer();
            player.SoundLocation = location;
            player.Play();
            Move move = _chessViewModel.ValidMovesAtCurrentState.Where(e => e.startPosition.Equals(pieceSquare) && e.endPosition.Equals(clickedPosition)).First();
            Button targetSquare = GetButtonAtPosition(clickedPosition, _squaresButtons);
            Button sourceSquare = GetButtonAtPosition(pieceSquare, _squaresButtons);

            ((Grid)targetSquare.Content).Children.Clear();

            var pieceImage = ((Grid)sourceSquare.Content).Children[0];

            ((Grid)sourceSquare.Content).Children.Clear();

            ((Grid)targetSquare.Content).Children.Add(pieceImage);

            if (move.is_castle_move)
            {
                //king side castle
                if (clickedPosition.Item2 - pieceSquare.Item2 == 2)
                {
                    Button rookSquare = GetButtonAtPosition(new Tuple<int, int>(pieceSquare.Item1, clickedPosition.Item2 + 1), _squaresButtons);
                    Image rookImage = (Image)(UIElement)((Grid)rookSquare.Content).Children[0];
                    ((Grid)rookSquare.Content).Children.Clear();
                    Button newRookSquare = GetButtonAtPosition(new Tuple<int, int>(pieceSquare.Item1, clickedPosition.Item2 - 1), _squaresButtons);
                    ((Grid)newRookSquare.Content).Children.Add(rookImage);
                }

                //queen side castle
                else
                {
                    Button rookSquare = GetButtonAtPosition(new Tuple<int, int>(pieceSquare.Item1, clickedPosition.Item2 - 2), _squaresButtons);
                    Image rookImage = (Image)(UIElement)((Grid)rookSquare.Content).Children[0];
                    ((Grid)rookSquare.Content).Children.Clear();
                    Button newRookSquare = GetButtonAtPosition(new Tuple<int, int>(pieceSquare.Item1, clickedPosition.Item2 + 1), _squaresButtons);
                    ((Grid)newRookSquare.Content).Children.Add(rookImage);
                }

            }

            if (move.is_enpassant_move)
            {
                Button deadPawnSquare;
                if (_chessViewModel.GameState.whiteToPlay)
                {
                    deadPawnSquare = GetButtonAtPosition(new Tuple<int, int>(clickedPosition.Item1 + 1, clickedPosition.Item2),_squaresButtons);

                }
                else
                {
                    deadPawnSquare = GetButtonAtPosition(new Tuple<int, int>(clickedPosition.Item1 - 1, clickedPosition.Item2), _squaresButtons);
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


        static public Button GetButtonAtPosition(Tuple<int, int> position, IEnumerable<Button> _squaresButtons)
        {
            return _squaresButtons.Where(e => e.Tag.Equals(position)).First();
        }


        static public void ExecuteAIMove(ChessViewModel _chessViewModel, IEnumerable<Button> _squaresButtons)
        {
            if (_chessViewModel.AIOnTurn())
            {
                Task AITask = Task<Move>.Run(() =>
                {
                    _chessViewModel.AIThinking = true;
                    return _chessViewModel.GetAIMove();

                }).ContinueWith(task =>
                {
                    Move AIMove = task.Result;

                    if (AIMove != null)
                    {
                        System.Media.SoundPlayer player = new System.Media.SoundPlayer(@"C:\Users\ASUS\Uni\MyProjects\ChessAI\SoundEffects\chess_move.wav");

                        MakeMove(AIMove, _squaresButtons,_chessViewModel);
                        _chessViewModel.AIThinking = false;
                    }

                    //After we make the move, we check the valid moves for the other player, so we can see if he is in check. Thus updating the game status
                    _chessViewModel.ValidMovesAtCurrentState = _chessViewModel.GameState.GetValidMoves();

                    // update gamestatus accordingly
                    UpdateGameStatus(_chessViewModel);

                }, TaskScheduler.FromCurrentSynchronizationContext());
            }
        }

        static public void MakeMove(Move move, IEnumerable<Button> _squaresButtons, ChessViewModel _chessViewModel)
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
            Button targetSquare = GetButtonAtPosition(clickedPosition, _squaresButtons);
            Button sourceSquare = GetButtonAtPosition(pieceSquare, _squaresButtons);

            ((Grid)targetSquare.Content).Children.Clear();

            var pieceImage = ((Grid)sourceSquare.Content).Children[0];

            ((Grid)sourceSquare.Content).Children.Clear();

            ((Grid)targetSquare.Content).Children.Add(pieceImage);

            if (move.is_castle_move)
            {
                //king side castle
                if (clickedPosition.Item2 - pieceSquare.Item2 == 2)
                {
                    Button rookSquare = GetButtonAtPosition(new Tuple<int, int>(pieceSquare.Item1, clickedPosition.Item2 + 1), _squaresButtons);
                    Image rookImage = (Image)(UIElement)((Grid)rookSquare.Content).Children[0];
                    ((Grid)rookSquare.Content).Children.Clear();
                    Button newRookSquare = GetButtonAtPosition(new Tuple<int, int>(pieceSquare.Item1, clickedPosition.Item2 - 1), _squaresButtons);
                    ((Grid)newRookSquare.Content).Children.Add(rookImage);
                }

                //queen side castle
                else
                {
                    Button rookSquare = GetButtonAtPosition(new Tuple<int, int>(pieceSquare.Item1, clickedPosition.Item2 - 2), _squaresButtons);
                    Image rookImage = (Image)(UIElement)((Grid)rookSquare.Content).Children[0];
                    ((Grid)rookSquare.Content).Children.Clear();
                    Button newRookSquare = GetButtonAtPosition(new Tuple<int, int>(pieceSquare.Item1, clickedPosition.Item2 + 1), _squaresButtons);
                    ((Grid)newRookSquare.Content).Children.Add(rookImage);
                }

            }

            if (move.is_enpassant_move)
            {
                Button deadPawnSquare;
                if (_chessViewModel.GameState.whiteToPlay)
                {
                    deadPawnSquare = GetButtonAtPosition(new Tuple<int, int>(clickedPosition.Item1 + 1, clickedPosition.Item2), _squaresButtons);

                }
                else
                {
                    deadPawnSquare = GetButtonAtPosition(new Tuple<int, int>(clickedPosition.Item1 - 1, clickedPosition.Item2), _squaresButtons);
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
        /// updates status (check, checkmate, stalemate)
        /// </summary>
        public static void UpdateGameStatus(ChessViewModel _chessViewModel)
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
        public static List<Move> GetPieceMoves(Piece piece, Tuple<int, int> position, ChessViewModel _chessViewModel)
        {
            List<Move> possibleMoves = piece.GetPossibleMoves(_chessViewModel.GameState);
            if (piece is King)
            {
                possibleMoves.AddRange(_chessViewModel.GameState.GetCastleMoves(position));
            }
            possibleMoves.RemoveAll(e => !_chessViewModel.ValidMovesAtCurrentState.Contains(e));
            return possibleMoves;

        }



        public static void SelectPossibleMovesSquares(List<Move> moves, ChessViewModel _chessViewModel, IEnumerable<Button> _squaresButtons)
        {

            if (moves.Any())
            {
                _chessViewModel.selected_squares.Add(moves[0].startPosition);
                SelectSquare(moves[0].startPosition,_squaresButtons, true);
                foreach (var move in moves)
                {
                    _chessViewModel.selected_squares.Add(move.endPosition);
                    SelectSquare(move.endPosition,_squaresButtons);
                }
            }
        }

        public static void DeselectSquares(ChessViewModel _chessViewModel, IEnumerable<Button> _squaresButtons)
        {
            foreach (var square in _chessViewModel.selected_squares)
            {
                DeselectSquare(square,_squaresButtons);

            }
        }
        /// <summary>
        /// graphically highlights the square on the chessboard
        /// </summary>
        /// <param name="position"></param>
        public static void SelectSquare(Tuple<int, int> position, IEnumerable<Button> _squaresButtons, bool pieceSquare = false)
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

        public static void DeselectSquare(Tuple<int, int> position, IEnumerable<Button> _squaresButtons)
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

        /// <summary>
        /// Check if piece has current turn color
        /// </summary>
        /// <param name="piece"></param>
        /// <returns></returns>
        public static bool PieceOnTurn(Piece piece,ChessViewModel _chessViewModel)
        {
            if ((piece.pieceColor == PieceColor.White && _chessViewModel.GameState.whiteToPlay)
                || (piece.pieceColor == PieceColor.Black && !_chessViewModel.GameState.whiteToPlay))
            {
                return true;
            }
            return false;
        }

        public static Rectangle GenerateSelectedSquare(SolidColorBrush color)
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
