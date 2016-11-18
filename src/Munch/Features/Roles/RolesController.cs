using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Net.Http;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace Munch.Features.Roles
{
    public class RolesController : Controller
    {
        private readonly AppSettings _appSettings;
        private readonly HttpClient _httpClient = new HttpClient();


        public RolesController(IOptions<AppSettings> options)
        {
            _appSettings = options.Value;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Role()
        {
            return View();
        }

        [HttpPost]
        public async  Task<IActionResult> Role(RolesViewModel vm)
        {
            var genreString = await _httpClient.GetStringAsync($"{_appSettings.RolesUrl}/Role");
            return View();
        }
    }
}
