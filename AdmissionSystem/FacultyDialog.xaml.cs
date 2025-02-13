using System;
using System.Windows;

namespace AdmissionSystem
{
    public partial class FacultyDialog : Window
    {
        public string FacultyName { get; private set; }
        public string Description { get; private set; }

        // Конструктор для создания нового факультета
        public FacultyDialog()
        {
            InitializeComponent();
        }

        // Конструктор для редактирования факультета
        public FacultyDialog(string facultyName, string description)
        {
            InitializeComponent();
            FacultyNameTextBox.Text = facultyName;
            DescriptionTextBox.Text = description;
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            FacultyName = FacultyNameTextBox.Text.Trim();
            Description = DescriptionTextBox.Text.Trim();

            if (string.IsNullOrWhiteSpace(FacultyName))
            {
                MessageBox.Show("Введите название факультета.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
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
