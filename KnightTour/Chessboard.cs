﻿using System;
using System.Collections.Generic;
using System.Linq;
using Gushwell.Puzzle;

namespace KnightTour {
    public class Piece {
        public static readonly Piece Empty = new Piece { Value = '.' };
        public static readonly Piece Knight = new Piece { Value = 'K' };
        public char Value { get; set; }

        public override string ToString() {
            return Value.ToString();
        }
    }

    // ナイトの足跡 Numberは、何歩目なのかを示す。
    public class Footmark : Piece {
        public int Number { get; set; }
        public Footmark(int num) {
            Number = num;
        }
    }

    // チェスボードを表すクラス
    // 描画は担当しない。
    public class Chessboard : BoardBase<Piece> {
        private int _count = 0;
        //private int _startPlace;
        private int _nowPlace;

        public Chessboard() : base(8, 8) {
            base.ClearAll();
            foreach (var p in this.GetAllIndexes())
                this[p] = Piece.Empty;
        }

        // EmptyPiece 以外の全てのPieceを列挙する

        public IEnumerable<Piece> GetAllPieces() {
            return GetAllIndexes().Select(i => this[i]).Where(p => p != Piece.Empty);

        }

        // 移動させる
        public void Jump(int place) {
            this[place] = new Footmark(++_count);

            _nowPlace = place;
        }

        // place位置をクリア (開始位置はクリアできない)
        public void Clear(int place) {
            _count = (this[place] as Footmark).Number - 1;
            this[place] = Piece.Empty;
            _nowPlace = this.GetAllPieces()
                            .OfType<Footmark>()
                            .Max(fm => fm.Number);
        }

        // 巡回したか
        public bool IsFin() {
            return this.GetAllIndexes().All(n => this[n] is Footmark);
        }

        // Start位置に戻ってこられる状態かを調べる。
        public bool CanBackHome(int nowPlace) {
            return Destinations(nowPlace).Any(n => {
                var piece = this[n] as Footmark;
                if (piece != null) {
                    if (piece.Number == 1)
                        return true;
                }
                return false;
            });
        }

        // placeから実際に移動できる位置を列挙する
        public IEnumerable<int> CanPutPlaces(int place) {
            return Destinations(place).Where(n => this[n] == Piece.Empty).ToArray();
        }

        // Knightの移動候補位置を列挙する （移動先にKnightがあるかどうかは考慮しない)
        public IEnumerable<int> Destinations(int place) {
            int width = this.XSize + 2;
            int[] candidates = { -(width + 2), +(width - 2), -(width * 2 + 1), (width * 2 - 1),
                                 -(width * 2 - 1), (width * 2 + 1), -(width - 2), +(width + 2), };
            IEnumerable<int> q;
            if (place % width == 0)
                q = candidates.Skip(4);
            else if (place % width == 1)
                q = candidates.Skip(2);
            else if (place % width == width - 1)
                q = candidates.Take(4);
            else if (place % width == width - 2)
                q = candidates.Take(6);
            else
                q = candidates;
            return q.Select(n => n + place).Where(n => 0 <= n && n < width * width);
        }

        public void Print() {
            for (int y = 1; y <= YSize; y++) {
                for (int x = 1; x <= XSize; x++) {
                    Footmark fm = this[x, y] as Footmark;
                    if (fm != null)
                        Console.Write("{0,2} ", fm.Number);
                    else
                        Console.Write("{0}  ", this[x, y] == Piece.Knight ? "K" : " ");
                }
                Console.WriteLine("");
            }
            Console.WriteLine();
        }
    }
}
