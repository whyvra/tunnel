using System;

namespace Whyvra.Tunnel.Domain
{
    public interface IEntity
    {
        int Id { get; set; }

        DateTime CreatedAt { get; set; }

        DateTime UpdatedAt { get; set; }
    }
}