using System;

namespace KnightTour {
    class Program {
        static void Main(string[] args) {
            var board = new Chessboard();
            var solver = new Solver();
            var answer = solver.Solve(board, board.ToIndex(2, 1));
            answer.Print();
        }
    }
}
