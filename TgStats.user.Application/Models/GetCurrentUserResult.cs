using OneOf;
using OneOf.Types;

namespace TgStats.user.Application.Models;

[GenerateOneOf]
public partial class GetCurrentUserResult : OneOfBase<UserModel, Error<string>>
{

}