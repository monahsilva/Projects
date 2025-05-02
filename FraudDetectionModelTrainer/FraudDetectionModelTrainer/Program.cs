using System;
using System.IO;
using Microsoft.ML;
using Microsoft.ML.Data;

public class TransactionData
{
    [LoadColumn(0)] public float Amount { get; set; }
    [LoadColumn(1)] public string UserId { get; set; }
    [LoadColumn(2)] public string Location { get; set; }
    [LoadColumn(3)] public int TransactionHour { get; set; }
    [LoadColumn(4)] public bool IsFraud { get; set; } // Rótulo
}

public class FraudPrediction
{
    [ColumnName("PredictedLabel")] public bool IsFraud { get; set; }
    public float Score { get; set; } // Probabilidade bruta
}

class Program
{
    static void Main(string[] args)
    {
        try
        {
            var mlContext = new MLContext(seed: 1);

            // 1. Carregar os dados
            var dataPath = Path.Combine(Environment.CurrentDirectory, "fraud_data.csv");

            if (!File.Exists(dataPath))
            {
                Console.WriteLine($"Arquivo de dados não encontrado: {dataPath}");
                return;
            }

            var dataView = mlContext.Data.LoadFromTextFile<TransactionData>(
                path: dataPath,
                separatorChar: ',',
                hasHeader: true);

            // 2. Dividir os dados em treino (80%) e teste (20%)
            var trainTestSplit = mlContext.Data.TrainTestSplit(dataView, testFraction: 0.2);
            var trainData = trainTestSplit.TrainSet;
            var testData = trainTestSplit.TestSet;

            // 3. Pipeline de transformação + modelo
            var pipeline = mlContext.Transforms
                .CopyColumns("Label", "IsFraud") // define o rótulo
                .Append(mlContext.Transforms.Categorical.OneHotEncoding("UserIdEncoded", "UserId"))
                .Append(mlContext.Transforms.Categorical.OneHotEncoding("LocationEncoded", "Location"))
                .Append(mlContext.Transforms.Conversion.ConvertType("TransactionHourFloat", "TransactionHour", DataKind.Single))
                .Append(mlContext.Transforms.Concatenate(
                    "Features",
                    "Amount",
                    "TransactionHourFloat",
                    "UserIdEncoded",
                    "LocationEncoded"))
                .Append(mlContext.BinaryClassification.Trainers.LightGbm());

            Console.WriteLine("Treinando o modelo...");
            var model = pipeline.Fit(trainData);

            // 4. Avaliação
            var predictions = model.Transform(testData);
            var metrics = mlContext.BinaryClassification.Evaluate(predictions, labelColumnName: "IsFraud");

            Console.WriteLine($"Acurácia: {metrics.Accuracy:P2}");
            Console.WriteLine($"AUC: {metrics.AreaUnderRocCurve:P2}");

            // 5. Salvar o modelo
            var modelPath = Path.Combine(Environment.CurrentDirectory, "Model", "fraud_model.zip");
            Directory.CreateDirectory(Path.GetDirectoryName(modelPath)!);

            mlContext.Model.Save(model, trainData.Schema, modelPath);

            Console.WriteLine($"Modelo salvo em: {modelPath}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erro: {ex.Message}");
        }
    }
}
