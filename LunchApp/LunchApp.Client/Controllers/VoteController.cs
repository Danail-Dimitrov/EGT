using LunchApp.Client.DTOs;
using LunchApp.Client.Services.Interfaces;
using LunchApp.Grains.Abstractions;
using Microsoft.AspNetCore.Mvc;

namespace LunchApp.Client.Controllers
{
    [Route("[controller]")]
    public class VoteController : Controller
    {
        private readonly IClusterClient _clusterClient;
        private readonly IVoteService _voteService;

        public VoteController(IClusterClient clusterClient, 
            IVoteService voteService)
        {
            _clusterClient = clusterClient ?? throw new ArgumentNullException(nameof(clusterClient));
            _voteService = voteService ?? throw new ArgumentNullException(nameof(_voteService));
        }

        [HttpGet]
        public async Task<IActionResult> Index([FromQuery] string userName)
        {
            if (userName == "clock")
                return RedirectToAction("Clock");

            var clockGrain = this._clusterClient.GetGrain<IClockGrain>(0);

            var time = await clockGrain.GetCurrentTime();
            // You can process the input here as needed
            return Content($"Received input: {time}");
        }

        [HttpGet("Clock")]
        public async Task<IActionResult> Clock()
        {
            return View();
        }

        [HttpPost("Clock")]
        public IActionResult Clock([FromForm] SetClockDto model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            _voteService.SetTime(model.Date);

            return Ok();
        }
    }
}
