using GraphLibrary;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WpfGraph {
	/// <summary>
	/// Логика взаимодействия для MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window {
		public MainWindow() {
			InitializeComponent();
		}

		private void BtChooseFile_Click(object sender, RoutedEventArgs e) {
			try {
				Grapf<int> graph = null;
				OpenFileDialog fd = new OpenFileDialog();
				fd.DefaultExt = ".dat";
				//fd.Filter = "Файлы данных(*.dat)|.dat";
				fd.Title = "Выбрать файл для загрузки графа";
				if(fd.ShowDialog() == true) {
					TxtFile.Text = fd.FileName;
					TxtResult.Text = "";
					graph = new Grapf<int>(fd.FileName);
				}

				PrintNeighbourList(TxtResult, graph.GetListNeighboring());

				var dt = graph.GetMatrixNeighboring();

				TxtResult.Text += "Матрица смежности: " + Environment.NewLine;

				foreach(DataRow row in dt.Rows) {
					TxtResult.Text += Environment.NewLine;
					foreach(var col in row.ItemArray) {
						TxtResult.Text += col + " ";
					}
				}

				TxtResult.Text += Environment.NewLine;

				PrintPeakPow(TxtResult, graph.GetAllPeakPow());

				PrintPeak(TxtResult, graph.GetAllIsolationPeak(), "Список изолированных вершин");

				PrintPeak(TxtResult, graph.GetAllPendantPeak(), "Список висячих вершин");

				PrintRib(TxtResult, graph.GetAllPendantRib(), "Список висячих ребер");

				PrintLoopPeakWithPow(TxtResult, graph.GetAllPeakLoopWithPow());

				PrintFoldRibWithPow(TxtResult, graph.GetAllFoldRibWithPow());

				var simpleGraph = graph.GetSimpleGraph();

				PrintNeighbourList(TxtResult, simpleGraph.GetListNeighboring());

				var connComponents = simpleGraph.GetConnectedComponents();

				PrintConnectedComponents(TxtResult, connComponents);

				GetSpanningTree(connComponents, simpleGraph);

				MessageBox.Show("Работа с графом проведена");
			}
			catch(Exception ex) {
				MessageBox.Show(ex.Message, "Ошибка");
			}
		}
		/// <summary>
		/// Печатаем список смежности
		/// </summary>
		/// <param name="txt"></param>
		/// <param name="dic"></param>
		private void PrintNeighbourList(TextBox txt, Dictionary<int, IEnumerable<int>> dic) {
			txt.Text += Environment.NewLine;
			txt.Text += "Список смежности: " + Environment.NewLine;
			foreach(var neigh in dic.Keys) {
				txt.Text += $"Вершина: {neigh} Смежные вершины: {string.Join("; ", dic[neigh])}" + Environment.NewLine;
			}
			txt.Text += Environment.NewLine;
		}
		/// <summary>
		/// Распечатать список степеней
		/// </summary>
		/// <param name="txt"></param>
		/// <param name="dic"></param>
		private void PrintPeakPow(TextBox txt, Dictionary<int,int> dic) {
			txt.Text += Environment.NewLine;
			txt.Text += "Список степеней вершин: " + Environment.NewLine;

			foreach(var key in dic.Keys) {
				TxtResult.Text += "Вершина: " + key + ", степень: " + dic[key] + Environment.NewLine;
			}

			txt.Text += Environment.NewLine;
		}
		/// <summary>
		/// Распечатать список компонент связности
		/// </summary>
		/// <param name="txt"></param>
		/// <param name="list"></param>
		private void PrintConnectedComponents(TextBox txt, List<List<int>> list) {
			txt.Text += Environment.NewLine;

			txt.Text += $"Количество компонент связности: {list.Count}" + Environment.NewLine;
			foreach(var l in list) {
				txt.Text += "Компонента связности: " + Environment.NewLine;
				txt.Text += string.Join("; ", l) + Environment.NewLine;
			}

			txt.Text += Environment.NewLine;
		}
		/// <summary>
		/// Распечатать остовное дерево
		/// </summary>
		private void PrintSpanningTree(bool isRecursive, List<int> tree) {
			TxtResult.Text += Environment.NewLine;

			TxtResult.Text += (isRecursive ? "Остовное дерево на основе рекурсивного алгоритма: " : "Остовное дерево на основе итерационного алгоритма: ") + Environment.NewLine;
			TxtResult.Text += string.Join("; ", tree) + Environment.NewLine;

			TxtResult.Text += Environment.NewLine;
		}
		/// <summary>
		/// Получаем и печатаем остовные деревья
		/// </summary>
		/// <param name="list"></param>
		/// <param name="gr"></param>
		private void GetSpanningTree(List<List<int>> list, Grapf<int> gr) {
			foreach (var l in list) {
				//отбрасываем тривиальные
				if(l.Count > 1) {
					//отправляем первую вершину для которой строить
					PrintSpanningTree(true, gr.GetSpanningTreeRecursive(l[0]));
					PrintSpanningTree(false, gr.GetSpanningTreeIterator(l[1]));
				}
			}
		}
		/// <summary>
		/// Распечатывает вершины с названием
		/// </summary>
		/// <param name="txt"></param>
		/// <param name="peaks"></param>
		private void PrintPeak(TextBox txt, List<int> peaks, string name) {
			txt.Text += Environment.NewLine;
			txt.Text += $"{name}: " + Environment.NewLine;

			txt.Text += string.Join("; ", peaks) + Environment.NewLine;

			txt.Text += Environment.NewLine;
		}

		/// <summary>
		/// Распечатывает ребра с названием
		/// </summary>
		/// <param name="txt"></param>
		/// <param name="ribs"></param>
		private void PrintRib(TextBox txt, List<Rib<int>> ribs, string name) {
			txt.Text += Environment.NewLine;
			txt.Text += $"{name}: " + Environment.NewLine;
			foreach(var rib in ribs) {
				txt.Text += $"{rib.Peak1.Value} - {rib.Peak2.Value}; ";
			}

			txt.Text += Environment.NewLine;
		}

		/// <summary>
		/// Распечатывает ребра с кратностью
		/// </summary>
		/// <param name="txt"></param>
		/// <param name="ribs"></param>
		private void PrintFoldRibWithPow(TextBox txt, Dictionary<Rib<int>, int> ribs) {
			txt.Text += Environment.NewLine;
			txt.Text += $"Список кратных ребер со степенью: " + Environment.NewLine;
			foreach(var rib in ribs.Keys) {

				txt.Text += $"Ребро: {rib.Peak1.Value} - {rib.Peak2.Value}, степень {ribs[rib]}; " + Environment.NewLine;
			}

			txt.Text += Environment.NewLine;
		}

		/// <summary>
		/// Распечатывает петли с кратностью
		/// </summary>
		/// <param name="txt"></param>
		/// <param name="ribs"></param>
		private void PrintLoopPeakWithPow(TextBox txt, Dictionary<int, int> peaks) {
			txt.Text += Environment.NewLine;
			txt.Text += $"Список вершин с петлями и кратностью: " + Environment.NewLine;
			foreach(var peak in peaks.Keys) {

				txt.Text += $"Вершина: {peak}, кратность петли: {peaks[peak]}; " + Environment.NewLine;
			}

			txt.Text += Environment.NewLine;
		}
	}
}
