using System;
using System.Diagnostics;
using System.IO;

namespace JapaneseCrossword
{
    class Program
    {
        static void Main(string[] args)
        {
			var solver = new CrosswordSolver(new FullSolverCore(new CrosswordSolverCore()));
	        var sw = new Stopwatch();
			sw.Start();
	        var result = solver.Solve(@"TestFiles\Winter.txt", "output.txt");
	        Console.WriteLine(result);
	        Console.WriteLine(sw.ElapsedMilliseconds);
        }
    }
}
