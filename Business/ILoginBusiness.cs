using loginjwt.Model.ValueObjects;

namespace loginjwt.Business
{
    public interface ILoginBusiness
    {
        TokenVO ValidateCredentials(UserVO user);
    }
}