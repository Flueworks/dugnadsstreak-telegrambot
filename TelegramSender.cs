using System;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace BKM.Dugnad
{
    public static class TelegramSender
    {
        [FunctionName("TelegramSender")]
        public static async Task Run([QueueTrigger("outbox", Connection = "AzureWebJobsStorage")]UserMessage userMessage, ILogger log)
        {
            var config = new ConfigurationBuilder()
                .AddEnvironmentVariables()
                .AddUserSecrets(Assembly.GetExecutingAssembly(), true)
                .Build();

            log.LogInformation($"C# Queue trigger function processed: {userMessage}");
            var bot = new Telegram.Bot.TelegramBotClient(config["apikey"]);

            // userMessage.Keyboard
            IReplyMarkup markup = userMessage.Keyboard;
            if(markup == null)
            {
                //markup = new ReplyKeyboardRemove(); // or we could remove the buttons
                markup = new ReplyKeyboardMarkup(new []{new KeyboardButton("/streak")}, true, false);
            }
            await bot.SendTextMessageAsync(userMessage.ChatId, userMessage.Message, ParseMode.Markdown, replyMarkup: markup);
        }
    }
}
