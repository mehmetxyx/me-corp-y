﻿namespace MeCorp.Y.Application.Dtos;

public class GetReferralTokenResponse
{
    public string Code { get; set; }
    public int Id { get; set; }
    public DateTime CreatedAtUtc { get; set; }
    public bool IsValid { get; set; }
}