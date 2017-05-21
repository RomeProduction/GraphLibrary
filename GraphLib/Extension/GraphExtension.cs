using GraphLib.Models;
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
		/// <summary>
		/// Алгоритм Прима получения минимального остовного дерева
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="graf"></param>
		/// <returns></returns>
		public static List<Rib<T>> AlgoritmPrima<T>(this Grapf<T> graf) where T : struct {
			List<Rib<T>> result = new List<Rib<T>>();
			List<Peak<T>> peaks = new List<Peak<T>>();

			var peak = graf.PeaksList.FirstOrDefault();
			if (peak == null) {
				return null;
			}

			peaks.Add(peak);

			while (peaks.Count != graf.PeakCount) {

				var minRib = graf.Ribs.Where(x => (peaks.Any(p => p == x.Peak1)
					&& !peaks.Any(p => p == x.Peak2))
					|| (!peaks.Any(p => p == x.Peak1)
					&& peaks.Any(p => p == x.Peak2))).OrderBy(x => x.Weight).FirstOrDefault();

				if (minRib == null) {
					break;
				}

				result.Add(minRib);
				if (peaks.Any(x => x == minRib.Peak2)) {
					peaks.Add(minRib.Peak1);
				} else {
					peaks.Add(minRib.Peak2);
				}

			}

			return result;
		}

		/// <summary>
		/// Алгоритм Крускала получения минимального остовного дерева
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="graf"></param>
		/// <returns></returns>
		public static List<Rib<T>> AlgoritmKruskal<T>(this Grapf<T> graf) where T : struct {
			List<Rib<T>> result = new List<Rib<T>>();
			List<Rib<T>> ignore = new List<Rib<T>>();
			List<ConnectedComponent<T>> components = new List<ConnectedComponent<T>>();

			var rib = graf.Ribs.OrderBy(x => x.Weight).FirstOrDefault();
			if (rib == null) {
				return null;
			}

			ConnectedComponent<T> component = new ConnectedComponent<T>();

			component.AddRib(rib);

			components.Add(component);

			result.Add(rib);

			while (component.Peaks.Count != graf.PeakCount) {

				var ribs = graf.Ribs.Where(x => !result.Any(r => r == x) && !ignore.Any(r => r == x)).OrderBy(x => x.Weight);

				Rib<T> minRib = null;

				foreach (var r in ribs) {
					if (r.IsSafeRib(component.Peaks)) {
						minRib = r;
						break;
					} else {
						ignore.Add(r);
					}
				}
				if (!component.ContainsPeak(minRib.Peak1) &&
					!component.ContainsPeak(minRib.Peak2)) {
					//Если компонента не содержит ни одну из вершин, то пытаемся найти компоненту которая их содержит
					var componentPeak1 = components.FirstOrDefault(x => x.ContainsPeak(minRib.Peak1));
					var componentPeak2 = components.FirstOrDefault(x => x.ContainsPeak(minRib.Peak2));
					if (componentPeak1 == null && componentPeak2 == null) {
						component = new ConnectedComponent<T>();
						component.AddRib(minRib);
						// если нет компонент содержащих обе вершины
					} else if ((componentPeak1 == null && componentPeak2 != null)
						|| (componentPeak1 != null && componentPeak2 == null)) {
						components.Remove(componentPeak1);
						components.Remove(componentPeak2);
						component = componentPeak1 + componentPeak2;
						component.AddRib(minRib);
					} else {
						components.Remove(componentPeak1);
						components.Remove(componentPeak2);
						component = componentPeak1 + componentPeak2;
					}
					components.Add(component);
				} else {
					//Если он уже содержит вершину, то проверяем нет ли второй вершины в другой компоненте
					ConnectedComponent<T> comp = null;
					if (!component.ContainsPeak(minRib.Peak1)) {
						comp = components.FirstOrDefault(x => x.ContainsPeak(minRib.Peak1));
					}
					if (!component.ContainsPeak(minRib.Peak2)) {
						comp = components.FirstOrDefault(x => x.ContainsPeak(minRib.Peak2));
					}

					if (comp != null) {
						components.Remove(comp);
						components.Remove(component);
						//если таковая компонента нашлась, объединяем их					
						component = component + comp;
					}

					components.Remove(component);
					component.AddRib(minRib);
					components.Add(component);
				}

				if (minRib == null) {
					break;
				}

				ignore.Add(minRib);

			}

			return component.Ribs;
		}

		/// <summary>
		/// Алгоритм Борувки получения минимального остовного дерева
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="graf"></param>
		/// <returns></returns>
		public static List<Rib<T>> AlgoritmBoruvki<T>(this Grapf<T> graf) where T : struct {
			List<Rib<T>> ignore = new List<Rib<T>>();
			List<ConnectedComponent<T>> components = new List<ConnectedComponent<T>>();

			graf.PeaksList.ForEach(x => {
				var c = new ConnectedComponent<T>();
				c.AddPeak(x);
				components.Add(c);

			});

			while (components.Count > 1) {
				for (int i = 0; i < components.Count; i++) {
					var c = components[i];
					for (int j = 0; j < c.Peaks.Count; j++) {
						var minRib = graf.Ribs.GetMinRibForPeak(c.Peaks, ignore);
						if (minRib == null) {
							continue;
						}
						var secondPeak = minRib.GetNeighboringPeak(minRib.Peak1);
						if (secondPeak == null || c.ContainsPeak(secondPeak)) {
							secondPeak = minRib.GetNeighboringPeak(minRib.Peak2);
						}

						if (!minRib.IsSafeRib(c.Peaks)) {
							ignore.Add(minRib);
							continue;
						}

						var componentPeak2 = components.FirstOrDefault(x => x.ContainsPeak(secondPeak));
						if (componentPeak2 != null) {

							//Компонента уже может содержать обе вершины, тогдаэто ребро не является безопасным
							if (!minRib.IsSafeRib(componentPeak2.Peaks)) {
								
								ignore.Add(minRib);
								continue;
							}

							components.Remove(componentPeak2);
							components.Remove(c);
							c.AddRib(minRib);
							ignore.Add(minRib);
							c = c + componentPeak2;

							components.Insert(i, c);
							//break;
						}
					}

				}


			}


			return components[0].Ribs;

		}
	}
}
