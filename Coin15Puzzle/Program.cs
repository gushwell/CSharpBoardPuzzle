using System;
using System.Linq;

namespace Coin15Puzzle {
    class Program {
        static void Main(string[] args) {
            var solver = new Solver();
            foreach (var x in solver.Solve()) {
                x.Print();
            }
        }
    }
}
