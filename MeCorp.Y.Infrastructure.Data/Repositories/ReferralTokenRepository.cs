using MeCorp.Y.Api;
using MeCorp.Y.Domain.DomainEntities;
using MeCorp.Y.Infrastructure.Data.PersistenceEntities;
using MeCorp.Y.Shared;
using Microsoft.EntityFrameworkCore;

namespace MeCorp.Y.Infrastructure.Data.Repositories;

public class ReferralTokenRepository : IReferralTokenRepository
{
    private readonly UserDbContext dbContext;

    public ReferralTokenRepository(UserDbContext dbContext)
    {
        this.dbContext = dbContext;
    }

    public async Task<Result<ReferralToken>> Add(ReferralToken referralToken)
    {
        var referralTokenEntity = new ReferralTokenEntity
        {
            Code = referralToken.Code,
            IsValid = referralToken.IsValid
        };

        await dbContext.ReferralTokens.AddAsync(referralTokenEntity);

        return new Result<ReferralToken>
        {
            IsSuccessful = true,
            Value = new ReferralToken
            {
                Id = referralTokenEntity.Id,
                Code = referralTokenEntity.Code,
                CreatedAtUtc = referralTokenEntity.CreatedAtUtc,
                IsValid = referralToken.IsValid
            }
        };
    }

    public async Task<Result<ReferralToken>> GetByCode(string code)
    {
        var referral = await dbContext.ReferralTokens
            .Where(r => r.Code == code && r.IsValid == true)
            .FirstOrDefaultAsync();

        if (referral is null)
            return new Result<ReferralToken> { Message = $"Referral code {code} not found!" };

        return new Result<ReferralToken>
        {
            IsSuccessful = true,
            Value = new ReferralToken
            {
                Code = referral.Code,
                IsValid = referral.IsValid
            }
        };
    }
}
