using GraphLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphLibrary.Models {
	/// <summary>
	/// Компонента связности
	/// </summary>
	public class ConnectedComponent<T> where T: struct {
		/// <summary>
		/// Вершины
		/// </summary>
		public List<Peak<T>> Peaks {
			get; private set;
		}
		/// <summary>
		/// Ребра
		/// </summary>
		public List<Rib<T>> Ribs {
			get;private set;
		}

		/// <summary>
		/// Конструктор
		/// </summary>
		public ConnectedComponent() {
			Peaks = new List<Peak<T>>();
			Ribs = new List<Rib<T>>();
		}

		#region Методы
		/// <summary>
		/// Добавить ребро в компоненту
		/// </summary>
		/// <param name="rib"></param>
		public void AddRib(Rib<T> rib) {
			if (ContainsRib(rib)) {
				return;
			}
			if (!ContainsPeak(rib.Peak1)) {
				Peaks.Add(rib.Peak1);
			}
			if (!ContainsPeak(rib.Peak2)) {
				Peaks.Add(rib.Peak2);
			}
			Ribs.Add(rib);
		}
		/// <summary>
		/// Добавить вершину
		/// </summary>
		/// <param name="peak"></param>
		/// <param name="rib"></param>
		public void AddPeak(Peak<T> peak, Rib<T> rib = null) {
			Peaks.Add(peak);
			if (rib != null) {
				AddRib(rib);
			}
		}

		/// <summary>
		/// Содержит ли компонента вершину
		/// </summary>
		/// <param name="peak"></param>
		/// <returns></returns>
		public bool ContainsPeak(Peak<T> peak) {
			return Peaks.Any(x => x == peak);
		}
		/// <summary>
		/// Содержит ли компонента ребро
		/// </summary>
		/// <param name="rib"></param>
		/// <returns></returns>
		public bool ContainsRib(Rib<T> rib) {
			return Ribs.Any(x => x == rib);
		}

		#endregion

		/// <summary>
		/// Оператор для объединения двух компонент
		/// </summary>
		/// <param name="c1"></param>
		/// <param name="c2"></param>
		/// <returns></returns>
		public static ConnectedComponent<T> operator +(ConnectedComponent<T> c1, ConnectedComponent<T> c2) {
			if (c1 == null) {
				return c2;
			}
			if (c2 == null) {
				return c1;
			}

			var res = new ConnectedComponent<T>();

			c1.Ribs.ForEach(x => {
				res.AddRib(x);
			});
			c2.Ribs.ForEach(x => {
				res.AddRib(x);
			});

			return res;
		}
	}
}
