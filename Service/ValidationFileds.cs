using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
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
    }
}

