using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace JapaneseCrossword
{
	public class Crossword : ICloneable
	{
		public Line[] Rows { get; private set; }
		public Line[] Colons { get; private set; }

		public Crossword(Line[] rows, Line[] colons)
		{
			Rows = rows;
			Colons = colons;
		}
	
		public object Clone()
		{
			return new Crossword(Rows.Select(x => x).ToArray(), Colons.Select(x => x).ToArray());
		}
	}
}
