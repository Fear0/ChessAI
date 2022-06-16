using ChessAI.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessAI.ViewModels
{
    internal class GameStateViewModel : ViewModelBase
    {

        private readonly GameState _gamestate;



        public GameStateViewModel(GameState gamestate)
        {
            _gamestate = gamestate;
        }   
    }
}
