using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace JapaneseCrossword
{
	class FileCrosswordWriter
	{
		private readonly string outputFilePath;

		public FileCrosswordWriter(string outputFilePath)
		{
			this.outputFilePath = outputFilePath;
		}

		public void Write(Crossword crossword)
		{
			using (var writer = new StreamWriter(outputFilePath))
			{
				foreach (var row in crossword.Rows)
				{
					writer.WriteLine(row.Cells.Select(CellToString).Aggregate((x, y) => x + y));
				}
			}
		}

		private string CellToString(Cell cell)
		{
			switch (cell)
			{
				case Cell.Empty:
					return ".";
				case Cell.Filled:
					return "*";
				case Cell.Unknown:
					return "?";
			}
			return "&";
		}
	}
}
