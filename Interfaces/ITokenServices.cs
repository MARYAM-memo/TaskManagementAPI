using Microsoft.AspNetCore.Identity;

namespace TodoAPI.Interfaces;

public interface ITokenServices
{
      Task<string> GenerateAccessToken(IdentityUser<int> user);

}
