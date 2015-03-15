using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using NUnit.Framework;

namespace JapaneseCrossword
{
	public class CrosswordSolverCore : ICrosswordSolverCore
	{
		public SolutionStatus Solve(Crossword crossword)
		{
			var needUpdateRows = Enumerable.Range(0, crossword.Rows.Length).Select(x => true).ToArray();
			var needUpdateColons = Enumerable.Range(0, crossword.Colons.Length).Select(x => true).ToArray();
			while (needUpdateColons.Any(cell => cell) || needUpdateRows.Any(cell => cell))
			{
				for (var i = 0; i < crossword.Rows.Length; i++)
				{
					if (needUpdateRows[i])
					{
						new LineSolver().UpdateLine(crossword.Rows[i]);
						for (var j = 0; j < crossword.Colons.Length; j++)
						{
							if (crossword.Rows[i].Cells[j] != Cell.Unknown && crossword.Colons[j].Cells[i] == Cell.Unknown)
							{
								crossword.Colons[j].Cells[i] = crossword.Rows[i].Cells[j];
								needUpdateColons[j] = true;
							}
						}
					}
					needUpdateRows[i] = false;
				}
				for (var j = 0; j < crossword.Colons.Length; j++)
				{
					if (needUpdateColons[j])
					{
						new LineSolver().UpdateLine(crossword.Colons[j]);
						for (var i = 0; i < crossword.Rows.Length; i++)
						{
							if (crossword.Colons[j].Cells[i] != Cell.Unknown && crossword.Rows[i].Cells[j] == Cell.Unknown)
							{
								crossword.Rows[i].Cells[j] = crossword.Colons[j].Cells[i];
								needUpdateRows[i] = true;
							}
						}
					}
					needUpdateColons[j] = false;
				}
			}
			return (crossword.Rows.Any(row => row.Cells.Any(cell => cell == Cell.Unknown)))
				? SolutionStatus.PartiallySolved
				: SolutionStatus.Solved;
		}
	}
}