﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JapaneseCrossword
{
	public class Line
	{
		public List<int> Blocks { get; private set; }
		public Cell[] Cells { get; private set; }

		public Line(List<int> blocks, Cell[] cells)
		{
			Blocks = blocks;
			Cells = cells;
		}
	}
}
