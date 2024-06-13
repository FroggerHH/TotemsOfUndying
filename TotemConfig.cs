using BepInEx.Configuration;
using UnityEngine.Serialization;

namespace TotemsOfUndying;

[Serializable]
public class TotemConfig
{
    public static readonly string[] AllBiomesStrings =
    [
        None.ToString(), Meadows.ToString(), Swamp.ToString(),
        Mountain.ToString(), BlackForest.ToString(), Plains.ToString(), AshLands.ToString(), DeepNorth
            .ToString(),
        Ocean.ToString(), Mistlands.ToString()
    ];

    public float fallDamageModifier;
    public float damageModifier;
    public float speedModifier;
    public int bossBuffTtl = 15;
    public int healthRightBiome = 1;
    public int healthWrongBiome = 1;
    public int staminaRightBiome = 1;
    public int staminaWrongBiome = 1;
    public int eitrRightBiome = 1;
    public int eitrWrongBiome = 1;
    public Biome bestBiome;
    public Biome badBiome;
    public List<Biome> additionalBiomes = [];
    public bool allBiomes;
    public bool teleportable = true;
    public int maxCountInInventory = 5;

    public List<string> buffs = [];
    public int chanceToActivateBufInAdditionalBiome = 45;
    public float additionalBiomeStatsModifier = 0.6f;

    public GameObject gameObject;
    public ItemDrop itemDrop;
    public GameObject fx;
    internal ConfigEntry<string> additionalBiomesConfig;
    internal ConfigEntry<float> additionalBiomeStatsModifierConfig;
    internal ConfigEntry<bool> allBiomesConfig;
    internal ConfigEntry<string> badBiomeConfig;
    internal ConfigEntry<int> bossBuffTtlConfig;
    internal ConfigEntry<string> buffsConfig;
    internal ConfigEntry<int> chanceToActivateBuffInAdditionalBiomeConfig;
    internal ConfigEntry<float> damageModifierConfig;

    internal ConfigEntry<float> fallDamageModifierConfig;
    internal ConfigEntry<int> healthRightBiomeConfig;
    internal ConfigEntry<int> healthWrongBiomeConfig;

    internal ConfigEntry<string> mainBiomeConfig;
    internal ConfigEntry<int> maxCountInInventoryConfig;
    internal ConfigEntry<float> speedModifierConfig;
    internal ConfigEntry<int> staminaRightBiomeConfig;
    internal ConfigEntry<int> staminaWrongBiomeConfig;
    internal ConfigEntry<int> eitrRightBiomeConfig;
    internal ConfigEntry<int> eitrWrongBiomeConfig;
    internal ConfigEntry<bool> teleportableConfig;


    public TotemConfig(Totem totem, string name)
    {
        this.Name = name;
        this.Totem = totem;
        gameObject = bundle.LoadAsset<GameObject>(name);
        itemDrop = gameObject.GetComponent<ItemDrop>();
        fx = bundle.LoadAsset<GameObject>($"fx_{name.Replace("TotemOf", string.Empty)}");
    }

    public string Name { get; private set; }
    public Totem Totem { get; private set; }

    public void UpdateValues()
    {
        fallDamageModifier = fallDamageModifierConfig.Value;
        damageModifier = damageModifierConfig.Value;
        speedModifier = speedModifierConfig.Value;
        bossBuffTtl = bossBuffTtlConfig.Value;
        healthRightBiome = healthRightBiomeConfig.Value;
        healthWrongBiome = healthWrongBiomeConfig.Value;
        staminaRightBiome = staminaRightBiomeConfig.Value;
        staminaWrongBiome = staminaWrongBiomeConfig.Value;
        eitrRightBiome = eitrRightBiomeConfig.Value;
        eitrWrongBiome = eitrWrongBiomeConfig.Value;
        maxCountInInventory = maxCountInInventoryConfig.Value;
        allBiomes = allBiomesConfig.Value;
        teleportable = teleportableConfig.Value;
        chanceToActivateBufInAdditionalBiome = chanceToActivateBuffInAdditionalBiomeConfig.Value;
        additionalBiomeStatsModifier = additionalBiomeStatsModifierConfig.Value;

        if (!Enum.TryParse(badBiomeConfig.Value, out badBiome))
            DebugError($"{badBiomeConfig.Value} is not a valid biome");

        if (!Enum.TryParse(mainBiomeConfig.Value, out bestBiome))
            DebugError($"{mainBiomeConfig.Value} is not a valid biome");
        additionalBiomes = additionalBiomesConfig.Value.Split([", "], StringSplitOptions
            .RemoveEmptyEntries).Select(x =>
        {
            if (Enum.TryParse<Biome>(x, out var biome)) return biome;

            DebugError($"{x} is not a biome.");
            return None;
        }).ToList();

        buffs = buffsConfig.Value.Split([", "], StringSplitOptions
            .RemoveEmptyEntries).ToList();
        if (ObjectDB.instance)
        {
            var effect = ObjectDB.instance.GetStatusEffect(Totem.bossBuff.GetStableHashCode()) as SE_Stats;
            if (effect)
            {
                effect.m_ttl = bossBuffTtl;
                effect.m_speedModifier = speedModifier;
                effect.m_damageModifier = damageModifier;
                effect.m_fallDamageModifier = fallDamageModifier;
            }
        }

        itemDrop.m_itemData.m_shared.m_teleportable = teleportable;
        if (!Player.m_localPlayer) return;

        var inventory = Player.m_localPlayer.GetInventory();
        if (inventory == null) return;
        foreach (var item in inventory.GetAllItems().Where(x => x.m_shared.m_name == itemDrop.m_itemData
                     .m_shared.m_name))
            item.m_shared.m_teleportable = teleportable;

        inventory.UpdateTotalWeight();
        InventoryGui.instance.UpdateInventory(Player.m_localPlayer);
    }

    public void Bind()
    {
        maxCountInInventoryConfig = config(Name, "Max count in inventory", maxCountInInventory,
            new ConfigDescription("",
                new AcceptableValueRange<int>(1, 25), new ConfigurationManagerAttributes()));

        fallDamageModifierConfig = config(Name, "Fall damage modifier applied by boss buff", fallDamageModifier,
            new ConfigDescription("",
                new AcceptableValueRange<float>(-5, 5), new ConfigurationManagerAttributes()));

        damageModifierConfig = config(Name, "Damage modifier applied by boss buff", damageModifier,
            new ConfigDescription("",
                new AcceptableValueRange<float>(-5, 5), new ConfigurationManagerAttributes()));

        speedModifierConfig = config(Name, "Speed modifier applied by boss buff", speedModifier,
            new ConfigDescription("",
                new AcceptableValueRange<float>(-2, 2), new ConfigurationManagerAttributes()));

        healthRightBiomeConfig = config(Name, "Health after dying in best biome", healthRightBiome,
            new ConfigDescription("",
                new AcceptableValueRange<int>(0, 1000), new ConfigurationManagerAttributes()));

        bossBuffTtlConfig = config(Name, "Boss buff time", bossBuffTtl, new ConfigDescription("",
            new AcceptableValueRange<int>(1, 30), new ConfigurationManagerAttributes()));

        healthWrongBiomeConfig = config(Name, "Health after dying in other biome", healthWrongBiome,
            new ConfigDescription("",
                new AcceptableValueRange<int>(0, 1000), new ConfigurationManagerAttributes()));

        staminaRightBiomeConfig = config(Name, "Stamina after dying in best biome", staminaRightBiome,
            new ConfigDescription("",
                new AcceptableValueRange<int>(0, 1000), new ConfigurationManagerAttributes()));

        staminaWrongBiomeConfig = config(Name, "Stamina after dying in other biome", staminaWrongBiome,
            new ConfigDescription("",
                new AcceptableValueRange<int>(0, 1000), new ConfigurationManagerAttributes()));

        mainBiomeConfig = config(Name, "Best biome", bestBiome.ToString(),
            new ConfigDescription("Related to healthBestBiome, healthWrongBiome, staminaBestBiome etc.",
                new AcceptableValueList<string>(AllBiomesStrings)));

        badBiomeConfig = config(Name, "Bad biome", badBiome.ToString(), new ConfigDescription(
            "Totem wouldn't work in this biome",
            new AcceptableValueList<string>(AllBiomesStrings)));

        additionalBiomesConfig = config(Name, "Additional biomes", "",
            new ConfigDescription(
                "The player will receive the same stats as when activated in the best biome multiplied by additionalBiomeStatsModifier"));

        allBiomesConfig = config(Name, "Work in all biomes", allBiomes, new ConfigDescription(""));
        teleportableConfig = config(Name, "Teleportable", teleportable,
            new ConfigDescription("Is it possible to transfer an item through the portal"));

        buffsConfig = config(Name, "Buffs", "",
            new ConfigDescription(
                "The effects that the player will receive when activating the totem of the best biome."));

        chanceToActivateBuffInAdditionalBiomeConfig = config(Name, "Chance to activate buf in additional biome",
            chanceToActivateBufInAdditionalBiome,
            new ConfigDescription(
                "The chance that the player will receive buffs when activated in an additional biome.",
                new AcceptableValueRange<int>(0, 100), new ConfigurationManagerAttributes()));

        additionalBiomeStatsModifierConfig = config(Name, "Additional biome stats modifier",
            additionalBiomeStatsModifier,
            new ConfigDescription(
                "When activating a totem in an additional biome, the player will receive the same characteristics as when activated in the best biome, but multiplied by this value.",
                new AcceptableValueRange<float>(0.05f, 5f), new ConfigurationManagerAttributes()));
    }
}