using System;
using System.Diagnostics;
using System.IO;

namespace JapaneseCrossword
{
    class Program
    {
        static void Main(string[] args)
        {
			var solver = new CrosswordSolver(new ParallelCrosswordSolverCore());
	        var sw = new Stopwatch();
			sw.Start();
	        solver.Solve(@"TestFiles\Newton.txt", "output.txt");
	        Console.WriteLine(sw.ElapsedMilliseconds);
        }
    }
}
