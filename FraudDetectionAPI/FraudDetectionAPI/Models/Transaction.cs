using FraudDetectionAPI.Models.Base;
using Microsoft.ML.Data;

namespace FraudDetectionAPI.Models
{
    public class Transaction : Entity
    {
        public string TransactionId { get; set; } = string.Empty;

        [LoadColumn(1)]
        public string UserId { get; set; } = string.Empty;  // ID do usuário (precisa ser convertido para numérico)

        [LoadColumn(0)]  // Atributo do ML.NET para mapear colunas do dataset
        public float Amount { get; set; } // Valor da transação
        public DateTime TransactionDate { get; set; }

        [LoadColumn(2)]
        public string Location { get; set; } = string.Empty; // País/Estado (ex: "BR", "US")
        public string DeviceInfo { get; set; } = string.Empty;

        [LoadColumn(3)]
        public int TransactionHour { get; set; }  // Hora da transação (0-23)
    }
}
