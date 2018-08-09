using System;
namespace LessThanThreePuzzle {
    public class ResultViewer : IObserver<Board> {

        public void OnCompleted() {
            Console.WriteLine("end");
        }

        public void OnError(Exception error) {
            Console.WriteLine("{0}", error.Message);
        }

        public void OnNext(Board board) {
            Print(board);
        }

        private void Print(Board board) {
            for (int y = 1; y <= board.YSize; y++) {
                for (int x = 1; x <= board.XSize; x++) {
                    Stone p = board[x, y];
                    Console.Write("{0} ", p.Value);
                }
                Console.WriteLine();
            }
            Console.WriteLine("---");
        }
    }
}
