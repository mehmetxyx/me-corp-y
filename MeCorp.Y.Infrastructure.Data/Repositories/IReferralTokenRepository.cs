using MeCorp.Y.Domain.DomainEntities;
using MeCorp.Y.Shared;

namespace MeCorp.Y.Infrastructure.Data.Repositories;

public interface IReferralTokenRepository
{
    Task<Result<ReferralToken>> Add(ReferralToken referralToken);
    Task<Result<ReferralToken>> GetByCode(string code);
}