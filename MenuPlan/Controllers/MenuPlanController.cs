using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using MenuPlan.Infrastruktur;
using MenuPlan.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace MenuPlan.Controllers
{
    [Route("api/menu")]
    [ApiController]
    public class MenuPlanController : ControllerBase
    {
        private readonly MenuPlanCRUDService _MenuPlanService;
        ILogger<MenuPlanController> _logger;
        IConfiguration _configuration;

        public MenuPlanController(MenuPlanCRUDService dbDriverService, ILogger<MenuPlanController> loggerIn, IConfiguration configuartionIn)
        {
            _MenuPlanService = dbDriverService;
            _logger = loggerIn;
            _configuration = configuartionIn;
        }

        [HttpGet]
        [Route("heartbeat")]
        public Heartbeat Info()
        {
            string envVar = Environment.GetEnvironmentVariable("MENU_SERVICE_HEARTBEAT_TEXT") ?? "Alive";
            _logger.LogInformation($"Aufruf -> {nameof(Info)}");

            return new Heartbeat { Answer = envVar };
        }

        [HttpGet]
        public ActionResult<List<Menu>> Get()
        {
            _logger.LogInformation($"Aufruf -> {nameof(Get)}");
            return _MenuPlanService.Get();
        }


        //[HttpGet("{id:length(24)}")]
        //public ActionResult<Menu> GetMenu(string id)
        [HttpGet("{id:length(24)}", Name = "GetMenu")]
        public ActionResult<Menu> GetMenu(string id)
        {
            _logger.LogInformation($"Aufruf -> {nameof(Get)} id={id}");
            var menu = _MenuPlanService.Get(id);

            if (menu == null)
            {
                return NotFound();
            }

            return menu;
        }

        [HttpPost]
        public ActionResult<Menu> Create(Menu menu)
        {
            _logger.LogInformation($"Aufruf -> {nameof(Create)}  menuIn={JsonSerializer.Serialize(menu)}");
            _MenuPlanService.Create(menu);

            return CreatedAtRoute("GetMenu", new { id = menu.Id.ToString() }, menu);
        }

        [HttpPut("{id:length(24)}")]
        public IActionResult Update(string idIn, Menu menuIn)
        {
            _logger.LogInformation($"Aufruf -> {nameof(Update)} id={idIn} menuIn={JsonSerializer.Serialize(menuIn)}");
            var menu = _MenuPlanService.Get(idIn);

            if (menu is null)
            {
                return NotFound();
            }

            _MenuPlanService.Update(idIn, menuIn);

            return NoContent();
        }

        [HttpDelete("{id:length(24)}")]
        public IActionResult Delete(string id)
        {
            _logger.LogInformation($"Aufruf -> {nameof(Delete)}  idIn={id}");
            var menu = _MenuPlanService.Get(id);

            if (menu == null)
            {
                return NotFound();
            }

            _MenuPlanService.Remove(menu.Id);

            return NoContent();
        }
    }
}
