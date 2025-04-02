﻿namespace MeCorp.Y.Infrastructure.Security;

public class GenerateTokenArguments
{
    public int UserId { get; set; }
    public required string Username { get; set; }
    public required string Role { get; set; }
}