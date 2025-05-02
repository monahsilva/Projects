namespace FraudDetectionAPI.Models
{
    public class FraudResult
    {
        public bool IsFraud { get; set; }
        public float Score { get; set; }
        public string Message => IsFraud ? "ALERTA: Transação suspeita!" : "Transação segura.";
    }
}
