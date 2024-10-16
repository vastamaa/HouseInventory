namespace HouseInventory.Data.Entities.Exceptions
{
    public sealed class RefreshTokenBadRequestException : BadHttpRequestException
    {
        public RefreshTokenBadRequestException() : base("Invalid client request. The tokenDto has some invalid values.") { }
    }
}
