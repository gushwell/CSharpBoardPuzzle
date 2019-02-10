using System;
using System.IO;
using System.Linq;

namespace Baikago {
    class Program {
        private static Board board;
        static void Main(string[] args) {
            board = new Board(15);
            string[] lines = File.ReadAllLines("board.txt");
            board.Initialize(lines);
            var solver = new Solver(board);
            var (stone, pattern) = solver.WhichWon();
            if (stone == null) {
                Console.WriteLine("見つかりません。");
                return;
            } else if (stone == Stone.White) {
                Console.WriteLine("White");
            } else {
                Console.WriteLine("Black");
            }
            Print(pattern);
        }

        public static void Print(int[] winPattern) {
            var currColor = Console.ForegroundColor;
            for (int y = 1; y <= board.YSize; y++) {
                for (int x = 1; x <= board.XSize; x++) {
                    if (winPattern.Contains(board.ToIndex(x, y))) {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.Write($"{board[x, y].Value} ");
                        Console.ForegroundColor = ConsoleColor.White;
                    } else {
                        Console.ForegroundColor = currColor;
                        Console.Write($"{board[x, y].Value} ");
                    }
                }
                Console.WriteLine();
            }
            Console.WriteLine();
        }

    }
}
