using System;
using System.Collections.Generic;
using System.Linq;
using Puzzle;

namespace HyperQueenPuzzle {
    public class Piece {
        public static readonly Piece Empty = new Piece { Value = '.' };
        public static readonly Piece Queen = new Piece { Value = 'Q' };
        public char Value { get; set; }

        public override string ToString() {
            return Value.ToString();
        }
    }

    class Board : BoardBase<Piece> {
        private int _width;

        public Board(int width) : base(width, width) {
            _width = width;
            foreach (var p in GetAllIndexes())
                this[p] = Piece.Empty;
        }

        // コピーコンストラクタ
        public Board(Board board) : base(board) {

        }

        // 駒を置く
        public void Put(int place) {
            this[place] = Piece.Queen;
        }

        // 指定した位置から駒を取り去る
        public void Clear(int place) {
            this[place] = Piece.Empty;
        }

        // 引数で与えた位置に駒を置けるか
        public bool CanPut(int place) {
            if (this[place] != Piece.Empty)
                return false;
            if (Courses(place).Any(p => this[p] == Piece.Queen))
                return false;
            return true;
        }

        // 与えられた位置の右、下、ななめ右下、ななめ左下の位置を
        // それぞれぐるっと一回転するまで列挙する。
        // ただし、与えられた位置は除く。
        public IEnumerable<int> Courses(int index) {
            var (x, y) = ToLocation(index);
            return Virtical(x, 1)
                .Concat(Horizontal(1, y))
                .Concat(Oblique(x, y))
                .Concat(BackOblique(x, y)).Where(p => p != index).Distinct();
        }

        // 左下がりの斜線の各位置を列挙する
        public IEnumerable<int> Oblique(int x, int y) {
            for (int i = 0; i < _width; i++) {
                x += 1;
                y -= 1;
                if (x > _width)
                    x = 1;
                if (y == 0)
                    y = _width;
                yield return ToIndex(x, y);
            }
        }

        // 右下がりの斜線の各位置を列挙する
        public IEnumerable<int> BackOblique(int x, int y) {
            for (int i = 0; i < _width; i++) {
                x += 1;
                y += 1;
                if (x > _width)
                    x = 1;
                if (y > _width)
                    y = 1;
                yield return ToIndex(x, y);
            }
        }

    }
}
