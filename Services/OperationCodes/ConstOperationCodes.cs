using System;
using System.Collections.Generic;
using System.Text;

namespace Services.OperationCodes
{
    public static class ConstOperationCodes
    {
        #region GENERAL
        public static readonly string SUCCESS_OPERATION = "T_100";
        public static readonly string FAILED_OPERATION = "F_999";
        #endregion

        #region AUTHOR_CODES
        public static readonly string AUTHOR_FOUND = "AUTHOR_100";
        public static readonly string AUTHOR_CREATED = "AUTHOR_101";
        public static readonly string AUTHOR_UPDATED = "AUTHOR_102";
        public static readonly string AUTHOR_DELETED = "AUTHOR_103";
        public static readonly string AUTHOR_NOT_FOUND = "AUTHOR_104";
        public static readonly string AUTHOR_ALREADY_EXISTS = "AUTHOR_105";
        public static readonly string AUTHOR_NAME_ALREADY_EXISTS = "AUTHOR_106";
        public static readonly string INVALID_AUTHOR_BIRTHDAY = "AUTHOR_107";
        #endregion

        #region COURSE_CODES
        public static readonly string COURSE_FOUND = "COURSE_100";
        public static readonly string COURSE_CREATED = "COURSE_101";
        public static readonly string COURSE_UPDATED = "COURSE_102";
        public static readonly string COURSE_DELETED = "COURSE_103";
        public static readonly string COURSE_NOT_FOUND = "COURSE_104";
        public static readonly string COURSE_NAME_ALREADY_EXISTS = "COURSE_105";
        #endregion

        #region USER_CODES
        public static readonly string USER_CREATED = "USER_100";
        public static readonly string USER_FOUND = "USER_101";
        public static readonly string USER_DELETED = "USER_102";
        public static readonly string USER_UPDATED = "USER_103";
        public static readonly string USER_NOT_FOUND = "USER_104";
        public static readonly string USER_PHONE_ALREADY_EXISTS = "USER_105";
        public static readonly string USER_EMAIL_ALREADY_EXISTS = "USER_106";
        public static readonly string USER_WRONG_PASSWORD = "USER_107";
        public static readonly string USER_SAME_OLD_PASSWORD = "USER_107";
        #endregion

        #region COUNTRY_CODES
        public static readonly string INVALID_COUNTRY = "COUNTRY_104";
        #endregion
    }

}
