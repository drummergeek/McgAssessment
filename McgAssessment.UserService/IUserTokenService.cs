namespace McgAssessment.UserService;

public interface IUserTokenService
{
    bool TryValidateUserToken(string token, out User user);
}
