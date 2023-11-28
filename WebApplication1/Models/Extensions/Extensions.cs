namespace WebApplication1.Models.Extensions;

public static class ModelExtensions
{
    public static Card FromDto(this CardRequestDto dto)
    {
        return new Card { CardHolderAccountId = dto.CardholderAccountId };
    }

    public static Account FromDto(this AccountCreationDto dto)
    {
        return new Account
        {
            Id = new Random((int)DateTime.Now.ToBinary()).Next(),
            Name = dto.Name,
            Password = dto.Password,
            Username = dto.Username
        };
    }
}