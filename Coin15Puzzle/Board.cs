using System;
using System.Collections.Generic;
using System.Linq;
using Gushwell.Puzzle;

namespace Coin15Puzzle {
    // 　盤面に置くコインクラス
    public class Coin {
        public int Number { get; set; }

        public static readonly Coin C10 = new Coin { Number = 10 };
        public static readonly Coin C5 = new Coin { Number = 5 };
        public static readonly Coin Zero = new Coin { Number = 0 };

        public static implicit operator int(Coin coin) {
            return coin.Number;
        }
    }

    public class Board : BoardBase<Coin> {
        public Board(int size) : base(size, size) {
        }

        public Board(Board board) : base(board) {
        }

        // 縦、横、対角線のすべてのLineを所得する
        public IEnumerable<int[]> GetAllLines() {
            for (int y = 1; y <= this.YSize; y++) {
                yield return Horizontal(1, y).ToArray();
            }
            for (int x = 1; x <= this.YSize; x++) {
                yield return Virtical(x, 1).ToArray();
            }
            yield return SlantR(1, 1).ToArray();
            yield return SlantL(XSize, 1).ToArray();
        }

        // coin を置けるか
        internal bool CanPut(int index, int coin) {
            if (this[index].Number > 0)
                return false;
            foreach (var line in GetLines(index).ToArray()) {
                var coins = line.Where(n => this[n].Number > 0).Select(n => this[n]).ToList();
                int count = coins.Count;
                if (count > 1)
                    return false;
                int sum = coins.Sum(x => x.Number);
                if (sum + coin > 15)
                    return false;
            }
            return true;
        }

        // position を通る、縦、横、対角線のLineを列挙する　（対角線は通らない場合もある）
        public IEnumerable<IEnumerable<int>> GetLines(int position) {
            var (x, y) = ToLocation(position);
            yield return Horizontal(1, y);
            yield return Virtical(x, 1);
            if (x == y)
                yield return SlantR(1, 1);
            else if (x + y == XSize + 1)
                yield return SlantL(XSize, 1);
        }

        // 終了か　（条件を満たしているか）
        public bool IsFin() {
            foreach (var line in GetAllLines()) {
                if (line.Sum(x => this[x].Number) != 15)
                    return false;
            }
            return true;
        }

        // 盤の状態をプリント
        public void Print() {
            Console.WriteLine("|--|--|--|--|--|--|");
            for (int y = 1; y <= YSize; y++) {
                Console.Write("|");
                for (int x = 1; x <= XSize; x++) {
                    int p = this[x, y].Number;
                    Console.Write("{0}|", p == 0 ? "  " : p == 10 ? "10" : " 5");
                }
                Console.WriteLine("\n|--|--|--|--|--|--|");

            }
            Console.WriteLine();
        }

    }
}
