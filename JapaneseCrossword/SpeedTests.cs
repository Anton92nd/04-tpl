using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace JapaneseCrossword
{
	[TestFixture]
	public class SpeedTests
	{
		private ICrosswordSolverCore[] solvers;
		private FileCrosswordReader reader;
		private string[] names;

		[TestFixtureSetUp]
		public void SetUp()
		{
			reader = new FileCrosswordReader();
			solvers = new ICrosswordSolverCore[] {new CrosswordSolverCore(), new ParallelCrosswordSolverCore()};
			names = new[] {"Simple solver", "Parallel solver"};
		}

		private void RunTest(Crossword crossword, ICrosswordSolverCore core)
		{
			core.Solve(crossword);
		}

		[TestCase(@"TestFiles\Flower.txt")]
		[TestCase(@"TestFiles\Dog.txt")]
		[TestCase(@"TestFiles\Newton.txt")]
		[TestCase(@"TestFiles\SuperBig.txt")]
		public void SpeedTest(string testName)
		{
			Console.WriteLine(testName + " :");
			for (var i = 0; i < solvers.Length; i++)
			{
				var crossword = reader.Read(testName);
				var sw = new Stopwatch();
				sw.Start();
				RunTest(crossword, solvers[i]);
				sw.Stop();
				Console.WriteLine("\t{0}: {1}", names[i], sw.ElapsedMilliseconds);
			}
		}
	}
}
