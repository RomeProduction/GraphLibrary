using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphLibrary {
	/// <summary>
	/// Часть для работы с ориентированным графом
	/// </summary>
	public partial class Grapf<T> {
		/// <summary>
		/// Получить список смежности
		/// </summary>
		/// <returns></returns>
		public Dictionary<T, IEnumerable<T>> GetListNeighboringForOrientGraph() {
			Dictionary<T, IEnumerable<T>> result = new Dictionary<T, IEnumerable<T>>();
			foreach (var peak in PeaksList) {
				result.Add(peak.Value,
					GetListNeighboringForOrientGraph(peak.Value));
			}
			return result;
		}

		/// <summary>
		/// Получить список смежности для ориентированного графа
		/// </summary>
		/// <param name="peak">название искомой вершины</param>
		/// <returns></returns>
		public List<T> GetListNeighboringForOrientGraph(T peak) {
			return Ribs.Where(x => x.GetNeighboringPeak(peak, true).HasValue)
				.Select(x => x.GetNeighboringPeak(peak, true).Value).ToList();
		}

		/// <summary>
		/// Получить транспонированный граф
		/// </summary>
		/// <returns></returns>
		public Grapf<T> GetTransparentGrapf() {
			var tRibs = Ribs.Select(x => x.GetTransparentRib());
			return new Grapf<T>(PeakCount, true, tRibs.ToArray());
		}
		/// <summary>
		/// Получить метаграф
		/// </summary>
		/// <returns></returns>
		public Grapf<int> GetMetagraf() {
			var dic = this.GetStrongСonnectivityСomponents();

			if (dic.Count == 0) {
				throw new Exception("Нельзя построить метаграф без наличия компонент сильной связности.");
			}

			var mGrapf = new Grapf<int>();

			//Создаем вершины, на 1 больше чем номер компоненты связности
			foreach (var key in dic.Keys) {
				mGrapf.PeaksList.Add(new Peak<int>() {
					Value = key + 1
				});
			}
			//получаем ребра между вершинами метаграфа 
			var ribs = new List<Rib<T>>();
			for (int i = 0; i < dic.Keys.Count; i++) {
				var iKey = dic.Keys.ElementAt(i);
				var iPeaks = dic[iKey++];
				for (int j = 0; j < dic.Keys.Count; j++) {
					if (i == j) {
						continue;
					}
					var jKey = dic.Keys.ElementAt(j);
					var jPeaks = dic[jKey++];

					var rs = Ribs.Where(x => (iPeaks.Any(ip => ip == x.Peak1) && jPeaks.Any(jp => jp == x.Peak2)));
					var rsTr = Ribs.Where(x => (iPeaks.Any(ip => ip == x.Peak2) && jPeaks.Any(jp => jp == x.Peak1)));

					foreach (var rib in rs) {
						var p1 = mGrapf.PeaksList.FirstOrDefault(x => x == iKey);					
						var p2 = mGrapf.PeaksList.FirstOrDefault(x => x == jKey);
						
						var tRib = new Rib<int>(p1, p2, rib.Weight);
						if (!mGrapf.Ribs.Any(x => x == tRib)) {
							p1.AddSemiDegreeExodus();
							p2.AddSemiDegreeSunset();
							mGrapf.Ribs.Add(new Rib<int>(p1, p2, rib.Weight));
						}
						
					}

					foreach (var rib in rsTr) {
						var p1 = mGrapf.PeaksList.FirstOrDefault(x => x == jKey);
						var p2 = mGrapf.PeaksList.FirstOrDefault(x => x == iKey);
						
						var tRib = new Rib<int>(p1, p2, rib.Weight);
						if (!mGrapf.Ribs.Any(x => x == tRib)) {
							p1.AddSemiDegreeExodus();
							p2.AddSemiDegreeSunset();
							mGrapf.Ribs.Add(new Rib<int>(p1, p2, rib.Weight));
						}
					}
				}
			}

			return mGrapf;
		}
	}
}
