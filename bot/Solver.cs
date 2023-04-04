namespace bot
{
    public class Solver
    {
        private readonly MiniMaxAi ai = new();
        public BotCommand GetCommand(State state, Countdown countdown)
        {
            var move = ai.GetMove(state.Board);
            var playerSymbol = state.Board.CurrentPlayer == 0 ? 'X' : 'O';
            var message = $"{playerSymbol} makes move at {move}";
            state.Board.ApplyMove(move);
            var cell = GetCellPosition(move);
            return new Move(cell.row, cell.column) {Message = $"{cell.row} {cell.column}"};
        }

        private (int row, int column) GetCellPosition(int move)
        {
            return (move / 3, move % 3);
        }
    }
}   