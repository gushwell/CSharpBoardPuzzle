using System;

namespace NQueenPuzzle {
    class Program {
        static void Main(string[] args) {
            var solver = new Solver();
            var bs = solver.Solve(5);
            var count = 0;
            foreach (var b in bs) {
                b.Print();
                count++;
            }
            Console.WriteLine(count);
        }
    }
}
