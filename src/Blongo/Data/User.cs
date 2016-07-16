﻿using MongoDB.Bson;
using System;
using System.Collections.Generic;

namespace Blongo.Data
{
    public class User
    {
        public string EmailAddress { get; set; }

        public ObjectId Id { get; set; }

        public string HashedPassword { get; set; }

        public string PasswordSalt { get; set; }

        public UserRoles Role { get; set; }
    }
}
