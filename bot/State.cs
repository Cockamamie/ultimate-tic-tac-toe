using System.Collections.Generic;
using System.Linq;

namespace bot
{
    public class State
    {
        public State()
        {
            LocalBoards = Enumerable.Range(0, 9).Select(_ => new Board()).ToList();
            GlobalBoard = new Board();
            PlayableLocalBoardIndexes = new List<int>();
            AiPlayer = -1;
        }

        public List<Board> LocalBoards { get; }
        public Board GlobalBoard { get; }
        public List<int> PlayableLocalBoardIndexes { get; }
        public int AiPlayer { get; private set; }
        public int EnemyAiPlayer => AiPlayer == 0 ? 1 : 0;

        public void Update(int opponentRow, int opponentColumn, IEnumerable<(int Row, int Column)> availableMoves)
        {
            DefinePlayerSide();
            UpdateBoards(opponentRow, opponentColumn);
            PlayableLocalBoardIndexes.Clear();
            foreach (var availableMove in availableMoves)
            {
                var localBoardIndex = GetLocalBoardIndex(availableMove.Row, availableMove.Column);
                if (PlayableLocalBoardIndexes.Contains(localBoardIndex))
                    continue;
                PlayableLocalBoardIndexes.Add(localBoardIndex);
            }
        }

        private void UpdateBoards(int opponentRow, int opponentColumn)
        {
            var localBoardIndex = GetLocalBoardIndex(opponentRow, opponentColumn);
            var localBoard = LocalBoards[localBoardIndex];
            localBoard.CurrentPlayer = EnemyAiPlayer;
            var localBoardCellNumber = opponentRow % 3 * 3 + opponentColumn % 3;
            localBoard.ApplyMove(localBoardCellNumber);
            if (localBoard.IsFinished)
            {
                var winner = localBoard.GetWinner();
                if (winner == -1)
                    GlobalBoard.MoveExceptions.Add(localBoardIndex);
                var previousCurrentPlayer = GlobalBoard.CurrentPlayer;
                GlobalBoard.CurrentPlayer = winner;
                GlobalBoard.ApplyMove(localBoardIndex);
                GlobalBoard.CurrentPlayer = previousCurrentPlayer;
            }
        }

        private static int GetLocalBoardIndex(int row, int column) => row / 3 * 3 + column / 3;

        private void DefinePlayerSide()
        {
            if (AiPlayer != -1)
                return;
            var madeMovesCount = LocalBoards.Sum(board => board.GetMoves().Count());
            if (madeMovesCount == 0)
                AiPlayer = 0;
            if (madeMovesCount == 1)
                AiPlayer = 1;
        }
    }
}