namespace SRB_WebPortal.Consts;

public static class ServerKey
{
   public const string CONTEXT_ITEM_TOKEN_INFO = "TokenPayloadInfo";
   public const string CONTEXT_ITEM_SESSION_LOGIN = "SessionLogin";
}

public static class CloudCNDKey
{
   public const string FOLDER_PROFILE_CV = "user_profiles/cvs";

   public const string FOLDER_PROFILE_AVATAR = "user_profiles/avatars";

   public const string FOLDER_APPLY_JOB_CV = "job_post/apply_cv";
}

public static class RedisCacheKeys
{
   public const string SystemPrefix = "system";
   public static string RoleIDBySlug(string roleSlugs) => $"{SystemPrefix}:role:{roleSlugs}";
   public static string SessionInfo(string userId, string sessionId) => $"user:{userId}:session:{sessionId}:info";
   public static string RefreshToken(string sessionId) => $"session:{sessionId}:refreshToken";
}
