using System;
using System.IO;
using Microsoft.Office.Interop.Word;
using System.Data.SqlClient;
using ResidentialRegistration.Storage;

namespace ResidentialRegistration.Service
{
    internal class WordRegistrationСard
    {
        Database database = new Database();
        private readonly Application _wordApplication;
        private readonly Document _document;

        public WordRegistrationСard()
        {
            // Создаем экземпляр приложения Word
            _wordApplication = new Application();
            // Загружаем шаблон Word документа
            string templatePath = @"D:\CourseWork\ResidentialRegistration\Resource\RegistrationCard.docx";
            _document = _wordApplication.Documents.Open(templatePath);
        }

        public void ReplaceFieldsFromDatabase(int selectedID)
        {
            // Создаем строку подключения к вашей базе данных
            string connectionString = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=Residential_Registration_Database;Integrated Security=True;Connect Timeout=30;Encrypt=False;";

            // Напишите SQL-запрос для получения данных из базы данных
            string query = "select CitizenID, LastName, FirstName, MiddleName, DateOfBirth, PlaceOfBirth from Citizens where CitizenID = @CitizenID";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                // Открываем подключение к базе данных
                connection.Open();

                // Создаем команду с SQL-запросом и параметром
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    // Добавляем параметр для передачи выбранного ID
                    command.Parameters.AddWithValue("@CitizenID", selectedID);

                    // Выполняем запрос и получаем результат
                    SqlDataReader reader = command.ExecuteReader();

                    if (reader.Read())
                    {
                        // Получаем данные из результата запроса
                        string citizenID = reader["CitizenID"].ToString();
                        string lastName = reader["LastName"].ToString();
                        string firstName = reader["FirstName"].ToString();
                        string middleName = reader["MiddleName"].ToString();
                        string dateOfBirth = Convert.ToDateTime(reader["DateOfBirth"]).ToString("dd.MM.yyyy");
                        string placeOfBirth = reader["PlaceOfBirth"].ToString();

                        // Заменяем метки в шаблоне на данные из базы данных
                        ReplaceField("{CitizenID}", citizenID);
                        ReplaceField("{LastName}", lastName);
                        ReplaceField("{FirstName}", firstName);
                        ReplaceField("{MiddleName}", middleName);
                        ReplaceField("{DateOfBirth}", dateOfBirth);
                        ReplaceField("{PlaceOfBirth}", placeOfBirth);

                        SaveAndCloseDocument(@"D:\CourseWork\ResidentialRegistration\Output\Карточка_Регистрации.docx");
                    }
                }
            }
        }

        private void ReplaceField(string field, string value)
        {
            // Ищем текстовое выделение с меткой field в документе
            foreach (Range range in _document.StoryRanges)
            {
                range.Find.ClearFormatting();
                range.Find.Execute(FindText: field, ReplaceWith: value);
            }
        }

        public void SaveAndCloseDocument(string outputPath)
        {
            // Сохраняем изменения и закрываем документ
            _document.SaveAs2(outputPath);
            _document.Close();

            // Открываем сохраненный файл
            System.Diagnostics.Process.Start(outputPath);
        }
    }
}
