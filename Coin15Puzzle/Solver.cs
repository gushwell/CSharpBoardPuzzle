using System;
using System.Collections.Generic;
using System.Linq;

namespace Coin15Puzzle {
    public class Solver {
        private Board board;

        public Solver() {
            this.board = new Board(6);
        }

        public IEnumerable<Board> Solve() {
            foreach (var b in _Solve(1))
                yield return b;
        }

        public IEnumerable<Board> _Solve(int y) {
            if (y > board.YSize && board.IsFin()) {
                yield return new Board(board);
                yield break;
            }

            // yで示す横一列のインデックスの一覧を得る。
            // 鏡像の一部を除く (すべてを取り除くには別の手法が必要？）
            var places = y == 1
                            ? board.Horizontal(1, y).Take(board.XSize / 2).ToArray()
                            : board.Horizontal(1, y).ToArray();
            foreach (var i in places) {
                // 10を試行
                if (board.CanPut(i, Coin.C10)) {
                    board[i] = Coin.C10;
                    // 10を試行
                    foreach (var j in board.Horizontal(1, y)) {
                        if (board.CanPut(j, Coin.C5)) {
                            board[j] = Coin.C5;
                            foreach (var b in _Solve(y + 1))
                                yield return b;
                            board[j] = Coin.Zero;
                        }
                    }
                    board[i] = Coin.Zero;
                }
            }
        }
    }
}
