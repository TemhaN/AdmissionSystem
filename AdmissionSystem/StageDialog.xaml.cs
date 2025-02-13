using System;
using System.Windows;

namespace AdmissionSystem
{
    public partial class StageDialog : Window
    {
        public string StageName { get; private set; }
        public DateTime? StartDate { get; private set; }
        public DateTime? EndDate { get; private set; }

        public StageDialog()
        {
            InitializeComponent();
        }

        public StageDialog(string stageName, DateTime? startDate, DateTime? endDate)
        {
            InitializeComponent();
            StageNameTextBox.Text = stageName;
            StartDatePicker.SelectedDate = startDate;
            EndDatePicker.SelectedDate = endDate;
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            StageName = StageNameTextBox.Text.Trim();
            StartDate = StartDatePicker.SelectedDate;
            EndDate = EndDatePicker.SelectedDate;

            if (string.IsNullOrWhiteSpace(StageName))
            {
                MessageBox.Show("Введите название этапа.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
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
