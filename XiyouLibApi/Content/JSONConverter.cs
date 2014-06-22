/*
 * Open Web API for Xi'an University Of Posts & Telecommunications Library.
 * 
 * Copyright (C) 2014 Xiyou Mobile Application Club Microsoft Technology Team All Rights Reserved.
 * 
 * File Description: Class for Serializing/Deserializing obj and json string.
 * 
 * Created By Yuan Guozheng 2014/6/23.
 *
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace XiyouLibApi.Content
{
    public class JSONConverter
    {
        /// <summary>
        /// Translate object to JSON String.
        /// </summary>
        /// <param name="obj">Object.</param>
        /// <returns>JSON string.</returns>
        public static string Serialize(object obj)
        {
            return JsonConvert.SerializeObject(obj);
        }

        /// <summary>
        /// Get object from JSON string, support object type only.
        /// </summary>
        /// <param name="json">JSON string using object.</param>
        /// <returns>JObject</returns>
        public static JObject Deserialize(string json)
        {
            return JObject.Parse(json);
        }
    }
}