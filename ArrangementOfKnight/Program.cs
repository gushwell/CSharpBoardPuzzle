using System;

namespace ArrangementOfKnights {
    class Program {
        static void Main(string[] args) {
            var board = new Chessboard();
            var solver = new Solver();
            var answer = solver.Solve(board);
            answer.Print();
        }
    }
}
