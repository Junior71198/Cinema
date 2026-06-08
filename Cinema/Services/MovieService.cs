namespace Cinema.Services
{
    public class MovieService
    {
        public enum MovieImageType
        {
            Main,
            Sub
        }   
        public string SaveFile(IFormFile ImageFile, MovieImageType imageType = MovieImageType.Main)
        {
            try
            {
              
                var fileName = Guid.NewGuid().ToString() + "_" + ImageFile.FileName;
                var filePath = "";
             if (imageType == MovieImageType.Main) {
                 filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\images\\movie_images\\", fileName);
                }
             else if(imageType == MovieImageType.Sub)
                {
                     filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\images\\movie_images\\movie_sub_images\\", fileName);
                }

                using (var stream = System.IO.File.Create(filePath))
                {
                    ImageFile.CopyTo(stream);
                }
                return fileName;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Errors {ex.Message}");
                return null;
            }

        }
        public bool RemoveFile(string fileName, MovieImageType imageType = MovieImageType.Main)
        {
            try
            {
                var oldPath = "";
                if (imageType == MovieImageType.Main)
                {
                    oldPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\images\\movie_images\\", fileName);
                }
                else if (imageType == MovieImageType.Sub)
                {
                    oldPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\images\\movie_images\\movie_sub_images\\", fileName);
                }

                if (System.IO.File.Exists(oldPath))
                
                {
                    oldPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\images\\product_images\\", fileName);
                }
                else if (imageType == MovieImageType.Sub)
                {
                    oldPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\images\\product_images\\product_sub_images\\", fileName);
                }

                if (System.IO.File.Exists(oldPath))
                {
                    System.IO.File.Delete(oldPath);
                }
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Errors {ex.Message}");
                return false;
            }

        }
    }
}
