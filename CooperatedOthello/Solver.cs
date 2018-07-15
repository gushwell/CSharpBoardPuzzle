using System;
using System.Collections.Generic;

namespace CooperatedOthello {
    public class Solver {
        // 幅優先探索で調べる
        public IEnumerable<Move> Solve(OthBoard board) {
            // 最初はどこに打っても同じなので、ひとつに固定。
            int p = board.ToIndex(3, 4);
            board.Put(board.Turn, p);

            var queu = new Queue<OthBoard>();
            queu.Enqueue(new OthBoard(board));
            var current = board;
            while (queu.Count != 0) {
                //キューの先頭からノード currentNode を取り出す
                current = queu.Dequeue();
                if ((current.StoneCount(Stone.Black) == 0) ||
                     (current.StoneCount(Stone.White) == 0)) {
                    // 解がひとつ見つかれば終了。その手順を返す。
                    return current.GetMoves();
                }
                foreach (var pos in current.PutablePlaces(current.Turn)) {
                    var next = new OthBoard(current);
                    next.Put(next.Turn, pos);
                    // 試した手の状態はキューに入れる
                    queu.Enqueue(next);
                }
            }
            // 見つからなかった
            return new Move[0];
        }
    }
}
