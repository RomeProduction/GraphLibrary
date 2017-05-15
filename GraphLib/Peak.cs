using GraphLib.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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
		internal bool IsMark { get; set; }
		/// <summary>
		/// Полустепень исхода
		/// </summary>
		[DefaultValue(0)]
		public int SemiDegreeExodus { get; set; }
		/// <summary>
		/// Полустепень захода
		/// </summary>
		[DefaultValue(0)]
		public int SemiDegreeSunset { get; set; }
		/// <summary>
		/// Список смежных вершин
		/// </summary>
		public List<Peak<T>> NeighboursPeaks { get; set; }
		/// <summary>
		/// Список достижимых вершин
		/// </summary>
		public List<Peak<T>> AvailablePeaks { get; set; }

		#region Конструктор
		/// <summary>
		/// Конструктор
		/// </summary>
		public Peak() {
			IsMark = false;
			NeighboursPeaks = new List<Peak<T>>();
			AvailablePeaks = new List<Peak<T>>();
		}
		/// <summary>
		/// Конструктор
		/// </summary>
		/// <param name="value">Значение вершины</param>
		/// <param name="isOutgoing">Исходящая ли вершина(для ориентированных графов)</param>
		public Peak(T value, bool? isOutgoing = null) {
			IsMark = false;
			Value = value;
			if(isOutgoing!= null && isOutgoing.HasValue) {
				if(isOutgoing.Value) {
					SemiDegreeExodus++;
				} else {
					SemiDegreeSunset++;
				}
			}
			NeighboursPeaks = new List<Peak<T>>();
			AvailablePeaks = new List<Peak<T>>();
		}
		/// <summary>
		/// Конструктор
		/// </summary>
		/// <param name="value"></param>
		public Peak(string value, bool? isOutgoing = null) {
			IsMark = false;
			Value =(T)Convert.ChangeType(value, typeof(T));
			if(isOutgoing != null && isOutgoing.HasValue) {
				if(isOutgoing.Value) {
					SemiDegreeExodus++;
				} else {
					SemiDegreeSunset++;
				}
			}
			NeighboursPeaks = new List<Peak<T>>();
			AvailablePeaks = new List<Peak<T>>();
		}
		#endregion

		/// <summary>
		/// Добавить полустепень захода
		/// </summary>
		public void AddSemiDegreeSunset() {
			SemiDegreeSunset++;
		}
		/// <summary>
		/// Добавить полустепень исхода
		/// </summary>
		public void AddSemiDegreeExodus() {
			SemiDegreeExodus++;
		}
		/// <summary>
		/// Вершина является источником
		/// </summary>
		/// <returns></returns>
		public bool IsSource {
			get {
				return SemiDegreeSunset == 0;
			}
		}
		/// <summary>
		/// Вершина является стоком
		/// </summary>
		/// <returns></returns>
		public bool IsSink {
			get {
				return SemiDegreeExodus == 0;
			}
		}
		/// <summary>
		/// Получить степень
		/// </summary>
		public int Pow {
			get {
				return SemiDegreeExodus + SemiDegreeSunset;
			}
		}
		/// <summary>
		/// Снять метку
		/// </summary>
		public void UnMark() {
			IsMark = false;
		}
		/// <summary>
		/// Добавить достижимую вершину
		/// </summary>
		/// <param name="peak">Вершина</param>
		public void AddAvailablePeak(Peak<T> peak) {
			if (AvailablePeaks.Any(x => x == peak)) {
				return;
			}
			AvailablePeaks.Add(peak);
		}
		/// <summary>
		/// Добавить смежную вершину
		/// </summary>
		/// <param name="peak"></param>
		public void AddNeighbourPeak(Peak<T> peak) {
			if (NeighboursPeaks.Any(x => x == peak)) {
				return;
			}
			NeighboursPeaks.Add(peak);
		}

		#region operator
		/// <summary>
		/// Оператор сравнения двух вершин на равенство
		/// </summary>
		/// <param name="peak1"></param>
		/// <param name="peak2"></param>
		/// <returns></returns>
		public static bool operator == (Peak<T> peak1, Peak<T> peak2) {
			if(peak1 + "" == "" || peak2 + "" == "") {
				return false;
			}
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
