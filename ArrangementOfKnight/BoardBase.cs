using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Puzzle {
    // 汎用の盤面クラス
    // Tは、盤面に置けるオブジェクトの型。参照型でnew()ができれば何でも良い。
    public abstract class BoardBase<T> where T : class, new() {
        private T[] _pieces;

        // 盤の行数（縦方向）
        public int YSize { get; private set; }

        // 盤のカラム数（横方向）
        public int XSize { get; private set; }

        // コンストラクタ
        public BoardBase(int xsize, int ysize) {
            this.YSize = ysize;
            this.XSize = xsize;

            _pieces = new T[(xsize + 2) * (ysize + 2)];

            // 盤データの初期化 - 盤の周りはnull(番兵)をセットしておく
            ClearAll();
        }

        // コピー用コンストラクタ
        public BoardBase(BoardBase<T> board) {
            XSize = board.XSize;
            YSize = board.YSize;
            this._pieces = board._pieces.ToArray();
        }

        // 番兵も含めたボード配列の長さ
        public int BoardLength {
            get { return _pieces.Length; }
        }

        // インデクサ (x,y)の位置の要素へアクセスする
        public T this[int index] {
            get { return _pieces[index]; }
            set { _pieces[index] = value; }
        }

        // インデクサ (x,y)の位置の要素へアクセスする
        public T this[int x, int y] {
            get { return this[ToIndex(x, y)]; }
            set { this[ToIndex(x, y)] = value; }
        }

        // Location から _coinのIndexを求める
        public int ToIndex(int x, int y) => x + y * (XSize + 2);

        // IndexからLocationを求める (ToIndexの逆演算)
        public (int, int) ToLocation(int index) => (index % (XSize + 2), index / (XSize + 2));

        // 本来のボード上の位置(index)かどうかを調べる
        public virtual bool IsOnBoard(int index) {
            if (0 <= index && index < BoardLength)
                return this[index] != null;
            return false;
        }

        // 全てのPieceをクリアする
        public virtual void ClearAll() {
            for (int index = 0; index < BoardLength; index++)
                this[index] = null;       // 番兵
            foreach (var index in GetAllIndexes())
                this[index] = new T();　　// 初期値
        }

        // 盤上のすべての位置(index)を列挙する
        public virtual IEnumerable<int> GetAllIndexes() {
            for (int y = 1; y <= this.YSize; y++) {
                for (int x = 1; x <= this.XSize; x++) {
                    yield return ToIndex(x, y);
                }
            }
        }

        // (x,y)からdirection方向の位置を列挙する　(x,y)含む
        public virtual IEnumerable<int> EnumerateIndexes(int x, int y, int direction) {
            for (int index = ToIndex(x, y); IsOnBoard(index) && this[index] != null; index += direction)
                yield return index;
        }

        // (x,y)から右(水平)の位置を列挙する　(x,y)含む
        public virtual IEnumerable<int> Horizontal(int x, int y)
            => EnumerateIndexes(x, y, 1);

        // (x,y)から下(垂直)の位置を列挙する　(x,y)含む
        public virtual IEnumerable<int> Virtical(int x, int y)
            => EnumerateIndexes(x, y, this.XSize + 2);

        // (x,y)から右斜め下(45度)の位置を列挙する　(x,y)含む
        public virtual IEnumerable<int> SlantR(int x, int y)
            => EnumerateIndexes(x, y, this.XSize + 2 + 1);

        // (x,y)から左斜め下(45度)の位置を列挙する　(x,y)含む
        public virtual IEnumerable<int> SlantL(int x, int y)
            => EnumerateIndexes(x, y, this.XSize + 2 - 1);

    }
}