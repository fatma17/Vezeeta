using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;


namespace Vezeeta.Core
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum Status
    {
        Pending,
        Completed,
        Cancelled
    }

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum Gender
    {
        Female,
        Male
    }

   [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum Days
    {
        Saturday,
        Sunday,
        Monday,
        Tuesday,
        Wednesday,
        Thursday,
        Friday
    }

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum DiscountType
    {
        Percentage,
        Value
    }
}
