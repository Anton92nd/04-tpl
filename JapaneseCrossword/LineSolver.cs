using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JapaneseCrossword
{
	public class LineSolver
	{
		private bool[] canBeFilled, canBeEmpty;

		private void UpdateCanBeArrays(List<int> blocks, int[] blockPositions)
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
			if (blocks.Count > 0)
			{
				for (var i = 0; i < blockPositions[0]; i++)
				{
					canBeEmpty[i] = true;
				}
				for (var i = blockPositions[blocks.Count - 1] + blocks[blocks.Count - 1]; i < canBeEmpty.Length; i++)
				{
					canBeEmpty[i] = true;
				}
			}
			else
			{
				for (var i = 0; i < canBeFilled.Length; i++)
				{
					canBeFilled[i] = false;
					canBeEmpty[i] = true;
				}
			}
		}

		bool CanPlaceBlockAtPosition(int position, Cell[] cells, int blockSize)
		{
			if (position + blockSize > cells.Count())
				return false;
			if (cells.Skip(position).Take(blockSize).Any(cell => cell == Cell.Empty))
				return false;
			return position + blockSize == cells.Count() || cells[position + blockSize] != Cell.Filled;
		}

		private bool TryRecursiveFilling(Line line, int[] blockPositions, int blockNumber = 0, int cellNumber = 0)
		{
			if (blockNumber == line.Blocks.Count)
			{
				if (line.Cells.Skip(cellNumber).Any(cell => cell == Cell.Filled))
					return false;
				UpdateCanBeArrays(line.Blocks, blockPositions);
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
					result = TryRecursiveFilling(line, blockPositions, blockNumber + 1, i + currentBlockSize + 1) || result;
				}
				if (line.Cells[i] == Cell.Filled)
					return result;
			}
			return result;
		}

		public void UpdateLine(Line line)
		{
			canBeFilled = new bool[line.Cells.Length];
			canBeEmpty = new bool[line.Cells.Length];
			var blockPositions = new int[line.Blocks.Count];
			if (!TryRecursiveFilling(line, blockPositions))
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
	}
}
