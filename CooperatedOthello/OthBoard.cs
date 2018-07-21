using Puzzle;
using System;
using System.Collections.Generic;
using System.Linq;


// Boardから継承し、特有の機能を追加

namespace CooperatedOthello {
    public class Stone {
        public static readonly Stone Black = new Stone { Value = 'X' };
        public static readonly Stone White = new Stone { Value = 'O' };
        public static readonly Stone Empty = new Stone { Value = '.' };

        public char Value { get; set; }
    }

    public class Move {
        public int Place { get; set; }
        public Stone Stone { get; set; }

        public Move(Stone stone, int place) {
            Stone = stone;
            Place = place;
        }
    }

    public class OthBoard : BoardBase<Stone> {
        // 現在の手番の事。
        public Stone Turn { get; set; }

        // これまでの棋譜
        private List<Move> Moves = new List<Move>();

        // コンストラクタ
        public OthBoard()
            : base(8, 8) {
            Turn = Stone.Black;
            Initialize();
        }

        // コンストラクタ （Cloneと同じ用途)
        public OthBoard(OthBoard board)
            : base(board) {
            Turn = board.Turn;
            Moves = board.Moves.ToList();
        }

        // 初期化
        public void Initialize() {
            foreach (var p in GetAllIndexes())
                this[p] = Stone.Empty;
            this[ToIndex(4, 4)] = Stone.White;
            this[ToIndex(5, 5)] = Stone.White;
            this[ToIndex(4, 5)] = Stone.Black;
            this[ToIndex(5, 4)] = Stone.Black;
        }

        // 相手の石
        public Stone Opponent(Stone stone) {
            return stone == Stone.White ? Stone.Black : Stone.White;
        }

        // 石の数をカウント
        public int StoneCount(Stone stone) {
            return this.GetAllIndexes().Count(p => this[p] == stone);
        }

        // ８つの方向を列挙 （このBoardは、番兵用に一回り大きなサイズとなっている）
        public IEnumerable<int> Directions() {
            //return new int[] { -OuterWidth-1, -OuterWidth, -OuterWidth+1, 1, OuterWidth + 1, OuterWidth, OuterWidth-1, -1 };
            return new int[] {
                ToDirection(-1, -1),
                ToDirection( 0, -1),
                ToDirection(+1, -1),
                ToDirection(+1,  0),
                ToDirection(+1, +1),
                ToDirection( 0, +1),
                ToDirection(-1, +1),
                ToDirection(-1,  0),
            };
        }

        // 置ける場所を列挙する
        public IEnumerable<int> PutablePlaces(Stone stone) {
            return this.GetVacantIndexes().Where(index => CanPut(stone, index));
        }

        private IEnumerable<int> GetVacantIndexes() {
            return this.GetAllIndexes().Where(p => this[p] == Stone.Empty);
        }

        // 石を置けるか
        public bool CanPut(Stone stone, int place) {
            if (this[place] != Stone.Empty)
                return false;
            return Directions().Any(d => CanReverse(stone, place, d));
        }

        // direction方向の石をひっくり返せるか
        public bool CanReverse(Stone stone, int place, int direction) {
            Stone opponent = Opponent(stone);
            int np = place + direction;
            while (this[np] == opponent)
                np += direction;
            return (this[np] == stone && np != place + direction);
        }

        // direction方向にstoneと同じ色の石があるか
        public bool FindStone(Stone stone, int place, int direction) {
            Stone opponent = Opponent(stone);
            int np = place + direction;
            while (this[np] == opponent)
                np += direction;
            return (this[np] == stone);
        }

        // direction方向の石をひっくり返えす 
        public void Reverse(Stone stone, int place, int direction) {
            if (!FindStone(stone, place, direction))
                return;
            Stone opponent = Opponent(stone);
            int np = (int)(place + direction);
            while (this[np] == opponent) {
                this[np] = stone;
                np += direction;
            }
        }

        // stoneをplace位置に置く (必ず置けることを前提としている）
        public void Put(Stone stone, int place) {
            this[place] = stone;
            Stone opponent = Opponent(stone);
            foreach (int direction in Directions()) {
                Reverse(stone, place, direction);
            }
            Moves.Add(new Move(stone, place));
            // 通常は相手の番になるが、相手が置ける場所が無い場合は、連続して打てる
            if (PutablePlaces(Turn).Any())
                Turn = opponent;
            else
                Turn = stone;
        }

        // 現時点での手順を返す。
        public IEnumerable<Move> GetMoves() {
            return Moves;
        }

        public void Print() {
            Console.Clear();
            for (int y = 1; y <= YSize; y++) {
                for (int x = 1; x <= XSize; x++) {
                    Console.Write("{0} ", this[x, y].Value);
                }
                Console.WriteLine("");
            }
            Console.WriteLine();
        }
    }

}
