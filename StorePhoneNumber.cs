using System;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;

namespace BKM.Dugnad
{
    public static class StorePhoneNumber
    {
        [FunctionName("StorePhoneNumber")]
        [return: Table("PhoneNumbers")]
        public static Contact Run([QueueTrigger("contacts", Connection = "AzureWebJobsStorage")]Contact contact, 
            [Queue("messages", Connection = "AzureWebJobsStorage")]ICollector<Message> messages,
            ILogger log)
        {
            log.LogInformation($"C# Queue trigger function processed: {contact.RowKey} {contact.PhoneNumber}");
            contact.PartitionKey = "Telegram";
            messages.Add(new Message(){
                ChatId = contact.RowKey,
                SentContact = true,
            });

            return contact;
        }
    }
}
