/*
 * Open Web API for Xi'an University Of Posts & Telecommunications Library.
 * 
 * Copyright (C) 2014 Xiyou Mobile Application Club Microsoft Technology Team All Rights Reserved.
 * 
 * File Description: Class for getting any param.
 * 
 * Created By Yuan Guozheng 2014/5/30.
 *
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace XiyouLibApi.Content
{
    public class Params : System.Web.UI.Page
    {
        /// <summary>
        /// Flag returned param 
        /// </summary>
        public static bool IsAvaliable { get; set; }
        public enum TransmitType
        {
            GET, POST, ALL
        }

        private static string RawGet(string key, TransmitType paramType)
        {
            string Param = null;
            if (!HttpContext.Current.Request.Params.AllKeys.Contains(key))
            {
                IsAvaliable = false;
                return null;
            }
            switch (paramType)
            {
                case TransmitType.GET:
                    Param = HttpContext.Current.Request.QueryString[key];
                    break;
                case TransmitType.POST:
                    Param = HttpContext.Current.Request.Form[key];
                    break;
            }
            return Param;
        }

        public static object GetObject(string key,TransmitType paramType)
        {
            IsAvaliable = true;
            return (object)RawGet(key, paramType);
        }

        public static string GetString(string key, TransmitType paramType)
        {
            IsAvaliable = true;
            return RawGet(key, paramType);
        }

        public static int GetInt(string key,TransmitType paramType)
        {
            int Result = -1;
            try
            {
                Result = int.Parse(RawGet(key, paramType));
            }
            catch
            {
                IsAvaliable = false;
                Result = -1;
            }
            return Result;
        }

        public static double GetDouble(string key, TransmitType paramType)
        {
            double Result = -1;
            try
            {
                Result = double.Parse(RawGet(key, paramType));
            }
            catch
            {
                IsAvaliable = false;
                Result = -1;
            }
            return Result;
        }

        public static DateTime GetDateTime(string key, TransmitType paramType)
        {
            DateTime Result = new DateTime();
            try
            {
                Result = DateTime.Parse(RawGet(key, paramType));
            }
            catch
            {
                IsAvaliable = false;
            }
            return Result;
        }
    }
}