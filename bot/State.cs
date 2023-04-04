using System.Collections.Generic;
using System.Linq;

namespace bot
{
    public class State
    {
        public State()
        {
            Board = new Board();
            AvailableMoves = new List<int>();
        }

        public Board Board { get; }
        public IEnumerable<int> AvailableMoves { get; private set; }

        public void Update(
            int opponentRow,
            int opponentColumn,
            IEnumerable<(int row, int column)> availableMoves)
        {
            var opponentMove = Board.CoordinatesToCellNumber(opponentRow, opponentColumn);
            Board.ApplyMove(opponentMove);
            AvailableMoves = availableMoves.Select(cell => Board.CoordinatesToCellNumber(cell.row, cell.column));
        }
    }
}