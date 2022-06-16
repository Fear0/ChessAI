using ChessAI.Model;
using ChessAI.Model.AI;
using ChessAI.ViewModels.Commands;
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
    internal class ChessViewModel : ViewModelBase
    {

        //public GameStateViewModel GameState { get; set; }
        public GameState GameState { get; set; }

        public List<Move> ValidMovesAtCurrentState;
        public bool Checkmate { get; set; }

        public bool EngineIsWhite = false;

        public static Agent AI => new MinimaxAlphaBetaAgent(EvaluationType2.BoardEvaluationFunction, 4);

        public bool GameVsEngine = false;

        private string _gameStatus;

        public  string GameStatus
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

        public List<Tuple<int,int>> selected_squares;
        public ICommand? SuggestMoveCommand { get; } 

        public SquareClickedCommand SquareClickedCommand { get; set; }

        public ICommand? NewGameCommand { get; }

        public ICommand? NewGameVsEngineCommand { get; }
        

        public ChessViewModel()
        {

            GameState = new GameState();
            selected_squares = new List<Tuple<int,int>>();
            SuggestMoveCommand = new SuggestMoveCommand();
            
            NewGameCommand = new NewGameCommand(this);
            NewGameVsEngineCommand = new NewGameVsEngineCommand(this);
            SquareClickedCommand = new SquareClickedCommand(this);

            

            int i = 0;
            /*Task.Run(() =>
            {
                while (i < 6)
                {
                    GameStatus = i.ToString();
                    i++;
                    Thread.Sleep(1000);
                }
            }
            );*/
         
        }

        public bool AIOnTurn()
        {
            return (GameState.whiteToPlay && EngineIsWhite) || (!GameState.whiteToPlay && !EngineIsWhite);
        }

        public Move GetAIMove()
        {
            return AI.GetAction(GameState,false);
        }
    }
}
