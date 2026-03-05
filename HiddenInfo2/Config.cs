using System.Text.Json.Serialization;

namespace HiddenInfo2;

public class Config {
    // Cards
    [JsonPropertyName("card.name")]
    public bool CardName { get; set; }

    [JsonPropertyName("card.description")]
    public bool CardDescription { get; set; }
    
    [JsonPropertyName("card.cost")]
    public bool CardCost { get; set; }
    
    [JsonPropertyName("card.rarity")]
    public bool CardRarity { get; set; }
    
    [JsonPropertyName("card.type")]
    public bool CardType { get; set; }

    // Relics
    [JsonPropertyName("relic.name")]
    public bool RelicName { get; set; }

    [JsonPropertyName("relic.description")]
    public bool RelicDescription { get; set; }

    [JsonPropertyName("relic.flavor")]
    public bool RelicFlavor { get; set; }

    // Potions
    [JsonPropertyName("potion.name")]
    public bool PotionName { get; set; }

    [JsonPropertyName("potion.description")]
    public bool PotionDescription { get; set; }
}
