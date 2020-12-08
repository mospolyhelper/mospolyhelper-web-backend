namespace Mospolyhelper.Domain.Account.Model
{
    public class AccountResult<T> where T : class
    {
        public AccountResult(T? value, bool isSuccess, bool isAuthorized)
        {
            Value = value;
            IsSuccess = isSuccess;
            IsAuthorized = isAuthorized;
        }

        public T? Value { get; }
        public bool IsSuccess { get; }
        public bool IsAuthorized {get;}
    }
}
