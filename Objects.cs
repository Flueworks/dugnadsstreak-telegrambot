namespace BKM.Dugnad
{
    public class Message
    {
        public string ChatId { get; set; }
        public string Text { get; set; }
        public bool SentContact { get; set; }
    }

    public class StreakData
    {
        public string PartitionKey { get; set; }
        
        // PhoneNumber
        public string RowKey { get; set; }

        public string Streak { get; set; }
    }

    public class Contact
    {
        public string PartitionKey { get; set; }
        
        // ChatId
        public string RowKey { get; set; }
        public string PhoneNumber { get; set; }
    }

    public class UserMessage
    {
        public string ChatId {get;set;}

        public string Message { get; set; }

        public Telegram.Bot.Types.ReplyMarkups.ReplyKeyboardMarkup Keyboard { get; set; }
    }

}