﻿using Decrypt_Library.Models;
using System;
using System.Globalization;
using System.Linq;

namespace Decrypt_Library.Readers
{
    internal class Readers
    {

        #region Stringreaders
        public static bool StringReaderSpecifyStringRange(string userInput, int minNum, int maxNum)
        {
            if (!StringReader(userInput))
                return false;

            if (userInput.Length < minNum || userInput.Length > maxNum)
                return false;

            return true;
        }

        public static bool EmailReader(string emailInput)
        {
            if (emailInput.Length < 6) return false;
            if (emailInput.Length > 15) return false;
            if (!Contains(emailInput, '.')) return false;
            if (!Contains(emailInput, '@')) return false;

            return true;
        }

        private static bool Contains(string accountdetails, char letter)
        {
            foreach (char character in accountdetails)
                if (character == letter) return true;

            return false;
        }


        public static bool StringReader(string userInput)
        {
            if (!userInput.Any(char.IsLetter) || string.IsNullOrEmpty(userInput))
                return false;

            return true;
        }

        public static bool StringLengthEqualsTo(string userInput, int stringLength)
        {
            if (!StringReader(userInput))
                return false;

            if (userInput.Length != stringLength)
                return false;

            return true;
        }
        public static bool IsStringAndIsInt(string userInput)
        {
            if (userInput.Any(char.IsDigit) && userInput.Any(char.IsLetter))
                return true;

            return false;
        }

        public static bool StringPasswordCorrect(string passwordInput, int minNum, bool digits)
        {

            if (!IsStringAndIsInt(passwordInput))
                return false;

            if (passwordInput.Length < minNum)
                return false;

            if (digits == false)
                return false;

            return true;
        }
        // Reader för email


        #endregion

        #region int reader
        public static bool IntReaderString(string userInput)
        {
            if (userInput.Any(char.IsLetter))
                return false;

            return true;
        }

        public static bool PhoneNrReader(string phoneNrInput, int minNum)
        {
            if (IntReaderConvertStringToInt(phoneNrInput, out int num))
                return true;

            if (IntReaderString(phoneNrInput))
                return true;

            if (num == minNum)
                return false;

            return true;
        }

        public static bool SSNReader(string SSNInput, int minNum)
        {
            if (IntReaderConvertStringToInt(SSNInput, out int num))
                return true;

            if (IntReaderString(SSNInput))
                return true;

            if (num != minNum)
                return false;

            var users = EntityFrameworkCode.EntityframeworkUsers.ShowAllUsers();
            using (var db = new Models.Decrypt_LibraryContext())
            {
                if (users.Where(u => u.Ssn == num).ToList().Count() > 0)
                {
                    return false;
                }
            }


            return true;


            /*  using (var db = new Models.Decrypt_LibraryContext())
              {
                  var existingUsers = db.Users;
                  var exist = existingUsers.Where(s => s.Ssn == num);
                  if (exist.Any())
                      return false;
              }
            */

        }

      

        public static bool IntReaderConvertStringToInt(string userInput, out int num)
        {
            if (!int.TryParse(userInput, out num))
                return false;

            return true;
        }

        public static bool IntEqualsToSelectedNumber(string userInput, int selectedNumber, out int num)
        {
            num = 0;
            if (Int32.TryParse(userInput, out num))
            {
                if (userInput.Length != selectedNumber)
                {
                    return false;
                }
            }
            return true;
        }

        public static bool IntReaderSpecifyIntRange(string userInput, int minNum, int maxNum, out int num)
        {
            num = 0;

            if (Int32.TryParse(userInput, out num))
            {
                if (num <= maxNum && num >= minNum)
                    return true;
            }
            return true;
        }


        #endregion

        #region default readers

        public static bool LegalIDRangeLanguage(string userInput, out int convertedUserIdInput)
        {
            var languageList = EntityFrameworkCode.EntityframeworkLanguages.ShowAllLanguages();

            if (!IntReaderConvertStringToInt(userInput, out convertedUserIdInput))
                return false;

            if (convertedUserIdInput > languageList.Count || convertedUserIdInput < 1)
                return false;

            return true;
        }

        public static bool LegalIDRangeCategoryId(string userInput, out int convertedUserIdInput)
        {
            var categoryList = EntityFrameworkCode.EntityframeworkCategories.ShowAllCategories();

            if (!IntReaderConvertStringToInt(userInput, out convertedUserIdInput))
                return false;

            if (convertedUserIdInput > categoryList.Count || convertedUserIdInput < 1)
                return false;

            return true;
        }

        public static bool LegalIDRangeProductId(string userInput, out int convertedUserIdInput)
        {
            var productList = EntityFrameworkCode.EntityframeworkProducts.ShowAllProducts();

            if (!IntReaderConvertStringToInt(userInput, out convertedUserIdInput))
                return false;

            if (convertedUserIdInput > productList.Count || convertedUserIdInput < 1)
                return false;

            return true;
        }

        public static bool LegalIDRangeShelfId(string userInput, out int convertedUserIdInput)
        {
            var shelfList = EntityFrameworkCode.EntityframeworkShelf.ShowAllShelves();

            if (!IntReaderConvertStringToInt(userInput, out convertedUserIdInput))
                return false;

            if (convertedUserIdInput > shelfList.Count || convertedUserIdInput < 1)
                return false;

            return true;
        }

        public static bool LegalIDRangeAudienceId(string userInput, out int convertedUserIdInput)
        {
            var audienceId = EntityFrameworkCode.EntityframeworkAudience.ShowAllAudiences();

            if (!IntReaderConvertStringToInt(userInput, out convertedUserIdInput))
                return false;

            if (convertedUserIdInput > audienceId.Count || convertedUserIdInput < 1)
                return false;

            return true;
        }

        public static bool LegalIDRangeMediaId(string userInput, out int convertedUserIdInput)
        {
            var MediaId = EntityFrameworkCode.EntityframeworkMediaTypes.ShowAllMediaTypes();

            if (!IntReaderConvertStringToInt(userInput, out convertedUserIdInput))
                return false;

            if (convertedUserIdInput > MediaId.Count || convertedUserIdInput < 1)
                return false;

            return true;
        }

        public static bool LegalIDRangeEvent(string userInput, out int convertedUserIdInput)
        {
            var eventsList = EntityFrameworkCode.EntityframeworkEvents.ShowAllEvents();

            if (!IntReaderConvertStringToInt(userInput, out convertedUserIdInput))
                return false;

            if (convertedUserIdInput > eventsList.Count || convertedUserIdInput < 1)
                return false;

            return true;
        }

        #endregion

        #region Double readers
        public static bool DoubleReaderOutDouble(string userInput, out double num)
        {
            if (Double.TryParse(userInput, out num))
                return true;

            return false;
        }

        #endregion

        #region Long Readers

        public static bool LongReaderOutLong(string userInput, out long num)
        {
            if (long.TryParse(userInput, out num))
                return true;

            return false;

        }

        public static bool LongReaderLengthEqualsTo(string userInput, int selectedNumber, out long num)
        {
            if (!LongReaderOutLong(userInput, out num))
                return false;

            if (userInput.Length != selectedNumber)
                return false;

            return true;
        }

        #endregion
        public static bool ReadDateTime(string userInput, out DateTime date)
        {
            if (!DateTime.TryParseExact(userInput, "yyyy/MM/dd", null, DateTimeStyles.None, out date))
                return false;

            return true;
        }

        public static bool ReadDateTime2(string userInput, out DateTime date)
        {
            if (!DateTime.TryParseExact(userInput, "yyyy/MM/dd hh:mm tt", null, DateTimeStyles.None, out date))
                return false;

            return true;
        }

    }
}


