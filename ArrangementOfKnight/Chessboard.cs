using Puzzle;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

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
            _allDestinations = new int[] {
                ToDirection(-1, -2),
                ToDirection(+1, -2),
                ToDirection(-2, -1),
                ToDirection(+2, -1),
                ToDirection(-1, +2),
                ToDirection(+1, +2),
                ToDirection(-2, +1),
                ToDirection(+2, +1),
            };
        }

        // コピーコンストラクタ
        public Chessboard(Chessboard board) : base(board) {
            this._allIndexes = board._allIndexes.ToArray();
            this._allDestinations = board._allDestinations.ToArray();
            // ここで、深いコピーをしてしまうが、Empty, Knightは唯一一つのオブジェクトにしたい。
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
        private static int[] _blockA = new int[] { 28, 29, 40, 41, 50, 51, 52, 53, 62, 63, 64, 65, 26, 27, 38, 39, };
        private static int[] _blockB = new int[] { 30, 31, 42, 43, 54, 55, 56, 57, 66, 67, 68, 69, 32, 33, 44, 45, };
        private static int[] _blockC = new int[] { 74, 75, 76, 77, 86, 87, 88, 89, 100, 101, 112, 113, 98, 99, 110, 111, };
        private static int[] _blockD = new int[] { 78, 79, 80, 81, 90, 91, 92, 93, 102, 103, 114, 115, 104, 105, 116, 117, };

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
