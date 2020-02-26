# synctest

Project entails putting a message on the service bus queue, having a azure web job run and call the functions.cs file to then pick up the message off the service bus queue and process it. 

The issue I'm having is the message takes too long to process, usually more than 20 minutes, then the lock gets removed from the message in the queue so the next instance of the jobs picks it up as the first attempt is still running. 

I'm working on completing the message as soon as the job runs to prevent this issue, but my code isn't working. 

I receieve errors when calling Functions.cs: 

Cannot bind parameter 'messageReceiver' to type MessageReceiver. Make sure the parameter Type is supported by the binding. 

Exception binding parameter 'message' ---> System.Runtime.Serialization.SerializationException : There was an error deserializing the object of type Microsoft.Azure.ServiceBus.Message. The input source is not correctly formatted. 
