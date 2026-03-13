public static class Permissions
{
   // Admin
   public const string Read_Users = "READ_USER";
   public const string WRITE_USER = "WRITE_USER";
   public const string ASSIGN_ROLE = "ASSIGN_ROLE";
   public const string BAN_USER = "BAN_USER";
   public const string VIEW_SYSTEM_LOGS = "VIEW_SYSTEM_LOGS";

   // Manager / Support
   public const string LOCK_JOB = "LOCK_JOB";
   public const string HIDE_JOB = "HIDE_JOB";
   public const string DELETE_JOB = "DELETE_JOB";
   public const string MODERATE_REPORT = "MODERATE_REPORT";
   public const string RESTRICT_BUSINESS = "RESTRICT_BUSINESS";

   // Business
   public const string MANAGE_BUSINESS = "MANAGE_BUSINESS";
   public const string PEOPLE_BUSINESS = "PEOPLE_BUSINESS";
   public const string CREATE_JOB = "CREATE_JOB";
   public const string MANAGER_JOB = "MANAGER_JOB";
   public const string APPROVE_JOB = "APPROVE_JOB";
   public const string MANAGE_TEMPLATE = "MANAGE_TEMPLATE";
   public const string VIEW_CANDIDATE = "VIEW_CANDIDATE";
   public const string INTERVIEW_CANDIDATE = "INTERVIEW_CANDIDATE";

   // Candidate
   public const string APPLY_JOB = "APPLY_JOB";
}
