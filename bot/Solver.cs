using System;

namespace bot
{
    public class Solver
    {
        private readonly MiniMaxAi ai = new();
        private readonly LocalBoardPicker picker = new();
        public BotCommand GetCommand(State state, Countdown countdown)
        {
            var localBoardToPlayIndex = picker.PickLocalBoardIndex(state); // TODO: randomly picks board to play on
            var localBoard = state.LocalBoards[localBoardToPlayIndex];
            localBoard.CurrentPlayer = state.AiPlayer;
            var move = ai.GetMove(localBoard); // TODO: selection isn't based on other boards state including global board, just tries to win on current board
            localBoard.ApplyMove(move);
            if (localBoard.IsFinished)
            {
                var winner = localBoard.GetWinner();
                if (winner == -1)
                    state.GlobalBoard.MoveExceptions.Add(localBoardToPlayIndex);
                var previousCurrentPlayer = state.GlobalBoard.CurrentPlayer;
                state.GlobalBoard.CurrentPlayer = winner;
                state.GlobalBoard.ApplyMove(localBoardToPlayIndex);
                state.GlobalBoard.CurrentPlayer = previousCurrentPlayer;
            }
            var cell = GetCellPosition(localBoardToPlayIndex, move);
            return new Move(cell.row, cell.column) {Message = $"{cell.row} {cell.column}"};
        }

        private (int row, int column) GetCellPosition(int localBoardIndex, int move)
        {
            var row = move / 3 + localBoardIndex / 3 * 3;
            var column = move % 3 + localBoardIndex % 3 * 3;
            Console.Error.WriteLine($"Calculated move: local board index = [{localBoardIndex}], move(0-8) = {move}" +
                                    $"\nConverted move : [{row} {column}]");
            return (row, column);
        }
    }
}   