﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JapaneseCrossword
{
	public interface ICrosswordSolver
	{
		SolutionStatus Solve(string inputFilePath, string outputFilePath);
	}
}
