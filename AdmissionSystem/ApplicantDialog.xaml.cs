using System;
using System.Windows;
using System.Windows.Controls;

namespace AdmissionSystem
{
    public partial class ApplicantDialog : Window
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string Gender { get; set; }
        public string Address { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string PassportNumber { get; set; }

        public ApplicantDialog(string firstName = "", string lastName = "", DateTime? dateOfBirth = null, string gender = "",
                            string address = "", string phoneNumber = "", string email = "", string passportNumber = "")
        {
            InitializeComponent();

            // Инициализация значений, если переданы (или оставляем пустыми)
            FirstName = firstName;
            LastName = lastName;
            DateOfBirth = dateOfBirth;
            Gender = gender;
            Address = address;
            PhoneNumber = phoneNumber;
            Email = email;
            PassportNumber = passportNumber;

            // Заполнение полей на форме
            FirstNameTextBox.Text = FirstName;
            LastNameTextBox.Text = LastName;
            DateOfBirthPicker.SelectedDate = DateOfBirth;
            GenderComboBox.SelectedItem = GenderComboBox.Items.Cast<ComboBoxItem>().FirstOrDefault(item => item.Content.ToString() == Gender);
            AddressTextBox.Text = Address;
            PhoneNumberTextBox.Text = PhoneNumber;
            EmailTextBox.Text = Email;
            PassportNumberTextBox.Text = PassportNumber;
        }


        public delegate void DataUpdatedHandler();

        public event DataUpdatedHandler DataUpdated;

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            // Сохраняем данные в свойства (которые уже привязаны к текстовым полям)
            FirstName = FirstNameTextBox.Text;
            LastName = LastNameTextBox.Text;
            DateOfBirth = DateOfBirthPicker.SelectedDate;
            Gender = ((ComboBoxItem)GenderComboBox.SelectedItem)?.Content.ToString();
            Address = AddressTextBox.Text;
            PhoneNumber = PhoneNumberTextBox.Text;
            Email = EmailTextBox.Text;
            PassportNumber = PassportNumberTextBox.Text;

            // Возвращаем DialogResult как true, чтобы в основном окне обновились данные
            DialogResult = true;

            // Вызываем делегат, чтобы обновить данные в главном окне
            DataUpdated?.Invoke();

            Close();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}
