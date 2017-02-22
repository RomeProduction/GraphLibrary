using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphLibrary {
	/// <summary>
	/// Ребро
	/// </summary>
	/// <typeparam name="T">простой тип</typeparam>
	public class Rib<T> where T : struct {
		/// <summary>
		/// Вершина
		/// </summary>
		public T Peak1 { get; set; }
		/// <summary>
		/// Вершина
		/// </summary>
		public T Peak2 { get; set; }

		#region Конструкторы
		/// <summary>
		/// Конструктор
		/// </summary>
		/// <param name="peak1"></param>
		/// <param name="peak2"></param>
		public Rib(T peak1, T peak2) {
			Peak1 = peak1;
			Peak2 = peak2;
		}
		/// <summary>
		/// Конструктор
		/// </summary>
		/// <param name="peak1"></param>
		/// <param name="peak2"></param>
		public Rib(string peak1, string peak2) {
			Peak1 = (T)Convert.ChangeType(peak1, typeof(T));
			Peak2 = (T)Convert.ChangeType(peak2, typeof(T));
		}
		/// <summary>
		/// Пустой конструктор
		/// </summary>
		public Rib() {
		}
		#endregion

		#region Методы
		/// <summary>
		/// Является ли ребро петлей
		/// </summary>
		/// <returns></returns>
		public bool IsLoop() {
			return EqualityComparer<T>.Default.Equals(Peak1, Peak2);
		}


		#endregion

		#region Operators
		/// <summary>
		/// Оператор сравнения на равенство
		/// </summary>
		/// <param name="rib1"></param>
		/// <param name="rib2"></param>
		/// <returns></returns>
		public static bool operator ==(Rib<T> rib1, Rib<T> rib2) {
			if(rib1 == null || rib2 == null)
				return false;
			if((EqualityComparer<T>.Default.Equals(rib1.Peak1, rib2.Peak1) 
				&& EqualityComparer<T>.Default.Equals(rib1.Peak2, rib2.Peak2)) ||
				(EqualityComparer<T>.Default.Equals(rib1.Peak1, rib2.Peak2)
				&& EqualityComparer<T>.Default.Equals(rib1.Peak2, rib2.Peak1)) ||
				(EqualityComparer<T>.Default.Equals(rib1.Peak2, rib2.Peak1)
				&& EqualityComparer<T>.Default.Equals(rib1.Peak1, rib2.Peak2))) {
				return true;
			}
			return false;
		}
		/// <summary>
		/// Оператор для сравнения на неравенство
		/// </summary>
		/// <param name="rib1"></param>
		/// <param name="rib2"></param>
		/// <returns></returns>
		public static bool operator !=(Rib<T> rib1, Rib<T> rib2) {
			return !(rib1 == rib2);
		}

			#endregion
		}
}
