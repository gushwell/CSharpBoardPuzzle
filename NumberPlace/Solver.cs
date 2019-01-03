using System.Collections.Generic;
using System.Linq;

namespace NumberPlace {
    class Solver {
        private Board _board;

        public IEnumerable<Board> Solve(int[,] mat) {
            _board = new Board(mat);
            return SolveInner();
        }

        private IEnumerable<Board> SolveInner() {
            if (_board.IsCompleted()) {
                // 完成したら、answerに答えを入れる。
                yield return _board.Clone();
            } else {
                // 空いている位置をひとつ取り出す
                var pos = _board.AlloLocations()
                                .Where(p => _board.IsVacant(p))
                                .First();
                // そこに、1-9 の数を置いてみる。
                for (int n = 1; n <= 9; n++) {
                    if (_board.CanPut(pos, n)) {
                        _board.Put(pos, n);
                        // 置けたので、再帰的に次の数を置いていく。
                        var answer = SolveInner();
                        foreach (var a in answer)
                            yield return a;
                        // 次の数を置くために、今置いた場所には 0 を入れなおす。
                        _board.Put(pos, 0);
                    }
                }
                // 1..9どれも駄目。つまり失敗->  呼び出し元に戻る
            }
        }
    }
}