using OneOf;
using OneOf.Types;

namespace TgStats.user.Application.Models;

[GenerateOneOf]
public partial class LoginUserResult : OneOfBase<AuthTokens, Error<string>>
{

}