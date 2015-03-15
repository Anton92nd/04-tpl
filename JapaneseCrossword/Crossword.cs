﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace JapaneseCrossword
{
	public class Crossword
	{
		public Line[] Rows { get; private set; }
		public Line[] Colons { get; private set; }

		public Crossword(Line[] rows, Line[] colons)
		{
			Rows = rows;
			Colons = colons;
		}
	}
}
