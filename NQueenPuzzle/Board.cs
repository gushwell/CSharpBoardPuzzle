using System;
using Gushwell.Puzzle;
using System.Collections.Generic;
using System.Linq;

namespace NQueenPuzzle {
    class Piece {
        public static readonly Piece Empty = new Piece { Value = '.' };
        public static readonly Piece Queen = new Piece { Value = 'Q' };
        public char Value { get; set; }

        public override string ToString() {
            return Value.ToString();
        }
    }

    class Board : BoardBase<Piece> {

        public int Width { get; private set; }

        public Board(int width) : base(width, width) {

            this.Width = width;
            foreach (var p in GetAllIndexes())
                this[p] = Piece.Empty;
        }

        public Board(Board board) : base(board) {
            this.Width = board.Width;
        }

        public void Put(int place) {
            this[place] = Piece.Queen;
        }

        public void Clear(int place) {
            this[place] = Piece.Empty;
        }


        public bool CanPut(int place) {
            foreach (int x in Settled()) {
                foreach (int p in Courses(x)) {
                    if (place == p)
                        return false;
                }
            }
            return true;
        }

        public IEnumerable<int> Vacants(int y) {
            return Horizontal(1, y).Where(p => this[p] == Piece.Empty);
        }

        private IEnumerable<int> Settled() {
            return GetAllIndexes().Where(p => this[p] == Piece.Queen);
        }

        private IEnumerable<int> Courses(int now) {
            var (x, y) = ToLocation(now);
            return Virtical(x, y)
                .Concat(Horizontal(x, y))
                .Concat(SlantL(x, y))
                .Concat(SlantR(x, y)).Distinct();
        }

        public void Print() {
            int i = 0;
            foreach (var p in GetAllIndexes()) {
                var c = this[p].Value;
                Console.Write($"{c} ");
                if (++i == Width) {
                    Console.WriteLine();
                    i = 0;
                }
            }
            Console.WriteLine();
        }
    }
}
