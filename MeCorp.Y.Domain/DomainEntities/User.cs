﻿using MeCorp.Y.Domain.Enums;

namespace MeCorp.Y.Domain.DomainEntities;

public class User
{
    public int Id { get; set; }
    public required string Username { get; set; }
    public required string PasswordHash { get; set; }
    public UserRole Role { get; set; }
    public DateTime CreatedAtUtc { get; set; }
}