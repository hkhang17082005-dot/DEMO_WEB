using System.Collections.Generic;

namespace SRB_ViewModel.Entities
{
    public class Location
    {
        public int LocationID { get; set; }

        public string LocationName { get; set; }

        public ICollection<Job> Jobs { get; set; }
    }
}