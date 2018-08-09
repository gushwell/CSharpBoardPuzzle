using System;
using System.Collections.Generic;
using System.Linq;

namespace LessThanThreePuzzle {
    public class Solver : IObservable<Board> {

        private Board board = new Board();
        private IEnumerable<int[]> _combinations;

        public void Solve() {
            // 重複無しの組み合わせを求める
            _combinations = Combination.Enumerate(new int[] { 1, 2, 3, 4, 5, 6 }, 2, false)
                                       .ToArray();

            // まずは、1行目から黒を置いてゆく
            InnerSolve(1, Stone.Black);
            Complete();
        }


        // ｙで指定した行にstone(White or Black)を置く
        public void InnerSolve(int y, Stone stone) {
            if (y == 7) {
                if (stone == Stone.Black)
                    // 今度は、１行目から白を置いてゆく
                    InnerSolve(1, Stone.White);
                else if (stone == Stone.White) {
                    // 黒白両方置き終わった（解が見つかったので通知する)
                    Publish(board);   
                }
                return;
            }
            // y行にstoneを２つずつ置く
            foreach (var combi in _combinations) {
                int a = combi[0];
                int b = combi[1];
                if (board[a, y] != Stone.Empty || board[b, y] != Stone.Empty)
                    continue;

                board[a, y] = stone;
                board[b, y] = stone;
                try {
                    if (board.IsCorrect(a, y, stone) && board.IsCorrect(b, y, stone)) {
                        // 条件を満たしているので、次の行を処理する。
                        InnerSolve(y + 1, stone);
                    }
                } finally {
                    // バックトラックするため、状態を戻す。
                    board[a, y] = Stone.Empty;
                    board[b, y] = Stone.Empty;
                }
            }
            return;
        }

        #region IObservable<Board> 関連

        private List<IObserver<Board>> observers = new List<IObserver<Board>>();


        public IDisposable Subscribe(IObserver<Board> observer) {
            observers.Add(observer);
            return observer as IDisposable;
        }

        #endregion

        // 状況変化を知らせるために購読者に通知する
        private void Publish(Board board) {
            foreach (var observer in observers)
                observer.OnNext(board);
        }

        // 終了を通知する
        private void Complete() {
            foreach (var observer in observers) {
                observer.OnCompleted();
            }
        }
    }
}


