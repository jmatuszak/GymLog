﻿namespace GymLog.ViewModels
{
    public class AccountVM
    {
        public string Id { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public List<string> Roles { get; set; }
    }
}