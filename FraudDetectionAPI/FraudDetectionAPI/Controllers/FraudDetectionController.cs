using FraudDetectionAPI.Services;
using Microsoft.AspNetCore.Mvc;
using Transaction = FraudDetectionAPI.Models.Transaction;

namespace FraudDetectionAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FraudDetectionController : ControllerBase
    {
        private readonly FraudDetectionService _fraudService;

        public FraudDetectionController(FraudDetectionService fraudService)
        {
            _fraudService = fraudService;
        }

        [HttpPost("detect")]
        public IActionResult DetectFraud([FromBody] Transaction transaction)
        {
            var result = _fraudService.Predict(transaction);
            return Ok(new { IsFraud = result.IsFraud, Score = result.Score });
        }
    }
}
