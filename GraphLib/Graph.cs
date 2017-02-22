using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace GraphLibrary
{
	/// <summary>
	/// Класс реализации графа
	/// </summary>
	/// <typeparam name="T">простой тип вроде (int,string)</typeparam>
    public class Grapf<T> where T: struct
    {
		#region Переменные и константы
		/// <summary>
		/// Количество вершин
		/// </summary>
		private int _peakCount = 0;
		/// <summary>
		/// Количество вершин
		/// </summary>
		public int PeakCount { get { return _peakCount; } }
		/// <summary>
		/// Количество ребер
		/// </summary>
		private int _ribsCount = 0;
		/// <summary>
		/// Количество ребер
		/// </summary>
		public int RibsCount { get { return _ribsCount; } }
		/// <summary>
		/// Списки смежности по вершине
		/// </summary>
		private Dictionary<T, List<T>> _adjLists = new Dictionary<T, List<T>>();
		/// <summary>
		/// Списки смежности с ключом по номеру вершины
		/// </summary>
		public Dictionary<T, List<T>> AdjLists { get { return _adjLists; } }
		/// <summary>
		/// Ребра
		/// </summary>
		private List<Rib<T>> _ribs = new List<Rib<T>>();
		/// <summary>
		/// Список ребер
		/// </summary>
		public List<Rib<T>> Ribs { get { return _ribs; } }
		/// <summary>
		/// Список вершин
		/// </summary>
		private List<T> _peaks = new List<T>();
		/// <summary>
		/// Список вершин
		/// </summary>
		public List<T> PeaksList {
			get { return _peaks; }
		}

		#endregion

		#region Конструкторы
		/// <summary>
		/// Конструктор
		/// </summary>
		/// <param name="countPeak">Количество вершин</param>
		/// <param name="ribsArray">Массив ребер</param>
		public Grapf(int countPeak, params Rib<T>[] ribsArray) {
			_peakCount = countPeak;
			_ribsCount = ribsArray.Length;
			_ribs = ribsArray.ToList();
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
		/// <remarks>Файл должен иметь формат первая строка количество вершин, далее все строки ребра</remarks>
		public Grapf(string filePath) {
			if(!File.Exists(filePath)) {
				new System.IO.FileNotFoundException("По заданному пути файл не найден");
			}
			var lines = File.ReadAllLines(filePath);
			if(lines.Length == 0) {
				new ArgumentException("Файл пустой");
			}
			if(!int.TryParse(lines[0], out _peakCount)) {
				new ArgumentException("Первой строкой в файле должно быть количество вершин, целое число.");
			}
			_ribsCount = lines.Length - 2;

			List<Rib<T>> _ribs = new List<Rib<T>>();
			for(int i=1; i< lines.Length; i++) {
				var peaks = lines[i].Split(' ');
				if(peaks.Length < 2 || peaks.Length > 2) {
					new ArgumentException("Строки с ребрами должны содержать две вершины, разделенные пробелом");
				}
				_ribs.Add(new Rib<T>(peaks[0], peaks[1]));
				//Попутно с перебором ребер создаем список вершин
				if(!PeaksList.Contains((T)Convert.ChangeType(peaks[0], typeof(T)))) {
					_peaks.Add((T)Convert.ChangeType(peaks[0], typeof(T)));
				}
				if(!PeaksList.Contains((T)Convert.ChangeType(peaks[1], typeof(T)))) {
					_peaks.Add((T)Convert.ChangeType(peaks[1], typeof(T)));
				}
			}
		}

		#endregion

		#region Методы
		/// <summary>
		/// Получаем все вершины у которых есть петли
		/// </summary>
		/// <returns></returns>
		public IEnumerable<T> GetAllPeakLoop() {
			return Ribs.Where(x => x.IsLoop()).Select(x => x.Peak1).Distinct();
		}
		/// <summary>
		/// Получаем все вершины у которых есть петли с кратностью
		/// </summary>
		/// <returns>Возвращает словарь в котором по вершине можно получить кратность</returns>
		public Dictionary<T, int> GetAllPeakLoopWithPow() {
			Dictionary<T, int> result = new Dictionary<T, int>();
			var list = GetAllPeakLoop();
			foreach(var peak in list) {
				result.Add(peak, Ribs.Count(x => x.IsLoop() 
					&& EqualityComparer<T>.Default.Equals(x.Peak1, peak)));
			}
			return result;
		}
		/// <summary>
		/// Получить список кратных ребер
		/// </summary>
		/// <returns></returns>
		public IEnumerable<Rib<T>> GetAllFoldRib() {
			List<Rib<T>> result = new List<Rib<T>>();
			foreach(var rib in Ribs) {
				result.AddRange(Ribs.Where(x => x == rib && !result.Any(r => r == x)));
			}
			return result;
		}

		/// <summary>
		/// Получить список кратных ребер со степенью
		/// </summary>
		/// <returns></returns>
		public Dictionary<Rib<T>, int> GetAllFoldRibWithPow() {
			Dictionary<Rib<T>, int> result = new Dictionary<Rib<T>, int>();
			var list = GetAllFoldRib();
			foreach(var item in list) {
				result.Add(item, Ribs.Count(x => x == item));
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
				if(GetPeakPow(peak) == 1) {
					result.Add(peak);
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
			foreach(var peak in _peaks) {
				if(GetPeakPow(peak) == 0) {
					result.Add(peak);
				}
			}
			return result;
		}
		/// <summary>
		/// Получить все висячие ребра
		/// </summary>
		/// <returns></returns>
		public List<Rib<T>> GetAllPendantRib() {
			List<Rib<T>> result = new List<Rib<T>>();
			foreach(var rib in Ribs) {
				if(GetRibPow(rib) == 1) {
					result.Add(rib);
				}
			}
			return result;
		}
		/// <summary>
		/// Получить все вершины со степенями
		/// </summary>
		/// <returns></returns>
		public Dictionary<T, int> GetAllPeakPow() {
			Dictionary<T, int> result = new Dictionary<T, int>();
			foreach(var peak in _peaks) {
				result.Add(peak, GetPeakPow(peak));
			}
			return result;
		}
		/// <summary>
		/// Получить степень вершины
		/// </summary>
		/// <param name="peak">Вершина</param>
		/// <returns></returns>
		public int GetPeakPow(T peak) {
			var list = Ribs.Where(x => EqualityComparer<T>.Default.Equals(x.Peak1, peak) ||
				EqualityComparer<T>.Default.Equals(x.Peak2, peak));
			int result = 0;
			foreach(var rib in list) {
				if(rib.IsLoop()) {
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
			foreach(var r in list) {
				if(r.IsLoop()) {
					result += 2;
				} else {
					result++;
				}
			}
			return result;
		}

		#endregion
	}
}
