using System.Text.Json.Serialization;

namespace HiddenInfo2;

public class Config {
    [JsonPropertyName("card.names")]
    public bool CardNames { get; set; }

    [JsonPropertyName("card.descriptions")]
    public bool CardDescriptions { get; set; }
    
    [JsonPropertyName("card.costs")]
    public bool CardCosts { get; set; }

    [JsonPropertyName("relic.names")]
    public bool RelicNames { get; set; }

    [JsonPropertyName("relic.descriptions")]
    public bool RelicDescriptions { get; set; }
}
