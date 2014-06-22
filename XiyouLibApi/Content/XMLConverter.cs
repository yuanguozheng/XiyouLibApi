/*
 * Open Web API for Xi'an University Of Posts & Telecommunications Library.
 * 
 * Copyright (C) 2014 Xiyou Mobile Application Club Microsoft Technology Team All Rights Reserved.
 * 
 * File Description: Class for Serializing/Deserializing obj and XML string.
 * 
 * Created By Yuan Guozheng 2014/6/23.
 *
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml;
using System.Xml.Serialization;

namespace XiyouLibApi.Content
{
    public class XMLConverter
    {
        /// <summary>
        /// Translate object to XML String.
        /// </summary>
        /// <param name="obj">Object.</param>
        /// <returns>XML string.</returns>
        public static string Serialize(Type type,object obj)
        {
            
        }

        /// <summary>
        /// Get object from XML string.
        /// </summary>
        /// <param name="XML">XML string.</param>
        /// <returns>Object</returns>
        public static object Deserialize(string XML)
        {
            
        }
    }
}