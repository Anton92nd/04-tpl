using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
}
