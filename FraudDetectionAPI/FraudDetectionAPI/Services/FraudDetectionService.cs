using FraudDetectionAPI.Models;
using Microsoft.ML;
using System.Transactions;
using Transaction = FraudDetectionAPI.Models.Transaction;

namespace FraudDetectionAPI.Services
{
    public class FraudDetectionService
    {
        private readonly MLContext _mlContext;
        private readonly ITransformer _model;

        public FraudDetectionService()
        {
            _mlContext = new MLContext();
            _model = _mlContext.Model.Load("Model/fraud_model.zip", out _);
        }

        public FraudResult Predict(Transaction transaction)
        {
            var predictionEngine = _mlContext.Model.CreatePredictionEngine<Transaction, FraudPrediction>(_model);
            var prediction = predictionEngine.Predict(transaction);

            return new FraudResult
            {
                IsFraud = prediction.IsFraud,
                Score = prediction.FraudProbability
            };
        }
    }
}
