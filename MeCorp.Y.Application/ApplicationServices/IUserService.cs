using MeCorp.Y.Application.Dtos;
using MeCorp.Y.Shared;

namespace MeCorp.Y.Application.ApplicationServices;

public interface IUserService
{
    Task<Result<List<GetAdminSummaryResponse>>> GetAdminSummary();
    Task<Result<GetUserResponse>> GetUserById(int id);
}