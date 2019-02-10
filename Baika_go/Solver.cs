using System;
using System.Linq;
using System.Text;

namespace Baikago {
    class Solver {

        private Board _board;

        public Solver(Board board) {
            _board = board;
        }

        // どちらが勝ったかを判定
        public (Stone, int[]) WhichWon() {
            (var isWin, var pattern) = IsWin(Stone.White);
            if (isWin)
                return (Stone.White, pattern);
            (isWin, pattern) = IsWin(Stone.Black);
            if (isWin)
                return (Stone.Black, pattern);
            return (null, null);
        }


        // pieceが勝ったかを判定
        public (bool, int[]) IsWin(Stone piece) {
            foreach (var loc in _board.GetAllIndexes()) {
                (var isWin, var pattern) = IsWinPattern(loc, piece);
                if (isWin)
                    return (isWin, pattern);
            }
            return (false, null);
        }

        // 指定した位置で指定したPieceが勝ちパターンを作ったかを調べる
        public (bool, int[]) IsWinPattern(int pos, Stone piece) {
            if (_board[pos] != piece)
                return (false, null);
            var (x, y) = _board.ToLocation(pos);
            int maxsize = Math.Min(_board.XSize, _board.YSize);
            for (int size = 1; size <= (maxsize - 1) / 2; size++) {
                if (!((size < x && x <= _board.XSize - size) &&
                     (size < y && y <= _board.YSize - size)))
                    continue;
                // パターン1
                var wp1 = CreateWinPattern1(pos, size);
                if (wp1.All(loc => _board[loc] == piece)) {
                    return (true, wp1);
                }
                var wp2 = CreateWinPattern2(pos, size);
                if (wp2.All(loc => _board[loc] == piece)) {
                    return (true, wp2);
                }
            }
            return (false, null);
        }

        // posを中心点とし、sizeの大きさの勝ちパターン（＋）を作成
        private int[] CreateWinPattern1(int pos, int size) {
            int[] wp = new int[5];
            var (x, y) = _board.ToLocation(pos);
            wp[0] = pos;
            wp[1] = _board.ToIndex(x - size, y);
            wp[2] = _board.ToIndex(x + size, y);
            wp[3] = _board.ToIndex(x, y - size);
            wp[4] = _board.ToIndex(x, y + size);
            return wp;
        }

        // posを中心点とし、sizeの大きさの勝ちパターン（Ⅹ）を作成
        private int[] CreateWinPattern2(int pos, int size) {
            int[] wp = new int[5];
            var (x, y) = _board.ToLocation(pos);
            wp[0] = pos;
            wp[1] = _board.ToIndex(x - size, y - size);
            wp[2] = _board.ToIndex(x - size, y + size);
            wp[3] = _board.ToIndex(x + size, y - size);
            wp[4] = _board.ToIndex(x + size, y + size);
            return wp;
        }

    }
}
