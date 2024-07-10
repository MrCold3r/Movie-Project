namespace ASP_MVC_Movie.Interfaces
{
    public interface IFileService
    {
        // All method name says what it can

        Task<string> SaveVideoAsync(IFormFile videoFile);
    }
}
