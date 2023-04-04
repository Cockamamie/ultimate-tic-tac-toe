using System.Collections.Generic;

namespace bot
{
    public static class StateReader
    {
        // ReSharper disable once InconsistentNaming
        public static void ReadState(this ConsoleReader Console, State state)
        {
            var inputs = Console.ReadLine().Split(' ');
            var opponentRow = int.Parse(inputs[0]);
            var opponentCol = int.Parse(inputs[1]);
            var validActionCount = int.Parse(Console.ReadLine());
            var availableMoves = new List<(int, int)>();
            for (var i = 0; i < validActionCount; i++)
            {
                inputs = Console.ReadLine().Split(' ');
                var row = int.Parse(inputs[0]);
                var col = int.Parse(inputs[1]);
                availableMoves.Add((row, col));
            }
            
            state.Update(opponentRow, opponentCol, availableMoves);
        }
    }
}