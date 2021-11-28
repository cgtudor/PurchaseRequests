using Newtonsoft.Json.Converters;
using PurchaseRequests.Enums;
using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace PurchaseRequests.DTOs
{
    public class PurchaseRequestEditDTO
    {
        [Required]
        [JsonConverter(typeof(StringEnumConverter))]
        public PurchaseRequestStatus PurchaseRequestStatus { get; set; }
    }
}