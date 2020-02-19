using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;

namespace BKM.Dugnad
{
    public static class GetStreak
    {
        [FunctionName("GetStreak")]
        public static void Run([QueueTrigger("requests", Connection = "AzureWebJobsStorage")]Contact contact,
        [Table("Streaks", "Streaks", "{PhoneNumber}")] StreakData streak,
        [Queue("outbox", Connection = "AzureWebJobsStorage")]ICollector<UserMessage> outbox,
         ILogger log)
        {
            log.LogInformation($"C# Queue trigger function processed: {contact.RowKey}");
            if(streak == null){
                outbox.Add(new UserMessage()
                {
                    ChatId = contact.RowKey,
                    Message = "Fant ingen streak. Kontakt Christoffer Tombre for feilsøking."
                });
                return;
            }

            outbox.Add(new UserMessage()
            {
                ChatId = contact.RowKey,
                Message = $"Din streak er {streak.Streak} 🔥"
            });
        }
    }
}
