using System;
using System.Linq;

namespace bot
{
    public class LocalBoardPicker
    {
        private static MiniMaxAi miniMaxAi = new(); 
        public int PickLocalBoardIndex(State state)
        {
            Console.Error.WriteLine("available local boards = " + string.Join(" ", state.PlayableLocalBoardIndexes));
            if (state.PlayableLocalBoardIndexes.Count == 1)
            {
                var singleMove = state.PlayableLocalBoardIndexes.First();
                Console.Error.WriteLine($"Chosen local board is {singleMove}");
                return singleMove;
            }
            state.GlobalBoard.CurrentPlayer = state.AiPlayer;
            var move = miniMaxAi.GetMove(state.GlobalBoard);
            Console.Error.WriteLine($"Chosen local board is {move}");
            return move;
        }
    }
}