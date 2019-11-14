using System;
using System.Collections.Generic;
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

namespace WeatherAPI
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		public MainWindow()
		{
			InitializeComponent();
		}

		private void searchButtonClick(object sender, RoutedEventArgs e)
		{
			string city = cityTextBox.Text;

			WeatherAPIService weatherAPI = new WeatherAPIService(city);

			if(weatherAPI.xmlDocument == null)
			{
				MessageBox.Show("Введен неверный город");
				return;
			}

			WeatherData weather = new WeatherData(city);

			List<TimeTemperature> timeTemperatures = weather.GetWeather();

			dataGrid.ItemsSource = timeTemperatures;
		}
	}
}
