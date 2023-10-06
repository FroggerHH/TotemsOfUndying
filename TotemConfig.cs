using BepInEx.Configuration;

namespace TotemsOfUndying;

[Serializable]
public class TotemConfig
{
    public static readonly string[] AllBiomesStrings =
    {
        None.ToString(), Meadows.ToString(), Swamp.ToString(),
        Mountain.ToString(), BlackForest.ToString(), Plains.ToString(), AshLands.ToString(), DeepNorth
            .ToString(),
        Ocean.ToString(), Mistlands.ToString()
    };

    public float fallDamageModifier;
    public float damageModifier;
    public float speedModifier;
    public int bossBuffTtl = 15;
    public int healthRightBiome = 1;
    public int healthWrongBiome = 1;
    public int staminaRightBiome = 1;
    public int staminaWrongBiome = 1;
    public Biome bestBiome;
    public Biome badBiome;
    public List<Biome> aditionalBiomes = new();
    public bool allBiomes;
    public bool teleportable = true;
    public int maxCountInInventory = 5;

    public List<string> buffs = new();
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
    internal ConfigEntry<bool> teleportableConfig;

    public TotemConfig(Totem totem, string name)
    {
        this.name = name;
        this.totem = totem;
        gameObject = bundle.LoadAsset<GameObject>(name);
        itemDrop = gameObject.GetComponent<ItemDrop>();
        fx = bundle.LoadAsset<GameObject>($"fx_{name.Replace("TotemOf", string.Empty)}");
    }

    public string name { get; private set; }
    public Totem totem { get; private set; }

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
        maxCountInInventory = maxCountInInventoryConfig.Value;
        allBiomes = allBiomesConfig.Value;
        teleportable = teleportableConfig.Value;
        chanceToActivateBufInAdditionalBiome = chanceToActivateBuffInAdditionalBiomeConfig.Value;
        additionalBiomeStatsModifier = additionalBiomeStatsModifierConfig.Value;

        if (!Enum.TryParse(badBiomeConfig.Value, out badBiome))
            DebugError($"{badBiomeConfig.Value} is not a valid biome");

        if (!Enum.TryParse(mainBiomeConfig.Value, out bestBiome))
            DebugError($"{mainBiomeConfig.Value} is not a valid biome");
        aditionalBiomes = additionalBiomesConfig.Value.Split(new[] { ", " }, StringSplitOptions
            .RemoveEmptyEntries).Select(x =>
        {
            if (Enum.TryParse<Biome>(x, out var biome)) return biome;

            DebugError($"{x} is not a biome.");
            return None;
        }).ToList();

        buffs = buffsConfig.Value.Split(new[] { ", " }, StringSplitOptions
            .RemoveEmptyEntries).ToList();
        if (ObjectDB.instance)
        {
            var effect = ObjectDB.instance.GetStatusEffect(totem.bossBuff.GetStableHashCode()) as SE_Stats;
            if (effect)
            {
                effect.m_ttl = bossBuffTtl;
                effect.m_speedModifier = speedModifier;
                effect.m_damageModifier = damageModifier;
                effect.m_fallDamageModifier = fallDamageModifier;
            }
        }

        itemDrop.m_itemData.m_shared.m_teleportable = teleportable;
        if (Player.m_localPlayer)
        {
            var inventory = Player.m_localPlayer.GetInventory();
            if (inventory != null)
            {
                foreach (var item in inventory.GetAllItems().Where(x => x.m_shared.m_name == itemDrop.m_itemData
                             .m_shared.m_name))
                    item.m_shared.m_teleportable = teleportable;

                inventory.UpdateTotalWeight();
                InventoryGui.instance.UpdateInventory(Player.m_localPlayer);
            }
        }
    }

    public void Bind()
    {
        maxCountInInventoryConfig = mod.config(name, "Max count in inventory", maxCountInInventory,
            new ConfigDescription("",
                new AcceptableValueRange<int>(1, 25), new ConfigurationManagerAttributes()));

        fallDamageModifierConfig = mod.config(name, "Fall damage modifier applied by boss buff", fallDamageModifier,
            new ConfigDescription("",
                new AcceptableValueRange<float>(-5, 5), new ConfigurationManagerAttributes()));

        damageModifierConfig = mod.config(name, "Damage modifier applied by boss buff", damageModifier,
            new ConfigDescription("",
                new AcceptableValueRange<float>(-5, 5), new ConfigurationManagerAttributes()));

        speedModifierConfig = mod.config(name, "Speed modifier applied by boss buff", speedModifier,
            new ConfigDescription("",
                new AcceptableValueRange<float>(-2, 2), new ConfigurationManagerAttributes()));

        healthRightBiomeConfig = mod.config(name, "Health after dying in best biome", healthRightBiome,
            new ConfigDescription("",
                new AcceptableValueRange<int>(0, 1000), new ConfigurationManagerAttributes()));

        bossBuffTtlConfig = mod.config(name, "Boss buff time", bossBuffTtl, new ConfigDescription("",
            new AcceptableValueRange<int>(1, 30), new ConfigurationManagerAttributes()));

        healthWrongBiomeConfig = mod.config(name, "Health after dying in other biome", healthWrongBiome,
            new ConfigDescription("",
                new AcceptableValueRange<int>(0, 1000), new ConfigurationManagerAttributes()));

        staminaRightBiomeConfig = mod.config(name, "Stamina after dying in best biome", staminaRightBiome,
            new ConfigDescription("",
                new AcceptableValueRange<int>(0, 1000), new ConfigurationManagerAttributes()));

        staminaWrongBiomeConfig = mod.config(name, "Stamina after dying in other biome", staminaWrongBiome,
            new ConfigDescription("",
                new AcceptableValueRange<int>(0, 1000), new ConfigurationManagerAttributes()));

        mainBiomeConfig = mod.config(name, "Best biome", bestBiome.ToString(),
            new ConfigDescription("Related to healthBestBiome, healthWrongBiome, staminaBestBiome etc.",
                new AcceptableValueList<string>(AllBiomesStrings)));

        badBiomeConfig = mod.config(name, "Bad biome", badBiome.ToString(), new ConfigDescription(
            "Totem wouldn't work in this biome",
            new AcceptableValueList<string>(AllBiomesStrings)));

        additionalBiomesConfig = mod.config(name, "Additional biomes", "",
            new ConfigDescription(
                "The player will receive the same stats as when activated in the best biome multiplied by additionalBiomeStatsModifier"));

        allBiomesConfig = mod.config(name, "Work in all biomes", allBiomes, new ConfigDescription(""));
        teleportableConfig = mod.config(name, "Teleportable", teleportable,
            new ConfigDescription("Is it possible to transfer an item through the portal"));

        buffsConfig = mod.config(name, "Buffs", "",
            new ConfigDescription(
                "The effects that the player will receive when activating the totem of the best biome."));

        chanceToActivateBuffInAdditionalBiomeConfig = mod.config(name, "Chance to activate buf in additional biome",
            chanceToActivateBufInAdditionalBiome,
            new ConfigDescription(
                "The chance that the player will receive buffs when activated in an additional biome.",
                new AcceptableValueRange<int>(0, 100), new ConfigurationManagerAttributes()));

        additionalBiomeStatsModifierConfig = mod.config(name, "Additional biome stats modifier",
            additionalBiomeStatsModifier,
            new ConfigDescription(
                "When activating a totem in an additional biome, the player will receive the same characteristics as when activated in the best biome, but multiplied by this value.",
                new AcceptableValueRange<float>(0.05f, 5f), new ConfigurationManagerAttributes()));
    }
}