namespace KnowledgeSiteApp.Backend.Core.ImageDirectory
{
    public class ImagePathConfig
    {
        public async Task<string?> profileImages(IFormFile? imageFile)
        {
            if (imageFile == null || imageFile.Length == 0)
                return null;

            string mainFolder = Path.Combine(Directory.GetCurrentDirectory(), "Images");
            string subFolder = Path.Combine(mainFolder, "ProfileImage");

            if (!Directory.Exists(mainFolder))
            {
                Directory.CreateDirectory(mainFolder);
            }
            if (!Directory.Exists(subFolder))
            {
                Directory.CreateDirectory(subFolder);
            }

            var fileName = Guid.NewGuid().ToString() + Path.GetExtension(imageFile.FileName);
            var filePath = Path.Combine(subFolder, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await imageFile.CopyToAsync(stream);
            }

            return Path.Combine("Images", "ProfileImage", fileName);
        }

        public async Task<string?> trainingImages(IFormFile? imageFile)
        {
            if (imageFile == null || imageFile.Length == 0)
                return null;

            string mainFolder = Path.Combine(Directory.GetCurrentDirectory(), "Images");
            string subFolder = Path.Combine(mainFolder, "TrainingImages");

            if (!Directory.Exists(mainFolder))
            {
                Directory.CreateDirectory(mainFolder);
            }
            if (!Directory.Exists(subFolder))
            {
                Directory.CreateDirectory(subFolder);
            }

            var fileName = Guid.NewGuid().ToString() + Path.GetExtension(imageFile.FileName);
            var filePath = Path.Combine(subFolder, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await imageFile.CopyToAsync(stream);
            }

            return Path.Combine("Images", "TrainingImages", fileName);
        }

        public async Task<string?> resourceImages(IFormFile? imageFile)
        {
            if (imageFile == null || imageFile.Length == 0)
                return null;

            string mainFolder = Path.Combine(Directory.GetCurrentDirectory(), "Images");
            string subFolder = Path.Combine(mainFolder, "ResourcesImages");

            if (!Directory.Exists(mainFolder))
            {
                Directory.CreateDirectory(mainFolder);
            }
            if (!Directory.Exists(subFolder))
            {
                Directory.CreateDirectory(subFolder);
            }

            var fileName = Guid.NewGuid().ToString() + Path.GetExtension(imageFile.FileName);
            var filePath = Path.Combine(subFolder, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await imageFile.CopyToAsync(stream);
            }

            return Path.Combine("Images", "ResourcesImages", fileName);
        }

        public async Task<string?> subTopicVideo(IFormFile? imageFile)
        {
            if (imageFile == null || imageFile.Length == 0)
                return null;

            string mainFolder = Path.Combine(Directory.GetCurrentDirectory(), "Images");
            string subFolder = Path.Combine(mainFolder, "SubTopicVideo");

            if (!Directory.Exists(mainFolder))
            {
                Directory.CreateDirectory(mainFolder);
            }
            if (!Directory.Exists(subFolder))
            {
                Directory.CreateDirectory(subFolder);
            }

            var fileName = Guid.NewGuid().ToString() + Path.GetExtension(imageFile.FileName);
            var filePath = Path.Combine(subFolder, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await imageFile.CopyToAsync(stream);
            }

            return Path.Combine("Images", "SubTopicVideo", fileName);
        }
    }
}
