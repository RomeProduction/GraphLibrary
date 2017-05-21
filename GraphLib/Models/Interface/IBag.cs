using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphLib.Models {
	/// <summary>
	/// Интерфейс для создания мешка
	/// </summary>
	public interface IBag<TValue> where TValue: struct {
		/// <summary>
		/// Положить объект в мешок
		/// </summary>
		/// <param name="val"></param>
		void PushVal(TValue val);
		/// <summary>
		/// Вытащить и удалить элемент из мешка
		/// </summary>
		/// <returns></returns>
		TValue PopVal();
		/// <summary>
		/// Вытащить и не удалять объект из мешка
		/// </summary>
		/// <returns></returns>
		TValue PeekVal();
		/// <summary>
		/// Количество эелементов
		/// </summary>
		/// <returns></returns>
		int CountElems();
	}
}
