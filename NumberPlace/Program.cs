using System;
using System.IO;

namespace NumberPlace {
    class Program {
        static void Main(string[] args) {
            var nums = ReadData(@"data.txt");
            var sol = new Solver();
            var answer = sol.Solve(nums);
            foreach (var board in answer)
                board.Print();
        }

        // データを読み込む
        static int[,] ReadData(string path) {
            int[,] nums = new int[9, 9];
            var lines = File.ReadAllLines(path);
            for (int y = 0; y < 9; y++) {
                int x = 0;
                foreach (var n in lines[y].Split(',')) {
                    nums[x++, y] = int.Parse(n);
                }
            }
            return nums;
        }
    }
}
