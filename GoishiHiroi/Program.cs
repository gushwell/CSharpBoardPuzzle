using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;

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
            string[] lines = File.ReadAllLines("data.txt")
                                 .Select(x => x.Trim('"'))
                                 .ToArray();
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
                Console.Write('=');
            }
            Console.SetCursorPosition(0, data.GetLength(1) + 1);
        }
    }
}
