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
		private int[,] triedToPlaceAt;

		private void UpdateCanBeArrays(List<int> blocks, int[] blockPositions, int blockNumber)
		{
			var position = blockPositions[blockNumber];
			var length = blocks[blockNumber];
			for (var i = position; i < position + length; i++)
			{
				canBeFilled[i] = true;
			}
			if (blockNumber == 0)
			{
				for (var i = 0; i < position; i++)
				{
					canBeEmpty[i] = true;
				}
			}
			else
			{
				var previousPosition = blockPositions[blockNumber - 1];
				var previousLength = blocks[blockNumber - 1];
				for (var i = previousPosition + previousLength; i < position; i++)
				{
					canBeEmpty[i] = true;
				}
			}
			if (blockNumber == blocks.Count - 1)
			{
				for (var i = position + length; i < canBeEmpty.Length; i++)
				{
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
				return !(line.Cells.Skip(cellNumber).Any(cell => cell == Cell.Filled));
			}
			if (triedToPlaceAt[cellNumber, blockNumber] != 0)
				return triedToPlaceAt[cellNumber, blockNumber] == 1;
			var currentBlockSize = line.Blocks[blockNumber];
			var rightBorder = line.Cells.Length - (line.Blocks.Skip(blockNumber).Sum() + line.Blocks.Count - blockNumber - 1);
			var result = false;
			for (var i = cellNumber; i <= rightBorder; i++)
			{
				if (CanPlaceBlockAtPosition(i, line.Cells, currentBlockSize))
				{
					blockPositions[blockNumber] = i;
					if (TryRecursiveFilling(line, blockPositions, blockNumber + 1, i + currentBlockSize + 1))
					{
						UpdateCanBeArrays(line.Blocks, blockPositions, blockNumber);
						result = true;
					}
				}
				if (line.Cells[i] == Cell.Filled)
					break;
			}
			triedToPlaceAt[cellNumber, blockNumber] = result ? 1 : -1;
			return result;
		}

		public void UpdateLine(Line line)
		{
			if (line.Blocks.Count == 0)
			{
				for (var i = 0; i < line.Cells.Length; i++)
					line.Cells[i] = Cell.Empty;
				return;
			}
			canBeFilled = new bool[line.Cells.Length];
			canBeEmpty = new bool[line.Cells.Length];
			triedToPlaceAt = new int[line.Cells.Length, line.Blocks.Count];
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
