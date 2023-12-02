using System.Security.Cryptography;

namespace KnowledgeSiteApp.Backend.Core.Token
{
    public class RandomToken
    {
        public static string CreateRandomToken()
        {
            return Convert.ToHexString(RandomNumberGenerator.GetBytes(64));
        }
    }
}
