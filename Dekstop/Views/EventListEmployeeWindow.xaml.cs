using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Dekstop.Models;
using Newtonsoft.Json;

namespace Dekstop.Views
{
    /// <summary>
    /// Логика взаимодействия для EventListEmployeeWindow.xaml
    /// </summary>
    public partial class EventListEmployeeWindow : Window
    {
        private static int idEmployee;
        public EventListEmployeeWindow()
        {
            InitializeComponent();
        }
        public EventListEmployeeWindow(int id)
        {
            InitializeComponent();
            idEmployee = id;
            LoadData();
        }
        public async Task LoadData()
        {
            using (var client = new HttpClient())
            {
                var response = await client.GetAsync($"https://localhost:7091/api/EventEmployee/id?id={idEmployee}");
                var content = await response.Content.ReadAsStringAsync();
                var eventList = JsonConvert.DeserializeObject<List<EventEmployeeModel>>(content);

                if (eventList == null)
                {
                    MessageBox.Show("У сотрудника отсутствуют события");
                    return;
                }

                dgTraining.ItemsSource = eventList.Where(p => p.EventEmployeeTypeId == 1).ToList();
                dgTimeOff.ItemsSource = eventList.Where(p => p.EventEmployeeTypeId == 3).ToList();
                dgVacation.ItemsSource = eventList.Where(p => p.EventEmployeeTypeId == 2).ToList();
            }
        }

        public async Task AddEvent(int typeId)
        {
            if (string.IsNullOrWhiteSpace(dpDateEnd.Text) || string.IsNullOrWhiteSpace(dpDateStart.Text))
            {
                MessageBox.Show("Выберите даты");
                return;
            }

            var eventAdd = new EventEmployeeRequest()
            {
                DateEnd = DateOnly.Parse(dpDateEnd.Text),
                DateStart = DateOnly.Parse(dpDateStart.Text),
                EmployeeId = idEmployee,
                EventEmployeeTypeId = typeId
            };

            var json = JsonConvert.SerializeObject(eventAdd);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            using (var client = new HttpClient())
            {
                var response = await client.PostAsync("https://localhost:7091/api/EventEmployee", content);
                if (response.IsSuccessStatusCode)
                {
                    MessageBox.Show("Событие добавлено");
                }
            }
            await LoadData();
        }

        private async void btnAddTraining_Click(object sender, RoutedEventArgs e)
        {
           await AddEvent(1);
        }

        private async void btnAddTimeOff_Click(object sender, RoutedEventArgs e)
        {
            await AddEvent(3);
        }

        private async void btnAddVacation_Click(object sender, RoutedEventArgs e)
        {
            await AddEvent(2);
        }
    }
}
