using System;
using System.Collections.Generic;

namespace GoishiHiroi {
    class Program {
        static void Main(string[] args) {
            char[,] data = Initialize();
            var answer = Solve(data);
            if (answer == null)
                Console.WriteLine("解は見つかりませんでした");
            else
                Print(data, answer);
        }

        // 初期状態設定
        private static char[,] Initialize() {
            string[] lines = new string[] {
                "             ",
                "  X X X X X  ",
                "   X X X X   ",
                "    X X X    ",
                "     X X     ",
                "  X  X X  X  ",
                "     X X     ",
                "    X X X    ",
                "   X X X X   ",
                "  X X X X X  ",
                "             ",
                "             ",
            };
            int w = lines[0].Length;
            int h = lines.Length;
            char[,] data = new char[w, h];
            for (var y = 0; y < h; y++) {
                for (int x = 0; x < w; x++) {
                    data[x, y] = lines[y][x];
                }
            }
            return data;
        }

        // 開始位置はすべての石が対象。ただし、解が見つかったところで探索はやめる。
        private static List<int> Solve(char[,] data) {
            var solver = new Solver(data);
            foreach (var p in solver.StoneIndexes()) {
                if (solver.Solve(p))
                    return solver.Moves;
            }
            return null;
        }

        private static void Print(char[,] data, IEnumerable<int> ans) {
            Console.Clear();
            var board = new Board(data);
            board.Print();
            foreach (var p in ans) {
                Console.SetCursorPosition(0, data.GetLength(1) + 1);
                Console.Write("Enterキーで進みます");
                Console.ReadLine();
                var (x, y) = board.ToLocation(p);
                // SetCursorPositionの座標は0から始まるので１引く
                Console.SetCursorPosition(--x * 2, --y);
                Console.Write('-');
            }
        }
    }
}
