﻿using System;
using System.Diagnostics.CodeAnalysis;

namespace bot
{
    public static class App
    {
        [SuppressMessage("ReSharper", "FunctionNeverReturns")]
        private static void Main(string[] args)
        {
            var reader = new ConsoleReader();
            var solver = new Solver();
            reader.FlushToStdErr();
            var first = true;
            var state = new State();
            while (true)
            {
                reader.ReadState(state);
                var timer = new Countdown(first ? 500 : 50); //TODO fix timeouts
                reader.FlushToStdErr();
                var command = solver.GetCommand(state, timer);
                Console.WriteLine(command.Message);
                // Console.WriteLine(command);
                Console.Error.WriteLine(timer);
                first = false;
            }
        }
    }
}