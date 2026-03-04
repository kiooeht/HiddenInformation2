using System.Text.Json.Serialization;

namespace HiddenInfo2;

public class Config {
    [JsonPropertyName("card.names")]
    public bool CardNames { get; set; }

    [JsonPropertyName("card.descriptions")]
    public bool CardDescriptions { get; set; }
}
