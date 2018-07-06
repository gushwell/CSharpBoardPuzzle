using System;
using System.Linq;
using System.Collections.Generic;

namespace ArrangementOfKnights {
    
    public class Solver {
        public Chessboard Solve(Chessboard board) {
            _Solve(board, 0, 0, board.BlockA());
            return _answer;
        }

        private int _mincount = 64;
        private Chessboard _answer;

        // ４つに区切って探索する。
        // ひとつのブロックには、最大でも４つのKnightを配置すれば条件を満たせるはず。
        private void _Solve(Chessboard board, int level, int nowpos, int[] block) {
            if (board.IsOver(block))
                return;
            if (board.IsCorrect(block)) {
                // 現在のブロックが正しいので、次のブロックを求める
                int[] nextblock = board.NextBlock(block);
                if (nextblock != null) {
                    // 次のブロックの解を求める
                    _Solve(board, level, nowpos, nextblock);
                } else {
                    // すべてのブロックを処理し終わったので、解が見つかったか調べる
                    if (board.IsFinish()) {
                        // Levelの値と置いた騎士の数は一致する。置いた騎士の数が少ないほうがより良い解。
                        if (_mincount > level) {
                            // より良い解が見つかったので、解を覚える。
                            _answer = new Chessboard(board);
                            // _answer.Print();  // デバッグ用　暫定解を表示
                            _mincount = level;
                        }
                    }
                }
                return;
            }

            if (level + 1 >= _mincount)
                //  現在の最良解よりも悪いものなので、この探索は途中で打ち切る
                return;

            // 現在のブロックのそれぞれに対し、騎士が置けるかを調べる
            var list = block.Where(ix => board[ix] == Piece.Empty).ToList();
            foreach (int pos in list) {
                if (Array.IndexOf(block, pos) > Array.IndexOf(block, nowpos)) {
                    // 直前に置いた騎士の位置よりもインデックスが小さいなら既に試してあるので、
                    // インデックスが大きい場合に、新たに騎士をおいて、次の探索に行く。
                    board.Put(pos);
                    _Solve(board, level + 1, pos, block);
                    // pos位置での試しは終わったので、次の探索ができるようにするためクリアする。
                    board.Clear(pos);
                }
            }
        }
    }
}
