﻿using API.Helpers;
using API.Interfaces;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.Extensions.Options;

namespace API.Services;

public class PhotoService : IPhotoService
{
    private readonly Cloudinary _cloudinary;

    public PhotoService(IOptions<CloudinarySettings> config)
    {
        var acc = new Account(config.Value.CloudName, config.Value.ApiKey, config.Value.ApiSecret);
        _cloudinary = new Cloudinary(acc);
    }

    public async Task<ImageUploadResult> AddPhotoAsync(IFormFile formFile)
    {
        var uploadResult = new ImageUploadResult();

        if(formFile.Length > 0)
        {
            using var stream = formFile.OpenReadStream();
            var uploadParams = new  ImageUploadParams
            {
                File = new FileDescription(formFile.FileName, stream),
                Transformation = new Transformation().Height(500).Width(500).Crop("fill").Gravity("face"),
                Folder = "da-net8"
            };
            uploadResult = await _cloudinary.UploadAsync(uploadParams);            
        }
        return uploadResult;
    }

    public Task<DeletionResult> DeletePhotoAsync(string publicId)
    {
        var deleteParams = new DeletionParams(publicId);
        return _cloudinary.DestroyAsync(deleteParams);
    }
}
