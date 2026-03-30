using System.Net;
using Microsoft.Extensions.Options;

using SRB_WebPortal.Consts;
using SRB_WebPortal.Shared;
using SRB_WebPortal.Configs;

namespace SRB_WebPortal.Services;

public interface IBunnyCNDService
{
   Task<BaseResponse<Uri>> UploadToBunnyRunBackground(Stream fileStream, string originalFileName, string folderPath);
   Task<BaseResponse<Uri>> UploadToBunny(IFormFile file, string folderPath);
   Task<BaseResponse> DeleteFileAsync(string fileName);
}

public class BunnyCNDService(
   IOptions<BunnyOptions> options,
   IHttpClientFactory httpClientFactory
) : IBunnyCNDService
{
   private readonly IOptions<BunnyOptions> _bunnyOptions = options;

   private readonly IHttpClientFactory _httpClientFactory = httpClientFactory;

   public async Task<BaseResponse<Uri>> UploadToBunnyRunBackground(Stream fileStream, string originalFileName, string folderPath)
   {
      var storageZoneName = _bunnyOptions.Value.StorageZoneName;
      var accessKey = _bunnyOptions.Value.AccessKey;

      var fileName = Guid.CreateVersion7().ToString() + Path.GetExtension(originalFileName);
      var uploadUrl = $"{_bunnyOptions.Value.Hostname.TrimEnd('/')}/{storageZoneName}/{folderPath}/{fileName}";

      var client = _httpClientFactory.CreateClient();
      client.DefaultRequestHeaders.Add("AccessKey", accessKey);

      var content = new StreamContent(fileStream);
      var response = await client.PutAsync(uploadUrl, content);

      if (response.IsSuccessStatusCode)
      {
         var publicUrlString = $"{_bunnyOptions.Value.BaseCdnUrl.TrimEnd('/')}/{folderPath}/{fileName}";

         return BaseResponse<Uri>.Success(new Uri(publicUrlString), "Upload thành công", HttpStatusCode.Created);
      }

      return BaseResponse<Uri>.Failure($"Lỗi: {response.ReasonPhrase}", HttpStatusCode.BadRequest);
   }


   public async Task<BaseResponse<Uri>> UploadToBunny(IFormFile file, string folderPath)
   {
      var storageZoneName = _bunnyOptions.Value.StorageZoneName;
      var accessKey = _bunnyOptions.Value.AccessKey;

      var fileName = Guid.CreateVersion7().ToString() + Path.GetExtension(file.FileName);

      var uploadUrl = $"{_bunnyOptions.Value.Hostname.TrimEnd('/')}/{storageZoneName}/{folderPath}/{fileName}";

      if (!Uri.IsWellFormedUriString(uploadUrl, UriKind.Absolute))
      {
         return BaseResponse<Uri>.Failure("URL gửi lên Bunny không hợp lệ: " + uploadUrl, HttpStatusCode.BadRequest);
      }

      var client = _httpClientFactory.CreateClient();
      client.Timeout = TimeSpan.FromSeconds(30);
      client.DefaultRequestHeaders.Add("AccessKey", accessKey);
      client.DefaultRequestHeaders.UserAgent.ParseAdd("Mozilla/5.0 (Windows NT 10.0; Win64; x64)");

      using var stream = file.OpenReadStream();
      var content = new StreamContent(stream);

      // Thực hiện đẩy file lên Bunny
      var response = await client.PutAsync(uploadUrl, content);

      if (response.IsSuccessStatusCode)
      {
         var publicUrlString = $"{_bunnyOptions.Value.BaseCdnUrl}/{folderPath}/{fileName}";

         Uri publicUri = new(publicUrlString);

         return BaseResponse<Uri>.Success(publicUri, "Đăng thông tin CV thành công", HttpStatusCode.Created);
      }

      return BaseResponse<Uri>.Failure($"Upload lên Bunny thất bại: {response.ReasonPhrase}", HttpStatusCode.BadRequest);
   }

   public async Task<BaseResponse> DeleteFileAsync(string fileName)
   {
      var hostname = _bunnyOptions.Value.Hostname.TrimEnd('/');
      var zoneName = _bunnyOptions.Value.StorageZoneName;
      var folderPath = CloudCNDKey.FOLDER_APPLY_JOB_CV;

      var deleteUrl = $"{hostname}/{zoneName}/{folderPath}/{fileName}";

      var client = _httpClientFactory.CreateClient();
      client.DefaultRequestHeaders.Add("AccessKey", _bunnyOptions.Value.AccessKey);

      var response = await client.DeleteAsync(deleteUrl);

      if (!response.IsSuccessStatusCode)
      {
         var errorContent = await response.Content.ReadAsStringAsync();

         return BaseResponse.Failure(
            $"Xóa File thất bại: {response.ReasonPhrase}",
            response.StatusCode
         );
      }

      return BaseResponse.Success("Xóa File CV lưu trữ thành công");
   }
}
