using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphLib.Models.Collection {
	/// <summary>
	/// Очередь
	/// </summary>
	/// <typeparam name="TValue"></typeparam>
	public class QueueApplication<TValue> : Queue<TValue>, IBag<TValue> where TValue: struct{
		public TValue PeekVal() {
			return Peek();
		}

		public void PushVal(TValue val) {
			Enqueue(val);
		}

		public TValue PopVal() {
			return Dequeue();
		}

		public int CountElems() {
			return Count;
		}
	}
}
