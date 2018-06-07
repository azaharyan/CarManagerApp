using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

using Newtonsoft.Json;

namespace CarLocker
{
    public class CarDomain
    {
        [JsonProperty("car_domain")]
        public string domain { get; set; }
        [JsonProperty("command")]
        public string command { get; set; }

    }
}