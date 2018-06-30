using System;
using System.Collections.Generic;
using System.Linq;

namespace KnightTour {
    class Solver {
        private int _startPlace;
        public Chessboard Solve(Chessboard board, int startIndex) {
            _startPlace = startIndex;
            board.Jump(_startPlace);
            return SolveInner(board, _startPlace);
        }

        private Chessboard SolveInner(Chessboard board, int nowPlace) {
            // 全ての位置に移動し、現在の位置からStart地点にジャンプできれば、解が求まった。
            if (board.IsFin()) {
                if (board.CanBackHome(nowPlace))
                    return board;
                return null;
            }
            // OrderBy を入れるか入れないかで、圧倒的な速度差が出る
            List<int> list = board.CanPutPlaces(nowPlace).OrderBy(n => board.CanPutPlaces(n).Count()).ToList();
            foreach (int pos in list) {
                board.Jump(pos);
                // 枝刈り用の処理：この判断が無いととてつもなく遅くなってしまう。
                if (board[pos] is Footmark fm) {
                    // 開始位置からJumpする場所が無いなら、これ以上探しても意味が無いので次の可能性へ。
                    if ((fm.Number != board.YSize * board.XSize) &&
                        (board.CanPutPlaces(_startPlace).Count() == 0)) {
                        board.Clear(pos);
                        continue;
                    }
                }
                // 枝刈り：ここまで
                var ans = SolveInner(board, pos);
                if (ans != null)
                    return ans;
                board.Clear(pos);
            }
            // ジャンプできる場所はすべて試した。解は見つからない。
            return null;
        }
    }
}
