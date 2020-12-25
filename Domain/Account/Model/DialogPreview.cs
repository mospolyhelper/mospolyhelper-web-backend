using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mospolyhelper.Domain.Account.Model
{
    public class DialogPreview
    {
        public DialogPreview(int id, string dialogKey, string authorName, 
            string authorGroup, string avatarUrl, string message, string date, bool hasAttachments, bool hasRead)
        {
            Id = id;
            DialogKey = dialogKey;
            AuthorName = authorName;
            AuthorGroup = authorGroup;
            AvatarUrl = avatarUrl;
            Message = message;
            Date = date;
            HasAttachments = hasAttachments;
            HasRead = hasRead;
        }

        public int Id { get; }
        public string DialogKey { get; }
        public string AuthorName { get; }
        public string AuthorGroup { get; }
        public string AvatarUrl { get; }
        public string Message { get; }
        public string Date { get; }
        public bool HasAttachments { get;  }
        public bool HasRead { get; }

    }
}
