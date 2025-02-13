using System;
using System.Windows;
using System.Windows.Controls;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace AdmissionSystem
{
    public partial class ApplicationForm : Window
    {
        private int? applicationID;

        private string connectionString = "Server=TEMHANLAPTOP\\TDG2022;Database=AdmissionSystem;Integrated Security=True;TrustServerCertificate=true;MultipleActiveResultSets=true;";

        public delegate void DataUpdatedHandler();
        public event DataUpdatedHandler DataUpdated;

        // Конструктор для создания новой заявки
        public ApplicationForm()
        {
            InitializeComponent();
            StatusComboBox.SelectedIndex = 0; // по умолчанию статус "Подана"
            LoadComboBoxes();
        }

        // Конструктор для редактирования существующей заявки
        public ApplicationForm(int applicationID, int applicantID, int facultyID, int specializationID, string status, DateTime dateSubmitted)
        {
            InitializeComponent();
            this.applicationID = applicationID;
            LoadComboBoxes();

            // Устанавливаем выбранные значения для абитуриента, факультета и специальности
            ApplicantComboBox.SelectedItem = ApplicantComboBox.Items.Cast<DataRowView>()
                .FirstOrDefault(item => item["ApplicantID"].ToString() == applicantID.ToString());

            FacultyComboBox.SelectedItem = FacultyComboBox.Items.Cast<DataRowView>()
                .FirstOrDefault(item => item["FacultyID"].ToString() == facultyID.ToString());

            SpecializationComboBox.SelectedItem = SpecializationComboBox.Items.Cast<DataRowView>()
                .FirstOrDefault(item => item["SpecializationID"].ToString() == specializationID.ToString());

            StatusComboBox.SelectedItem = StatusComboBox.Items.Cast<ComboBoxItem>().FirstOrDefault(item => ((ComboBoxItem)item).Content.ToString() == status);
            DateSubmittedPicker.SelectedDate = dateSubmitted;
        }

        // Загружаем данные в ComboBox
        private void LoadComboBoxes()
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                // Загрузка абитуриентов
                string applicantQuery = "SELECT ApplicantID, CONCAT(FirstName, ' ', LastName) AS ApplicantName FROM Applicants";
                SqlDataAdapter applicantAdapter = new SqlDataAdapter(applicantQuery, conn);
                DataTable applicantTable = new DataTable();
                applicantAdapter.Fill(applicantTable);
                ApplicantComboBox.ItemsSource = applicantTable.DefaultView;

                // Загрузка факультетов
                string facultyQuery = "SELECT FacultyID, FacultyName FROM Faculties";
                SqlDataAdapter facultyAdapter = new SqlDataAdapter(facultyQuery, conn);
                DataTable facultyTable = new DataTable();
                facultyAdapter.Fill(facultyTable);
                FacultyComboBox.ItemsSource = facultyTable.DefaultView;

                // Загрузка специальностей
                string specializationQuery = "SELECT SpecializationID, SpecializationName FROM Specializations";
                SqlDataAdapter specializationAdapter = new SqlDataAdapter(specializationQuery, conn);
                DataTable specializationTable = new DataTable();
                specializationAdapter.Fill(specializationTable);
                SpecializationComboBox.ItemsSource = specializationTable.DefaultView;
            }
        }

        // Обработчик кнопки "Сохранить"
        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            int applicantID = int.Parse(((DataRowView)ApplicantComboBox.SelectedItem)["ApplicantID"].ToString());
            int facultyID = int.Parse(((DataRowView)FacultyComboBox.SelectedItem)["FacultyID"].ToString());
            int specializationID = int.Parse(((DataRowView)SpecializationComboBox.SelectedItem)["SpecializationID"].ToString());
            string status = ((ComboBoxItem)StatusComboBox.SelectedItem)?.Content.ToString();
            DateTime? dateSubmitted = DateSubmittedPicker.SelectedDate;

            if (applicationID.HasValue)
            {
                // Логика редактирования
                try
                {
                    using (SqlConnection conn = new SqlConnection(connectionString))
                    {
                        conn.Open();
                        string query = "UPDATE Applications SET ApplicantID = @ApplicantID, FacultyID = @FacultyID, SpecializationID = @SpecializationID, " +
                                       "Status = @Status, SubmissionDate = @SubmissionDate WHERE ApplicationID = @ApplicationID";
                        SqlCommand cmd = new SqlCommand(query, conn);
                        cmd.Parameters.AddWithValue("@ApplicantID", applicantID);
                        cmd.Parameters.AddWithValue("@FacultyID", facultyID);
                        cmd.Parameters.AddWithValue("@SpecializationID", specializationID);
                        cmd.Parameters.AddWithValue("@Status", status);
                        cmd.Parameters.AddWithValue("@SubmissionDate", dateSubmitted ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@ApplicationID", applicationID);
                        cmd.ExecuteNonQuery();
                    }

                    MessageBox.Show("Заявка успешно обновлена!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                    DataUpdated?.Invoke();
                    DialogResult = true;
                    Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка обновления данных: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                // Логика создания новой заявки
            }
        }

        // Обработчик кнопки "Отмена"
        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}
