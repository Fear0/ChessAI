using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChessAI.Model;

namespace ChessAI
{
    internal class Program
    {
        [STAThread]
        public static void Main(string[] args)
        {
            GameState game = new GameState();
            Console.WriteLine(game.ToString());
            game.MakeMove(new Move(Tuple.Create(1, 0), Tuple.Create(2, 0), game.board));
            Console.WriteLine(game.ToString());
            //var app = new App();
            //app.InitializeComponent();
            //Console.WriteLine("Hajba Zallou9");
            //app.Run();
            //Console.WriteLine("Hajba Zallou9");
        }
    }
}
