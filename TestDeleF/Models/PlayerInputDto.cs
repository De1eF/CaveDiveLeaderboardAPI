using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace TestDeleF.Models
{
    public class PlayerInputDto(string name, long highscore)
    {
        public string Name { get; set; } = name;

        public long Highscore { get; set; } = highscore;

        public PlayerDto ToPlayerDto()
        {
            return new PlayerDto("", Name , Highscore, 0);
        }
    }
}
