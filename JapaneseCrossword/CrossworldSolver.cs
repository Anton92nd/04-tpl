using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JapaneseCrossword
{
	public class CrosswordSolver : ICrosswordSolver
	{
		private readonly ICrosswordSolverCore solver;

		public CrosswordSolver(ICrosswordSolverCore solver)
		{
			this.solver = solver;
		}

		public SolutionStatus Solve(string inputFilePath, string outputFilePath)
		{
			Crossword crossword;
			try
			{
				var reader = new FileCrosswordReader();
				crossword = reader.Read(inputFilePath);
			}
			catch
			{
				return SolutionStatus.BadInputFilePath;
			}
			SolutionStatus result;
			try
			{
				result = solver.Solve(crossword);
			}
			catch (MyException)
			{
				return SolutionStatus.IncorrectCrossword;
			}
			try
			{
				var writer = new FileCrosswordWriter();
				writer.Write(crossword, outputFilePath);
			}
			catch
			{
				return SolutionStatus.BadOutputFilePath;
			}
			return result;
		}
	}
}
