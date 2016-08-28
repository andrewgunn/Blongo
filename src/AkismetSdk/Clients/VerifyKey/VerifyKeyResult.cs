namespace AkismetSdk.Clients.VerifyKey
{
    public class VerifyKeyResult
    {
        public VerifyKeyResult(bool isValid)
        {
            IsValid = isValid;
        }

        public bool IsValid { get; }
    }
}