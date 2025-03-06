using OneOf;
using OneOf.Types;
using TgStats.user.Domain;

namespace TgStats.user.Application.Models;

[GenerateOneOf]
public partial class UserCreateResult : OneOfBase<UserEntity.Id, Error<string>>
{
  
}