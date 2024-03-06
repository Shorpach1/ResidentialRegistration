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

    }
}

