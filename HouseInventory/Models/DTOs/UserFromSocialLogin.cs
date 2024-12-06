namespace HouseInventory.Models.DTOs
{
    public record UserFromSocialLogin
    {
        public string? FirstName{ get; init; }
        public string? LastName { get; init; }
        public string Email { get; init; }
        public string LoginProviderSubject { get; init; }

    }
}
