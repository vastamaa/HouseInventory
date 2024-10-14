namespace HouseInventory.Data.Entities.Exceptions
{
    public sealed class RefreshTokenBadRequest : BadHttpRequestException
    {
        public RefreshTokenBadRequest() : base("Invalid client request. The tokenDto has some invalid values.") { }
    }
}
