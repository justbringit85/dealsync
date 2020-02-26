using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using log4net;
using log4net.Config;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using Microsoft.Azure.ServiceBus;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.ServiceBus;
using Microsoft.ServiceBus.Messaging;
using Newtonsoft.Json;
using RestSharp;
using StructureMap;
using ExceptionReceivedEventArgs = Microsoft.Azure.ServiceBus.ExceptionReceivedEventArgs;
using QueueClient = Microsoft.Azure.ServiceBus.QueueClient;

namespace DealSync
{
    class Program
    {

        static QueueClient queueClient;
        static void Main()
        {
           try
            {
                System.Net.ServicePointManager.DefaultConnectionLimit = System.Int32.MaxValue;
                XmlConfigurator.Configure();

                var config = new JobHostConfiguration
                {
                    JobActivator = new UnityJobActivatorDependencyScope(UnityConfig.GetConfiguredContainer())
                };

                var sbConfig = new ServiceBusConfiguration
                {
                    MessageOptions = new OnMessageOptions
                    {
                        AutoComplete = false
                    }
                };
                
                sbConfig.MessageOptions.MaxConcurrentCalls = 1;
                config.UseServiceBus(sbConfig);

                var host = new JobHost(config);

                //host.CallAsync(typeof(Functions).GetMethod("SyncDeals"));

                host.RunAndBlock();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

        }

    }
}
