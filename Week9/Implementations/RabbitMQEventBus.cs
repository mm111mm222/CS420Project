using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Week9.Interfaces;

namespace Week9.Implementations
{
    public class RabbitMQEventBus : IEventBus
    {
        String _hostname;
        int _portNumber;
        public string HostName { get => _hostname; set => _hostname = value; }

        public int PortNumber { get => _portNumber; set => _portNumber = value; }

        public void PublishEvent<T>(String queueName, T e)
        {
            if (String.IsNullOrEmpty(HostName))
                throw new Exception("Hostname needs provided");

            var factory = new ConnectionFactory() { HostName = _hostname, Port = _portNumber };
            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.QueueDeclare(queue: queueName,
                                     durable: true,
                                     exclusive: false,
                                     autoDelete: false,
                                     arguments: null);

                string message = JsonSerializer.Serialize(e);
                var body = Encoding.UTF8.GetBytes(message);

                channel.BasicPublish(exchange: "",
                                     routingKey: queueName,
                                     basicProperties: null,
                                     body: body);
            }
        }
    }
}
