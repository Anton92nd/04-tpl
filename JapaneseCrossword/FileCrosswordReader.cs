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
		private List<List<int>> GetListArray(string[] lines, int count)
		{
			return lines.Skip(1).Take(count)
				.Select(line => line.Split(' ').Select(int.Parse).ToList())
				.ToList();
		}

		public Crossword Read(string inputFilePath)
		{
			var lines = File.ReadAllLines(inputFilePath);

			var firstLine = lines[0].Split(':');
			var firstLinesCount = int.Parse(firstLine[1]);
			var firstList = GetListArray(lines, firstLinesCount);

			lines = lines.Skip(firstLinesCount + 1).ToArray();

			var secondLine = lines[0].Split(':');
			var secondLinesCount = int.Parse(secondLine[1]);
			var secodnList = GetListArray(lines, secondLinesCount);

			var firstArray = firstList.Select(line => new Line(line, new Cell[secondLinesCount])).ToArray();
			var secondArray = secodnList.Select(line => new Line(line, new Cell[firstLinesCount])).ToArray();
			CheckLines(firstArray);
			CheckLines(secondArray);
			return (firstLine[0].Trim() == "rows")
				? new Crossword(firstArray, secondArray)
				: new Crossword(secondArray, firstArray);
		}

		private void CheckLines(Line[] lines)
		{
			if (lines.Any(line => line.Cells.Count() < line.Blocks.Sum() + line.Blocks.Count - 1))
			{
				throw new MyException("line has less cells than needed");
			}
		}
	}
}
