using Newtonsoft.Json.Converters;
using PurchaseRequests.Enums;
using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace PurchaseRequests.DomainModels
{
    public class PurchaseRequestDomainModel
    {
        [Key]
        public int PurchaseRequestID { get; set; }
        [Required]
        public string AccountName { get; set; }
        [Required]
        public string CardNumber{ get; set; }
        [Required]
        public int ProductId { get; set; }
        [Required]
        public int Quantity { get; set; }
        [Required]
        public DateTime When { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public string ProductEan { get; set; }
        [Required]
        public int BrandId { get; set; }
        [Required]
        public string BrandName { get; set; }
        [Required]
        public double Price { get; set; }
        [Required]
        public double TotalPrice { get; set; }
        [Required]
        [JsonConverter(typeof(StringEnumConverter))]
        public PurchaseRequestStatus PurchaseRequestStatus { get; set; }
    }
}