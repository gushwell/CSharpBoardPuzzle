using System;
using System.Collections.Generic;
using System.Linq;

namespace GoishiHiroi {
    class Solver {
        private Board _board;

        public List<int> Moves { get; set; } = new List<int>();

        public Solver(char[,] data) {
            _board = new Board(data);
        }

        public bool Solve(int start) {
            return SolveInner(start, 0);
        }

        private bool SolveInner(int p, int prevDir) {
            if (_board.IsFin()) {
                return true;
            }
            foreach (var dir in Directions(prevDir)) {
                var np = _board.Go(p, dir);
                if (np < 0)
                    continue;
                _board[np] = Stone.Empty;
                Moves.Add(np);
                if (SolveInner(np, dir))
                    return true;
                _board[np] = Stone.Black;
                Moves.Remove(np);
            }
            return false;
        }

        private IEnumerable<int> Directions(int prevDir) {
            if (prevDir != _board.Left)
                yield return _board.Right;
            if (prevDir != _board.Right)
                yield return _board.Left;
            if (prevDir != _board.Up)
                yield return _board.Down;
            if (prevDir != _board.Down)
                yield return _board.Up;
        }

        public IEnumerable<int> StoneIndexes() {
            return _board.GetAllIndexes().Where(i => _board[i] == Stone.Black);
        }
    }
}
