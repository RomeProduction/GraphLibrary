using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphLibrary {
	/// <summary>
	/// Класс Вершины
	/// </summary>
	/// <typeparam name="T">простой тип</typeparam>
	public class Peak<T> where T:struct {
		/// <summary>
		/// Значение вершины
		/// </summary>
		public T Value { get; set; }
		/// <summary>
		/// Помечена ли вершина
		/// </summary>
		public bool IsMark { get; set; }

		#region Конструктор
		/// <summary>
		/// Конструктор
		/// </summary>
		public Peak() {
			IsMark = false;
		}
		/// <summary>
		/// Конструктор
		/// </summary>
		/// <param name="value"></param>
		public Peak(T value) {
			IsMark = false;
			Value = value;
		}
		/// <summary>
		/// Конструктор
		/// </summary>
		/// <param name="value"></param>
		public Peak(string value) {
			IsMark = false;
			Value =(T)Convert.ChangeType(value, typeof(T));
		}
		/// <summary>
		/// Конструктор
		/// </summary>
		/// <param name="value"></param>
		/// <param name="isMark"></param>
		public Peak(T value, bool isMark) {
			IsMark = isMark;
			Value = value;
		}
		#endregion


		#region operator
		/// <summary>
		/// Оператор сравнения двух вершин на равенство
		/// </summary>
		/// <param name="peak1"></param>
		/// <param name="peak2"></param>
		/// <returns></returns>
		public static bool operator == (Peak<T> peak1, Peak<T> peak2) {
			return EqualityComparer<T>.Default.Equals(peak1.Value, peak2.Value);
		}
		/// <summary>
		/// Сравнение с значением вершины на равенство
		/// </summary>
		/// <param name="peak"></param>
		/// <param name="value"></param>
		/// <returns></returns>
		public static bool operator == (Peak<T> peak, T value) {
			return EqualityComparer<T>.Default.Equals(peak.Value, value);
		}
		/// <summary>
		/// Сравнение с значением вершины на неравенство
		/// </summary>
		/// <param name="peak"></param>
		/// <param name="value"></param>
		/// <returns></returns>
		public static bool operator !=(Peak<T> peak, T value) {
			return !(peak == value);
		}

		/// <summary>
		/// Сравнение с значением вершины на равенство
		/// </summary>
		/// <param name="peak"></param>
		/// <param name="value"></param>
		/// <returns></returns>
		public static bool operator == (T value, Peak<T> peak) {
			return EqualityComparer<T>.Default.Equals(peak.Value, value);
		}
		/// <summary>
		/// Сравнение с значением вершины на неравенство
		/// </summary>
		/// <param name="peak"></param>
		/// <param name="value"></param>
		/// <returns></returns>
		public static bool operator != (T value, Peak<T> peak) {
			return !(peak == value);
		}

		public static bool operator != (Peak<T> peak1, Peak<T> peak2) {
			return !(peak1 == peak2);
		}

		#endregion
	}
}
