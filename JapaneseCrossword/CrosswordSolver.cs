using System;
using System.IO;

namespace JapaneseCrossword
{
    public class CrosswordSolver : ICrosswordSolver
    {

        public SolutionStatus Solve(string inputFilePath, string outputFilePath)
        {
	        Crossword crossword;
	        try
	        {
				var reader = new FileCrosswordReader(inputFilePath);
		        crossword = reader.Read();
	        }
	        catch
	        {
		        return SolutionStatus.BadInputFilePath;
	        }
			var result = Solve(crossword);
	        try
	        {
				var writer = new FileCrosswordWriter(outputFilePath);
		        writer.Write(crossword);
	        }
			catch
	        {
		        return SolutionStatus.BadOutputFilePath;
	        }
	        return result;
        }

	    private SolutionStatus Solve(Crossword crossword)
	    {
		    return SolutionStatus.Solved;
	    }
    }
}