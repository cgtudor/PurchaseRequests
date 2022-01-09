using System;
using System.ComponentModel.DataAnnotations;

namespace PurchaseRequests.DTOs
{
    public class PurchaseRequestCreateDTO
    {
        [Required]
        public int ProductId { get; set; }
        [Required]
        public int Quantity { get; set; }
        [Required]
        public string Name { get; set; }
    }
}