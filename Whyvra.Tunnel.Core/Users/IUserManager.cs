using System;

namespace Whyvra.Tunnel.Core.Users
{
    public interface IUserManager
    {
        string GetSourceAddress();

        Guid GetUserUid();
    }
}
