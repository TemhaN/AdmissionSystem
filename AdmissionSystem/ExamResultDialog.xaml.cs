using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Windows;

namespace AdmissionSystem
{
    public partial class ExamResultDialog : Window
    {
        private string connectionString = "Server=TEMHANLAPTOP\\TDG2022;Database=AdmissionSystem;Integrated Security=True;TrustServerCertificate=true;MultipleActiveResultSets=true;";

        public int ApplicationID => (int)ApplicationComboBox.SelectedValue;
        public int StageID => (int)StageComboBox.SelectedValue;
        public double ExamScore { get; private set; }

        public ExamResultDialog()
        {
            InitializeComponent();
            LoadApplications();
            LoadStages();
        }

        public ExamResultDialog(int applicationId, int stageId, double examScore)
        {
            InitializeComponent();
            LoadApplications();
            LoadStages();

            ApplicationComboBox.SelectedValue = applicationId;
            StageComboBox.SelectedValue = stageId;
            ExamScoreTextBox.Text = examScore.ToString();
        }

        private void LoadApplications()
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string query = @"
            SELECT a.ApplicationID, CONCAT(ap.FirstName, ' ', ap.LastName) AS ApplicationName
            FROM Applications a
            JOIN Applicants ap ON a.ApplicantID = ap.ApplicantID";

                SqlDataAdapter adapter = new SqlDataAdapter(query, conn);
                DataTable dt = new DataTable();
                adapter.Fill(dt);
                ApplicationComboBox.ItemsSource = dt.DefaultView;
            }
        }


        private void LoadStages()
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                SqlDataAdapter adapter = new SqlDataAdapter("SELECT StageID, StageName FROM AdmissionStages", conn);
                DataTable dt = new DataTable();
                adapter.Fill(dt);
                StageComboBox.ItemsSource = dt.DefaultView;
            }
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (!double.TryParse(ExamScoreTextBox.Text, out double examScore))
            {
                MessageBox.Show("Введите корректные баллы!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            ExamScore = examScore;
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
