using System;
using System.Collections.Generic;
using System.ComponentModel;
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
		/// <summary>
		/// Вес ребра
		/// </summary>
		[DefaultValue(0)]
		public double Weight {
			get;
			set;
		}

		#region Конструкторы
		/// <summary>
		/// Конструктор
		/// </summary>
		/// <param name="peak1">1 смежная вершина, считается начальной</param>
		/// <param name="peak2">2 смежная верщина, считается конечной</param>
		public Rib(T peak1, T peak2, double weight) {
			Peak1 = new Peak<T>(peak1, true);
			Peak2 = new Peak<T>(peak2, false);
			Weight = weight;
		}
		/// <summary>
		/// Конструктор
		/// </summary>
		/// <param name="peak1">1 смежная вершина, считается начальной</param>
		/// <param name="peak2">2 смежная верщина, считается конечной</param>
		public Rib(string peak1, string peak2, double weight) {
			Peak1 = new Peak<T>((T)Convert.ChangeType(peak1, typeof(T)), true);
			Peak2 = new Peak<T>((T)Convert.ChangeType(peak2, typeof(T)), false);
			Weight = weight;
		}
		/// <summary>
		/// Конструктор
		/// </summary>
		/// <param name="peak1">1 смежная вершина, считается начальной</param>
		/// <param name="peak2">2 смежная верщина, считается конечной</param>
		public Rib(Peak<T> peak1, Peak<T> peak2, double weight) {
			if(peak1 == null || peak2 == null) {
				throw new ArgumentNullException("Вершина", "Одна из вершин при создании ребра равна null.");
			}
			Peak1 = peak1;
			Peak2 = peak2;
			Weight = weight;
			if(!Peak1.NeighboursPeaks.Any(x => x == Peak2)) {
				Peak1.NeighboursPeaks.Add(Peak2);
			}
			if(!Peak1.AvailablePeaks.Any(x => x == Peak2)) {
				Peak1.AvailablePeaks.Add(Peak2);
			}		
			if(!Peak2.NeighboursPeaks.Any(x => x == Peak1)) {
				Peak2.NeighboursPeaks.Add(Peak1);
			}
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
		public T? GetNeighboringPeakValue(T peak) {
			return GetNeighboringPeak(peak, false);
		}
		/// <summary>
		/// Возвращает смежную вершину из ребра или null 
		/// если переданная вершина не соответствует ни одной из концевых вершин
		/// </summary>
		/// <param name="peak">Вершина для которой ищется смежная</param>
		/// <returns></returns>
		public Peak<T> GetNeighboringPeak(Peak<T> peak) {
			if (peak == Peak1) {
				return Peak2;
			} else if (peak == Peak2) {
				return Peak1;
			}
			return null;
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
		/// <summary>
		/// Получить транспанированное ребро
		/// </summary>
		/// <returns></returns>
		public Rib<T> GetTransparentRib() {
			return new Rib<T>(new Peak<T>(Peak2.Value, true), new Peak<T>(Peak1.Value, false), Weight);
		}
		/// <summary>
		/// Является ли ребро безопасным
		/// </summary>
		/// <param name="peaksList">Список вершин одной компоненты связности</param>
		/// <returns></returns>
		public bool IsSafeRib(IEnumerable<Peak<T>> peaksList) {
			if (peaksList.Any(x => x == Peak1) && peaksList.Any(x => x == Peak2)) {
				return false;
			}
			return true;
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
			if (rib1 + "" == "" || rib2 + "" == "") {
				if (rib1 + "" == "" && rib2 + "" == "") {
					return true;
				}
				return false;
			}
			if(((rib1.Peak1 == rib2.Peak1
				&& rib1.Peak2 == rib2.Peak2) ||
				(rib1.Peak1 == rib2.Peak2
				&& rib1.Peak2 == rib2.Peak1) ||
				(rib1.Peak2 == rib2.Peak1
				&& rib1.Peak1 == rib2.Peak2)) && rib1.Weight == rib2.Weight) {
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
