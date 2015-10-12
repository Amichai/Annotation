using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Annotation.Web.Models {
    public class TimeZoneModel {
        public string Name { get; set; }
        public string City { get; set; }
        public double GMTOffset { get; set; }
        public string GMTOffsetString {
            get {
                if (GMTOffset > 0) {
                    return "+" + GMTOffset.ToString();
                } else {
                    return GMTOffset.ToString();
                }
            }
        }
        public Guid? Id { get; set; }

        public JObject ToJson() {
            JObject toReturn = new JObject();
            toReturn["Name"] = this.Name;
            toReturn["City"] = this.City;
            toReturn["Offset"] = this.GMTOffset;
            toReturn["Id"] = this.Id;
            return toReturn;
        }

        internal static TimeZoneModel FromJson(JObject tz) {
            return new TimeZoneModel() {
                Name = tz["Name"].Value<string>(),
                City = tz["City"].Value<string>(),
                GMTOffset = tz["Offset"].Value<double>()
            };
        }
    }
}