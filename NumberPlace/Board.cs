using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NumberPlace {

    class Board {
        private int[,] sheet;

        // コンストラクタ
        public Board(int[,] mat) {
            this.sheet = mat;
        }

        // コンストラクタ
        private Board() {
            sheet = new int[9, 9];
        }

        // クローン
        public Board Clone() {
            var nboard = new Board();
            nboard.sheet = (int[,])this.sheet.Clone();
            return nboard;
        }

        // チェックはしない CanPutを呼び出しているのが前提
        public void Put(Location pos, int num) {
            sheet[pos.X, pos.Y] = num;
        }

        // 置けるか
        public bool CanPut(Location pos, int num) {
            if (sheet[pos.X, pos.Y] != 0)
                return false;
            // 仮に置いてみる。
            sheet[pos.X, pos.Y] = num;
            try {
                return IsValid(pos);
            } finally {
                // 元に戻す
                sheet[pos.X, pos.Y] = 0;
            }
        }

        // 完成か
        public bool IsCompleted() {
            if (!Enumerable.Range(0, 9).All(nums => IsCompleted(VerticalNums(nums))))
                return false;
            if (!Enumerable.Range(0, 9).All(nums => IsCompleted(HorizontalNums(nums))))
                return false;
            return BoxLocations().All(pos => IsCompleted(BoxNums(pos)));
        }

        // 完成したか
        public bool IsCompleted(IEnumerable<int> nums) {
            return nums.Where(n => n >= 1).Distinct().Count() == 9;
        }

        // 空いているか
        public bool IsVacant(Location pos) {
            return sheet[pos.X, pos.Y] == 0;
        }

        // posの位置に関する配置は違反していないか
        public bool IsValid(Location pos) {
            return IsValid(VerticalNums(pos.X)) &&
                   IsValid(HorizontalNums(pos.Y)) &&
                   IsValid(BoxNums(pos));
        }

        // numsに重複はないか （つまり違反していないか）IsCompletedとは別。
        public bool IsValid(IEnumerable<int> nums) {
            return nums.Where(n => n >= 1).Distinct().Count() == nums.Where(n => n >= 1).Count();
        }

        // ９つある小さな四角形の左上のLocationを列挙
        public IEnumerable<Location> BoxLocations() {
            return from x in Enumerable.Range(0, 2)
                   from y in Enumerable.Range(0, 2)
                   select new Location(x * 3, y * 3);
        }

        // 注目している位置が含まれる3*3の領域の数を列挙
        public IEnumerable<int> BoxNums(Location pos) {
            return from x in Enumerable.Range(0, 3)
                   from y in Enumerable.Range(0, 3)
                   select sheet[pos.X / 3 * 3 + x, pos.Y / 3 * 3 + y];
        }

        // 注目している位置が含まれる縦一列の数を列挙
        public IEnumerable<int> VerticalNums(int x) {
            for (int y = 0; y < 9; y++) {
                yield return sheet[x, y];
            }
        }

        // 注目している位置が含まれる横一列の数を列挙
        public IEnumerable<int> HorizontalNums(int y) {
            for (int x = 0; x < 9; x++) {
                yield return sheet[x, y];
            }
        }

        // すべての位置を列挙する
        public IEnumerable<Location> AlloLocations() {
            return from x in Enumerable.Range(0, 9)
                   from y in Enumerable.Range(0, 9)
                   select new Location(x, y);
        }

        internal void Print() {
            for (int y = 0; y < 9; y++) {
                for (int x = 0; x < 9; x += 3) {
                    Console.Write(" {0} {1} {2}", sheet[x, y], sheet[x + 1, y], sheet[x + 2, y]);
                    if (x == 0 || x == 3)
                        Console.Write("|");
                }
                Console.WriteLine();
                if (y == 2 || y == 5)
                    Console.WriteLine("------+------+------");
            }
            Console.WriteLine();
        }
    }
}