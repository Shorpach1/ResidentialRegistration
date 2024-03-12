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
    internal class WordApartmentCard
    {
        Database database = new Database();
        private readonly Application _wordApplication;
        private readonly Document _document;

        public WordApartmentCard()
        {
            // Создаем экземпляр приложения Word
            _wordApplication = new Application();
            // Загружаем шаблон Word документа
            string templatePath = @"D:\CourseWork\ResidentialRegistration\Resource\ApartmentCard.docx";
            _document = _wordApplication.Documents.Open(templatePath);
        }

        public void ReplaceFieldsFromDatabase(int selectedID, string address)
        {
            // Создаем строку подключения к вашей базе данных
            string connectionString = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=Residential_Registration_Database;Integrated Security=True;Connect Timeout=30;Encrypt=False;";

            // Напишите SQL-запрос для получения данных о владельце
            string ownerQuery = "select Citizens.CitizenID, LastName, FirstName, MiddleName, DateOfBirth from Citizens, ResidentialUnits where ResidentialUnits.CitizenID = Citizens.CitizenID and isOwner = N'Владелец' and ResidentialUnits.CitizenID = @CitizenID and ResidentialUnits.Address = @Address";

            // Напишите SQL-запрос для получения списка граждан, проживающих по указанному адресу
            string citizensQuery = "select Citizens.CitizenID, Citizens.LastName + ' ' + Citizens.FirstName + ' ' + Citizens.MiddleName as Fio, Citizens.DateOfBirth from Citizens, ResidentialUnits where Citizens.CitizenID = ResidentialUnits.CitizenID and ResidentialUnits.Address = @Address";

            string fileName = "";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                // Открываем подключение к базе данных
                connection.Open();

                // Создаем команду для запроса данных о владельце
                using (SqlCommand ownerCommand = new SqlCommand(ownerQuery, connection))
                {
                    // Добавляем параметры для передачи выбранного ID и адреса
                    ownerCommand.Parameters.AddWithValue("@CitizenID", selectedID);
                    ownerCommand.Parameters.AddWithValue("@Address", address);

                    // Выполняем запрос и получаем результат
                    SqlDataReader ownerReader = ownerCommand.ExecuteReader();

                    fileName = $"Поквартирная карточка №{selectedID}.docx";

                    if (ownerReader.Read())
                    {
                        // Получаем данные о владельце из результата запроса
                        string ownerLastName = ownerReader["LastName"].ToString();
                        string ownerFirstName = ownerReader["FirstName"].ToString();
                        string ownerMiddleName = ownerReader["MiddleName"].ToString();

                        // Заменяем метки в шаблоне на данные о владельце
                        ReplaceField("{OwnerLastName}", ownerLastName);
                        ReplaceField("{OwnerFirstName}", ownerFirstName);
                        ReplaceField("{OwnerMiddleName}", ownerMiddleName);
                    }
                    ownerReader.Close();
                }

                // Заменяем метку в шаблоне на адрес
                ReplaceField("{Address}", address);

                // Создаем команду для запроса списка граждан по указанному адресу
                using (SqlCommand citizensCommand = new SqlCommand(citizensQuery, connection))
                {
                    // Добавляем параметр для передачи адреса
                    citizensCommand.Parameters.AddWithValue("@Address", address);

                    // Выполняем запрос и получаем результат
                    SqlDataReader citizensReader = citizensCommand.ExecuteReader();

                    int index = 1;

                    // Заменяем метки в шаблоне на информацию о гражданах
                    while (citizensReader.Read())
                    {
                        string citizenID = citizensReader["CitizenID"].ToString();
                        string fio = citizensReader["Fio"].ToString();
                        string dateOfBirth = Convert.ToDateTime(citizensReader["DateOfBirth"]).ToString("dd.MM.yyyy");

                        ReplaceField($"{{CitizenID_{index}}}", citizenID);
                        ReplaceField($"{{Fio_{index}}}", fio);
                        ReplaceField($"{{DateOfBirth_{index}}}", dateOfBirth);

                        index++;
                    }

                    citizensReader.Close();

                    // Заменяем оставшиеся метки, если не хватило строк для граждан
                    for (int i = index; i <= 10; i++)
                    {
                        ReplaceField($"{{CitizenID_{i}}}", "");
                        ReplaceField($"{{Fio_{i}}}", "");
                        ReplaceField($"{{DateOfBirth_{i}}}", "");
                    }
                }

                SaveAndCloseDocument($"D:\\CourseWork\\ResidentialRegistration\\Output\\ApartmentCard\\{fileName}");
            }
        }

        private void ReplaceField(string field, string value)
        {
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

