using loginjwt.Model.ValueObjects;

namespace loginjwt.Business
{
    public interface ILoginBusiness
    {
        TokenVO ValidateCredentials(UserVO user);
        //For Refresh Token
        TokenVO ValidateCredentials(TokenVO token);
        bool RevokeToken(string username);
    }
}