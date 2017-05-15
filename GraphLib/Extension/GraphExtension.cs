using GraphLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphLibrary {
	/// <summary>
	/// Полезные методы при работе с графом
	/// </summary>
	public static class GraphExtension {
		/// <summary>
		/// Получить компоненты сильной связности
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="graf"></param>
		/// <param name="trGraph"></param>
		public static Dictionary<int, List<Peak<T>>> GetStrongСonnectivityСomponents<T>(this Grapf<T> graf, Grapf<T> trGraph = null) where T : struct {
			if (trGraph == null) {
				trGraph = graf.GetTransparentGrapf();
			}

			Dictionary<int, List<Peak<T>>> result = new Dictionary<int, List<Peak<T>>>();

			var peaks = graf.PeaksList.OrderBy(x => x.IsSource).ToList();
			var trPeaks = trGraph.PeaksList.OrderBy(x => x.IsSource).ToList();

			peaks.ClearMark();
			trPeaks.ClearMark();

			int num = 0;

			while (peaks.Count != 0) {
				var peak = peaks.FirstOrDefault();
				var trPeak = trPeaks.FirstOrDefault(x => x == peak);
				if (trPeak == null) {
					continue;
				}
				var res = peak.CrossAvailablePeaks(trPeak);
				if (res == null) {
					continue;
				}

				result.Add(num, res);
				peaks = peaks.Where(x => !res.Any(r => r == x)).ToList();
				num++;
				
			}

			return result;
		}
	}
}
