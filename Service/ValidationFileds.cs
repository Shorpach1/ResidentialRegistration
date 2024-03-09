using ResidentialRegistration.CB;
using System;
using System.Text.RegularExpressions;
using System.Windows;

namespace ResidentialRegistration.Service
{
    internal class ValidationFileds
    {
        #region Проверка при регистрации
        public bool ValidateFields(string username, string firstPassword, string secondPassword, string surname, string firstName, string middleName)
        {
            if (!ValidationAuth(username))
            {
                return false;
            }

            if (!ValidationTwoPasswords(firstPassword, secondPassword))
            {
                return false;
            }

            if (!ValidationPassword(firstPassword))
            {
                return false;
            }

            if (!ValidationSurname(surname))
            {
                return false;
            }

            if (!ValidationFirstName(firstName))
            {
                return false;
            }

            if (!ValidationMiddleName(middleName))
            {
                return false;
            }

            return true;
        }
        #endregion

        #region Проверка при добавлении гражданина
        public bool ValidationCitizenAdd(string surname, string firstName, string middleName, DateTime dateOfBrith, string gender, string placeOfBrith)
        {
            if (!ValidationSurname(surname))
            {
                return false;
            }

            if (!ValidationFirstName(firstName))
            {
                return false;
            }

            if (!ValidationMiddleName(middleName))
            {
                return false;
            }
            
            if (!ValiditonDateOfBirth(dateOfBrith))
            {
                return false;
            }

            if (!ValidateNotEmpty(gender))
            {
                return false;
            }

            if (!ValidateBirthPlace(placeOfBrith))
            {
                return false;
            }

            return true;
        }
        #endregion

        #region Проверка при добавлении документа
        public bool ValidationIssuedDocumentsAdd(CBDocumentType dtId, string docNum, DateTime dateOfIssue, string issuingAuthority, CBDocumentType citId)
        {
            if (!ValidationComboBoxItemDT(dtId))
            {
                return false;
            }

            if (!ValidateDocumentNumber(docNum))
            {
                return false;
            }

            if (!ValiditonDateOfIssue(dateOfIssue))
            {
                return false;
            }

            if (!ValidateIssuingAuthority(issuingAuthority))
            {
                return false;
            }

            if (!ValidationComboBoxItemC(citId))
            {
                return false;
            }

            return true;
        }
        #endregion

        #region Проверка при добавлении жилой площади
        public bool ValidationResidentialUnitsAdd(string address, string area, string numOfRooms, CBDocumentType citId, DateTime date, string ifOwner)
        {
            if (!ValidateAddress(address))
            {
                return false;
            }

            if (!ValidateQuantity(area))
            {
                return false;
            }

            if (!ValidationNumOfRooms(numOfRooms))
            {
                return false;
            }

            if (!ValiditonDateOfConstruction(date))
            {
                return false;
            }

            if (!ValidateNotEmptyRU(ifOwner))
            {
                return false;
            }

            if (!ValidationComboBoxItemC(citId))
            {
                return false;
            }

            return true;
        }
        #endregion

        #region Проверка при добавлении ад. листка прибыития
        public bool ValidationAASAdd(CBDocumentType citId, string address, DateTime date, string regAuth)
        {
            if (!ValidateAddress(address))
            {
                return false;
            }

            if (!ValiditonDateOfDepature(date))
            {
                return false;
            }

            if (!ValidateRegistrationAuthority(regAuth))
            {
                return false;
            }

            if (!ValidationComboBoxItemC(citId))
            {
                return false;
            }

            return true;
        }
        #endregion

        #region На схожесть поролей
        public bool ValidationTwoPasswords(string firstPassword, string secondPassword)
        {
            if (!firstPassword.Equals(secondPassword))
            {
                MessageBox.Show("Пароли должны совпадать!");
                return false;
            }

            return true;
        }
        #endregion

        #region На фамилию
        public bool ValidationSurname(string surname)
        {
            string surnamePattern = @"^[a-zA-Zа-яА-Я]*-?[a-zA-Zа-яА-Я]{3,29}$";

            if (!Regex.IsMatch(surname, surnamePattern) || (surname.Length < 3) || (surname.Length > 30))
            {
                MessageBox.Show("Минимальный размер фамилии - 3, без знаков и цифр, максимальный размер 30! " + surname);
                return false;
            }

            return true;
        }

        #endregion

        #region На имя
        public bool ValidationFirstName(string firstName)
        {
            string firstNamePattern = @"^[a-zA-Zа-яА-Я]*-?[a-zA-Zа-яА-Я]{3,24}$";

            if (!Regex.IsMatch(firstName, firstNamePattern) || (firstName.Length < 3) || (firstName.Length > 25))
            {
                MessageBox.Show("Минимальный размер имени - 3, без знаков и цифр, максимальный размер 25!");
                return false;
            }

            return true;
        }
        #endregion

        #region На отчество
        public bool ValidationMiddleName(string middleName)
        {
            string middleNamePattern = @"^[a-zA-Zа-яА-Я]*-?[a-zA-Zа-яА-Я]{3,29}$";

            if (!Regex.IsMatch(middleName, middleNamePattern) || (middleName.Length < 3) || (middleName.Length > 25))
            {
                MessageBox.Show("Минимальный размер отчества - 3, без символов и цифр, максимальный размер 30!");
                return false;
            }

            return true;
        }
        #endregion

        #region На пароль
        public bool ValidationPassword(string password)
        {
            string pattern = @"^(?=.*[A-Z])(?=.*\d)[A-Za-z\d]{6,30}$";

            if (!Regex.IsMatch(password, pattern))
            {
                MessageBox.Show("Минимальный размер пароля - 6, минимум 1 заглавная буква и минимум 1 цифра, максимальный размер 30!");
                return false;
            }

            return true;
        }
        #endregion

        #region На логин
        public bool ValidationAuth(string username)
        {
            string patternUsername = @"^[a-zA-Z]{4,12}$";

            if (!Regex.IsMatch(username, patternUsername))
            {
                MessageBox.Show("Логин должен быть длинной от 4х и до 12 латинскими символами!");
                return false;
            }

            return true;
        }
        #endregion

        #region На роль
        public string ReturnRole(bool isAdmin)
        {
            if (isAdmin)
                return "ROLE_ADMIN";
            else
                return "ROLE_USER";
        }
        #endregion

        #region На дату рождения
        public bool ValiditonDateOfBirth(DateTime dateOfBrith)
        {
            if (dateOfBrith > DateTime.Today)
            {
                MessageBox.Show("Дата рождения не может быть позже сегодняшней даты!");
                return false;
            }

            return true;
        }
        #endregion

        #region На пустоту в "Пол"
        public bool ValidateNotEmpty(string fieldValue)
        {
            if (string.IsNullOrWhiteSpace(fieldValue))
            {
                MessageBox.Show("Выберите пол");
                return false;
            }

            return true;
        }
        #endregion

        #region На место рождения
        public bool ValidateBirthPlace(string placeOfBrith)
        {
            string placeOfBrithPattern = @"^[a-zA-Zа-яА-Я0-9\s.,!?-]{3,50}$";

            if (!Regex.IsMatch(placeOfBrith, placeOfBrithPattern) || placeOfBrith.Length > 50)
            {
                MessageBox.Show("Некорректное место рождения. Минимальная длина - 3 символа, максимальная - 50!");
                return false;
            }

            return true;
        }
        #endregion

        #region На выбор типа документа
        public bool ValidationComboBoxItemDT(CBDocumentType combo)
        {
            if (combo == null)
            {
                MessageBox.Show("Выберите тип документа!");
                return false;
            }

            return true;
        }
        #endregion

        #region На выбор владельца
        public bool ValidationComboBoxItemC(CBDocumentType combo)
        {
            if (combo == null)
            {
                MessageBox.Show("Выберите владельца!");
                return false;
            }

            return true;
        }
        #endregion

        #region На дату регистрации
        public bool ValiditonDateOfIssue(DateTime dateOfIssue)
        {
            if (dateOfIssue > DateTime.Today)
            {
                MessageBox.Show("Дата регистрации не может быть позже сегодняшней даты!");
                return false;
            }

            return true;
        }
        #endregion

        #region На орган выдачи
        public bool ValidateIssuingAuthority(string IssuingAuthority)
        {
            string IssuingAuthorityPattern = @"^[a-zA-Zа-яА-Я0-9\s.,!?-]{3,50}$";

            if (!Regex.IsMatch(IssuingAuthority, IssuingAuthorityPattern) || IssuingAuthority.Length > 50)
            {
                MessageBox.Show("Некорректное наименование органа выдачи. Минимальная длина - 3 символа, максимальная - 50!");
                return false;
            }

            return true;
        }
        #endregion

        #region На номер документа
        public bool ValidateDocumentNumber(string DocumentNumber)
        {
            string DocumentNumberPattern = @"^\d{10}$";

            if (!Regex.IsMatch(DocumentNumber, DocumentNumberPattern) )
            {
                MessageBox.Show("Некорректный номер документа. Номер документа должен состоять из 10 цифр");
                return false;
            }

            return true;
        }
        #endregion

        #region На адрес
        public bool ValidateAddress(string address)
        {
            string addressPattern = @"^[a-zA-Zа-яА-ЯёЁ0-9\s.,!?-]{3,50}$";

            if (!Regex.IsMatch(address, addressPattern) || address.Length > 50)
            {
                MessageBox.Show("Некорректный адрес. Минимальная длина - 3 символа, максимальная - 50!");
                return false;
            }

            return true;
        }
        #endregion

        #region На дату постройки
        public bool ValiditonDateOfConstruction(DateTime DateOfConstruction)
        {
            if (DateOfConstruction > DateTime.Today)
            {
                MessageBox.Show("Дата постройки не может быть позже сегодняшней даты!");
                return false;
            }

            return true;
        }
        #endregion

        #region На пустоту в комбобоксе Владельца
        public bool ValidateNotEmptyRU(string fieldValue)
        {
            if (string.IsNullOrWhiteSpace(fieldValue))
            {
                MessageBox.Show("Выберите Владелец/проживающий");
                return false;
            }

            return true;
        }
        #endregion

        #region На площадь
        private bool ValidateQuantity(string quantity)
        {
            if (int.TryParse(quantity, out int quantityInt))
            {
                if (quantityInt < 10 || quantityInt > 1000)
                {
                    MessageBox.Show("Площадь должна быть не меньше 10 и не больше 1000");
                    return false;
                }
            }
            else
            {
                MessageBox.Show("Укажите корректное значение!");
                return false;
            }

            return true;
        }


        #endregion

        #region На кол-во комнат
        private bool ValidationNumOfRooms(string quantity)
        {
            string pattern = @"^\d+$";


            if (int.TryParse(quantity, out int quantityInt))
            {
                if (!Regex.IsMatch(quantityInt.ToString(), pattern) || quantityInt <= 0 || quantityInt > 50)
                {
                    MessageBox.Show("Количество комнат не должно быть меньше 0 и больше 50 ");
                    return false;
                }
            }
            else
            {
                MessageBox.Show("Введите кол-во комнат!");
                return false;
            }

            return true;
        }
        #endregion

        #region На дату прибытия
        public bool ValiditonDateOfDepature(DateTime DateOfDepature)
        {
            if (DateOfDepature > DateTime.Today)
            {
                MessageBox.Show("Дата прибытия не может быть позже сегодняшней даты!");
                return false;
            }

            return true;
        }
        #endregion

        #region На регистрациооный орган
        public bool ValidateRegistrationAuthority(string regAuth)
        {
            string regAuthPattern = @"^[a-zA-Zа-яА-ЯёЁ0-9\s.,!?-]{3,50}$";

            if (!Regex.IsMatch(regAuth, regAuthPattern) || regAuth.Length > 50)
            {
                MessageBox.Show("Некорректное наименование регистрационного органа. Минимальная длина - 3 символа, максимальная - 50!");
                return false;
            }

            return true;
        }
        #endregion
    }
}

