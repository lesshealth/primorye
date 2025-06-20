﻿namespace primorye.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Login { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;

        public int TeamId { get; set; }
        public Team Team { get; set; } = null!;
    }
}
