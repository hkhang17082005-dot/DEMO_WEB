using SRB_ViewModel.Entities;

namespace SRB_WebPortal.Data;

public static class LocationMock
{
   public static List<Location> GetLocations()
   {
      return [
         new Location {
            LocationID = 1,
            LocationName = "Location One"
         },

         new Location {
            LocationID = 2,
            LocationName = "Location Two"
         },

         new Location {
            LocationID = 3,
            LocationName = "Location Three"
         },

         new Location {
            LocationID = 4,
            LocationName = "Location Four"
         },

         new Location {
            LocationID = 5,
            LocationName = "Location Five"
         }
      ];
   }
}
