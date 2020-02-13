using System;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;

namespace BKM.Dugnad
{
    public static class GetPhoneNumber
    {
        [FunctionName("GetPhoneNumber")]
        public static void Run([QueueTrigger("messages", Connection = "AzureWebJobsStorage")]string chatId,
        [Table("PhoneNumbers", "Telegram", "{queueTrigger}")] Contact contact,
        [Queue("outbox", Connection = "AzureWebJobsStorage")]ICollector<UserMessage> outbox,
        [Queue("requests", Connection = "AzureWebJobsStorage")]ICollector<Contact> requests,
         ILogger log)
        {
            log.LogInformation($"C# Queue trigger function processed: {chatId}");
            log.LogInformation($"Got phone number {contact?.PhoneNumber}");
            if(contact == null)
            {
                outbox.Add(new UserMessage{
                    ChatId = chatId,
                    Message = "Vi trenger telefonnummeret ditt for kunne sende deg din dugnadsstreak",
                    Keyboard = new Telegram.Bot.Types.ReplyMarkups.ReplyKeyboardMarkup(new Telegram.Bot.Types.ReplyMarkups.KeyboardButton[]{new Telegram.Bot.Types.ReplyMarkups.KeyboardButton("Send telefonnummeret mitt"){
                        RequestContact = true,
                    }}, true, true)
                });
                return;
            }

            requests.Add(contact);
        }
    }
}
