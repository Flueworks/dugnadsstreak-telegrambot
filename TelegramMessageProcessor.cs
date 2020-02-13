using System;
using System.Collections.Generic;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace BKM.Dugnad
{
    public static class TelegramMessageProcessor
    {
        [FunctionName("TelegramMessageProcessor")]
        public static void Run([QueueTrigger("incoming", Connection = "AzureWebJobsStorage")]string json,
            [Queue("contacts", Connection = "AzureWebJobsStorage")]ICollector<Contact> contacts,
            [Queue("messages", Connection = "AzureWebJobsStorage")]ICollector<string> messages,
            ILogger log)
        {
            var update = JsonConvert.DeserializeObject<Update>(json);
            log.LogInformation($"C# Queue trigger function processed: {update.Type}");

            if(update.Type != UpdateType.Message || update.Message == null){
                return; // only handle message types
            }

            // get user
            if(update.Message.Contact != null)
            {
                if(update.Message.Contact.UserId == update.Message.Chat.Id)
                {
                    contacts.Add(new Contact{
                        PhoneNumber = update.Message.Contact.PhoneNumber,
                        RowKey = update.Message.Chat.Id.ToString()
                    });
                    return;
                }
            }

            if(update.Message.Text == "/streak" || update.Message.Text == "/start")
            {
                messages.Add(update.Message.Chat.Id.ToString());
            }
        }

        // private void RequestPhoneNumber(long chatId)
        // {
        //     var rows = new List<KeyboardButton>(){
        //         new KeyboardButton("Send telefonnummeret mitt"){
        //             RequestContact = true,
        //         }
        //     };
        //     var replyMarkup = new ReplyKeyboardMarkup(rows, true, true);
        //     _telegramBot.SendTextMessageAsync(chatId, 
        //     "Velkommen til Dugnadstrikebotten\nFor at botten skal kunne gi deg riktige opplysninger må vi ha telefonnummeret ditt.",
        //     ParseMode.MarkdownV2, replyMarkup: replyMarkup);
        // }

        // private void SendAboutMessage(long chatId)
        // {
        //     var rows = new List<KeyboardButton>(){
        //         new KeyboardButton("/strike")
        //     };
        //     var replyMarkup = new ReplyKeyboardMarkup(rows, true, true);
        //     _telegramBot.SendTextMessageAsync(chatId, 
        //     "Send /strike for å få se striken din",
        //     ParseMode.MarkdownV2, replyMarkup: replyMarkup);
        // }

        // private void StorePhoneNumber(int userId, string phone)
        // {
        //     return;
        // }

        // private User GetRegisteredUser(int userId)
        // {
        //     // todo

        //     return new User(){
        //         PhoneNumber = "+4740451802"
        //     };
        // }
    }
}
