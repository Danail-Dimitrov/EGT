using LunchApp.Client.DTOs;
using LunchApp.Client.Services.Interfaces;
using LunchApp.Grains.Abstractions;
using Microsoft.AspNetCore.Mvc;

namespace LunchApp.Client.Controllers
{
    [Route("[controller]")]
    public class VoteController : Controller
    {
        private readonly IVoteService _voteService;

        public VoteController(IClusterClient clusterClient,
            IVoteService voteService)
        {
            _voteService = voteService ?? throw new ArgumentNullException(nameof(_voteService));
        }

        [HttpGet]
        public async Task<IActionResult> Index([FromQuery] string userName)
        {
            if (userName == "clock")
                return RedirectToAction("Clock");

            var currentDate = await _voteService.GetTime();
            var existsVoteForDay = await _voteService.ExistsVoteForDay(currentDate);

            if (!existsVoteForDay)
                return RedirectToAction("CreateVote", new { userName });

            return RedirectToAction("Vote", new { userName });
        }

        [HttpGet("Clock")]
        public IActionResult Clock()
        {
            return View();
        }

        [HttpPost("Clock")]
        public IActionResult Clock([FromForm] SetClockDto model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            _voteService.SetTime(model.Date);

            return RedirectToPage("/Index");
        }

        [HttpGet("CreateVote")]
        public async Task<IActionResult> CreateVote([FromQuery] string userName)
        {
            if (await _voteService.HasVotingEnded())
                return BadRequest("Voting Ended");

            ViewData["UserName"] = userName;
            return View();
        }

        [HttpPost("CreateVote")]
        public async Task<IActionResult> CreateVote([FromForm] List<string> strings, [FromQuery] string userName)
        {
            await _voteService.CreateVote(strings);

            return RedirectToAction("Vote", new { userName });
        }

        [HttpGet("Vote")]
        public async Task<IActionResult> Vote([FromQuery] string userName)
        {
            if (string.IsNullOrEmpty(userName))
                return BadRequest("Username is required.");

            if (await _voteService.HasVotingEnded())
                return RedirectToAction("ViewVote");

            ViewData["UserName"] = userName;
            ViewData["Locations"] = await _voteService.GetLocations();

            return View();
        }

        [HttpPost("Vote")]
        public async Task<IActionResult> Vote([FromForm] string location, [FromQuery] string userName)
        {
            if (string.IsNullOrEmpty(location) || string.IsNullOrEmpty(userName))
                return BadRequest("Location and username are required.");

            await _voteService.CastVote(userName, location);

            // Redirect to a confirmation page or back to the index
            return RedirectToAction("Index", new { userName });
        }

        [HttpGet("ViewVote")]
        public async Task<IActionResult> ViewVote()
        {
            if (!await _voteService.CanDisplayVote())
                return BadRequest("Voting has ended");

            var votes = await _voteService.GetVotes();
            ViewData["Votes"] = votes;
            return View();
        }
    }
}
