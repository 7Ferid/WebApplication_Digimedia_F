using System.Threading.Tasks;

namespace WebApplication_Digimedia_F.Helpers
{
    public  static class FileHelpers
    {
        public static bool GetSize(this IFormFile file ,int mb)
        {
            return file.Length < mb * 1024 * 1024;
        }
        public static bool GetType(this IFormFile file,string type)
        {
            return file.ContentType.Contains(type);
        }
        public static async Task<string> FileUpload(this IFormFile file , string folderpath)
        {
            string UniqueFileName = Guid.NewGuid().ToString() + file.FileName;
            string path = Path.Combine(folderpath, UniqueFileName);
            using FileStream stream = new(path, FileMode.Create);
            await file.CopyToAsync(stream);
            return UniqueFileName;
        }
        public static void FileDelete(string path)
        {
            if (File.Exists(path))
                File.Delete(path);
        }
    }
}
