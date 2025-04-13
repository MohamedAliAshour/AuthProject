using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace AuthProject.Interface
{
    public interface IAuth
    {
        string CreateToken(IdentityUser user, List<Claim> claims);
    }
}
