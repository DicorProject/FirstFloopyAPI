using Newtonsoft.Json;

namespace Floppy.Application.Models.Response
{
    public class OrderEntity
    {
    }
    public class OrderEntityDetails
    {
        [JsonProperty("auth_id")]
        public string AuthId { get; set; }

        [JsonProperty("authorization")]
        public string Authorization { get; set; }

        [JsonProperty("bank_reference")]
        public string BankReference { get; set; }

        [JsonProperty("cf_payment_id")]
        public string CfPaymentId { get; set; }

        [JsonProperty("entity")]
        public string Entity { get; set; }

        [JsonProperty("error_details")]
        public string ErrorDetails { get; set; }

        [JsonProperty("is_captured")]
        public bool IsCaptured { get; set; }

        [JsonProperty("order_amount")]
        public decimal OrderAmount { get; set; }

        [JsonProperty("order_id")]
        public string OrderId { get; set; }

        [JsonProperty("payment_amount")]
        public decimal PaymentAmount { get; set; }

        [JsonProperty("payment_completion_time")]
        public DateTime PaymentCompletionTime { get; set; }

        [JsonProperty("payment_currency")]
        public string PaymentCurrency { get; set; }

        [JsonProperty("payment_gateway_details")]
        public string PaymentGatewayDetails { get; set; }

        [JsonProperty("payment_group")]
        public string PaymentGroup { get; set; }

        [JsonProperty("payment_message")]
        public string PaymentMessage { get; set; }

        [JsonProperty("payment_status")]
        public string PaymentStatus { get; set; }

        [JsonProperty("payment_time")]
        public DateTime PaymentTime { get; set; }

        [JsonProperty("payment_method")]
        public PaymentMethod PaymentMethod { get; set; }
    }

    public class PaymentMethod
    {
        [JsonProperty("upi")]
        public Upi Upi { get; set; }
    }

    public class Upi
    {
        [JsonProperty("channel")]
        public string Channel { get; set; }

        [JsonProperty("upi_id")]
        public string UpiId { get; set; }
    }

}
