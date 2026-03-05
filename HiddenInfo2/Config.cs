using System.Text.Json.Serialization;

namespace HiddenInfo2;

public class Config {
    // Cards
    [JsonPropertyName("card.names")]
    public bool CardNames { get; set; }

    [JsonPropertyName("card.descriptions")]
    public bool CardDescriptions { get; set; }
    
    [JsonPropertyName("card.costs")]
    public bool CardCosts { get; set; }
    
    [JsonPropertyName("card.rarity")]
    public bool CardRarity { get; set; }
    
    [JsonPropertyName("card.type")]
    public bool CardType { get; set; }

    // Relics
    [JsonPropertyName("relic.names")]
    public bool RelicNames { get; set; }

    [JsonPropertyName("relic.descriptions")]
    public bool RelicDescriptions { get; set; }

    [JsonPropertyName("relic.flavor")]
    public bool RelicFlavor { get; set; }

    // Potions
    [JsonPropertyName("potion.names")]
    public bool PotionNames { get; set; }

    [JsonPropertyName("potion.descriptions")]
    public bool PotionDescriptions { get; set; }
}
