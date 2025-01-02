using Floppy.Domain.Entities;

namespace Floppy.Application.Models.Request
{
    public class OrderRequest
    {
        public int UserId { get; set; } 
        public double TotalAmount { get; set; }   
        public float TotalQuantity { get; set; }  
        public string FirstName{ get; set; }
        public string LastName { get; set; }
        public string Address { get; set; } 
        public string Email { get; set; }   
        public string Phone { get; set; }   
        public string State { get; set; }
        public string City { get; set; }    
        public string ZipCode { get; set; } 
        public string Country { get; set; } 
        public string Latitude { get; set; }    
        public string Longitude { get; set; }   
        public DateTime Date{ get; set; }  
        public string Slot { get; set; }    
        public string? Coupon { get; set; }
        public List<ProductInfo> Products { get; set; }
        public string currency { get; set; }
        public string returnUrl { get; set; }
        public bool IsCashOnDelivery { get; set; }

    }
    public class ProductInfo
    {
        public int ProductId { get; set; }
        public int VendorId { get; set; }
    }
    public class paymentUpdate
    {
        public string? PaymentReferenceOrderId { get; set; } 
        public string? OrderId { get; set; }
        public int UserId { get; set; } 
        public bool IsCashOnDelivery { get; set; }
        public string? Email { get; set; }
    }
    public class CashFreePaymentResponse
    {
        public string? PaymentOrderReferenceId { get; set; }
        public string? PaymentSessionId { get; set; }
        public string? PaymentMode { get; set; }   
        public string? OrderId { get; set; }    
    }
    public class LeadEntryUpdateModel
    {
        public int OrderId { get; set; }
        public DateTime NewDateTime { get; set; }
        public  string NewSlot { get; set; }    
        public int UserId { get; set; }
    }
}
