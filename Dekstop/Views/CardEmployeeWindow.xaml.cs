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
    /// Логика взаимодействия для CardEmployeeWindow.xaml
    /// </summary>
    public partial class CardEmployeeWindow : Window
    {
        private static bool isEdit { get; set; } = false;
        private static int idEmployee;
        public CardEmployeeWindow()
        {
            InitializeComponent();
        }
        public CardEmployeeWindow(EmployeeModel employee)
        {
            InitializeComponent();
            idEmployee = employee.EmployeeId;
            LoadEmployeeData(employee);
            LoadDataDepartament();
            LoadDataEmployee();
            LoadDataPosition();

        }
        public async void LoadEmployeeData(EmployeeModel employee)
        {
            txbEmail.Text = employee.Email;
            txbInfo.Text = employee.AdditionalInfo;
            txbName.Text = employee.FirstName;
            txbLastName.Text = employee.LastName;
            txbMiddleName.Text = employee.MiddleName;
            txbMobilePhone.Text = employee.MobilePhone;
            txbWorkPhone.Text = employee.WorkPhone;
            txbOffice.Text = employee.Office;
            dpDateBirth.Text = employee.BirthDate.ToString();

        }

        public async Task LoadDataDepartament()
        {
            try
            {
                //получение данных отделов с последующим записем их в combobox
                using (var client = new HttpClient())
                {
                    var response = await client.GetAsync("https://localhost:7091/api/Department");
                    var content = await response.Content.ReadAsStringAsync();
                    var list = JsonConvert.DeserializeObject<List<DepartmentModel>>(content);
                    cmbDepartament.ItemsSource = list;

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public async Task LoadDataPosition()
        {
            try
            {
                //получение должностей с последующим записем их в combobox
                using (var client = new HttpClient())
                {
                    var response = await client.GetAsync("https://localhost:7091/api/Position");
                    var content = await response.Content.ReadAsStringAsync();
                    var list = JsonConvert.DeserializeObject<List<PositionModel>>(content);
                    cmbPosition.ItemsSource = list;

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public async Task LoadDataEmployee()
        {
            try
            {
                //получение сотрудников с последующим записем их в combobox
                using (var client = new HttpClient())
                {
                    var response = await client.GetAsync("https://localhost:7091/api/Employee");
                    var content = await response.Content.ReadAsStringAsync();
                    var list = JsonConvert.DeserializeObject<List<EmployeeModel>>(content);
                    cmbDirector.ItemsSource = list;
                    cmbAssistent.ItemsSource = list;

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            if (isEdit == false)
            {
                gridBlock1.IsEnabled = true;
                gridBlock2.IsEnabled = true;
                isEdit = true;
            }
            else
            {
                gridBlock1.IsEnabled = false;
                gridBlock2.IsEnabled = false;
                isEdit = false;
            }
        }

        private void btnExit_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private async void btnEditSave_Click(object sender, RoutedEventArgs e)
        {
            var selectedDirector = cmbDirector.SelectedItem as EmployeeModel;
            var selectedAsistent = cmbAssistent.SelectedItem as EmployeeModel;

            var selectedDepartament = cmbDepartament.SelectedItem as DepartmentModel;
            var selectedPosition = cmbPosition.SelectedItem as PositionModel;

            //проверка на пустые поля
            TextBox[] textBoxes = { txbEmail, txbInfo, txbName, txbLastName, txbMiddleName, txbMobilePhone, txbWorkPhone, txbOffice };
            foreach (var textBox in textBoxes)
            {
                if (string.IsNullOrWhiteSpace(textBox.Text))
                {
                    MessageBox.Show($"Все поля должны быть заполнены");
                    return;
                }
            }
            //проверка выбран ли директор и ассистент
            if (selectedDirector == null || selectedAsistent == null || selectedDepartament == null || selectedPosition == null)
            {
                MessageBox.Show("Все поля должны быть заполнены");
                return;
            }

            //проверка выбрана ли дата рождения
            if (string.IsNullOrWhiteSpace(dpDateBirth.Text))
            {
                MessageBox.Show("Дата рождения не выбрана");
                return;
            }

            var employee = new EmployeeModel()
            {
                AdditionalInfo = txbInfo.Text,
                FirstName = txbName.Text,
                LastName = txbLastName.Text,
                MiddleName = txbMiddleName.Text,
                Email = txbEmail.Text,
                AssistantId = selectedAsistent.EmployeeId,
                SupervisorId = selectedDirector.EmployeeId,
                Office = txbOffice.Text,
                BirthDate = DateOnly.Parse(dpDateBirth.Text),
                EmployeeId = idEmployee,
                MobilePhone = txbMobilePhone.Text,
                WorkPhone = txbWorkPhone.Text,
                PositionId = selectedPosition.PositionId,
                DepartmentId = selectedDepartament.DepartmentId,
                DepartmentName = selectedDepartament.DepartmentName,
                PositionName = selectedPosition.PositionName,
            };
            var json = JsonConvert.SerializeObject(employee);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            using (var client = new HttpClient())
            {
                var request = new HttpRequestMessage(new HttpMethod("PATCH"), "https://localhost:7091/api/Employee")
                {
                    Content = content
                };

                var response = await client.SendAsync(request);
                if (response.IsSuccessStatusCode)
                {
                    MessageBox.Show("Данные сотрудника обновлены");

                }
            }
        }

        private void btnEventEmployee_Click(object sender, RoutedEventArgs e)
        {
            var win = new EventListEmployeeWindow(idEmployee);
            win.Show();
            Close();
        }
    }
}
