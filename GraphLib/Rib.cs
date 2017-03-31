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
		public Peak<T> Peak1 { get; set; }
		/// <summary>
		/// Вершина
		/// </summary>
		public Peak<T> Peak2 { get; set; }

		#region Конструкторы
		/// <summary>
		/// Конструктор
		/// </summary>
		/// <param name="peak1">1 смежная вершина, считается начальной</param>
		/// <param name="peak2">2 смежная верщина, считается конечной</param>
		public Rib(T peak1, T peak2) {
			Peak1 = new Peak<T>(peak1, true);
			Peak2 = new Peak<T>(peak2, false);
		}
		/// <summary>
		/// Конструктор
		/// </summary>
		/// <param name="peak1">1 смежная вершина, считается начальной</param>
		/// <param name="peak2">2 смежная верщина, считается конечной</param>
		public Rib(string peak1, string peak2) {
			Peak1 = new Peak<T>((T)Convert.ChangeType(peak1, typeof(T)), true);
			Peak2 = new Peak<T>((T)Convert.ChangeType(peak2, typeof(T)), false);
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
			return Peak1 == Peak2;
		}
		/// <summary>
		/// Возвращает смежную вершину из ребра или null 
		/// если переданная вершина не соответствует ни одной из концевых вершин
		/// </summary>
		/// <param name="peak">Вершина для которой ищется смежная</param>
		/// <returns></returns>
		public T? GetNeighboringPeak(T peak) {
			return GetNeighboringPeak(peak, false);
		}
		/// <summary>
		/// Возвращает смежную вершину из ребра или null 
		/// если переданная вершина не соответствует ни одной из концевых вершин
		/// </summary>
		/// <param name="peak">Вершина для которой ищется смежная</param>
		/// <param name="isOrGraph">Обозначает, что нужно учитывать направленность для ориентированных графов</param>
		/// <returns></returns>
		public T? GetNeighboringPeak(T peak, bool isOrGraph) {
			if(peak == Peak1) {
				return Peak2.Value;
			} else if(peak == Peak2 && !isOrGraph) {
				return Peak1.Value;
			}
			return null;
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
			if(rib1 + "" == "" || rib2 + "" == "")
				return false;
			if((rib1.Peak1 == rib2.Peak1
				&& rib1.Peak2 == rib2.Peak2) ||
				(rib1.Peak1 == rib2.Peak2
				&& rib1.Peak2 == rib2.Peak1) ||
				(rib1.Peak2 == rib2.Peak1
				&& rib1.Peak1 == rib2.Peak2)) {
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
