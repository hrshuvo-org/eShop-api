using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Framework.App.Models.Entities;
using Framework.App.Services.Interfaces;
using Framework.Core.Repositories.Interfaces;
using Framework.Core.Services;
using Framework.Settings;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;

namespace Framework.App.Services;

public class PhotoService : BaseService<Photo, long>, IPhotoService
{
    private readonly Cloudinary _cloudinary;
    
    public PhotoService(IBaseRepository<Photo, long> repo, IOptions<CloudinarySettings> config) : base(repo)
    {
        var acc = new Account()
        {
            Cloud = config.Value.CloudName,
            ApiKey = config.Value.ApiKey,
            ApiSecret = config.Value.ApiSecret
        };
        _cloudinary = new Cloudinary(acc);
    }
    
    public async Task<ImageUploadResult> AddPhotoAsync(IFormFile file)
    {
        var uploadResult = new ImageUploadResult();
        if (file.Length <= 0) return uploadResult;
        
        await using var stream = file.OpenReadStream();
        var uploadParams = new ImageUploadParams()
        {
            File = new FileDescription(file.FileName, stream),
            Transformation = new Transformation()
                .Height(500)
                .Width(500)
                .Crop("fill")
                .Gravity("face"),
            Folder = "da-net7"
        };
        uploadResult = await _cloudinary.UploadAsync(uploadParams);

        return uploadResult;
    }
}