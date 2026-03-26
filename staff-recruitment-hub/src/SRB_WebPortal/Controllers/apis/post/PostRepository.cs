using SRB_ViewModel;
using SRB_ViewModel.Entities;

namespace SRB_WebPortal.Controllers.apis.post;

public interface IPostRepository
{
   Task CreateNewJobPost(CreateJobPostDTO formData);
}

public class PostRepository(DatabaseContext context) : IPostRepository
{
   private readonly DatabaseContext _context = context;

   public async Task CreateNewJobPost(CreateJobPostDTO formData)
   {
      try
      {
         var newJobPost = new JobPost
         {
            Title = formData.Title,
            Description = formData.Description,
            SalaryRange = formData.SalaryRange,
            Location = formData.Location,
            IsActive = true,
            BusinessID = formData.BusinessID
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
