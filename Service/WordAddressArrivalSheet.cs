using Microsoft.Office.Interop.Word;
using ResidentialRegistration.Storage;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ResidentialRegistration.Service
{
    internal class WordAddressArrivalSheet
    {
        Database database = new Database();
        private readonly Application _wordApplication;
        private readonly Document _document;

        public WordAddressArrivalSheet()
        {
            // Создаем экземпляр приложения Word
            _wordApplication = new Application();
            // Загружаем шаблон Word документа
            string templatePath = @"D:\CourseWork\ResidentialRegistration\Resource\AddressArrivalSheet.docx";
            _document = _wordApplication.Documents.Open(templatePath);
        }

        public void ReplaceFieldsFromDatabase(int selectedID)
        {
            // Создаем строку подключения к вашей базе данных
            string connectionString = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=Residential_Registration_Database;Integrated Security=True;Connect Timeout=30;Encrypt=False;";

            // Напишите SQL-запрос для получения данных из базы данных
            string query = "select AddressArrivalSheets.AddressArrivalSheetID, Citizens.LastName, Citizens.FirstName, Citizens.MiddleName, Citizens.DateOfBirth, " +
                "Citizens.Gender, Citizens.PlaceOfBirth, ResidentialUnits.Address, AddressArrivalSheets.DateOfDeparture, AddressArrivalSheets.DepartureAddress, " +
                "AddressArrivalSheets.RegistrationAuthority from Citizens, ResidentialUnits, AddressArrivalSheets where Citizens.CitizenID = ResidentialUnits.CitizenID and " +
                "Citizens.CitizenID = AddressArrivalSheets.CitizenID and AddressArrivalSheets.AddressArrivalSheetID = @ID;";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                // Открываем подключение к базе данных
                connection.Open();

                // Создаем команду с SQL-запросом и параметром
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    // Добавляем параметр для передачи выбранного ID
                    command.Parameters.AddWithValue("@ID", selectedID);

                    // Выполняем запрос и получаем результат
                    SqlDataReader reader = command.ExecuteReader();

                    if (reader.Read())
                    {
                        // Получаем данные из результата запроса
                        string addressArrivalSheetID = reader["AddressArrivalSheetID"].ToString();
                        string lastName = reader["LastName"].ToString();
                        string firstName = reader["FirstName"].ToString();
                        string middleName = reader["MiddleName"].ToString();
                        string dateOfBirth = Convert.ToDateTime(reader["DateOfBirth"]).ToString("dd.MM.yyyy");
                        string gender = reader["Gender"].ToString();
                        string placeOfBirth = reader["PlaceOfBirth"].ToString();
                        string address = reader["Address"].ToString();
                        string dateOfDeparture = Convert.ToDateTime(reader["DateOfDeparture"]).ToString("dd.MM.yyyy");
                        string departureAddress = reader["DepartureAddress"].ToString();
                        string registrationAuthority = reader["RegistrationAuthority"].ToString();

                        string fileName = $"Адресной листок прибытия №{selectedID}.docx";

                        // Заменяем метки в шаблоне на данные из базы данных
                        ReplaceField("{AddressArrivalSheetID}", addressArrivalSheetID);
                        ReplaceField("{LastName}", lastName);
                        ReplaceField("{FirstName}", firstName);
                        ReplaceField("{MiddleName}", middleName);
                        ReplaceField("{DateOfBirth}", dateOfBirth);
                        ReplaceField("{Gender}", gender);
                        ReplaceField("{PlaceOfBirth}", placeOfBirth);
                        ReplaceField("{Address}", address);
                        ReplaceField("{DateOfDeparture}", dateOfDeparture);
                        ReplaceField("{DepartureAddress}", departureAddress);
                        ReplaceField("{RegistrationAuthority}", registrationAuthority);

                        SaveAndCloseDocument($"D:\\CourseWork\\ResidentialRegistration\\Output\\AddressArrivalSheets\\{fileName}");
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
