using System;
using System.Collections.Generic;

namespace CooperatedOthello {
    class Program {
        static void Main(string[] args) {
            var board = new OthBoard();
            var sol = new Solver();

            var moves = sol.Solve(board);

            // 結果の棋譜を表示
            foreach (var move in moves) {
                var (x, y) = board.ToLocation(move.Place);
                Console.WriteLine($"{move.Stone.Value} ({x}, {y})");
            }
            Console.ReadLine();
            // 棋譜を再現
            Replay(moves);
        }

        // 棋譜を再現
        private static void Replay(IEnumerable<Move> moves) {
            var board = new OthBoard();
            board.Print();
            Console.ReadLine();
            foreach (var move in moves) {
                Console.SetCursorPosition(0, 0);
                board.Put(move.Stone, move.Place);
                board.Print();
                Console.ReadLine();
            }
        }
    }
}
