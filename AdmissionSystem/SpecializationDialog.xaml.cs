using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows;

namespace AdmissionSystem
{
    public partial class SpecializationDialog : Window
    {
        public string SpecializationName { get; private set; }
        public int FacultyID { get; private set; }

        private string connectionString = "Server=TEMHANLAPTOP\\TDG2022;Database=AdmissionSystem;Integrated Security=True;TrustServerCertificate=true;MultipleActiveResultSets=true;";

        // Конструктор для создания нового
        public SpecializationDialog()
        {
            InitializeComponent();
            LoadFaculties();
        }

        // Конструктор для редактирования
        public SpecializationDialog(string specializationName, int facultyId)
        {
            InitializeComponent();
            LoadFaculties();
            SpecializationNameTextBox.Text = specializationName;
            FacultyComboBox.SelectedValue = facultyId;
        }

        private void LoadFaculties()
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                SqlDataAdapter adapter = new SqlDataAdapter("SELECT FacultyID, FacultyName FROM Faculties", conn);
                DataTable dt = new DataTable();
                adapter.Fill(dt);
                FacultyComboBox.ItemsSource = dt.DefaultView;
            }
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            SpecializationName = SpecializationNameTextBox.Text.Trim();
            FacultyID = FacultyComboBox.SelectedValue != null ? (int)FacultyComboBox.SelectedValue : -1;

            if (string.IsNullOrWhiteSpace(SpecializationName) || FacultyID == -1)
            {
                MessageBox.Show("Заполните все поля.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            DialogResult = true;
            Close();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}
