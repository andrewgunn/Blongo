using System.Collections.Generic;

namespace AkismetSdk
{
    public class CommentTypeToString
    {
        static CommentTypeToString()
        {
            _mappings = new Dictionary<CommentType, string>
            {
                { CommentType.BlogPost, "blog-post" },
                { CommentType.Comment, "comment" },
                { CommentType.ContactForm, "contact-form" },
                { CommentType.ForumPost, "forum-post" },
                { CommentType.Pingback, "pingback" },
                { CommentType.SignUp, "sign-up" },
                { CommentType.Trackback, "trackback" },
                { CommentType.Tweet, "tweet" }
            };
        }

        public string Map(CommentType commentType)
        {
            string value;

            return _mappings.TryGetValue(commentType, out value) ? value : null;
        }

        private static readonly IDictionary<CommentType, string> _mappings;
       
    }
}
