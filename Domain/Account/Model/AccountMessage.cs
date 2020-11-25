namespace Mospolyhelper.Domain.Account.Model
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public class AccountMessage
    {
        public AccountMessage(
            int id, 
            string avatarUrl, 
            string authorName, 
            string message, 
            string? attachmentUrl, 
            string removeUrl
            )
        {
            Id = id;
            AvatarUrl = avatarUrl;
            AuthorName = authorName;
            Message = message;
            AttachmentUrl = attachmentUrl;
            RemoveUrl = removeUrl;
        }

        public int Id { get; }
        public string AvatarUrl { get; }
        public string AuthorName { get; }
        public string Message { get; }
        public string? AttachmentUrl { get; }
        public string RemoveUrl { get; }
    }
}
