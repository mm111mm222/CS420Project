using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Week9.Interfaces;
using Week9.Models;
using Week9.Models.Events;

namespace Week9.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SourceController : ControllerBase
    {
        IConfiguration _configuration;
        IEventBus _eventBus;

        public SourceController(IConfiguration configuration, IEventBus eventBus)
        {
            _eventBus = eventBus;
            _configuration = configuration;
            eventBus.HostName = _configuration["rabbitmqhostname"];
            eventBus.PortNumber = Convert.ToInt32(_configuration["rabbitmqport"]);
        }

        [HttpPost]
        public ActionResult HaveEmptyGlass([FromBody] EmptyGlass emptyGlass, int tableNumber, int seatNumber)
        {

            EmptyGlassEvent ege = new EmptyGlassEvent();
            ege.EmptyGlass = emptyGlass;
            ege.TableNumber = tableNumber;
            ege.SeatNumber = seatNumber;
            ege.TimeStamp = DateTime.Now;

            _eventBus.PublishEvent<EmptyGlassEvent>("emptyglass", ege);

            return new JsonResult(ege);
        }

    }
}
