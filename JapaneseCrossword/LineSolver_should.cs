using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace JapaneseCrossword
{
	[TestFixture]
	class LineSolver_should
	{
		private LineSolver solver;

		[TestFixtureSetUp]
		public void SetUp()
		{
			solver = new LineSolver();
		}

		[Test]
		public void Solve_Line_Without_Blocks()
		{
			var line = new Line(new List<int>(), new[] {Cell.Unknown, Cell.Unknown, Cell.Unknown});
			solver.UpdateLine(line);
			Assert.AreEqual(line.Cells, new [] {Cell.Empty, Cell.Empty, Cell.Empty});
		}

		[Test]
		public void Solve_Line_With_One_Big_Block()
		{
			var line = new Line(new List<int> {5}, Enumerable.Range(0, 5).Select(x => Cell.Unknown).ToArray());
			solver.UpdateLine(line);
			Assert.AreEqual(line.Cells, Enumerable.Range(0, 5).Select(x => Cell.Filled).ToArray());
		}

		[Test]
		public void Partially_Solve_Line_With_One_Medium_Block()
		{
			var line = new Line(new List<int> {6}, Enumerable.Range(0, 8).Select(x => Cell.Unknown).ToArray());
			solver.UpdateLine(line);
			Assert.AreEqual(line.Cells, new []{
				Cell.Unknown, Cell.Unknown, 
				Cell.Filled, Cell.Filled, Cell.Filled, Cell.Filled,
				Cell.Unknown, Cell.Unknown
			});
		}

		[Test]
		public void Solve_Line_With_One_Known_Cell()
		{
			var cells = new[] {Cell.Unknown, Cell.Unknown, Cell.Unknown, Cell.Unknown, Cell.Unknown, Cell.Filled};
			var line = new Line(new List<int> {4}, cells);
			solver.UpdateLine(line);
			Assert.AreEqual(line.Cells, new []{Cell.Empty, Cell.Empty, Cell.Filled, Cell.Filled, Cell.Filled, Cell.Filled});
		} 

		[Test]
		public void Find_Only_Solution_With_Many_Blocks()
		{
			var line = new Line(new List<int> {1, 2, 2, 1}, Enumerable.Range(0, 9).Select(x => Cell.Unknown).ToArray());
			solver.UpdateLine(line);
			Assert.AreEqual(line.Cells, new []
			{
				Cell.Filled, Cell.Empty, Cell.Filled, Cell.Filled, Cell.Empty, 
				Cell.Filled, Cell.Filled, Cell.Empty, Cell.Filled
			});
		}

		[Test]
		public void Partially_Solve_Line_With_Two_Blocks()
		{
			var line = new Line(new List<int> {4, 7}, Enumerable.Range(0, 13).Select(x => Cell.Unknown).ToArray());
			solver.UpdateLine(line);
			Assert.AreEqual(line.Cells, new []
			{
				Cell.Unknown, Cell.Filled, Cell.Filled, Cell.Filled, Cell.Unknown, Cell.Unknown, 
				Cell.Filled, Cell.Filled, Cell.Filled, Cell.Filled, Cell.Filled, Cell.Filled, 
				Cell.Unknown
			});
		}
	}
}
