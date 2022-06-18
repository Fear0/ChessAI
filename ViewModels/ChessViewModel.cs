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

        public Agent AI = new NegaMaxAlphaBetaAgent(EvaluationType1.BoardEvaluationFunction, 4);

        public bool GameVsEngine = false;

        private ObservableCollection<IconViewModel> _capturedWhitePieces = new();

        private ObservableCollection<IconViewModel> _capturedBlackPieces  = new();

        //public Tuple<Tuple<int, int>, Tuple<int, int>> AIMovePositions = null;

        public ICollection<IconViewModel> CapturedWhitePieces => _capturedWhitePieces;
        public ICollection<IconViewModel> CapturedBlackPieces => _capturedBlackPieces;

        // if engine is computing AI's turn
        public bool AIThinking = false;

        // flag signals if engine is computing move to be recommended
        public bool Suggesting = false;

        // if recommended move already displayed
        public bool AlreadySuggested = false;

        public Move EngineMove;

        
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

        public List<Tuple<int, int>> AI_selected_squares;
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
            AI_selected_squares = new();
            SuggestMoveCommand = new SuggestMoveCommand(this);

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
            EngineMove = AI.GetAction(GameState);
            return EngineMove;
        }
    }
}
