using Puzzle;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Baikago {

    public class Stone {
        public static readonly Stone Black = new Stone { Value = 'X' };
        public static readonly Stone White = new Stone { Value = 'O' };
        public static readonly Stone Empty = new Stone { Value = '.' };

        public char Value { get; set; }
    }

    public class Board : BoardBase<Stone> {
        public Board(int size) : base(size, size) {
            foreach (var index in GetAllIndexes()) {
                this[index] = Stone.Empty;
            }
        }

        // 初期状態の表示
        public void Initialize(string[] lines) {

            int y = 1;
            foreach (var line in lines) {
                int x = 1;
                foreach (var c in line) {
                    if (c == 'X')
                        this[x, y] = Stone.Black;
                    else if (c == 'O')
                        this[x, y] = Stone.White;
                    x++;
                }
                y++;
            }
        }
    }
}
