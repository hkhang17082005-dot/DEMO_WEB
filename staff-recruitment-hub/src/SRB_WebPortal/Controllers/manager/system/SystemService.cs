using SRB_WebPortal.Shared;
using SRB_WebPortal.Consts;
using SRB_WebPortal.Services;

namespace SRB_WebPortal.Controllers.manager.system;

public interface ISystemService
{
   Task<BaseResponse> UpdateUserRole(string userID, UpdateRoleRequest requestData);
}

public class SystemService(
   IRedisService redisService,
   ISystemRepository systemRepository
) : ISystemService
{
   private readonly IRedisService _redisService = redisService;
   private readonly ISystemRepository _systemRepository = systemRepository;

   public async Task<BaseResponse> UpdateUserRole(string userID, UpdateRoleRequest requestData)
   {
      var cacheTasks = requestData.RoleSlugs.Select(slug => _redisService.GetAsync<int?>(RedisCacheKeys.RoleKey(slug)));
      var cachedResults = await Task.WhenAll(cacheTasks);

      var roleMap = requestData.RoleSlugs
         .Zip(cachedResults, (slug, id) => new { slug, id })
         .ToDictionary(x => x.slug, x => x.id);

      List<int> listRoleID = [];
      List<string> missingSlugs = [.. roleMap.Where(x => x.Value == null).Select(x => x.Key)];

      if (missingSlugs.Count != 0)
      {
         var databaseRoles = await _systemRepository.GetRoleIDsBySlugs(missingSlugs);

         foreach (var slug in missingSlugs)
         {
            if (databaseRoles.TryGetValue(slug, out int roleID))
            {
               roleMap[slug] = roleID;

               _ = _redisService.SetAsync(RedisCacheKeys.RoleKey(slug), roleID);
            }
            else
            {
               return BaseResponse.BadRequest($"Role '{slug}' không tồn tại trong hệ thống!");
            }
         }
      }

      listRoleID = [.. roleMap.Values.Select(v => v!.Value)];

      await _systemRepository.UpdateUserRole(userID, listRoleID);

      return BaseResponse.Success("Update Successful!");
   }
}
