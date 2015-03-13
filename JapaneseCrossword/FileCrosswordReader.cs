using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.SqlServer.Server;

namespace JapaneseCrossword
{
	class FileCrosswordReader
	{
		private readonly string inputFilePath;

		public FileCrosswordReader(string inputFilePath)
		{
			this.inputFilePath = inputFilePath;
		}

		public Crossword Read()
		{
			int rowsCount = 0, colonsCount = 0;
			var lines = File.ReadAllLines(inputFilePath);
			var firstLine = lines[0].Split(':');
			var firstLinesCount = int.Parse(firstLine[1]);
			var firstIsRows = firstLine[0].Trim() == "rows";
			if (firstIsRows)
			{
				rowsCount = firstLinesCount;
			}
			else
			{
				colonsCount = firstLinesCount;
			}
			var secondLine = lines[firstLinesCount + 1].Split(':');
			var secondLinesCount = int.Parse(secondLine[1]);
			var secondIsRows = secondLine[0].Trim() == "rows";
			if (secondIsRows)
			{
				rowsCount = secondLinesCount;
			}
			else
			{
				colonsCount = secondLinesCount;
			}
			var rows = new Line[rowsCount];
			var colons = new Line[colonsCount];
			for (var i = 1; i <= firstLinesCount; i++)
			{
				var blocks = lines[i].Split(' ').Select(int.Parse).ToList();
				if (firstIsRows)
				{
					rows[i - 1] = new Line(blocks, new Cell[colonsCount]);
				}
				else
				{
					colons[i - 1] = new Line(blocks, new Cell[rowsCount]);
				}
			}
			for (var i = 1; i <= secondLinesCount; i++)
			{
				var blocks = lines[i + firstLinesCount + 1].Split(' ').Select(int.Parse).ToList();
				if (secondIsRows)
				{
					rows[i - 1] = new Line(blocks, new Cell[colonsCount]);
				}
				else
				{
					colons[i - 1] = new Line(blocks, new Cell[rowsCount]);
				}
			}
			CheckLines(rows);
			CheckLines(colons);
			return new Crossword(rows, colons);
		}

		private void CheckLines(Line[] lines)
		{
			if (lines.Any(line => line.Cells.Count() < line.Blocks.Sum() + line.Blocks.Count - 1))
			{
				throw new Exception("line has less cells than needed");
			}
		}
	}
}
