﻿using System;

namespace task1.DTOs
{
    public class UserResponse
    {
        public Guid ID { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string Role { get; set; }
    }
}