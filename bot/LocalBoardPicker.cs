using System;
using System.Linq;

namespace bot
{
    public class LocalBoardPicker
    {
        private static readonly MiniMaxAi miniMaxAi = new();
        public int PickLocalBoardIndex(State state)
        {
            Console.Error.WriteLine("available local boards = " + string.Join(" ", state.PlayableLocalBoardIndexes));
            if (state.PlayableLocalBoardIndexes.Count == 1)
            {
                var singleMove = state.PlayableLocalBoardIndexes.First();
                return singleMove;
            }
            var move = miniMaxAi.GetMove(state.GlobalBoard, state.AiPlayer, 4);
            Console.Error.WriteLine($"Chosen local board is {move}");
            return move;
        }
    }
}