using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using log4net;
using Microsoft.Azure.WebJobs;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Transactions;
using Microsoft.Azure.ServiceBus;
using Microsoft.Azure.ServiceBus.Core;
using Microsoft.ServiceBus.Messaging;
using Newtonsoft.Json;
using RestSharp;

namespace SyncTest
{
    public class Functions
    {
        private static IConfigurator Configurator => new Configurator();
        private static readonly ILog logger = LogManager.GetLogger(typeof(Functions));
        static QueueClient queueClient;
        string sbQueueName = "syncqueue";

        public async Task SyncTest([ServiceBusTrigger("syncqueue")] Message message, TextWriter log, MessageReceiver messageReceiver)
        {
            try
            {
                logger.Info("Sync started"); 
                
                messageReceiver.CompleteAsync(message.SystemProperties.LockToken);
                
                Upsert(message.PartitionKey); // this call takes over 20 minutes to complete, I have no included this api call in the source, issues are before this is even called
                
                logger.Info("Sync ended.");
            }
            catch (Exception e)
            {
                Environment.ExitCode = 1; 
                Console.WriteLine(e);
                throw;
            }
        }
    }
}
