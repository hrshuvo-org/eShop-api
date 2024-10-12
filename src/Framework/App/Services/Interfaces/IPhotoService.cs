using CloudinaryDotNet.Actions;
using Framework.App.Models.Entities;
using Framework.Core.Services.Interfaces;
using Microsoft.AspNetCore.Http;

namespace Framework.App.Services.Interfaces;

public interface IPhotoService : IBaseService<Photo, long>
{
    Task<ImageUploadResult> AddPhotoAsync(IFormFile file);
}