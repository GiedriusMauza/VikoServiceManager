﻿using System.Security.Claims;

namespace VikoServiceManager.Data
{
    public static class ClaimStore
    {
        public static List<Claim> claimsList = new List<Claim>()
        { 
            new Claim("Create", "Create"),
            new Claim("Edit", "Edit"),
            new Claim("Delete", "Delete")
        };
    }
}
