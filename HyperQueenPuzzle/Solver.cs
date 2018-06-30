using System;
using System.Collections.Generic;

namespace HyperQueenPuzzle {
    class Solver {

        public IEnumerable<Board> Solve(Board board) {
            return SolveInner(board, 1);
        }

        // y行よりも下に置く駒を求める － 鏡像、回転を除くことは考慮していない
        private IEnumerable<Board> SolveInner(Board board, int y) {
            if (y > board.YSize) {
                yield return new Board(board);
                yield break;
            }
            foreach (int pos in board.Horizontal(1, y)) {
                if (board.CanPut(pos)) {
                    board.Put(pos);
                    foreach (var b in SolveInner(board, y + 1))
                        yield return b;
                    board.Clear(pos);
                }
            }
        }
    }
}
