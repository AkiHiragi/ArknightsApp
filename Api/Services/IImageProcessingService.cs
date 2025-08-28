using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace ArknightsApp.Services;

public interface IImageProcessingService
{
    Task<(string e0Url, string avatarUrl, string previewUrl)> ProcessE0ArtAsync(
        IFormFile file,
        int operatorId,
        int avatarX, int avatarY, int avatarSize,
        int previewX, int previewY, int previewWidth, int previewHeight);
}