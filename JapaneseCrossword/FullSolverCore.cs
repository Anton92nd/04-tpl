using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace JapaneseCrossword
{
	public class FullSolverCore : ICrosswordSolverCore
	{
		private ICrosswordSolverCore baseCore;

		public FullSolverCore(ICrosswordSolverCore baseCore)
		{
			this.baseCore = baseCore;
		}

		private Crossword CopyCrossword(Crossword oldCrossword)
		{
			return JsonConvert.DeserializeObject<Crossword>(JsonConvert.SerializeObject(oldCrossword));
		}

		private bool TryRecursiveFilling(ref Crossword crossword)
		{
			var crosswordCopy = CopyCrossword(crossword);
			try
			{
				baseCore.Solve(crosswordCopy);
			}
			catch
			{
				return false;
			}
			for (var i = 0; i < crossword.Rows.Length; i++)
			{
				for (var j = 0; j < crossword.Colons.Length; j++)
				{
					if (crosswordCopy.Rows[i].Cells[j] == Cell.Unknown)
					{
						crosswordCopy.Rows[i].Cells[j] = Cell.Filled;
						crosswordCopy.Colons[j].Cells[i] = Cell.Filled;
						if (TryRecursiveFilling(ref crosswordCopy))
						{
							crossword = crosswordCopy;
							return true;
						}
						crosswordCopy = CopyCrossword(crossword);
						baseCore.Solve(crosswordCopy);
						crosswordCopy.Rows[i].Cells[j] = Cell.Empty;
						crosswordCopy.Colons[j].Cells[i] = Cell.Empty;
						if (TryRecursiveFilling(ref crosswordCopy))
						{
							crossword = crosswordCopy;
							return true;
						}
						return false;
					}
				}
			}
			crossword = crosswordCopy;
			return true;
		}

		public SolutionStatus Solve(Crossword crossword)
		{
			var crosswordCopy = CopyCrossword(crossword);
			baseCore.Solve(crosswordCopy);
			var result = TryRecursiveFilling(ref crosswordCopy) ? SolutionStatus.Solved : SolutionStatus.IncorrectCrossword;
			if (result == SolutionStatus.Solved)
			{
				for (var i = 0; i < crossword.Rows.Length; i++)
				{
					for (var j = 0; j < crossword.Colons.Length; j++)
					{
						crossword.Rows[i].Cells[j] = crosswordCopy.Rows[i].Cells[j];
						crossword.Colons[j].Cells[i] = crosswordCopy.Colons[j].Cells[i];
					}
				}
			}
			return result;
		}
	}
}
