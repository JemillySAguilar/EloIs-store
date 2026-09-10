using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EloisStore.Api.Controllers;

[ApiController]
[Authorize(Roles = "Admin")]
[Route("api/uploads")]
public sealed class UploadsController(IWebHostEnvironment environment) : ControllerBase
{
    private static readonly HashSet<string> AllowedExtensions = [".jpg", ".jpeg", ".png", ".webp"];

    [HttpPost("product-image")]
    [RequestSizeLimit(5 * 1024 * 1024)]
    public async Task<ActionResult<object>> UploadProductImage(IFormFile image)
    {
        if (image.Length == 0 || image.Length > 5 * 1024 * 1024)
            return BadRequest("A imagem deve ter no máximo 5 MB.");

        var extension = Path.GetExtension(image.FileName).ToLowerInvariant();
        if (!AllowedExtensions.Contains(extension))
            return BadRequest("Formato inválido. Use JPG, PNG ou WEBP.");

        var root = environment.WebRootPath ?? Path.Combine(environment.ContentRootPath, "wwwroot");
        var directory = Path.Combine(root, "uploads", "products");
        Directory.CreateDirectory(directory);
        var fileName = $"{Guid.NewGuid():N}{extension}";
        await using var stream = System.IO.File.Create(Path.Combine(directory, fileName));
        await image.CopyToAsync(stream);

        return Ok(new { url = $"{Request.Scheme}://{Request.Host}/uploads/products/{fileName}" });
    }
}
