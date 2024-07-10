using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ASP_MVC_Movie.Interfaces
{
    public interface ICommentService
    {
        // All method name says what it can

        Task<IActionResult> AddComment(int movieId, string commentText, ClaimsPrincipal user);
        Task<IActionResult> DeleteComment(int commentId, int movieId, ClaimsPrincipal user);
    }
}
