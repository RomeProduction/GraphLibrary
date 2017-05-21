using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphLib.Models {
	/// <summary>
	/// Интерфейс для создания мешка
	/// </summary>
	public interface IBag<TPriority, TValue> : IBag<TValue> where TValue : struct 
		where TPriority: struct{
		/// <summary>
		/// Положить объект в мешок
		/// </summary>
		/// <param name="val"></param>
		void PushVal(TPriority priority, TValue val);
	}
}
