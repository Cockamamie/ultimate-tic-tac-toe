﻿using System;
using System.Linq;

namespace bot
{
    public class MiniMaxAi
    {
        private static int AiPlayer;
        public int GetMove(Board board)
        {
            if (board.GetMoves().Count() == 9)
                return 4;
            AiPlayer = board.CurrentPlayer;
            var bestScore = int.MinValue;
            var bestMove = -1;

            foreach (var move in board.GetMoves())
            {
                var boardCopy = board.Clone();
                var score = MiniMax(boardCopy, move, 1);
                if (score <= bestScore)
                    continue;
                bestScore = score;
                bestMove = move;
            }
            
            if (bestMove < 0)
                throw new Exception();
            return bestMove;
        }
        
        private int MiniMax(Board board, int previousMove, int depth)
        {
            board.ApplyMove(previousMove);
            if (board.IsFinished)
                return GetScore(board, depth);
            
            var scores = board
                .GetMoves()
                .Select(move => MiniMax(board.Clone(), move, depth + 1))
                .ToList();

            return board.CurrentPlayer == AiPlayer
                ? scores.Max()
                : scores.Min();
        }

        private static int GetScore(Board board, int depth)
        {
            var aiOpponent = AiPlayer == 1 ? 0 : 1;
            if (board.GetWinner() == aiOpponent)
                return depth - 10;
            if (board.GetWinner() == AiPlayer)
                return 10 - depth;
            return 0;
        }
    }
}