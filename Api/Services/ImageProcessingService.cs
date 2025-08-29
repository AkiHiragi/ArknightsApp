using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;

namespace ArknightsApp.Services;

public class ImageProcessingService(IWebHostEnvironment environment) : IImageProcessingService
{
    public async Task<(string e0Url, string avatarUrl, string previewUrl)> ProcessE0ArtAsync(
        IFormFile file, int operatorId,
        int avatarX, int avatarY, int avatarSize,
        int previewX, int previewY, int previewWidth, int previewHeight)
    {
        using var image = await Image.LoadAsync(file.OpenReadStream());

        var e0FileName = $"{operatorId}_e0_{Guid.NewGuid()}.png";
        var e0Path = Path.Combine(environment.WebRootPath, "images", "operators", "e0");
        Directory.CreateDirectory(e0Path);
        await image.SaveAsPngAsync(Path.Combine(e0Path, e0FileName));

        var avatar = image.Clone(x => x.Crop(new Rectangle(avatarX, avatarY, avatarSize, avatarSize)));
        avatar.Mutate(x => x.Resize(70, 70));
        var avatarFileName = $"{operatorId}_avatar.png";
        var avatarPath = Path.Combine(environment.WebRootPath, "images", "operators", "avatar");
        Directory.CreateDirectory(avatarPath);
        await avatar.SaveAsPngAsync(Path.Combine(avatarPath, avatarFileName));

        var preview = image.Clone(x => x.Crop(new Rectangle(previewX, previewY, previewWidth, previewHeight)));
        preview.Mutate(x => x.Resize(200, 300));
        var previewFileName = $"{operatorId}_preview.png";
        var previewPath = Path.Combine(environment.WebRootPath, "images", "operators", "preview");
        Directory.CreateDirectory(previewPath);
        await preview.SaveAsPngAsync(Path.Combine(previewPath, previewFileName));

        return (
            $"/images/operators/e0/{e0FileName}",
            $"/images/operators/avatar/{avatarFileName}",
            $"/images/operators/preview/{previewFileName}"
        );
    }
}