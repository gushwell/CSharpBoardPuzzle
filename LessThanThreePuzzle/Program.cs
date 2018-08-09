using System;

namespace LessThanThreePuzzle {
    class Program {
        static void Main() {
            Solver sol = new Solver();
            sol.Subscribe(new ResultViewer());
            sol.Solve();
        }
    }
}
