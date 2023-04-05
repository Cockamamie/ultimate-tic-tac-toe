using System;
using System.Linq;

namespace bot
{
    public class LocalBoardPicker
    {
        private static Random random = new(); 
        public int PickLocalBoardIndex(State state)
        {
            if (state.PlayableLocalBoardIndexes.Count == 1)
                return state.PlayableLocalBoardIndexes.First();
            return state.PlayableLocalBoardIndexes[random.Next(state.PlayableLocalBoardIndexes.Count)];
        }
    }
}