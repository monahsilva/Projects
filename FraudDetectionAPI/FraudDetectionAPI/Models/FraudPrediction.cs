using Microsoft.ML.Data;

namespace FraudDetectionAPI.Models
{
    public class FraudPrediction
    {
        [ColumnName("PredictedLabel")]
        public bool IsFraud { get; set; }  // true/false para fraude

        [ColumnName("Score")]
        public float FraudProbability { get; set; }  // Probabilidade (0 a 1)
    }
}
