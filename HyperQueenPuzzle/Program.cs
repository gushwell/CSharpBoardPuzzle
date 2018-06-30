using System;

namespace HyperQueenPuzzle {
    class Program {
        static void Main(string[] args) {
            var solver = new Solver();
            var board = new Board(7);
            foreach (var r in solver.Solve(board)) {
                Print(board);
            }
        }


        static void Print(Board board) {
            for (int y = 1; y <= board.YSize; y++) {
                for (int x = 1; x <= board.XSize; x++) {
                    var p = board[x, y];
                    char c = (p == Piece.Queen) ? 'Q' : '.';
                    Console.Write(c + " ");
                }
                Console.WriteLine();
            }
            Console.WriteLine();
        }
    }
}
