using ApiExample.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ApiExample.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MessageController : ControllerBase
    {
        [HttpPost()]
        public IActionResult Post([FromForm]User model)
        {
            ConnectionFactory factory = new ConnectionFactory();
            factory.Uri = new Uri("");//(AMQP URL)
            using IConnection connection = factory.CreateConnection();
            using IModel channel = connection.CreateModel();

            channel.QueueDeclare("messagequeue", false, false, false);

            string serializeData = JsonSerializer.Serialize(model);//encoding işlemi için serialize etmeliyiz.

            byte[] data = Encoding.UTF8.GetBytes(serializeData);
            channel.BasicPublish("", "messagequeue", body: data);

            return Ok();
        }
    }
}
