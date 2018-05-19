using System;
using System.Linq;
using Gushwell.Puzzle;

namespace GoishiHiroi {
    class Stone {
        public static readonly Stone Empty = new Stone { Value = '.' };
        public static readonly Stone White = new Stone { Value = 'O' };
        public char Value { get; set; }
    }

    class Board : BoardBase<Stone> {
        // コンストラクタ
        public Board(char[,] data) : base(data.GetLength(0), data.GetLength(1)) {
            for (int x = 0; x < data.GetLength(0); x++)
                for (int y = 0; y < data.GetLength(1); y++)
                    this[x + 1, y + 1] = data[x, y] == ' ' ? Stone.Empty : Stone.White;
        }

        // コピーコンストラクタ
        public Board(Board board) : base(board) {
        }

        // すべてを取り除けたか
        internal bool IsFin() {
            return GetAllIndexes().All(p => this[p] == Stone.Empty);
        }

        // directionの方向へ移動
        public int Go(int p, int direction) {
            while (this[p] != null) {
                if (this[p] == Stone.White)
                    return p;
                p += direction;
            }
            return -1;
        }

        public int Left {
            get { return -1; }
        }
        public int Right {
            get { return 1; }
        }
        public int Up {
            get { return -this.XSize - 2; }
        }
        public int Down {
            get { return this.XSize + 2; }
        }

        public void Print() {
            for (int y = 1; y <= this.YSize; y++) {
                for (int x = 1; x <= this.XSize; x++) {
                    var p = this[x, y];
                    Console.Write(p.Value + " ");
                }
                Console.WriteLine();
            }
            Console.WriteLine();
        }
    }
}
