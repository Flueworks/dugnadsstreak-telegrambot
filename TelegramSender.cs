using System;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;
using Telegram.Bot.Types.Enums;

namespace BKM.Dugnad
{
    public static class TelegramSender
    {
        [FunctionName("TelegramSender")]
        public static async Task Run([QueueTrigger("outbox", Connection = "AzureWebJobsStorage")]UserMessage userMessage, ILogger log)
        {
            log.LogInformation($"C# Queue trigger function processed: {userMessage}");
            var bot = new Telegram.Bot.TelegramBotClient("");

            await bot.SendTextMessageAsync(userMessage.ChatId, userMessage.Message, ParseMode.MarkdownV2, replyMarkup: userMessage.Keyboard);
        }
    }
}
