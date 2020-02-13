namespace BKM.Dugnad
{
    public class StreakData
    {
        public string PartitionKey { get; set; }
        public string RowKey { get; set; }

        public string Streak { get; set; }
    }

    public class Contact
    {
        public string PartitionKey { get; set; }
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