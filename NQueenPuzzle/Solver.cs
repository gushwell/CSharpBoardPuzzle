using System;
using System.Collections.Generic;

namespace NQueenPuzzle {
    class Solver {
        private int count = 0;

        public IEnumerable<Board> Solve(int n) {
            var board = new Board(n);
            return SolveInner(board, 1);
        }

        private IEnumerable<Board> SolveInner(Board board, int y) {
            if (y > board.Width) {
                count++;
                yield return new Board(board);
            }
            foreach (int pos in board.Vacants(y)) {
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
