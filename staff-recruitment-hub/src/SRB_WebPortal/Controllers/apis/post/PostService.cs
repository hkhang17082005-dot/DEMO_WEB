using System.Net;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using SRB_ViewModel;
using SRB_ViewModel.Entities;
using SRB_WebPortal.Shared;

namespace SRB_WebPortal.Controllers.apis.post;

public interface IPostService
{
   Task<BaseResponse> CreateNewPost(CreateJobPostDTO formData);
   Task<BaseResponse> UploadCVAsync(IFormFile file);
}

public class PostService(IPostRepository postRepository, Cloudinary cloudinary) : IPostService
{
   private readonly IPostRepository _postRepository = postRepository;

   private readonly Cloudinary _cloudinary = cloudinary;

   public async Task<BaseResponse> CreateNewPost(CreateJobPostDTO formData)
   {
      await _postRepository.CreateNewJobPost(formData);

      return BaseResponse.Success("Tạo thành công!");
   }

   public async Task<BaseResponse> UploadCVAsync(IFormFile file)
   {
      if (file is null || file.Length == 0)
      {
         return BaseResponse.BadRequest("File CV không hợp lệ");
      }

      using var stream = file.OpenReadStream();
      var uploadParams = new RawUploadParams
      {
         File = new FileDescription(file.FileName, stream),
         Folder = "CV_Storage"
      };

      var uploadResult = await _cloudinary.UploadAsync(uploadParams);
      if (uploadResult.Error is not null)
      {
         throw new Exception(uploadResult.Error.Message);
      }

      // var urlCV = _cloudinary.Api.Url
      //    .ResourceType("raw")
      //    .Signed(true)
      //    .BuildUrl(uploadResult.PublicId);
      // window.open(urlCV)

      // UserCVs(
      //    Id               UUID(PK),
      //    UserId           UUID(FK),
      //    PublicId         VARCHAR(255) NOT NULL,
      //    ResourceType     VARCHAR(20) DEFAULT 'raw',
      //    Folder           VARCHAR(100),
      //    OriginalFileName VARCHAR(255),
      //    MimeType         VARCHAR(100),
      //    FileSize         BIGINT,
      //    UploadedAt       TIMESTAMP,
      //    IsActive         BOOLEAN
      // )

      Console.WriteLine("CLOUD NAME RUNTIME: " + _cloudinary.Api.Account.Cloud);
      Console.WriteLine($"New URL SecureUrl: {uploadResult.SecureUrl}");
      Console.WriteLine($"New URL ResourceType: {uploadResult.ResourceType}");
      Console.WriteLine($"New URL Bytes: {uploadResult.Bytes}");
      Console.WriteLine($"New URL PublicId: {uploadResult.PublicId}");
      Console.WriteLine($"New URL Format: {uploadResult.Format}");

      return BaseResponse.Success("Đăng thông tin CV thành công", HttpStatusCode.Created);
   }

   public async Task<BaseResponse> UpdatePost()
   {
      return BaseResponse.Success("Update Post Successful..");
   }
}
