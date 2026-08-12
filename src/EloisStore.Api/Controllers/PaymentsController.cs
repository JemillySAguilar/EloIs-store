using Microsoft.AspNetCore.Authorization;
using EloisStore.Api.Models.Payments;
using EloisStore.Api.Services.Payments;
using Microsoft.AspNetCore.Mvc;

namespace EloisStore.Api.Controllers;

[ApiController]
[Authorize]
[Route("api/payments")]
public sealed class PaymentsController(PaymentService payments) : ControllerBase
{
    [HttpGet("order/{orderId:guid}")]
    public Task<List<Payment>> ListByOrder(Guid orderId) => payments.ListByOrderIdAsync(orderId);

    [HttpPost]
    public Task<Payment> Pay(PaymentRequest request) =>
        payments.PayAsync(request.OrderId, request.Amount, request.Method);
}
