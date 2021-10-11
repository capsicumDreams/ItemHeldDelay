using ItemHeldDelay;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
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

namespace Example
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		private const int NumberOfQuotations = 15;




		//	---------------------------------------------------------------------------------------------------
		#region Constructor
		//	---------------------------------------------------------------------------------------------------

		public MainWindow()
		{
			InitializeComponent();

			CommandBindings.AddRange(new[]
			{
				new CommandBinding(ItemHeldCommand, SendFiles),
			});

			DataContext = this;

			Loaded += MainWindowLoadedAsync;
		}

		#endregion
		//	---------------------------------------------------------------------------------------------------


		//	---------------------------------------------------------------------------------------------------
		#region Public Properties
		//	---------------------------------------------------------------------------------------------------

		public ObservableCollection<Quotation> Quotations { get; set; } = new ObservableCollection<Quotation>();

		public static readonly RoutedCommand ItemHeldCommand = new();

		#endregion
		//	---------------------------------------------------------------------------------------------------



		//	---------------------------------------------------------------------------------------------------
		#region Private Methods
		//	---------------------------------------------------------------------------------------------------

		private async void MainWindowLoadedAsync(Object sender, RoutedEventArgs e)
		{
			var client = new HttpClient();

			try
			{
				var response = await client.GetStreamAsync($"https://api.quotable.io/quotes?limit={NumberOfQuotations}");
				var allQuotations = await JsonSerializer.DeserializeAsync<QuotationsResponse>(response);

				if (allQuotations is null)
				{
					for (int index = 1; index <= NumberOfQuotations; index++)
					{
						Quotations.Add(new Quotation { content = $"Some text for quote {index}", author = $"Author {index}" });
					}
					return;
				}

				foreach (var quotation in allQuotations.results)
				{
					Quotations.Add(quotation);
				}

			}
			catch (HttpRequestException hre)
			{
				Debug.WriteLine(hre.Message);
			}
			catch (Exception)
			{
				for (int index = 1; index <= NumberOfQuotations; index++)
				{
					Quotations.Add(new Quotation { content = $"Some text for quote {index}", author = $"Author {index}" });
				}
			}
		}


		private void SendFiles(Object sender, ExecutedRoutedEventArgs e)
		{
			Debug.WriteLine(sender);
		}



		#endregion
		//	---------------------------------------------------------------------------------------------------

	}
}
