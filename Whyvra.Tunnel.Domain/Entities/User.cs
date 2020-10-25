using System;

namespace Whyvra.Tunnel.Domain.Entitites
{
    public class User : IEntity
    {
        public int Id { get; set; }

        public string Email { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public Guid Uid { get; set; }

        public string Username { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }
    }
}