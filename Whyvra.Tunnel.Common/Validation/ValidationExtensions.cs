using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text.RegularExpressions;

namespace Whyvra.Tunnel.Common.Validation
{
    public static class ValidationExtensions
    {
        public static bool IsAllDigits(this string str)
        {
            if(string.IsNullOrWhiteSpace(str)) return false;

            return Regex.IsMatch(str, @"^\d+$");
        }

        public static bool IsBase64(this string encoded)
        {
            if(string.IsNullOrWhiteSpace(encoded)) return false;

            return encoded.Length % 4 == 0
                && Regex.IsMatch(encoded, @"^[a-zA-Z0-9\+/]*={0,3}$", RegexOptions.None);
        }

        public static bool IsIPAddressWithCidr(this string addr)
        {
            if(string.IsNullOrWhiteSpace(addr)) return false;
            var chunks = addr.Split('/');

                return chunks.Length == 2
                    && (chunks[0].IsIPv4Address() || chunks[0].IsIPv6Address())
                    && int.TryParse(chunks[1], out var num)
                    && num >= 0
                    && num <= (chunks[0].IsIPv4Address() ? 32 : 64);
        }

        public static bool IsIPv4Address(this string addr)
        {
            if(string.IsNullOrWhiteSpace(addr)) return false;
            var chunks = addr.Split('.');

            return chunks.Length == 4
                && chunks.All(x => x.Length > 0 && x.Length <= 3)
                && chunks.All(x => byte.TryParse(x, out byte num))
                && chunks.All(IsAllDigits);
        }

        public static bool IsIPv6Address(this string addr)
        {
            return IPAddress.TryParse(addr, out var ip) && ip.AddressFamily == AddressFamily.InterNetworkV6;
        }
    }
}