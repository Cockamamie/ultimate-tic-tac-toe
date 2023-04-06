using System;
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
        public List<int> PlayableLocalBoardIndexes { get; } //TODO: rewrite via Board.GetMoves()
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
            var localBoardCellNumber = opponentRow % 3 * 3 + opponentColumn % 3;
            localBoard.ApplyMove(localBoardCellNumber, EnemyAiPlayer);
            UpdateGlobalBoardIfNeeded(localBoardIndex);
        }

        public void UpdateGlobalBoardIfNeeded(int localBoardIndex)
        {
            var localBoard = LocalBoards[localBoardIndex];
            if (!localBoard.IsFinished) 
                return;
            var boardWinner = localBoard.GetWinner();
            if (boardWinner == -1)
                GlobalBoard.MoveExceptions.Add(localBoardIndex);
            GlobalBoard.ApplyMove(localBoardIndex, boardWinner);
        }

        private static int GetLocalBoardIndex(int row, int column) => row / 3 * 3 + column / 3;

        private void DefinePlayerSide()
        {
            if (AiPlayer != -1)
                return;
            var maxAvailableMoves = 81;
            var madeMovesCount = maxAvailableMoves - LocalBoards.Sum(board => board.GetMoves().Count());
            AiPlayer = madeMovesCount % 2;
            Console.Error.WriteLine($"AiPlayer = {AiPlayer} because moves made = {madeMovesCount}");
        }
    }
}