using System.Diagnostics.CodeAnalysis;

namespace HouseInventory.Data.Entities.Exceptions
{
    [ExcludeFromCodeCoverage]
    public sealed class RefreshTokenBadRequestException : BadHttpRequestException
    {
        public RefreshTokenBadRequestException() : base("Invalid client request. The tokenDto has some invalid values.") { }
    }
}
