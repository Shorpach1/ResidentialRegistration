using Microsoft.Office.Interop.Word;
using ResidentialRegistration.Storage;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ResidentialRegistration.Service
{
    internal class WordAddressDepatureSheet
    {
        Database database = new Database();
        private readonly Application _wordApplication;
        private readonly Document _document;

        public WordAddressDepatureSheet()
        {
            // Создаем экземпляр приложения Word
            _wordApplication = new Application();
            // Загружаем шаблон Word документа
            string templatePath = @"D:\CourseWork\ResidentialRegistration\Resource\AddressDepatureSheet.docx";
            _document = _wordApplication.Documents.Open(templatePath);
        }

        public void ReplaceFieldsFromDatabase(int selectedID)
        {
            string connectionString = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=Residential_Registration_Database;Integrated Security=True;Connect Timeout=30;Encrypt=False;";

            string query = "SELECT AddressedDepartureSheet.AddressedDepartureSheetID, Citizens.LastName, Citizens.FirstName, Citizens.MiddleName, Citizens.DateOfBirth, " +
                "Citizens.Gender, Citizens.PlaceOfBirth, AddressedDepartureSheet.AddressOfFormerResidence, AddressedDepartureSheet.DateOfDeparture, " +
                "AddressedDepartureSheet.DepartureAddress, AddressedDepartureSheet.PlaceOfArrival FROM Citizens, AddressedDepartureSheet " +
                "WHERE Citizens.CitizenID = AddressedDepartureSheet.CitizenID AND AddressedDepartureSheet.AddressedDepartureSheetID = @ID;";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@ID", selectedID);

                    SqlDataReader reader = command.ExecuteReader();

                    if (reader.Read())
                    {
                        string addressDepartureSheetID = reader["AddressedDepartureSheetID"].ToString();
                        string lastName = reader["LastName"].ToString();
                        string firstName = reader["FirstName"].ToString();
                        string middleName = reader["MiddleName"].ToString();
                        string dateOfBirth = Convert.ToDateTime(reader["DateOfBirth"]).ToString("dd.MM.yyyy");
                        string gender = reader["Gender"].ToString();
                        string placeOfBirth = reader["PlaceOfBirth"].ToString();
                        string addressOfFormerResidence = reader["AddressOfFormerResidence"].ToString();
                        string dateOfDeparture = Convert.ToDateTime(reader["DateOfDeparture"]).ToString("dd.MM.yyyy");
                        string departureAddress = reader["DepartureAddress"].ToString();
                        string placeOfArrival = reader["PlaceOfArrival"].ToString();

                        string fileName = $"Адресной листок убытия №{selectedID}.docx";

                        ReplaceField("{AddressDepartureSheetID}", addressDepartureSheetID);
                        ReplaceField("{LastName}", lastName);
                        ReplaceField("{FirstName}", firstName);
                        ReplaceField("{MiddleName}", middleName);
                        ReplaceField("{DateOfBirth}", dateOfBirth);
                        ReplaceField("{Gender}", gender);
                        ReplaceField("{PlaceOfBirth}", placeOfBirth);
                        ReplaceField("{AddressOfFormerResidence}", addressOfFormerResidence);
                        ReplaceField("{DateOfDeparture}", dateOfDeparture);
                        ReplaceField("{DepartureAddress}", departureAddress);
                        ReplaceField("{PlaceOfArrival}", placeOfArrival);

                        SaveAndCloseDocument($"D:\\CourseWork\\ResidentialRegistration\\Output\\AddressDepatureSheets\\{fileName}");
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
