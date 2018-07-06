using System;
using Puzzle;
using System.Linq;
using System.Collections.Generic;

namespace ArrangementOfKnights {
    public class Piece {
        public static readonly Piece Empty = new Piece { Value = '.' };
        public static readonly Piece Knight = new Piece { Value = 'K' };
        public char Value { get; set; }

        public override string ToString() {
            return Value.ToString();
        }

        public virtual Piece Clone() {
            return new Piece {
                Value = this.Value
            };
        }
    }

    public class Footmark : Piece {
        public int Count { get; set; }

        public Footmark() {
            Value = 'F';
        }

        public override Piece Clone() {
            return new Footmark {
                Count = this.Count,
                Value = this.Value
            };
        }
    }


    public class Chessboard : BoardBase<Piece> {
        // 移動可能な８つの方向
        private int[] _allDestinations;
        // 番兵部分を除いたすべてのインデックス(少しでも高速化するためキャッシュしておく)
        private int[] _allIndexes;

        // コンストラクタ
        public Chessboard()
            : base(8, 8) {
            _allIndexes = GetAllIndexes().ToArray();
            foreach (var p in _allIndexes)
                this[p] = Piece.Empty;
            int width2 = 8 + 2;
            _allDestinations = new int[] { -(width2 + 2), +(width2 - 2), -(width2 * 2 + 1), (width2 * 2 - 1),
                                 -(width2 * 2 - 1), (width2 * 2 + 1), -(width2 - 2), +(width2 + 2), };
        }

        // コピーコンストラクタ
        public Chessboard(Chessboard board) : base(board) {
            this._allIndexes = board._allIndexes.ToArray();
            this._allDestinations = board._allDestinations.ToArray();
            // ここで、深いコピーをしているが、Empty, Knightは唯一一つのオブジェクトにしたい。
            foreach (var ix in board.GetAllIndexes()) {
                if (this[ix] is Footmark)
                    this[ix] = board[ix].Clone();
            }
        }

        // 指定Blockは正しい状態か。他のブロックからは届かない隅の４つが埋まっていないといけない。
        public bool IsCorrect(int[] block) {
            if (block.Skip(12).All(ix => this[ix] != Piece.Empty))
                return true;
            return false;
        }

        // 騎士の数がオーバーしたか
        public bool IsOver(int[] block) {
            if (block.Count(ix => this[ix] == Piece.Knight) > 4)
                return true;
            return false;
        }

        // 正解にたどり着いたか
        public bool IsFinish() {
            return _allIndexes.All(index => this[index] != Piece.Empty);
        }

        // now位置からの移動可能な場所(index)を返す
        public IEnumerable<int> Destinations(int now) {
            return _allDestinations.Select(n => n + now).Where(n => IsOnBoard(n));
        }

        // 利き筋には、Footmarkを配置する
        public void Put(int place) {
            this[place] = Piece.Knight;
            foreach (var dest in Destinations(place)) {
                var fm = this[dest] as Footmark;
                if (fm == null)
                    this[dest] = new Footmark() { Count = 1 };
                else
                    fm.Count++;
            }
        }

        // 駒(騎士)を取り去る
        public int Clear(int place) {
            int count = 0;
            this[place] = Piece.Empty;
            foreach (var dest in Destinations(place)) {
                var fm = this[dest] as Footmark; // fm が null なら PGミス
                if (fm.Count <= 1) {
                    count++;
                    this[dest] = Piece.Empty;
                } else
                    fm.Count--;
            }
            return count;
        }

        // 騎士が移動可能な場所の数
        public int MovableCount(int place) {
            return Destinations(place).Count(ix => this[ix] == Piece.Empty);
        }

        // ４つのブロックの座標(インデックス）を設定。かなり力業だが...
        // 4隅の座標は、各配列の最後に格納する。
        private static int[] _blockA = new int[] { 31, 32, 33, 34, 41, 42, 43, 44, 13, 14, 23, 24, 11, 12, 21, 22, };
        private static int[] _blockB = new int[] { 35, 36, 37, 38, 45, 46, 47, 48, 15, 16, 25, 26, 17, 18, 27, 28, };
        private static int[] _blockC = new int[] { 51, 52, 53, 54, 61, 62, 63, 64, 73, 74, 83, 84, 71, 72, 81, 82, };
        private static int[] _blockD = new int[] { 55, 56, 57, 58, 65, 66, 67, 68, 75, 76, 85, 86, 77, 78, 87, 88, };

        public int[] BlockA() {
            return _blockA;
        }

        public int[] BlockB() {
            return _blockB;
        }

        public int[] BlockC() {
            return _blockC;
        }

        public int[] BlockD() {
            return _blockD;
        }

        public int[] NextBlock(int[] block) {
            if (block == BlockA())
                return BlockB();
            if (block == BlockB())
                return BlockC();
            if (block == BlockC())
                return BlockD();
            return null;
        }

        public void Print() {
            for (int y = 1; y <= YSize; y++) {
                for (int x = 1; x <= XSize; x++) {
                    Footmark fm = this[x, y] as Footmark;
                    if (fm != null)
                        Console.Write("{0} ", fm.Count);
                    else
                        Console.Write("{0} ", this[x, y].Value);
                }
                Console.WriteLine("");
            }
            Console.WriteLine();
        }
    }
}
