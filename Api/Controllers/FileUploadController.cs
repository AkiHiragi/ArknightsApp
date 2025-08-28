using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using ArknightsApp.Data;
using ArknightsApp.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ArknightsApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FileUploadController(
        ApplicationDbContext context,
        IWebHostEnvironment environment,
        IImageProcessingService imageServiсe) : ControllerBase
    {
        [HttpPost("upload/{operatorId}/{imageType}")]
        public async Task<IActionResult> UploadImage(int operatorId, string imageType, IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest("Файл не выбран");

            var allowedTypes = new[] { "avatar", "preview", "e0", "e2", "skin" };
            if (!allowedTypes.Contains(imageType.ToLower()))
                return BadRequest("Неверный тип изображения");

            var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".webp" };
            var extension = Path.GetExtension(file.FileName).ToLower();

            if (!allowedExtensions.Contains(extension))
                return BadRequest("Неподдерживаемый формат файла");

            var op = await context.Operators.FindAsync(operatorId);
            if (op == null)
                return NotFound("Оператор не найден");

            var fileName = $"{Guid.NewGuid()}{extension}";
            var uploadsPath = Path.Combine(environment.WebRootPath, "images", "operators", imageType.ToLower());
            Directory.CreateDirectory(uploadsPath);

            var filePath = Path.Combine(uploadsPath, fileName);

            await using var stream = new FileStream(filePath, FileMode.Create);
            await file.CopyToAsync(stream);

            switch (imageType.ToLower())
            {
                case "avatar":
                    op.AvatarUrl = $"/images/operators/{imageType.ToLower()}/{fileName}";
                    break;
                case "preview":
                    op.PreviewUrl = $"/images/operators/{imageType.ToLower()}/{fileName}";
                    break;
                case "e0":
                    op.E0ArtUrl = $"/images/operators/{imageType.ToLower()}/{fileName}";
                    break;
                case "e2":
                    op.E2ArtUrl = $"/images/operators/{imageType.ToLower()}/{fileName}";
                    break;
            }

            await context.SaveChangesAsync();
            return Ok(new { url = $"/images/operators/{imageType.ToLower()}/{fileName}" });
        }

        [HttpPost("process-e0/{operatorId}")]
        public async Task<IActionResult> ProcessE0Art(
            int operatorId,
            IFormFile file,
            int avatarX, int avatarY, int avatarSize,
            int previewX, int previewY, int previewWidth, int previewHeight)
        {
            if (file == null || file.Length == 0)
                return BadRequest("Файл не выбран");

            var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".webp" };
            var extension = Path.GetExtension(file.FileName).ToLower();
            if (!allowedExtensions.Contains(extension))
                return BadRequest("Неподдерживаемый формат файла");

            var op = await context.Operators.FindAsync(operatorId);
            if (op == null)
                return NotFound("Оператор не найден");

            var (e0Url, avatarUrl, previewUrl) = await imageServiсe.ProcessE0ArtAsync(
                file, operatorId,
                avatarX, avatarY, avatarSize,
                previewX, previewY, previewWidth, previewHeight);

            op.E0ArtUrl = e0Url;
            op.AvatarUrl = avatarUrl;
            op.PreviewUrl = previewUrl;

            await context.SaveChangesAsync();

            return Ok(new { e0Url, avatarUrl, previewUrl });
        }
    }
}