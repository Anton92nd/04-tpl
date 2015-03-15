using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using NUnit.Framework;

namespace JapaneseCrossword
{
	public class ParallelCrosswordSolverCore : ICrosswordSolverCore
	{
		private bool[] needUpdateRows, needUpdateColons;

		public SolutionStatus Solve(Crossword crossword)
		{
			needUpdateRows = Enumerable.Range(0, crossword.Rows.Length).Select(x => true).ToArray();
			needUpdateColons = Enumerable.Range(0, crossword.Colons.Length).Select(x => true).ToArray();
			while (needUpdateColons.Any(cell => cell) || needUpdateRows.Any(cell => cell))
			{
				var tasks = Enumerable.Range(0, crossword.Rows.Length)
					.Where(i => needUpdateRows[i])
					.Select(i => Task.Run(() =>
				{
					new LineSolver().UpdateLine(crossword.Rows[i]);
					needUpdateRows[i] = false;
				})).ToArray();
				try
				{
					Task.WaitAll(tasks);
				}
				catch (AggregateException ex)
				{
					if (ex.InnerException is MyException)
						return SolutionStatus.IncorrectCrossword;
					throw ex.InnerException;
				}
				UpdateLines(crossword.Rows, crossword.Colons, needUpdateColons);
				tasks = Enumerable.Range(0, crossword.Colons.Length)
					.Where(i => needUpdateColons[i])
					.Select(i => Task.Run(() =>
					{
						new LineSolver().UpdateLine(crossword.Colons[i]);
						needUpdateColons[i] = false;
					})).ToArray();
				try
				{
					Task.WaitAll(tasks);
				}
				catch (AggregateException ex)
				{
					if (ex.InnerException is MyException)
						return SolutionStatus.IncorrectCrossword;
					throw ex.InnerException;
				}
				UpdateLines(crossword.Colons, crossword.Rows, needUpdateRows);
			}
			return (crossword.Rows.Any(row => row.Cells.Any(cell => cell == Cell.Unknown)))
				? SolutionStatus.PartiallySolved
				: SolutionStatus.Solved;
		}

		private void UpdateLines(Line[] fromLines, Line[] toLines, bool[] needUpdateLines)
		{
			for (var i = 0; i < fromLines.Length; i++)
			{
				for (var j = 0; j < toLines.Length; j++)
				{
					if (fromLines[i].Cells[j] != Cell.Unknown && toLines[j].Cells[i] == Cell.Unknown)
					{
						toLines[j].Cells[i] = fromLines[i].Cells[j];
						needUpdateLines[j] = true;
					}
				}
			}
		}
	}
}