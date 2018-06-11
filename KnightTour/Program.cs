using System;

namespace KnightTour {
    class Program {
        static void Main(string[] args) {
            var board = new Chessboard(8);
            // board.StartPlace = board.ToIndex(2, 3);  // 開始位置を変更したい場合
            var solver = new Solver();
            var answer = solver.Solve(board);
            answer.Print();
        }
    }
}
