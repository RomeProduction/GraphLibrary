﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Data;

namespace GraphLibrary {
	/// <summary>
	/// Класс реализации графа
	/// </summary>
	/// <typeparam name="T">простой тип вроде (int,string)</typeparam>
	public partial class Grapf<T> where T : struct {
		#region Переменные и константы
		/// <summary>
		/// Количество вершин
		/// </summary>
		private int _peakCount = 0;
		/// <summary>
		/// Количество вершин
		/// </summary>
		public int PeakCount {
			get {
				return _peakCount;
			}
		}
		/// <summary>
		/// Количество ребер
		/// </summary>
		private int _ribsCount = 0;
		/// <summary>
		/// Количество ребер
		/// </summary>
		public int RibsCount {
			get {
				return _ribsCount;
			}
		}
		/// <summary>
		/// Ребра
		/// </summary>
		private List<Rib<T>> _ribs = new List<Rib<T>>();
		/// <summary>
		/// Список ребер
		/// </summary>
		public List<Rib<T>> Ribs {
			get {
				return _ribs;
			}
		}
		/// <summary>
		/// Список вершин
		/// </summary>
		private List<Peak<T>> _peaks = new List<Peak<T>>();
		/// <summary>
		/// Список вершин
		/// </summary>
		public List<Peak<T>> PeaksList {
			get {
				return _peaks;
			}
		}
		/// <summary>
		/// Флаг означающий, что имена вершин упорядочены в порядке от 1 до n
		/// </summary>
		public bool IsOrderedPeak {
			get {
				return _isOrderedPeak;
			}
			set {
				_isOrderedPeak = value;
			}
		}
		/// <summary>
		/// Флаг упорядоченности вершин
		/// </summary>
		private bool _isOrderedPeak = false;

		#endregion

		#region Конструкторы
		/// <summary>
		/// Конструктор
		/// </summary>
		/// <param name="countPeak">Количество вершин</param>
		/// <param name="ribsArray">Массив ребер</param>
		public Grapf(int countPeak, bool isOrderedPeak, params Rib<T>[] ribsArray) {
			_peakCount = countPeak;
			_ribsCount = ribsArray.Length;
			_isOrderedPeak = isOrderedPeak;
			_ribs = ribsArray.ToList();
			foreach (var rib in Ribs) {
				var peak1 = new Peak<T>(rib.Peak1.Value, true);
				var peak2 = new Peak<T>(rib.Peak2.Value, false);
				if (!PeaksList.Any(x => x == rib.Peak1)) {
					_peaks.Add(peak1);
					rib.Peak1 = peak1;
				} else {
					rib.Peak1 = PeaksList.FirstOrDefault(x => x == rib.Peak1);
				}
				if (!PeaksList.Any(x => x == rib.Peak2)) {
					_peaks.Add(peak2);
					rib.Peak2 = peak2;
				} else {
					rib.Peak2 = PeaksList.FirstOrDefault(x => x == rib.Peak2);
				}
				peak1.AddSemiDegreeExodus();
				peak1.AddAvailablePeak(peak2);
				peak1.AddNeighbourPeak(peak2);
				peak2.AddSemiDegreeSunset();
				peak2.AddNeighbourPeak(peak1);
			}
			_peaks = _peaks.OrderBy(x => x.Value).ToList();
			foreach (var rib in Ribs) {
				rib.Peak1.AddAvailablePeak(rib.Peak2);
				rib.Peak1.AddNeighbourPeak(rib.Peak2);
				rib.Peak2.AddNeighbourPeak(rib.Peak1);
			}

			CheckPeaks(isOrderedPeak);

		}
		/// <summary>
		/// Пустой конструктор
		/// </summary>
		public Grapf() {

		}
		/// <summary>
		/// Конструктор с путем к файлу
		/// </summary>
		/// <param name="filePath">Путь к файлу</param>
		/// <param name="isOrderedPeak">если наименования вершин упорядочены, то вместо ошибки будут добавлены автоматически</param>
		/// <remarks>Файл должен иметь формат первая строка количество вершин, далее все строки ребра</remarks>
		public Grapf(string filePath, bool isOrderedPeak) {
			if (!File.Exists(filePath)) {
				new System.IO.FileNotFoundException("По заданному пути файл не найден");
			}
			_isOrderedPeak = isOrderedPeak;
			var lines = File.ReadAllLines(filePath);
			if (lines.Length == 0) {
				new ArgumentException("Файл пустой");
			}
			if (!int.TryParse(lines[0], out _peakCount)) {
				new ArgumentException("Первой строкой в файле должно быть количество вершин, целое число.");
			}

			_ribsCount = lines.Length - 2;

			_ribs = new List<Rib<T>>();
			for (int i = 1; i < lines.Length; i++) {
				var peaks = lines[i].Split(' ');
				if ((peaks.Length < 2 || peaks.Length > 2) && peaks.Length != 3) {
					new ArgumentException("Строки с ребрами должны содержать две вершины, разделенные пробелом, через пробел также может быть введен вес ребра");
				}
				var peak1 = new Peak<T>(peaks[0], true);
				var peak2 = new Peak<T>(peaks[1], false);
				double weight = 0;
				if (peaks.Length == 3) {
					peaks[2] = peaks[2].Replace(',', '.');
					double.TryParse(peaks[2], out weight);
				}

				//Попутно с перебором ребер создаем список вершин
				if (PeaksList.Any(x => x == peak1)) {
					var peak = PeaksList.FirstOrDefault(x => x == peak1);
					if (peak != null) {
						peak.AddSemiDegreeExodus();
						peak1 = peak;
					}
				}
				if (PeaksList.Any(x => x == peak2)) {
					var peak = PeaksList.FirstOrDefault(x => x == peak2);
					if (peak != null) {
						peak.AddSemiDegreeSunset();
						peak2 = peak;
					}
				}
				if (!PeaksList.Any(x => x == peak1)) {
					_peaks.Add(peak1);
				}
				if (!PeaksList.Any(x => x == peak2)) {
					_peaks.Add(peak2);
				}
				_ribs.Add(new Rib<T>(peak1, peak2, weight));
			}

			CheckPeaks(isOrderedPeak);

			_peaks = _peaks.OrderBy(x => x.Value).ToList();
		}

		/// <summary>
		/// Проверяет на правильность введенных вершин
		/// </summary>
		/// <param name="isOrderedPeaks"></param>
		private void CheckPeaks(bool isOrderedPeaks) {
			if (isOrderedPeaks) {
				for (int i = 1; i < PeakCount + 1; i++) {
					var peak = new Peak<T>(i + "");
					if (!PeaksList.Any(x => x == (T)Convert.ChangeType(i, typeof(T)))) {
						PeaksList.Insert(i - 1, new Peak<T>(i + ""));
					}
				}
				return;
			}

			if (_peaks.Count > _peakCount) {
				throw new Exception("Получено большее количество вершин из ребер, чем передано.");
			} else if (_peaks.Count < _peakCount) {
				throw new Exception("Вершин из ребер получено меньше, чем передано.");
			}


		}

		#endregion

		#region Методы
		/// <summary>
		/// Получаем все вершины у которых есть петли
		/// </summary>
		/// <returns></returns>
		public IEnumerable<T> GetAllPeakLoop() {
			return Ribs.Where(x => x.IsLoop()).Select(x => x.Peak1.Value).Distinct();
		}
		/// <summary>
		/// Получаем все вершины у которых есть петли с кратностью
		/// </summary>
		/// <returns>Возвращает словарь в котором по вершине можно получить кратность</returns>
		public Dictionary<T, int> GetAllPeakLoopWithPow() {
			Dictionary<T, int> result = new Dictionary<T, int>();
			var list = GetAllPeakLoop();
			foreach (var peak in list) {
				result.Add(peak, Ribs.Count(x => x.IsLoop()
					&& x.Peak1 == peak));
			}
			return result;
		}
		/// <summary>
		/// Получить список кратных ребер
		/// </summary>
		/// <returns></returns>
		public IEnumerable<Rib<T>> GetAllFoldRib() {
			List<Rib<T>> result = new List<Rib<T>>();
			foreach (var rib in Ribs) {
				result.AddRange(Ribs.Where(x => x == rib && !result.Any(r => r == x)));
			}
			return result;
		}
		/// <summary>
		/// Получить компоненты связности
		/// </summary>
		/// <returns></returns>
		public List<List<T>> GetConnectedComponents(bool isOrientGraph) {
			List<List<T>> result = new List<List<T>>();
			foreach (var peak in PeaksList) {
				if (!result.Any(x => x.Contains(peak.Value))) {
					result.Add(isOrientGraph ? GetConnectedComponentNotOrder(peak.Value, isOrientGraph)
						: GetConnectedComponent(peak.Value, isOrientGraph));
				}
			}

			return result;
		}
		/// <summary>
		/// Получить компонент связности по вершине
		/// </summary>
		/// <param name="peak">Вершина</param>
		/// <returns></returns>
		public List<T> GetConnectedComponent(T peak, bool isOrientGraph) {
			return RecursiveDetour(peak, true, isOrientGraph);
		}
		/// <summary>
		/// Получить компонент связности по вершине
		/// </summary>
		/// <param name="peak">Вершина</param>
		/// <returns></returns>
		public List<T> GetConnectedComponentNotOrder(T peak, bool isOrientGraph) {
			return RecursiveDetour(peak, false, isOrientGraph);
		}
		/// <summary>
		/// Создать остовное дерево через рекурсивный алгоритм
		/// </summary>
		/// <param name="peak">Вершина с которой начать обход</param>
		/// <returns></returns>
		public List<T> GetSpanningTreeRecursive(T peak) {
			return RecursiveDetour(peak, false, false);
		}
		/// <summary>
		/// Получить остовное дерево через итеративный алгоритм
		/// </summary>
		/// <param name="peak">Вершина с которой начать обход</param>
		/// <returns></returns>
		public List<T> GetSpanningTreeIterator(T peak) {
			return IteratorDetour(peak, false);
		}
		/// <summary>
		/// Рекурсивный обход графа
		/// </summary>
		/// <param name="peak"></param>
		/// <param name="isOrder"></param>
		/// <param name="result"></param>
		/// <returns></returns>
		private List<T> RecursiveDetour(T peak, bool isOrder, bool isOrientGraph, List<T> result = null) {
			if (result == null) {
				result = new List<T>() { peak };
			}
			foreach (var rib in Ribs) {
				var peakNeigh = rib.GetNeighboringPeak(peak, isOrientGraph);

				if (peakNeigh.HasValue) {
					if (!result.Contains(peakNeigh.Value)) {
						result.Add(peakNeigh.Value);
						result = RecursiveDetour(peakNeigh.Value, isOrder, isOrientGraph, result);
					}
				}
			}

			return isOrder ? result.OrderBy(x => x).ToList() : result;
		}
		/// <summary>
		/// Рекурсивный обход графа проверка на ацикличность
		/// </summary>
		/// <param name="peak"></param>
		/// <param name="isOrder"></param>
		/// <param name="result"></param>
		/// <returns></returns>
		private bool RecursiveAcyclic(Peak<T> basePeak) {
			if (basePeak.IsMark) {
				return false;
			}
			basePeak.IsMark = true;
			foreach (var p in basePeak.AvailablePeaks) {
				if (!RecursiveAcyclic(p)) {
					return false;
				}
				basePeak.UnMark();
				basePeak.AvailablePeaks.ClearMark();
			}

			return true;
		}
		/// <summary>
		/// Итеративный обход графа
		/// </summary>
		/// <returns></returns>
		private List<T> IteratorDetour(T peak, bool isOrder) {
			List<T> result = new List<T>() { peak };
			Stack<T> st = new Stack<T>();

			st.Push(peak);
			while (st.Count != 0) {
				var p = st.Pop();

				var list = GetListNeighboring(p);
				foreach (var item in list) {
					if (!result.Contains(item)) {
						result.Add(item);
						st.Push(item);
					}
				}
			}

			return isOrder ? result.OrderBy(x => x).ToList() : result;
		}
		/// <summary>
		/// Получить список кратных ребер со степенью
		/// </summary>
		/// <returns></returns>
		public Dictionary<Rib<T>, int> GetAllFoldRibWithPow() {
			Dictionary<Rib<T>, int> result = new Dictionary<Rib<T>, int>();
			var list = GetAllFoldRib();
			foreach (var item in list) {
				var count = Ribs.Count(x => x == item);
				if (count > 1) {
					result.Add(item, count);
				}
			}
			return result;
		}
		/// <summary>
		/// Получить все висячие вершины
		/// </summary>
		/// <returns></returns>
		public List<T> GetAllPendantPeak() {
			List<T> result = new List<T>();
			foreach (var peak in _peaks) {
				if (GetPeakPow(peak.Value) == 1) {
					result.Add(peak.Value);
				}
			}
			return result;
		}
		/// <summary>
		/// Получить все изолированные вершины
		/// </summary>
		/// <returns></returns>
		public List<T> GetAllIsolationPeak() {
			List<T> result = new List<T>();
			foreach (var peak in _peaks) {
				if (GetPeakPow(peak.Value) == 0) {
					result.Add(peak.Value);
				}
			}
			return result;
		}
		/// <summary>
		/// Получить все висячие ребра
		/// </summary>
		/// <returns></returns>
		public List<Rib<T>> GetAllPendantRib() {
			var list = GetAllPendantPeak();
			List<Rib<T>> result = new List<Rib<T>>();
			IEnumerable<Rib<T>> ribs = null;
			foreach (var peak in list) {

				ribs = Ribs.Where(x => x.GetNeighboringPeakValue(peak).HasValue);
				result.AddRange(ribs);
			}

			return result.Distinct().ToList();
		}
		/// <summary>
		/// Получить все вершины со степенями
		/// </summary>
		/// <returns></returns>
		public Dictionary<T, int> GetAllPeakPow() {
			Dictionary<T, int> result = new Dictionary<T, int>();
			foreach (var peak in _peaks) {
				result.Add(peak.Value, peak.Pow);
			}
			return result;
		}
		/// <summary>
		/// Получить степень вершины
		/// </summary>
		/// <param name="peak">Вершина</param>
		/// <returns></returns>
		public int GetPeakPow(T peak) {
			var list = Ribs.Where(x => x.Peak1 == peak ||
				x.Peak2 == peak);
			int result = 0;
			foreach (var rib in list) {
				if (rib.IsLoop()) {
					result += 2;
				} else {
					result++;
				}
			}

			return result;
		}
		/// <summary>
		/// Получить степень ребра
		/// </summary>
		/// <param name="rib">ребро</param>
		/// <returns></returns>
		public int GetRibPow(Rib<T> rib) {
			var list = Ribs.Where(x => x == rib);
			int result = 0;
			foreach (var r in list) {
				if (r.IsLoop()) {
					result += 2;
				} else {
					result++;
				}
			}
			return result;
		}
		/// <summary>
		/// Получить список смежности
		/// </summary>
		/// <returns></returns>
		public Dictionary<T, IEnumerable<T>> GetListNeighboring() {
			Dictionary<T, IEnumerable<T>> result = new Dictionary<T, IEnumerable<T>>();
			foreach (var peak in PeaksList) {
				result.Add(peak.Value,
					GetListNeighboring(peak));
			}
			return result;
		}
		/// <summary>
		/// Получить список смежности
		/// </summary>
		/// <param name="peak">название искомой вершины</param>
		/// <returns></returns>
		public List<T> GetListNeighboring(T peak) {
			return Ribs.Where(x => x.GetNeighboringPeakValue(peak).HasValue)
				.Select(x => x.GetNeighboringPeakValue(peak).Value).ToList();
		}

		/// <summary>
		/// Получить список смежности
		/// </summary>
		/// <param name="peak">Искомая вершина</param>
		/// <returns></returns>
		public List<T> GetListNeighboring(Peak<T> peak) {
			return GetListNeighboring(peak.Value);
		}
		/// <summary>
		/// Возвращает матрицу смежности в таблице
		/// строки и столбцы с названиями вершин
		/// </summary>
		/// <returns></returns>
		public DataTable GetMatrixNeighboring() {
			DataTable dt = new DataTable("Матрица смежности");
			foreach (var peak in PeaksList) {
				dt.Columns.Add(new DataColumn(peak.Value + "", typeof(int)));
				dt.Columns[peak.Value + ""].DefaultValue = 0;
			}
			var dicNeigh = GetListNeighboring();
			foreach (var neigh in dicNeigh.Keys) {
				var row = dt.Rows.Add();
				var list = dicNeigh[neigh];
				foreach (var value in list) {
					if (dt.Columns.Contains(value + "")) {
						row[value + ""] = (int)row[value + ""] + 1;
					}
				}
			}

			return dt;
		}
		/// <summary>
		/// Преобразует данный граф в простой
		/// </summary>
		/// <returns></returns>
		public Grapf<T> GetSimpleGraph() {
			//Добавляем ребра кроме петель
			var ribs = Ribs.Where(x => !x.IsLoop()).ToList();
			//теперь отберем по одному кратному ребру
			foreach (var rib in Ribs) {
				if (ribs.Count(x => x == rib) > 1) {
					ribs.Remove(rib);
				}
			}
			return new Grapf<T>(PeakCount, IsOrderedPeak, ribs.ToArray());
		}
		/// <summary>
		/// Получить список вершин источников
		/// </summary>
		/// <returns></returns>
		public List<Peak<T>> GetSourcePeaks() {
			return PeaksList.Where(x => x.IsSource).ToList();
		}
		/// <summary>
		/// Получить список вершин стоков
		/// </summary>
		/// <returns></returns>
		public List<Peak<T>> GetSinkPeaks() {
			return PeaksList.Where(x => x.IsSink).ToList();
		}
		/// <summary>
		/// Является ли граф ацикличным
		/// </summary>
		/// <returns></returns>
		public bool IsAcyclic() {
			PeaksList.ClearMark();
			try {
				foreach (var peak in PeaksList) {
					PeaksList.ClearMark();
					if (!RecursiveAcyclic(peak)) {
						return false;
					}
				}
				return true;
			} finally {
				PeaksList.ClearMark();
			}
		}
		/// <summary>
		/// Топологическая сортировка
		/// </summary>
		public List<T> TopologicalSort() {
			if (!IsAcyclic()) {
				throw new Exception("Топологическая сортировка возможна только для ацикличных графов.");
			}
			List<Peak<T>> result = new List<Peak<T>>();

			return TopSortRecursive(
				PeaksList.Where(x => x.IsSource).ToList(),
				result).Select(x => x.Value).ToList();
		}
		/// <summary>
		/// топологическая сортировка, возвращает вершины
		/// </summary>
		/// <returns></returns>
		public List<Peak<T>> TopologicalSortPeak() {
			if (!IsAcyclic()) {
				throw new Exception("Топологическая сортировка возможна только для ацикличных графов.");
			}
			List<Peak<T>> result = new List<Peak<T>>();

			TopSortRecursive(
				PeaksList.Where(x => x.IsSource).ToList(),
				result);
			return result;
		}
		/// <summary>
		/// Рекурсивный обход для топологической сортировки
		/// </summary>
		/// <param name="peaks"></param>
		/// <param name="result"></param>
		/// <returns></returns>
		private List<Peak<T>> TopSortRecursive(List<Peak<T>> peaks, List<Peak<T>> result) {
			List<Peak<T>> temp = new List<Peak<T>>();
			foreach (var peak in peaks) {
				if (!result.Any(x => x == peak)) {
					result.Add(peak);
					temp.Add(peak);
				}
			}
			if (temp.Any(x => !x.IsSink)) {
				foreach (var p in temp) {
					result = TopSortRecursive(p.AvailablePeaks, result);
				}
			}
			return result;
		}
		#endregion
	}
}
