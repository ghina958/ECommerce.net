using CloudinaryDotNet.Actions;

namespace KaracadanWebApp.Interfaces
{
    public interface IPhotoService
    {
        Task<ImageUploadResult> AddPhotoAsync(IFormFile file);
        Task<DeletionResult> DeletePhotoAsync(String publicUrl);
    }
}
