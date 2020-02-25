using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using Telegram.Bot.Types.ReplyMarkups;

namespace BKM.Dugnad
{
    public static class ProcessMessage
    {
        [FunctionName("ProcessMessage")]
        public static void Run([QueueTrigger("messages", Connection = "AzureWebJobsStorage")]Message message,
        [Table("PhoneNumbers", "Telegram", "{ChatId}")] Contact contact,
        [Queue("outbox", Connection = "AzureWebJobsStorage")]ICollector<UserMessage> outbox,
        [Queue("requests", Connection = "AzureWebJobsStorage")]ICollector<Contact> streakRequests,
         ILogger log)
        {
            log.LogInformation($"C# Queue trigger function processed: {message.ChatId} {message.Text}");
            log.LogInformation($"Got phone number {contact?.PhoneNumber}");

            // if(message.Text == "/start" && contact == null){
            //     // send welcome message

            //     outbox.Add(new UserMessage(){
            //         ChatId = message.ChatId,
            //         Message = "Velkommen til Aksjonsstreak botten. Send /streak for å vise din streak.",
            //     });
            // }

            if(contact == null)
            {
                outbox.Add(new UserMessage{
                    ChatId = message.ChatId,
                    Message = "Velkommen til Aksjonsstreak botten. Vi trenger telefonnummeret ditt for kunne sende deg din dugnadsstreak",
                    Keyboard = new ReplyKeyboardMarkup(new []{new KeyboardButton("Trykk her for å sende telefonnummeret mitt"){
                        RequestContact = true,
                    }}, false, true)
                });
                return;
            }

            if(message.SentContact)
            {
                outbox.Add(new UserMessage(){
                    ChatId = message.ChatId,
                    Message = "Ditt telefonnummer er registrert. Send /streak for å vise din streak.",
                });
                // evt sende streak direkte?
                return;
            }

            if(message.Text == "/start" || message.Text == "/hjelp"){
                outbox.Add(new UserMessage(){
                    ChatId = message.ChatId,
                    Message = "Velkommen til Aksjonsstreak botten. Send /streak for å vise din streak.",
                });
                return;
            }

            if(message.Text == "/streak"){
                streakRequests.Add(contact);
                return;
            }

            // send error message?
            
        }
    }
}
