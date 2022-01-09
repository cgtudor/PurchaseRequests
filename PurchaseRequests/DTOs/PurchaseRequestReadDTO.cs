using Newtonsoft.Json.Converters;
using PurchaseRequests.Enums;
using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace PurchaseRequests.DTOs
{
    public class PurchaseRequestReadDTO
    {
        [Key]
        public int PurchaseRequestID { get; set; }
        [Required]
        public int ProductId { get; set; }
        [Required]
        public int Quantity { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        [JsonConverter(typeof(StringEnumConverter))]
        public PurchaseRequestStatus PurchaseRequestStatus { get; set; }
    }
}