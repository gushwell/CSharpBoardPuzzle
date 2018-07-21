using System;

namespace KnightTourApp {
    class Program {
        static void Main(string[] args) {
            var board = new Chessboard(8);
            board.StartPlace = board.ToIndex(8, 1);
            var solver = new Solver();
            var answer = solver.Solve(board);
            answer.Print();
        }
    }
}
