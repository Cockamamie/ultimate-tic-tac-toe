using System;
using System.Linq;

namespace bot
{
    public class MiniMaxAi
    {
        private static int AiPlayer;
        private static int OpponentAiPlayer => AiPlayer == 0 ? 1 : 0;
        private int MaxDepth;
        private static Random random = new();
        private static int[] firstMoves = { 0, 2, 4, 6, 8 };
        public int GetMove(Board board, int maximizingPlayer, int maxDepth = 9)
        {
            if (board.GetMoves().Count() == 9)
                // too often lets enemy take global board center,
                // the most important one imo
                // return 4;
                return firstMoves[random.Next(firstMoves.Length - 1)];
            AiPlayer = maximizingPlayer;
            MaxDepth = maxDepth;
            var bestScore = int.MinValue;
            var bestMove = -1;

            foreach (var move in board.GetMoves())
            {
                var boardCopy = board.Clone();
                var score = MiniMax(boardCopy, move, 1, AiPlayer);
                if (score <= bestScore)
                    continue;
                bestScore = score;
                bestMove = move;
            }
            
            if (bestMove < 0)
                throw new Exception();
            return bestMove;
        }
        
        private int MiniMax(Board board, int previousMove, int depth, int player)
        {
            board.ApplyMove(previousMove, player);
            if (board.IsFinished || depth > MaxDepth)
                return GetScore(board, depth);
            var nextPlayer = player == AiPlayer ? OpponentAiPlayer : AiPlayer;
            var scores = board
                .GetMoves()
                .Select(move => MiniMax(board.Clone(), move, depth + 1, nextPlayer))
                .ToList();

            return player == AiPlayer
                ? scores.Max()
                : scores.Min();
        }

        private static int GetScore(Board board, int depth)
        {
            if (board.GetWinner() == OpponentAiPlayer)
                return depth - 10;
            if (board.GetWinner() == AiPlayer)
                return 10 - depth;
            return 0;
        }
    }
}