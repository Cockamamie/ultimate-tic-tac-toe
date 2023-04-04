using System.Collections.Generic;
using System.Linq;
using System.Numerics;

namespace bot
{
    public class Board
    {
        private int x; // bitmask of Xs
        private int o; // bitmask of Os

        // PopCount - counts 1-bits in number
        public int CurrentPlayer => (BitOperations.PopCount((uint)x) + BitOperations.PopCount((uint)o)) & 1;

        public Board() : this(0, 0)
        {
        }

        public Board(int x, int o)
        {
            this.x = x;
            this.o = o;
        }

        public Board Clone()
        {
            return new Board(x, o);
        }

        public static int CoordinatesToCellNumber(int row, int col) => row * 3 + col;
        public char this[int row, int col] => this[CoordinatesToCellNumber(row, col)];
        public char this[int move]
        {
            get
            {
                var index = move;
                if (((x >> index) & 1) == 1) return 'X';
                if (((o >> index) & 1) == 1) return 'O';
                return ' ';
            }
        }

        public bool IsFinished => !GetMoves().Any();

        internal void ApplyMove(int move)
        {
            if (CurrentPlayer == 0) x |= (1 << move);
            else o |= (1 << move);
        }

        public IEnumerable<int> GetMoves()
        {
            if (GetWinner() != -1) yield break;
            for (int move = 0; move < 9; move++)
                if (this[move] == ' ') yield return move;
        }

        public override string ToString()
        {
            return
                string.Join("\n",
                    Enumerable.Range(0, 3)
                    .Select(y =>
                        string.Join("", Enumerable.Range(0, 3).Select(x => this[y, x] == ' ' ? '.' : this[y, x]))));
        }

        public bool IsForcedMove(int m)
        {
            return Win(x | (1 << m)) || Win(o | (1 << m));
        }

        private static bool Win(int board)
        {
            return winMasks.Any(m => (m & board) == m);
        }

        private static int[] winMasks = { 0x7, 0x38, 0x1c0, 0x49, 0x92, 0x124, 0x111, 0x54 };
        public int GetWinner()
        {
            if (Win(x)) return 0;
            if (Win(o)) return 1;
            return -1;
        }

        public override bool Equals(object obj)
        {
            return obj is Board board &&
                   x == board.x &&
                   o == board.o;
        }

        public override int GetHashCode()
        {
            return x*512 + o;
        }
    }
}