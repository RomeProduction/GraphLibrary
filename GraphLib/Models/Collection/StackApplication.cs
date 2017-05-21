using GraphLib.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphLibrary.Models.Collection {
	/// <summary>
	/// Стек
	/// </summary>
	/// <typeparam name="TValue"></typeparam>
	public class StackApplication<TValue> : Stack<TValue>, IBag<TValue> where TValue : struct {
		public TValue PeekVal() {
			return Peek();
		}

		public TValue PopVal() {
			return Pop();
		}

		public void PushVal(TValue val) {
			Push(val);
		}

		public int CountElems() {
			return Count;
		}
	}
}
