using Microsoft.Win32;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using ClosedXML.Excel;
using Microsoft.Win32;
using System;
using System.Data;
using System.IO;
using System.Windows;

namespace AdmissionSystem
{
    public partial class MainWindow : Window
    {
        private string connectionString = "Server=TEMHANLAPTOP\\TDG2022;Database=AdmissionSystem;Integrated Security=True;TrustServerCertificate=true;MultipleActiveResultSets=true;";

        private DataTable ExecuteQuery(string query)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    using (var command = new SqlCommand(query, connection))
                    {
                        using (var adapter = new SqlDataAdapter(command))
                        {
                            var dataTable = new DataTable();
                            adapter.Fill(dataTable);
                            return dataTable;
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка при выполнении запроса: {ex.Message}");
                    return null;
                }
            }
        }

        private int ExecuteNonQuery(string query)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    using (var command = new SqlCommand(query, connection))
                    {
                        return command.ExecuteNonQuery();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка при выполнении запроса: {ex.Message}");
                    return 0;
                }
            }
        }


        public MainWindow()
        {
            InitializeComponent();
            LoadAllDataOnStartup();

        }


        // Метод для загрузки всех данных при запуске приложения
        private void LoadAllDataOnStartup()
        {
            LoadApplicantsData();
            LoadApplicationsData();
            LoadFacultiesData();
            LoadSpecializationsData();
            LoadAdmissionStages();
            LoadExamResults();
            LoadUsersData();
        }


        private void RefreshAllDataButton_Click(object sender, RoutedEventArgs e)
        {
            LoadAllDataOnStartup();
        }

        private void RefreshAllDataButton_Click2(object sender, RoutedEventArgs e)
        {
            LoadAllDataOnStartup();
        }
        private void RefreshAllDataButton_Click3(object sender, RoutedEventArgs e)
        {
            LoadAllDataOnStartup();
        }
        private void RefreshAllDataButton_Click4(object sender, RoutedEventArgs e)
        {
            LoadAllDataOnStartup();
        }
        private void RefreshAllDataButton_Click5(object sender, RoutedEventArgs e)
        {
            LoadAllDataOnStartup();
        }
        private void RefreshAllDataButton_Click6(object sender, RoutedEventArgs e)
        {
            LoadAllDataOnStartup();
        }

        private void LoadData(string query, System.Windows.Controls.DataGrid dataGrid)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    SqlDataAdapter da = new SqlDataAdapter(query, conn);
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    // Log column names to check if FacultyID exists
                    foreach (DataColumn column in dt.Columns)
                    {
                        Console.WriteLine(column.ColumnName);  // Check if FacultyID is listed
                    }

                    // Привязка данных к DataGrid
                    dataGrid.ItemsSource = dt.DefaultView;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки данных: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }


        private void LoadApplicantsData() => LoadData("SELECT * FROM Applicants", ApplicantsDataGrid);
        private void LoadApplicationsData()
        {
            string query = @"
        SELECT 
            a.ApplicationID,
            a.ApplicantID,
            ap.FirstName + ' ' + ap.LastName AS ApplicantName,
            sp.SpecializationName,
            a.SubmissionDate,
            a.Status,
            f.FacultyName,
            a.FacultyID,  -- Столбец FacultyID
            a.SpecializationID  -- Добавьте столбец SpecializationID
        FROM Applications a
        INNER JOIN Applicants ap ON a.ApplicantID = ap.ApplicantID
        INNER JOIN Specializations sp ON a.SpecializationID = sp.SpecializationID
        INNER JOIN Faculties f ON a.FacultyID = f.FacultyID";

            // Выполнить запрос и загрузить данные в DataGrid
            var dataTable = ExecuteQuery(query);
            ApplicationsDataGrid.ItemsSource = dataTable.DefaultView;
        }



        private void LoadFacultiesData() => LoadData("SELECT * FROM Faculties", FacultiesDataGrid);
        private void LoadSpecializationsData()
        {
            string query = @"
    SELECT 
        sp.SpecializationID,
        sp.SpecializationName,
        sp.FacultyID,  -- Добавляем FacultyID в выборку
        f.FacultyName
    FROM Specializations sp
    INNER JOIN Faculties f ON sp.FacultyID = f.FacultyID";

            // Выполняем запрос и загружаем данные в DataGrid
            var dataTable = ExecuteQuery(query);
            if (dataTable != null)
            {
                SpecializationsDataGrid.ItemsSource = dataTable.DefaultView;
            }
        }


        private void LoadAdmissionStages() => LoadData("SELECT * FROM AdmissionStages", AdmissionStagesDataGrid);

        private DataTable _originalDataTable; // Храним оригинальные данные для восстановления

        private void LoadExamResults()
        {
            string query = @"
    SELECT 
        er.ResultID,
        a.ApplicationID,
        CONCAT(app.FirstName, ' ', app.LastName) AS ApplicantName,
        s.StageID,   -- Добавляем StageID в выборку
        s.StageName,
        er.ExamScore
    FROM ExamResults er
    INNER JOIN Applications a ON er.ApplicationID = a.ApplicationID
    INNER JOIN AdmissionStages s ON er.StageID = s.StageID
    INNER JOIN Applicants app ON a.ApplicantID = app.ApplicantID";  // Добавляем Applicants для имени

            // Выполняем запрос и загружаем данные в DataGrid
            var dataTable = ExecuteQuery(query);
            if (dataTable != null)
            {
                _originalDataTable = dataTable.Copy();  // Сохраняем оригинальные данные
                ExamResultsDataGrid.ItemsSource = dataTable.DefaultView;
            }
        }



        private void LoadUsersData() => LoadData("SELECT * FROM Users", UsersDataGrid);

        private void LoadDataButton_Click(object sender, RoutedEventArgs e)
        {
            LoadAllDataOnStartup();
        }

        private void AddApplicantButton_Click(object sender, RoutedEventArgs e)
        {
            ApplicantDialog dialog = new ApplicantDialog(); // Пустые поля для нового абитуриента
            if (dialog.ShowDialog() == true)
            {
                try
                {
                    using (SqlConnection conn = new SqlConnection(connectionString))
                    {
                        conn.Open();
                        string query = "INSERT INTO Applicants (FirstName, LastName, DateOfBirth, Gender, Address, PhoneNumber, Email, PassportNumber) " +
                                       "VALUES (@FirstName, @LastName, @DateOfBirth, @Gender, @Address, @PhoneNumber, @Email, @PassportNumber)";
                        SqlCommand cmd = new SqlCommand(query, conn);
                        cmd.Parameters.AddWithValue("@FirstName", dialog.FirstName);
                        cmd.Parameters.AddWithValue("@LastName", dialog.LastName);
                        cmd.Parameters.AddWithValue("@DateOfBirth", dialog.DateOfBirth ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@Gender", dialog.Gender ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@Address", dialog.Address ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@PhoneNumber", dialog.PhoneNumber ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@Email", dialog.Email ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@PassportNumber", dialog.PassportNumber ?? (object)DBNull.Value);
                        cmd.ExecuteNonQuery();
                    }

                    MessageBox.Show("Абитуриент успешно добавлен!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                    LoadApplicantsData();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка добавления данных: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }



        private void EditApplicantButton_Click(object sender, RoutedEventArgs e)
        {
            if (ApplicantsDataGrid.SelectedItem is DataRowView row)
            {
                // Извлекаем данные из выбранного абитуриента
                string firstName = row["FirstName"].ToString();
                string lastName = row["LastName"].ToString();
                DateTime? dateOfBirth = row["DateOfBirth"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(row["DateOfBirth"]);
                string gender = row["Gender"].ToString();
                string address = row["Address"].ToString();
                string phoneNumber = row["PhoneNumber"].ToString();
                string email = row["Email"].ToString();
                string passportNumber = row["PassportNumber"].ToString();

                // Открываем диалог с переданными данными
                ApplicantDialog dialog = new ApplicantDialog(firstName, lastName, dateOfBirth, gender, address, phoneNumber, email, passportNumber);

                if (dialog.ShowDialog() == true)
                {
                    try
                    {
                        using (SqlConnection conn = new SqlConnection(connectionString))
                        {
                            conn.Open();
                            string query = "UPDATE Applicants SET FirstName = @FirstName, LastName = @LastName, DateOfBirth = @DateOfBirth, Gender = @Gender, " +
                                           "Address = @Address, PhoneNumber = @PhoneNumber, Email = @Email, PassportNumber = @PassportNumber WHERE ApplicantID = @ApplicantID";
                            SqlCommand cmd = new SqlCommand(query, conn);
                            cmd.Parameters.AddWithValue("@FirstName", dialog.FirstName);
                            cmd.Parameters.AddWithValue("@LastName", dialog.LastName);
                            cmd.Parameters.AddWithValue("@DateOfBirth", dialog.DateOfBirth ?? (object)DBNull.Value);
                            cmd.Parameters.AddWithValue("@Gender", dialog.Gender ?? (object)DBNull.Value);
                            cmd.Parameters.AddWithValue("@Address", dialog.Address ?? (object)DBNull.Value);
                            cmd.Parameters.AddWithValue("@PhoneNumber", dialog.PhoneNumber ?? (object)DBNull.Value);
                            cmd.Parameters.AddWithValue("@Email", dialog.Email ?? (object)DBNull.Value);
                            cmd.Parameters.AddWithValue("@PassportNumber", dialog.PassportNumber ?? (object)DBNull.Value);
                            cmd.Parameters.AddWithValue("@ApplicantID", row["ApplicantID"]);
                            cmd.ExecuteNonQuery();
                        }

                        MessageBox.Show("Абитуриент успешно обновлен!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                        LoadApplicantsData();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Ошибка обновления данных: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
            else
            {
                MessageBox.Show("Выберите запись для изменения.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }



        private void DeleteApplicantButton_Click(object sender, RoutedEventArgs e)
        {
            if (ApplicantsDataGrid.SelectedItem == null)
            {
                MessageBox.Show("Выберите запись для удаления.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                DataRowView row = (DataRowView)ApplicantsDataGrid.SelectedItem;
                int applicantId = (int)row["ApplicantID"];

                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    // Удаление всех записей в Applications, связанных с этим ApplicantID
                    string deleteApplicationsQuery = "DELETE FROM Applications WHERE ApplicantID = @ApplicantID";
                    SqlCommand deleteApplicationsCmd = new SqlCommand(deleteApplicationsQuery, conn);
                    deleteApplicationsCmd.Parameters.AddWithValue("@ApplicantID", applicantId);
                    deleteApplicationsCmd.ExecuteNonQuery();

                    // Теперь можно удалить абитуриента
                    string deleteApplicantQuery = "DELETE FROM Applicants WHERE ApplicantID = @ApplicantID";
                    SqlCommand deleteApplicantCmd = new SqlCommand(deleteApplicantQuery, conn);
                    deleteApplicantCmd.Parameters.AddWithValue("@ApplicantID", applicantId);
                    deleteApplicantCmd.ExecuteNonQuery();

                    MessageBox.Show("Абитуриент и связанные данные удалены!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                    LoadApplicantsData();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка удаления данных: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }


        private void CreateApplicationButton_Click(object sender, RoutedEventArgs e)
        {
            ApplicationForm dialog = new ApplicationForm(); // Создание нового экземпляра формы
            if (dialog.ShowDialog() == true)
            {
                LoadApplicationsData(); // Обновляем данные
            }
        }

        private void EditApplicationButton_Click(object sender, RoutedEventArgs e)
        {
            if (ApplicationsDataGrid.SelectedItem is DataRowView row)
            {
                int applicationID = row["ApplicationID"] != DBNull.Value ? (int)row["ApplicationID"] : 0;
                int applicantID = row["ApplicantID"] != DBNull.Value ? (int)row["ApplicantID"] : 0;

                // Проверяем, существует ли столбец "FacultyID"
                int facultyID = row.Row.Table.Columns.Contains("FacultyID") && row["FacultyID"] != DBNull.Value
                    ? (int)row["FacultyID"]
                    : 0;

                int specializationID = row["SpecializationID"] != DBNull.Value ? (int)row["SpecializationID"] : 0;
                string status = row["Status"] != DBNull.Value ? row["Status"].ToString() : string.Empty;
                DateTime submissionDate = row["SubmissionDate"] != DBNull.Value ? (DateTime)row["SubmissionDate"] : DateTime.MinValue;

                // Передаем данные в форму редактирования
                ApplicationForm dialog = new ApplicationForm(applicationID, applicantID, facultyID, specializationID, status, submissionDate);

                if (dialog.ShowDialog() == true)
                {
                    LoadApplicationsData(); // Обновляем данные после редактирования
                }
            }
        }



        private void DeleteApplicationButton_Click(object sender, RoutedEventArgs e)
        {
            if (ApplicationsDataGrid.SelectedItem == null)
            {
                MessageBox.Show("Выберите запись для удаления.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                DataRowView row = (DataRowView)ApplicationsDataGrid.SelectedItem;
                int applicationId = (int)row["ApplicationID"];

                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    string query = "DELETE FROM Applications WHERE ApplicationID = @ApplicationID";
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@ApplicationID", applicationId);

                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Заявление удалено!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                    LoadApplicationsData();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка удаления данных: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }


        private void CreateFacultyButton_Click(object sender, RoutedEventArgs e)
        {
            FacultyDialog dialog = new FacultyDialog(); // Открываем диалог для создания факультета
            if (dialog.ShowDialog() == true)
            {
                try
                {
                    using (SqlConnection conn = new SqlConnection(connectionString))
                    {
                        conn.Open();
                        string query = "INSERT INTO Faculties (FacultyName, Description) VALUES (@FacultyName, @Description)";
                        SqlCommand cmd = new SqlCommand(query, conn);
                        cmd.Parameters.AddWithValue("@FacultyName", dialog.FacultyName);
                        cmd.Parameters.AddWithValue("@Description", dialog.Description ?? (object)DBNull.Value);
                        cmd.ExecuteNonQuery();
                    }

                    MessageBox.Show("Факультет успешно добавлен!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                    LoadFacultiesData();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка добавления данных: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void EditFacultyButton_Click(object sender, RoutedEventArgs e)
        {
            if (FacultiesDataGrid.SelectedItem is DataRowView row)
            {
                string facultyName = row["FacultyName"].ToString();
                string description = row["Description"].ToString();

                FacultyDialog dialog = new FacultyDialog(facultyName, description);
                if (dialog.ShowDialog() == true)
                {
                    try
                    {
                        using (SqlConnection conn = new SqlConnection(connectionString))
                        {
                            conn.Open();
                            string query = "UPDATE Faculties SET FacultyName = @FacultyName, Description = @Description WHERE FacultyID = @FacultyID";
                            SqlCommand cmd = new SqlCommand(query, conn);
                            cmd.Parameters.AddWithValue("@FacultyName", dialog.FacultyName);
                            cmd.Parameters.AddWithValue("@Description", dialog.Description ?? (object)DBNull.Value);
                            cmd.Parameters.AddWithValue("@FacultyID", row["FacultyID"]);
                            cmd.ExecuteNonQuery();
                        }

                        MessageBox.Show("Факультет успешно обновлен!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                        LoadFacultiesData();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Ошибка обновления данных: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
            else
            {
                MessageBox.Show("Выберите запись для редактирования.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void DeleteFacultyButton_Click(object sender, RoutedEventArgs e)
        {
            if (FacultiesDataGrid.SelectedItem == null)
            {
                MessageBox.Show("Выберите запись для удаления.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                DataRowView row = (DataRowView)FacultiesDataGrid.SelectedItem;
                int facultyId = (int)row["FacultyID"];

                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    string query = "DELETE FROM Faculties WHERE FacultyID = @FacultyID";
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@FacultyID", facultyId);
                    cmd.ExecuteNonQuery();
                }

                MessageBox.Show("Факультет удалён!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                LoadFacultiesData();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка удаления данных: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }



        private void CreateSpecializationButton_Click(object sender, RoutedEventArgs e)
        {
            SpecializationDialog dialog = new SpecializationDialog();
            if (dialog.ShowDialog() == true)
            {
                try
                {
                    using (SqlConnection conn = new SqlConnection(connectionString))
                    {
                        conn.Open();
                        string query = "INSERT INTO Specializations (SpecializationName, FacultyID) VALUES (@SpecializationName, @FacultyID)";
                        SqlCommand cmd = new SqlCommand(query, conn);
                        cmd.Parameters.AddWithValue("@SpecializationName", dialog.SpecializationName);
                        cmd.Parameters.AddWithValue("@FacultyID", dialog.FacultyID);
                        cmd.ExecuteNonQuery();
                    }

                    MessageBox.Show("Специальность успешно добавлена!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                    LoadSpecializationsData();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка добавления данных: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void EditSpecializationButton_Click(object sender, RoutedEventArgs e)
        {
            if (SpecializationsDataGrid.SelectedItem is DataRowView row)
            {
                string specializationName = row["SpecializationName"].ToString();
                int facultyId = Convert.ToInt32(row["FacultyID"]);

                SpecializationDialog dialog = new SpecializationDialog(specializationName, facultyId);
                if (dialog.ShowDialog() == true)
                {
                    try
                    {
                        using (SqlConnection conn = new SqlConnection(connectionString))
                        {
                            conn.Open();
                            string query = "UPDATE Specializations SET SpecializationName = @SpecializationName, FacultyID = @FacultyID WHERE SpecializationID = @SpecializationID";
                            SqlCommand cmd = new SqlCommand(query, conn);
                            cmd.Parameters.AddWithValue("@SpecializationName", dialog.SpecializationName);
                            cmd.Parameters.AddWithValue("@FacultyID", dialog.FacultyID);
                            cmd.Parameters.AddWithValue("@SpecializationID", row["SpecializationID"]);
                            cmd.ExecuteNonQuery();
                        }

                        MessageBox.Show("Специальность успешно обновлена!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                        LoadSpecializationsData();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Ошибка обновления данных: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
            else
            {
                MessageBox.Show("Выберите запись для редактирования.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void DeleteSpecializationButton_Click(object sender, RoutedEventArgs e)
        {
            if (SpecializationsDataGrid.SelectedItem == null)
            {
                MessageBox.Show("Выберите запись для удаления.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                DataRowView row = (DataRowView)SpecializationsDataGrid.SelectedItem;
                int specializationId = (int)row["SpecializationID"];

                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    string query = "DELETE FROM Specializations WHERE SpecializationID = @SpecializationID";
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@SpecializationID", specializationId);
                    cmd.ExecuteNonQuery();
                }

                MessageBox.Show("Специальность удалена!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                LoadSpecializationsData();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка удаления данных: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void CreateStageButton_Click(object sender, RoutedEventArgs e)
        {
            StageDialog dialog = new StageDialog();
            if (dialog.ShowDialog() == true)
            {
                try
                {
                    using (SqlConnection conn = new SqlConnection(connectionString))
                    {
                        conn.Open();
                        string query = "INSERT INTO AdmissionStages (StageName, StartDate, EndDate) VALUES (@StageName, @StartDate, @EndDate)";
                        SqlCommand cmd = new SqlCommand(query, conn);
                        cmd.Parameters.AddWithValue("@StageName", dialog.StageName);
                        cmd.Parameters.AddWithValue("@StartDate", dialog.StartDate ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@EndDate", dialog.EndDate ?? (object)DBNull.Value);
                        cmd.ExecuteNonQuery();
                    }

                    MessageBox.Show("Этап приема успешно добавлен!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                    LoadAdmissionStages();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка добавления данных: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void EditStageButton_Click(object sender, RoutedEventArgs e)
        {
            if (AdmissionStagesDataGrid.SelectedItem is DataRowView row)
            {
                string stageName = row["StageName"].ToString();
                DateTime? startDate = row["StartDate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(row["StartDate"]);
                DateTime? endDate = row["EndDate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(row["EndDate"]);

                StageDialog dialog = new StageDialog(stageName, startDate, endDate);
                if (dialog.ShowDialog() == true)
                {
                    try
                    {
                        using (SqlConnection conn = new SqlConnection(connectionString))
                        {
                            conn.Open();
                            string query = "UPDATE AdmissionStages SET StageName = @StageName, StartDate = @StartDate, EndDate = @EndDate WHERE StageID = @StageID";
                            SqlCommand cmd = new SqlCommand(query, conn);
                            cmd.Parameters.AddWithValue("@StageName", dialog.StageName);
                            cmd.Parameters.AddWithValue("@StartDate", dialog.StartDate ?? (object)DBNull.Value);
                            cmd.Parameters.AddWithValue("@EndDate", dialog.EndDate ?? (object)DBNull.Value);
                            cmd.Parameters.AddWithValue("@StageID", row["StageID"]);
                            cmd.ExecuteNonQuery();
                        }

                        MessageBox.Show("Этап приема успешно обновлен!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                        LoadAdmissionStages();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Ошибка обновления данных: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
            else
            {
                MessageBox.Show("Выберите запись для редактирования.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void DeleteStageButton_Click(object sender, RoutedEventArgs e)
        {
            if (AdmissionStagesDataGrid.SelectedItem == null)
            {
                MessageBox.Show("Выберите запись для удаления.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                DataRowView row = (DataRowView)AdmissionStagesDataGrid.SelectedItem;
                int stageId = (int)row["StageID"];

                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    string query = "DELETE FROM AdmissionStages WHERE StageID = @StageID";
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@StageID", stageId);
                    cmd.ExecuteNonQuery();
                }

                MessageBox.Show("Этап приема удалён!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                LoadAdmissionStages();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка удаления данных: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }


        private void CreateExamResultButton_Click(object sender, RoutedEventArgs e)
        {
            ExamResultDialog dialog = new ExamResultDialog();
            if (dialog.ShowDialog() == true)
            {
                try
                {
                    using (SqlConnection conn = new SqlConnection(connectionString))
                    {
                        conn.Open();
                        string query = "INSERT INTO ExamResults (ApplicationID, StageID, ExamScore) VALUES (@ApplicationID, @StageID, @ExamScore)";
                        SqlCommand cmd = new SqlCommand(query, conn);
                        cmd.Parameters.AddWithValue("@ApplicationID", dialog.ApplicationID);
                        cmd.Parameters.AddWithValue("@StageID", dialog.StageID);
                        cmd.Parameters.AddWithValue("@ExamScore", dialog.ExamScore);
                        cmd.ExecuteNonQuery();
                    }

                    MessageBox.Show("Результат экзамена добавлен!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                    LoadExamResults();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка добавления данных: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void EditExamResultButton_Click(object sender, RoutedEventArgs e)
        {
            if (ExamResultsDataGrid.SelectedItem is DataRowView row)
            {
                int applicationId = Convert.ToInt32(row["ApplicationID"]);
                int stageId = Convert.ToInt32(row["StageID"]);
                double examScore = Convert.ToDouble(row["ExamScore"]);

                ExamResultDialog dialog = new ExamResultDialog(applicationId, stageId, examScore);
                if (dialog.ShowDialog() == true)
                {
                    try
                    {
                        using (SqlConnection conn = new SqlConnection(connectionString))
                        {
                            conn.Open();
                            string query = "UPDATE ExamResults SET ApplicationID = @ApplicationID, StageID = @StageID, ExamScore = @ExamScore WHERE ResultID = @ResultID";
                            SqlCommand cmd = new SqlCommand(query, conn);
                            cmd.Parameters.AddWithValue("@ApplicationID", dialog.ApplicationID);
                            cmd.Parameters.AddWithValue("@StageID", dialog.StageID);
                            cmd.Parameters.AddWithValue("@ExamScore", dialog.ExamScore);
                            cmd.Parameters.AddWithValue("@ResultID", row["ResultID"]);
                            cmd.ExecuteNonQuery();
                        }

                        MessageBox.Show("Результат экзамена обновлен!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                        LoadExamResults();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Ошибка обновления данных: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
            else
            {
                MessageBox.Show("Выберите запись для редактирования.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void DeleteExamResultButton_Click(object sender, RoutedEventArgs e)
        {
            if (ExamResultsDataGrid.SelectedItem == null)
            {
                MessageBox.Show("Выберите запись для удаления.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                DataRowView row = (DataRowView)ExamResultsDataGrid.SelectedItem;
                int resultId = (int)row["ResultID"];

                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    string query = "DELETE FROM ExamResults WHERE ResultID = @ResultID";
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@ResultID", resultId);
                    cmd.ExecuteNonQuery();
                }

                MessageBox.Show("Результат экзамена удалён!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                LoadExamResults();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка удаления данных: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }



        private void ApplicantSearchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            var filter = ApplicantSearchTextBox.Text.ToLower();
            var dataView = (DataView)ApplicantsDataGrid.ItemsSource;
            if (dataView != null)
            {
                // Фильтрация по нескольким столбцам
                dataView.RowFilter = $"FirstName LIKE '%{filter}%' OR LastName LIKE '%{filter}%' OR Address LIKE '%{filter}%' OR Gender LIKE '%{filter}%' OR PhoneNumber LIKE '%{filter}%' OR PassportNumber LIKE '%{filter}%' OR Email LIKE '%{filter}%'";
                ApplicantsDataGrid.ItemsSource = dataView;
            }
        }

        private void ApplicationSearchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            var filter = ApplicationSearchTextBox.Text.ToLower();
            var dataView = (DataView)ApplicationsDataGrid.ItemsSource;
            if (dataView != null)
            {
                // Фильтрация по нескольким столбцам: ApplicantName, SpecializationName, FacultyName
                dataView.RowFilter = $"ApplicantName LIKE '%{filter}%' OR SpecializationName LIKE '%{filter}%' OR FacultyName LIKE '%{filter}%' OR Status LIKE '%{filter}%'";
                ApplicationsDataGrid.ItemsSource = dataView;
            }
        }


        private void FacultySearchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            var filter = FacultySearchTextBox.Text.ToLower();
            var dataView = (DataView)FacultiesDataGrid.ItemsSource;
            if (dataView != null)
            {
                dataView.RowFilter = $"FacultyName LIKE '%{filter}%' OR Description LIKE '%{filter}%'";
                FacultiesDataGrid.ItemsSource = dataView;
            }
        }

        private void SpecializationSearchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            var filter = SpecializationSearchTextBox.Text.ToLower();
            var dataView = (DataView)SpecializationsDataGrid.ItemsSource;
            if (dataView != null)
            {
                dataView.RowFilter = $"SpecializationName LIKE '%{filter}%' OR FacultyName LIKE '%{filter}%'";
                SpecializationsDataGrid.ItemsSource = dataView;
            }
        }

        private void AdmissionStageSearchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            var filter = AdmissionStageSearchTextBox.Text.ToLower();
            var dataView = (DataView)AdmissionStagesDataGrid.ItemsSource;
            if (dataView != null)
            {
                dataView.RowFilter = $"StageName LIKE '%{filter}%'";
                AdmissionStagesDataGrid.ItemsSource = dataView;
            }
        }

        private void ExamResultSearchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            var filter = ExamResultSearchTextBox.Text.ToLower();

            // Если фильтр пустой, восстанавливаем все данные
            if (string.IsNullOrWhiteSpace(filter))
            {
                ExamResultsDataGrid.ItemsSource = _originalDataTable.DefaultView;
            }
            else
            {
                var dataView = (DataView)ExamResultsDataGrid.ItemsSource;

                if (dataView != null)
                {
                    // Применяем фильтрацию вручную, обрабатывая данные
                    var filteredRows = dataView.Table.AsEnumerable()
                        .Where(row =>
                        {
                            var applicantName = row.Field<string>("ApplicantName")?.ToLower();
                            var stageName = row.Field<string>("StageName")?.ToLower();
                            var examScore = row.Field<decimal>("ExamScore").ToString().ToLower(); // Преобразуем ExamScore в строку

                            return (applicantName?.Contains(filter) ?? false) ||
                                   (stageName?.Contains(filter) ?? false) ||
                                   (examScore?.Contains(filter) ?? false);
                        })
                        .ToList();

                    // Если есть отфильтрованные строки, обновляем DataGrid
                    if (filteredRows.Any())
                    {
                        var filteredTable = filteredRows.CopyToDataTable();  // Создаем новый DataTable из отфильтрованных строк
                        ExamResultsDataGrid.ItemsSource = filteredTable.DefaultView;  // Обновляем источник данных
                    }
                    else
                    {
                        // Если нет отфильтрованных данных, показываем все
                        ExamResultsDataGrid.ItemsSource = _originalDataTable.DefaultView;
                    }
                }
            }
        }

        private void TabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Обработчик изменения вкладки, если нужен
        }

        private void ExportToExcelButton_Click(object sender, RoutedEventArgs e)
        {
            if (TabControl.SelectedItem != null)
            {
                TabItem selectedTab = (TabItem)TabControl.SelectedItem;

                DataGrid dataGrid = null;

                if (selectedTab.Header.ToString() == "Абитуриенты")
                {
                    dataGrid = ApplicantsDataGrid;
                }
                else if (selectedTab.Header.ToString() == "Заявления")
                {
                    dataGrid = ApplicationsDataGrid;
                }
                else if (selectedTab.Header.ToString() == "Факультеты")
                {
                    dataGrid = FacultiesDataGrid;
                }
                else if (selectedTab.Header.ToString() == "Специальности")
                {
                    dataGrid = SpecializationsDataGrid;
                }
                else if (selectedTab.Header.ToString() == "Этапы приема")
                {
                    dataGrid = AdmissionStagesDataGrid;
                }
                else if (selectedTab.Header.ToString() == "Результаты экзаменов")
                {
                    dataGrid = ExamResultsDataGrid;
                }
                else if (selectedTab.Header.ToString() == "Пользователи")
                {
                    dataGrid = UsersDataGrid;
                }

                if (dataGrid != null)
                {
                    var dataTable = (DataTable)((DataView)dataGrid.ItemsSource).ToTable();

                    var saveFileDialog = new SaveFileDialog
                    {
                        Filter = "Excel Files (*.xlsx)|*.xlsx",
                        FileName = "export.xlsx"
                    };

                    if (saveFileDialog.ShowDialog() == true)
                    {
                        string filePath = saveFileDialog.FileName;

                        using (var workbook = new XLWorkbook())
                        {
                            var worksheet = workbook.Worksheets.Add("Sheet1");

                            // Заголовки столбцов
                            for (int col = 0; col < dataTable.Columns.Count; col++)
                            {
                                var headerCell = worksheet.Cell(1, col + 1);
                                headerCell.Value = dataTable.Columns[col].ColumnName;

                                headerCell.Style.Font.Bold = true;
                                headerCell.Style.Fill.BackgroundColor = XLColor.FromArgb(92, 184, 92); // Зеленый фон
                                headerCell.Style.Font.FontColor = XLColor.White; // Белый цвет текста
                                headerCell.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                                headerCell.Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                                headerCell.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                                headerCell.Style.Border.InsideBorder = XLBorderStyleValues.Thin;
                            }

                            // Данные
                            for (int row = 0; row < dataTable.Rows.Count; row++)
                            {
                                for (int col = 0; col < dataTable.Columns.Count; col++)
                                {
                                    var value = dataTable.Rows[row][col];

                                    // Приводим значение к строковому типу
                                    worksheet.Cell(row + 2, col + 1).Value = value?.ToString() ?? string.Empty;

                                    worksheet.Cell(row + 2, col + 1).Style.Fill.BackgroundColor = XLColor.FromArgb(235, 248, 235); // Светло-зеленый фон
                                    worksheet.Cell(row + 2, col + 1).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                                    worksheet.Cell(row + 2, col + 1).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                                    worksheet.Cell(row + 2, col + 1).Style.Border.InsideBorder = XLBorderStyleValues.Thin;
                                }
                            }

                            worksheet.Columns().AdjustToContents();
                            foreach (var column in worksheet.Columns())
                            {
                                column.Width = column.Width + 2; // Немного увеличиваем ширину столбцов
                            }

                            worksheet.Range(worksheet.FirstCellUsed(), worksheet.LastCellUsed()).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                            worksheet.Range(worksheet.FirstCellUsed(), worksheet.LastCellUsed()).Style.Border.InsideBorder = XLBorderStyleValues.Thin;
                            worksheet.Range(worksheet.FirstCellUsed(), worksheet.LastCellUsed()).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;

                            workbook.SaveAs(filePath);
                            MessageBox.Show("Экспорт завершен!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Невозможно экспортировать данные: вкладка не поддерживает экспорт.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                MessageBox.Show("Не удалось получить активную вкладку.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void UpdateExamResultsStatusButton_Click(object sender, RoutedEventArgs e)
        {
            string updateQuery = @"
        UPDATE ExamResults
        SET StageID = CASE 
            WHEN ExamScore >= 70 THEN 9  -- Зачисление
            ELSE 20  -- Не принят
        END
        WHERE StageID != 9 AND StageID != 20";  // Только если ещё не установлено

            int rowsAffected = ExecuteNonQuery(updateQuery);

            if (rowsAffected > 0)
            {
                MessageBox.Show($"Обновлено записей: {rowsAffected}", "Статусы обновлены", MessageBoxButton.OK, MessageBoxImage.Information);
                LoadExamResults(); // Перезагружаем данные
            }
            else
            {
                MessageBox.Show("Изменений нет, все статусы актуальны.", "Обновление завершено", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }



    }
}
