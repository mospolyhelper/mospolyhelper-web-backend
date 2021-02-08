namespace Mospolyhelper.Domain.Account.Model.V0_1
{
    public class DialogPreview
    {
        public DialogPreview(
            int id, 
            string dialogKey, 
            string authorName, 
            string authorGroup, 
            string avatarUrl, 
            string message, 
            string date,
            string senderImageUrl,
            string senderName,
            bool hasAttachments, 
            bool hasRead)
        {
            Id = id;
            DialogKey = dialogKey;
            AuthorName = authorName;
            AuthorGroup = authorGroup;
            AvatarUrl = avatarUrl;
            Message = message;
            Date = date;
            SenderImageUrl = senderImageUrl;
            SenderName = senderName;
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
        public string SenderImageUrl { get; }
        public string SenderName { get; }
        public bool HasAttachments { get;  }
        public bool HasRead { get; }

    }
}
