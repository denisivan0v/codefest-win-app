using Windows.Security.Cryptography;
using Windows.Security.Cryptography.Core;
using Windows.System.Profile;

namespace CodeFestApp.Utils
{
    public static class DeviceInfo
    {
        public static string GetDeviceIdentity()
        {
            var token = HardwareIdentification.GetPackageSpecificToken(null);
            
            var hasher = HashAlgorithmProvider.OpenAlgorithm(HashAlgorithmNames.Md5);
            var hashed = hasher.HashData(token.Id);

            return CryptographicBuffer.EncodeToHexString(hashed);
        }
    }
}
