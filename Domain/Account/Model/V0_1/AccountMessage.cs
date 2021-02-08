namespace Mospolyhelper.Domain.Account.Model.V0_1
{
    using System.Collections.Generic;

    public class AccountMessage
    {
        public AccountMessage(
            int id, 
            string avatarUrl, 
            string authorName, 
            string message,
            IList<AccountAttachment> attachments, 
            string removeUrl
            )
        {
            Id = id;
            AvatarUrl = avatarUrl;
            AuthorName = authorName;
            Message = message;
            Attachments = attachments;
            RemoveUrl = removeUrl;
        }

        public int Id { get; }
        public string AvatarUrl { get; }
        public string AuthorName { get; }
        public string Message { get; }
        public IList<AccountAttachment> Attachments { get; }
        public string RemoveUrl { get; }
    }
}
