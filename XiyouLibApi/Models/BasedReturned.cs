/*
 * Open Web API for Xi'an University Of Posts & Telecommunications Library.
 * 
 * Copyright (C) 2014 Xiyou Mobile Application Club Microsoft Technology Team All Rights Reserved.
 * 
 * File Description: Model for basic data.
 * 
 * Created By Yuan Guozheng 2014/6/1.
 *
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace XiyouLibApi.Models
{
    public class BasedReturned
    {
        /// <summary>
        /// Show if the request is successful.
        /// </summary>
        public bool Result { get; set; }
        /// <summary>
        /// If 'Result' is 'true', 'Detail' contains information we need, but error information if 'Result' is 'false'.
        /// </summary>
        public object Detail { get; set; }
    }
}