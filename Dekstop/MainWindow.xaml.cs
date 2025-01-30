using System.Collections.ObjectModel;
using System.Net.Http;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Dekstop.Models;
using Dekstop.Views;
using Newtonsoft.Json;

namespace Dekstop
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private static ObservableCollection<EmployeeModel>? EmployeeList {  get; set; }
        public MainWindow()
        {
            InitializeComponent();
            LoadDataAsync();
        }

        public async Task LoadDataAsync()
        {
            try
            {
                using (var client = new HttpClient())
                {
                    var response = await client.GetAsync("https://localhost:7091/api/Employee");
                    response.EnsureSuccessStatusCode(); // вызывает исключение если запрос не удается

                    var content = await response.Content.ReadAsStringAsync();
                    EmployeeList = JsonConvert.DeserializeObject<ObservableCollection<EmployeeModel>>(content);
                    listViewEmployee.ItemsSource = EmployeeList;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void listViewEmployee_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            try
            {
                var selectedEmployee = listViewEmployee.SelectedItem as EmployeeModel;
                
                if (selectedEmployee == null) throw new Exception("Выберите сотрудника");

                var win = new CardEmployeeWindow(selectedEmployee);
                win.Show();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void ButtonSelectDepartament_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button; // получаем название кнопок
            if (button == null) return;
      
            if (button.Name == "btn1")
            {
                listViewEmployee.ItemsSource = EmployeeList;
            }
            else if (button.Name == "btn2")
            {
                listViewEmployee.ItemsSource = EmployeeList?.Where(p => p.DepartmentName == "Административный департамент").ToList();
            }
            else if (button.Name == "btn3")
            {
                listViewEmployee.ItemsSource = EmployeeList?.Where(p => p.DepartmentName == "Академия умные дорогие").ToList();
            }
            else if (button.Name == "btn4")
            {
                listViewEmployee.ItemsSource = EmployeeList?.Where(p => p.DepartmentName == "Договорной отдел").ToList();

            }
            else if (button.Name == "btn5")
            {
                listViewEmployee.ItemsSource = EmployeeList?.Where(p => p.DepartmentName == "Общий отдел").ToList();
            }
            else if (button.Name == "btn6")
            {
                listViewEmployee.ItemsSource = EmployeeList?.Where(p => p.DepartmentName == "Лицензионный отдел").ToList();
            }
            else if (button.Name == "btn7")
            {
                listViewEmployee.ItemsSource = EmployeeList?.Where(p => p.DepartmentName == "Управление маркетинга").ToList();
            }
        }
    }
}