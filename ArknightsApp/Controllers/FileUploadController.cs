using ArknightsApp.Attributes;
using ArknightsApp.Models;
using Microsoft.AspNetCore.Mvc;

namespace ArknightsApp.Controllers;

[ApiController]
[Route("api/[controller]")]
[RequiredRole(UserRole.Admin)]
public class FileUploadController : ControllerBase
{
    private readonly IWebHostEnvironment           _environment;
    private readonly ILogger<FileUploadController> _logger;

    public FileUploadController(IWebHostEnvironment environment, ILogger<FileUploadController> logger)
    {
        _environment = environment;
        _logger      = logger;
    }

    [HttpPost("operator-image")]
    public async Task<IActionResult> UploadOperatorImage(IFormFile file, [FromForm] string operatorName)
    {
        if (file == null || file.Length == 0)
            return BadRequest(new { message = "Файл не выбран" });

        var allowedTypes = new[] { "image/jpeg", "image/jpg", "image/png", "image/webp" };
        if (!allowedTypes.Contains(file.ContentType.ToLower()))
            return BadRequest(new { message = "Поддерживаются только изображения (JPG, PNG, WEBP,JPEG)" });

        if (file.Length > 15 * 1024 * 1024)
            return BadRequest(new { message = "Размер файла не должен превышать 15MD" });

        try
        {
            var fileName    = $"{operatorName.ToLower().Replace(' ', '-')}.jpg";
            var uploadsPath = Path.Combine(_environment.WebRootPath, "images", "operators");

            if (!Directory.Exists(uploadsPath))
            {
                Directory.CreateDirectory(uploadsPath);
            }
            
            var filePath = Path.Combine(uploadsPath, fileName);

            await using var stream = new FileStream(filePath, FileMode.Create);
            await file.CopyToAsync(stream);

            var imageUrl = $"/images/operators/{fileName}";
            
            _logger.LogInformation("Загружено изображение оператора: {FileName}", fileName);

            return Ok(new
            {
                imageUrl = imageUrl,
                fileName = fileName,
                message  = "Изображение успешно загружено"
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ошибка при загрузке изображения оператора");
            return StatusCode(500, new { message = "Ошибка сервера при загрузке файла" });
        }
    }
}