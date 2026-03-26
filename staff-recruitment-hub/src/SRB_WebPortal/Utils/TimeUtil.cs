namespace SRB_WebPortal.Utils;

public class TimeUtil
{
   public static TimeSpan ParseExpTime(string exp)
   {
      if (exp.EndsWith('h')) return TimeSpan.FromHours(double.Parse(exp.Replace("h", "")));
      if (exp.EndsWith('m')) return TimeSpan.FromMinutes(double.Parse(exp.Replace("m", "")));
      if (exp.EndsWith('d')) return TimeSpan.FromDays(double.Parse(exp.Replace("d", "")));

      return TimeSpan.FromMinutes(60);
   }
}
