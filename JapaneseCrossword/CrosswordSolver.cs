using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using NUnit.Framework;

namespace JapaneseCrossword
{
	public class MyException : Exception
	{
		private readonly string message;

		public MyException(string message)
		{
			this.message = message;
		}
	}

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
	        SolutionStatus result;
	        try
	        {
		        result = Solve(crossword);
	        }
	        catch (MyException ex)
	        {
		        return SolutionStatus.IncorrectCrossword;
	        }
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

	    private bool[] needUpdateRows, needUpdateColons;

	    private void UpdateCanBeArrays(List<int> blocks, int[] blockPositions, bool[] canBeFilled, bool[] canBeEmpty)
	    {
		    for (var i = 0; i < blocks.Count; i++)
		    {
			    for (var j = blockPositions[i]; j < blockPositions[i] + blocks[i]; j++)
			    {
				    canBeFilled[j] = true;
			    }
			    if (i < blocks.Count - 1)
			    {
				    for (var j = blockPositions[i] + blocks[i]; j < blockPositions[i + 1]; j++)
				    {
					    canBeEmpty[j] = true;
				    }
			    }
		    }
		    for (var i = 0; i < blockPositions[0]; i++)
		    {
			    canBeEmpty[i] = true;
		    }
		    for (var i = blockPositions[blocks.Count - 1] + blocks[blocks.Count - 1]; i < canBeEmpty.Length; i++)
		    {
			    canBeEmpty[i] = true;
		    }
	    }

	    bool CanPlaceBlockAtPosition(int position, Cell[] cells, int blockSize)
	    {
		    return position + blockSize <= cells.Count() && 
				cells.Skip(position).Take(blockSize).All(cell => cell != Cell.Empty) &&
				(position + blockSize == cells.Count() || cells[position + blockSize] != Cell.Filled);
	    }

	    private bool TryRecursiveFilling(Line line, int[] blockPositions, bool[] canBeFilled, bool[] canBeEmpty, int blockNumber = 0, int cellNumber = 0)
	    {
		    if (blockNumber == line.Blocks.Count)
		    {
			    if (line.Cells.Skip(cellNumber).Any(cell => cell == Cell.Filled))
				    return false;
			    UpdateCanBeArrays(line.Blocks, blockPositions, canBeFilled, canBeEmpty);
			    return true;
		    }
		    var currentBlockSize = line.Blocks[blockNumber];
		    var rightBorder = line.Cells.Length - (line.Blocks.Skip(blockNumber).Sum() + line.Blocks.Count - blockNumber - 1);
		    var result = false;
		    for (var i = cellNumber; i <= rightBorder; i++)
		    {
			    if (CanPlaceBlockAtPosition(i, line.Cells, currentBlockSize))
			    {
				    blockPositions[blockNumber] = i;
				    result = TryRecursiveFilling(line, blockPositions, canBeFilled, canBeEmpty,
						blockNumber + 1, i + currentBlockSize) || result;
			    }
				if (line.Cells[i] == Cell.Filled)
					return result;
		    }
		    return result;
	    }

	    private void UpdateLine(Line line)
	    {
		    var canBeFilled = new bool[line.Cells.Length];
		    var canBeEmpty = new bool[line.Cells.Length];
		    var blockPositions = new int[line.Blocks.Count];
		    if (!TryRecursiveFilling(line, blockPositions, canBeFilled, canBeEmpty))
		    {
			    throw new MyException("incorrect data in line");
		    }
		    for (var i = 0; i < line.Cells.Length; i++)
		    {
			    if (!canBeFilled[i] && !canBeEmpty[i])
					throw new MyException("incorrect data in line");
			    if (canBeFilled[i] && !canBeEmpty[i])
					line.Cells[i] = Cell.Filled;
				if (!canBeFilled[i] && canBeEmpty[i])
					line.Cells[i] = Cell.Empty;
		    }
	    }

	    private SolutionStatus Solve(Crossword crossword)
	    {
		    needUpdateRows = Enumerable.Range(0, crossword.Rows.Length).Select(x => true).ToArray();
		    needUpdateColons = Enumerable.Range(0, crossword.Colons.Length).Select(x => true).ToArray();
		    while (needUpdateColons.Any(cell => cell) || needUpdateRows.Any(cell => cell))
		    {
			    for (var i = 0; i < crossword.Rows.Length; i++)
			    {
				    if (needUpdateRows[i])
				    {
					    UpdateLine(crossword.Rows[i]);
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
					    UpdateLine(crossword.Colons[j]);
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