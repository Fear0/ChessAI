using ChessAI.Model;
using ChessAI.Model.AI;
using ChessAI.ViewModels.Commands;
using ChessAI.ViewModels.Util;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace ChessAI.ViewModels
{
    public class ChessViewModel : ViewModelBase
    {

        //public GameStateViewModel GameState { get; set; }
        public GameState GameState { get; set; }

        public List<Move> ValidMovesAtCurrentState;
        public bool Checkmate { get; set; }

        public bool EngineIsWhite = false;

        public static Agent AI => new NegaMaxAlphaBetaAgent(EvaluationType1.BoardEvaluationFunction, 4);

        public bool GameVsEngine = false;

        public ObservableCollection<IconViewModel> _capturedWhitePieces = new();

        public ObservableCollection<IconViewModel> _capturedBlackPieces  = new();

        public ICollection<IconViewModel> CapturedWhitePieces => _capturedWhitePieces;
        public ICollection<IconViewModel> CapturedBlackPieces => _capturedBlackPieces;

        public bool AIThinking = false;
        private string _gameStatus;


        public string GameStatus
        {
            get
            {

                return _gameStatus;
            }
            set
            {
                _gameStatus = value;
                OnPropertyChanged();
            }
        }

        public List<Tuple<int, int>> selected_squares;
        public ICommand? SuggestMoveCommand { get; set; }

        public ICommand? SquareClickedCommand { get; set; }

        public ICommand? NewGameVsSelfCommand { get; set; }

        public ICommand? NewGameVsEngineCommand { get; set; }


        public ChessViewModel()
        {

            //_capturedWhitePieces = new();
            //_capturedBlackPieces = new();
            GameState = new GameState();
            selected_squares = new List<Tuple<int, int>>();
            SuggestMoveCommand = new SuggestMoveCommand();

            NewGameVsSelfCommand = new NewGameVsSelfCommand(this);
            NewGameVsEngineCommand = new NewGameVsEngineCommand(this);
            SquareClickedCommand = new SquareClickedCommand(this);


        }

        public bool AIOnTurn()
        {
            return GameVsEngine && (GameState.whiteToPlay && EngineIsWhite) || (!GameState.whiteToPlay && !EngineIsWhite);
        }

        public Move GetAIMove()
        {
            return AI.GetAction(GameState, false);
        }
    }
}
