namespace SRB_ViewModel.Entities
{
    public class Job
    {
        public int JobID { get; set; }

        public string Title { get; set; }

        public string CompanyName { get; set; }

        public string Salary { get; set; }

        public string JobType { get; set; }

        public int LocationID { get; set; }

        public Location Location { get; set; }
    }
}