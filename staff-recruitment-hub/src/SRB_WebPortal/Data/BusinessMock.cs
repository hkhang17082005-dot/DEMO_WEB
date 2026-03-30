using SRB_ViewModel.Entities;

namespace SRB_WebPortal.Data;

public static class BusinessMock
{
   public static List<Business> ListBusiness()
   {
      return [
         new Business {
            BusinessID = "019d3701-6c0b-7c78-8aed-c362b3897e31",
            BusinessName = "Business One",
         }
      ];
   }
}
