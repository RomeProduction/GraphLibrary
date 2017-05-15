
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphLibrary {
	/// <summary>
	/// Полезные методы для работы с вершинами
	/// </summary>
	public static class PeakExtension{
		/// <summary>
		/// Снять выделение со всех вершин в списке
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="peaks"></param>
		public static void ClearMark<T>(this List<Peak<T>> peaks)  where T: struct {
			foreach (var peak in peaks) {
				peak.UnMark();
			}
		}

		/// <summary>
		/// Получить пересечение достижимых вершин
		/// </summary>
		/// <typeparam name="T">Тип вершины</typeparam>
		/// <param name="peak">Вершина</param>
		/// <param name="peak2">Вершина2</param>
		public static List<Peak<T>> CrossAvailablePeaks<T>(this Peak<T> peak, Peak<T> peak2) where T : struct {
			var result = new List<Peak<T>>();

			result.Add(peak);

			List<Peak<T>> avPeaks = new List<Peak<T>>();
			List<Peak<T>> avPeaks2 = new List<Peak<T>>();

			avPeaks = peak.GetAllAvailablePeaks(avPeaks);
			avPeaks2 = peak2.GetAllAvailablePeaks(avPeaks2);

			foreach (var p in avPeaks) {
				if (avPeaks2.Any(x => x == p)) {
					if (!result.Any(x => x == p)) {
						result.Add(p);
					}
				}
			}

			if (result.Count == 1) {
				return null;
			}
			return result;
		}

		/// <summary>
		/// Получить разность достижимых вершин (получим только те что не входят в метаграф)
		/// </summary>
		/// <typeparam name="T">Тип вершины</typeparam>
		/// <param name="peak">Вершина</param>
		/// <param name="peak2">Вершина2</param>
		public static List<Peak<T>> DifferenceAvailablePeaks<T>(this Peak<T> peak, Peak<T> peak2) where T : struct {
			var result = new List<Peak<T>>();

			result.Add(peak);

			List<Peak<T>> avPeaks = new List<Peak<T>>();
			List<Peak<T>> avPeaks2 = new List<Peak<T>>();

			avPeaks = peak.GetAllAvailablePeaks(avPeaks);
			avPeaks2 = peak2.GetAllAvailablePeaks(avPeaks2);

			foreach (var p in avPeaks) {
				if (!avPeaks2.Any(x => x == p)) {
					if (!result.Any(x => x == p)) {
						result.Add(p);
					}
				}
			}

			if (result.Count == 0) {
				return null;
			}

			return result;
		}
		/// <summary>
		/// Получить все достижимые вершины
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="peak"></param>
		/// <returns></returns>
		public static List<Peak<T>> GetAllAvailablePeaks<T>(this Peak<T> peak, List<Peak<T>> res) where T : struct {
		
			foreach (var p in peak.AvailablePeaks) {
				if (res.Any(x => x == p)){
					continue;
				}
				res.Add(p);
				if (p.AvailablePeaks.Count > 0) {
					res = p.GetAllAvailablePeaks(res);
				}
			}

			return res;
		}
	}
}
