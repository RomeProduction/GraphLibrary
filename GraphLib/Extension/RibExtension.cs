
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphLibrary {
	public static class RibExtension {
		/// <summary>
		/// Получить минимальное ребро для вершин из списка ребер
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="ribs">ребра</param>
		/// <param name="peak">вершина</param>
		/// <param name="ignorePeaks">игнорируемые вершины</param>
		/// <returns></returns>
		public static Rib<T> GetMinRibForPeak<T>(this IEnumerable<Rib<T>> ribs, 
			List<Peak<T>> peaks, IEnumerable<Rib<T>> ignoreRibs = null) where T : struct {
			if (ignoreRibs == null) {
				ignoreRibs = new List<Rib<T>>();
			}
			//Выбираем ребра где присутствует эта вершина и вторая вершина не входит в список игнорируемых
			ribs = ribs.Where(x => (peaks.Any(p => p == x.Peak1) 
				|| peaks.Any(p => p == x.Peak2)) && !ignoreRibs.Any(i => i == x )).OrderBy(x => x.Weight);

			return ribs.FirstOrDefault();
		}
	}
}
