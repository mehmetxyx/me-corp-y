﻿namespace MeCorp.Y.Domain.DomainEntities;

public class ReferralToken
{
    public required string Code { get; set; }
    public bool IsValid { get; set; }
    public DateTime CreatedAtUtc { get; set; }
    public int Id { get; set; }
}