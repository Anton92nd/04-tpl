using System;
using System.Diagnostics;
using System.IO;

namespace JapaneseCrossword
{
    class Program
    {
        static void Main(string[] args)
        {
			var solver = new FileCrosswordSolver(new ParallelCrosswordSolverCore());
	        var sw = new Stopwatch();
			sw.Start();
	        solver.Solve(@"TestFiles\Winter.txt", "output.txt");
	        Console.WriteLine(sw.ElapsedMilliseconds);
        }
    }
}
