using fetch_receipt_rewards.Models;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("receipts")]
public class ReceiptsController : ControllerBase
{
    private static readonly Dictionary<string, Receipt> _receipts = new();

    /// <summary>
    /// Given a receipt, processes it and returns a receipt ID.
    /// </summary>
    /// <param name="receipt">Receipt object to parse</param>
    /// <returns>Response Object containing ID</returns>
    [HttpPost("process")]
    public ActionResult<ReceiptResponse> ProcessReceipt([FromBody] Receipt receipt)
    {
        if (!IsValidReceipt(receipt))
        {
            return BadRequest("The receipt is invalid.");
        }

        var id = Guid.NewGuid().ToString();
        _receipts[id] = receipt;

        return Ok(new ReceiptResponse { Id = id });
    }

    /// <summary>
    /// Given a receipt ID, returns the number of points earned.
    /// </summary>
    /// <param name="id">ID of the Receipt to lookup</param>
    /// <returns>Object containing points value of receipt</returns>
    [HttpGet("{id}/points")]
    public ActionResult<PointsResponse> GetPoints(string id)
    {
        if (!_receipts.TryGetValue(id, out var receipt))
        {
            return NotFound("No receipt found for that ID.");
        }

        var points = CalculatePoints(receipt);
        return Ok(new PointsResponse { Points = points });
    }

    // This could go to its own Service. For ease of evaluation, it's here.
    private int CalculatePoints(Receipt receipt)
    {
        var points = 0;

        // Rule 1: Point per alphanumeric character in retailer name
        points += receipt.Retailer.Count(char.IsLetterOrDigit);

        // Rule 2/3: Points for divisibility
        var total = decimal.Parse(receipt.Total);
        if (total == Math.Floor(total))
        {
            points += 50;
        }
        if (total % 0.25m == 0)
        {
            points += 25;
        }

        // Rule 4: 5 points for every two items
        points += receipt.Items.Count / 2 * 5;

        // Rule 5: Points for item description length
        foreach (var item in receipt.Items)
        {
            var trimmedDescription = item.ShortDescription.Trim();
            if (trimmedDescription.Length % 3 == 0)
            {
                var itemPrice = decimal.Parse(item.Price);
                points += (int)Math.Ceiling(itemPrice * 0.2m);
            }
        }

        // Rule 6: 6 points on odd purchase dates
        if (receipt.PurchaseDate.Day % 2 != 0)
        {
            points += 6;
        }

        // Rule 7: 10 points if order is between 2-4PM non-inclusive (it says after 2:00pm and before 4:00pm.)
        var purchaseTime = receipt.PurchaseTime;
        if (purchaseTime > new TimeOnly(14, 0, 0) && purchaseTime < new TimeOnly(16, 0, 0))
        {
            points += 10;
        }

        return points;
    }

    private static bool IsValidReceipt(Receipt receipt)
    {
        if (string.IsNullOrEmpty(receipt.Total)) return false;
        if (string.IsNullOrEmpty(receipt.Retailer)) return false;
        if (receipt.Items.Count == 0) return false;

        return true;
    }
}