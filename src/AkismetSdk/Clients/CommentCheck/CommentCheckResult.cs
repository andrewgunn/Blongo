namespace AkismetSdk.Clients.CommentCheck
{
    public class CommentCheckResult
    {
        public CommentCheckResult(bool isSpam)
        {
            IsSpam = isSpam;
        }

        public bool IsSpam { get; }
    }
}
