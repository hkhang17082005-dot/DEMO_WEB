using SRB_ViewModel;
using SRB_ViewModel.Entities;

namespace SRB_WebPortal.Controllers.apis.post;

public interface IPostRepository
{
   Task CreateNewJobPost(CreateJobPostDTO formData, string createdByID);
}

public class PostRepository(DatabaseContext context) : IPostRepository
{
   private readonly DatabaseContext _context = context;

   public async Task CreateNewJobPost(CreateJobPostDTO formData, string createdByID)
   {
      try
      {
         var newJobPost = new JobPost
         {
            JobPostID = Guid.CreateVersion7().ToString(),
            Title = formData.Title,
            Description = formData.Description,
            SalaryRange = formData.SalaryRange,
            Location = formData.Location,
            IsActive = true,
            BusinessID = formData.BusinessID,
            CreatedByID = createdByID
         };

         await _context.JobPosts.AddAsync(newJobPost);
         await _context.SaveChangesAsync();
      }
      catch (Exception ex)
      {
         Console.Error.WriteLine($"Error in Create New JobPost: {ex.Message}");
      }
   }
}
