using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using TestDeleF.Models;
using TestDeleF.Services;

namespace TestDeleF.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PlayersController : ControllerBase
    {
        private readonly PlayerService _playerService;

        static readonly Random random = new();

        private static readonly long DELETE_AFTER_MILIS = 60000; //31556952000;

        public PlayersController(PlayerService playerService) =>
       _playerService = playerService;

        [HttpGet("{id:length(24)}")]
        public async Task<ActionResult<PlayerDto>> GetPlayer(string id)
        {
            var player = await _playerService.GetAsync(id);

            if (player is null)
            {
                return NotFound();
            }

            return player;
        }

        [HttpPost]
        public async Task<IActionResult> PostPlayer(PlayerInputDto playerInput)
        {
            string generatedId = GetUniqueIdForPlayer();

            PlayerDto newPlayerDto = playerInput.ToPlayerDto();

            SetExpieryDate(newPlayerDto);
            newPlayerDto.Id = generatedId;

            await _playerService.CreateAsync(newPlayerDto);

            return CreatedAtAction(nameof(GetPlayer), new { id = newPlayerDto.Id }, newPlayerDto);
        }

        [HttpPut("{id:length(24)}")]
        public async Task<IActionResult> Update(string id, PlayerInputDto playerInput)
        {
            PlayerDto updatePlayerDto = playerInput.ToPlayerDto();
            var player = await _playerService.GetAsync(id);

            if (player is null)
            {
                return NotFound();
            }

            SetExpieryDate(updatePlayerDto);
            updatePlayerDto.Id = player.Id;

            await _playerService.UpdateAsync(id, updatePlayerDto);

            return NoContent();
        }

        private string GetUniqueIdForPlayer()
        {
            string generatedId = GetRandomHexNumber(24);

            if (GetPlayer(generatedId).Result.Value != null)
            {
                GetUniqueIdForPlayer();
            }

            return generatedId;
        }

        public static string GetRandomHexNumber(int digits)
        {
            byte[] buffer = new byte[digits / 2];
            random.NextBytes(buffer);
            string result = String.Concat(buffer.Select(x => x.ToString("X2")).ToArray());
            if (digits % 2 == 0)
                return result;
            return result + random.Next(16).ToString("X");
        }

        private PlayerDto SetExpieryDate(PlayerDto player)
        {
            player.ExpieryTime = DateTimeOffset.Now.ToUnixTimeMilliseconds() + DELETE_AFTER_MILIS;
            return player;
        }
    }
}
