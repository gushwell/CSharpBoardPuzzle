using System;
using System.Collections.Generic;
using System.Linq;
using Puzzle;

namespace LessThanThreePuzzle {
    public class Stone {
        public static readonly Stone Black = new Stone { Value = 'X' };
        public static readonly Stone White = new Stone { Value = 'O' };
        public static readonly Stone Empty = new Stone { Value = '.' };
        public char Value { get; set; }
    }


    public class Board : BoardBase<Stone> {
        private int Up;
        private int Down;
        private int Left;
        private int Right;
        private int UpperLeft;
        private int UpperRight;
        private int BottomLeft;
        private int BottomRight;

        private int UpperLeftLeft;
        private int UpperRightRight;
        private int BottomLeftLeft;
        private int BottomRightRight;

        private int UpperUpperLeft;
        private int UpperUpperRight;
        private int BottomBottomLeft;
        private int BottomBottomRight;


        public Board() : base(6, 6) {
            foreach (var index in GetAllIndexes()) {
                this[index] = Stone.Empty;
            }
            // 計算回数を少なくするために、事前に求めておく。
            Up = this.ToDirection(0, -1);
            Down = this.ToDirection(0, 1);
            Left = this.ToDirection(-1, 0);
            Right = this.ToDirection(1, 0);
            UpperLeft = this.ToDirection(-1, -1);
            UpperRight = this.ToDirection(1, -1);
            BottomLeft = this.ToDirection(-1, 1);
            BottomRight = this.ToDirection(1, 1);

            UpperLeftLeft = this.ToDirection(-2, -1);
            UpperRightRight = this.ToDirection(2, -1);
            BottomLeftLeft = this.ToDirection(-2, 1);
            BottomRightRight = this.ToDirection(2, 1);

            UpperUpperLeft = this.ToDirection(-1, -2);
            UpperUpperRight = this.ToDirection(1, -2);
            BottomBottomLeft = this.ToDirection(-1, 2);
            BottomBottomRight = this.ToDirection(1, 2);
        }

        // x,y 座標にpieceを置いたが、条件を満たしているか
        internal bool IsCorrect(int x, int y, Stone piece) {
            return EightLines(x, y)
                       .All(line => line.Count(p => this[p] == piece) <= 2);
        }


        // indexで指定した位置を通る８本の線を列挙する
        public IEnumerable<int[]> EightLines(int x, int y) {
            // 縦方向 （この配列要素順は問わない、以下も同様）
            yield return this.EnumerateIndexes(x, y, Up).Skip(1)
                       .Concat(EnumerateIndexes(x, y, Down))
                       .ToArray();

            // 横方向
            yield return this.EnumerateIndexes(x, y, Left).Skip(1)
                       .Concat(EnumerateIndexes(x, y, Right))
                       .ToArray();

            // 右斜め４５％  （＼）
            yield return this.EnumerateIndexes(x, y, UpperLeft).Skip(1)
                       .Concat(EnumerateIndexes(x, y, BottomRight))
                       .ToArray();

            // 左斜め４５％ （／）
            yield return this.EnumerateIndexes(x, y, UpperRight).Skip(1)
                       .Concat(EnumerateIndexes(x, y, BottomLeft))
                       .ToArray();

            // 下右右  （傾斜が緩い 右斜め）
            yield return this.EnumerateIndexes(x, y, UpperLeftLeft).Skip(1)
                       .Concat(EnumerateIndexes(x, y, BottomRightRight))
                       .ToArray();

            // 下左左  （傾斜が緩い 左斜め）
            yield return this.EnumerateIndexes(x, y, UpperRightRight).Skip(1)
                       .Concat(EnumerateIndexes(x, y, BottomLeftLeft))
                       .ToArray();

            // 下下右  （傾斜がきつい 右斜め）
            yield return this.EnumerateIndexes(x, y, UpperUpperLeft).Skip(1)
                       .Concat(EnumerateIndexes(x, y, BottomBottomRight))
                       .ToArray();

            // 下下左 （傾斜がきつい 左斜め）
            yield return this.EnumerateIndexes(x, y, UpperUpperRight).Skip(1)
                       .Concat(EnumerateIndexes(x, y, BottomBottomLeft))
                       .ToArray();
        }
    }
}