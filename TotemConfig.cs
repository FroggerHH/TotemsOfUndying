using System;
using System.Collections.Generic;
using System.Linq;
using BepInEx.Configuration;
using ServerSync;
using UnityEngine;
using UnityEngine.Serialization;
using static Heightmap;
using static Heightmap.Biome;
using static TotemsOfUndying.Plugin;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

namespace TotemsOfUndying;

[Serializable]
public class TotemConfig
{
    public static readonly string[] AllBiomesStrings = new string[]
    {
        None.ToString(), Meadows.ToString(), Swamp.ToString(),
        Mountain.ToString(), BlackForest.ToString(), Plains.ToString(), AshLands.ToString(), DeepNorth
            .ToString(),
        Ocean.ToString(), Mistlands.ToString()
    };

    public string name { get; private set; }
    public Totem totem { get; private set; }

    internal ConfigEntry<float> fallDamageModifierConfig;
    internal ConfigEntry<float> speedModifierConfig;
    internal ConfigEntry<float> damageModifierConfig;
    internal ConfigEntry<int> bossBuffTtlConfig;
    internal ConfigEntry<int> healthRightBiomeConfig;
    internal ConfigEntry<int> healthWrongBiomeConfig;
    internal ConfigEntry<int> staminaRightBiomeConfig;
    internal ConfigEntry<int> staminaWrongBiomeConfig;

    internal ConfigEntry<string> mainBiomeConfig;
    internal ConfigEntry<string> badBiomeConfig;
    internal ConfigEntry<string> additionalBiomesConfig;
    internal ConfigEntry<bool> allBiomesConfig;
    internal ConfigEntry<string> buffsConfig;
    internal ConfigEntry<int> chanceToActivateBuffInAdditionalBiomeConfig;
    internal ConfigEntry<float> additionalBiomeStatsModifierConfig;

    public float fallDamageModifier = 0;
    public float damageModifier = 0;
    public float speedModifier = 0;
    public int bossBuffTtl = 15;
    public int healthRightBiome = 1;
    public int healthWrongBiome = 1;
    public int staminaRightBiome = 1;
    public int staminaWrongBiome = 1;
    public Biome bestBiome;
    public Biome badBiome;
    public List<Biome> aditionalBiomes = new();
    public bool allBiomes = false;
    public GameObject gameObject;
    public ItemDrop itemDrop;
    public GameObject fx;
    public List<string> buffs = new();
    public int chanceToActivateBufInAdditionalBiome = 45;
    public float additionalBiomeStatsModifier = 0.6f;

    public TotemConfig(Totem totem, string name)
    {
        this.name = name;
        this.totem = totem;
        gameObject = bundle.LoadAsset<GameObject>(name);
        itemDrop = gameObject?.GetComponent<ItemDrop>();
        fx = bundle.LoadAsset<GameObject>($"fx_{name.Replace("TotemOf", string.Empty)}");
    }

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

        if (!Biome.TryParse(badBiomeConfig.Value, out badBiome))
            DebugError($"{badBiomeConfig.Value} is not a valid biome");

        if (!Biome.TryParse(mainBiomeConfig.Value, out bestBiome))
            DebugError($"{mainBiomeConfig.Value} is not a valid biome");
        aditionalBiomes = additionalBiomesConfig.Value.Split(new string[] { ", " }, StringSplitOptions
            .RemoveEmptyEntries).Select(x =>
        {
            if (Enum.TryParse<Biome>(x, out var biome)) return biome;
            else
            {
                DebugError($"{x} is not a biome.");
                return Biome.None;
            }
        }).ToList();

        allBiomes = allBiomesConfig.Value;
        buffs = buffsConfig.Value.Split(new string[] { ", " }, StringSplitOptions
            .RemoveEmptyEntries).ToList();

        chanceToActivateBufInAdditionalBiome = chanceToActivateBuffInAdditionalBiomeConfig.Value;
        additionalBiomeStatsModifier = additionalBiomeStatsModifierConfig.Value;

        var effect = ObjectDB.instance.GetStatusEffect(totem.bossBuff.GetStableHashCode()) as SE_Stats;
        effect.m_ttl = bossBuffTtl;
        effect.m_speedModifier = speedModifier;
        effect.m_damageModifier = damageModifier;
        effect.m_fallDamageModifier = fallDamageModifier;
    }

    public void Bind()
    {
        fallDamageModifierConfig = config(name, "Fall damage modifier applied by boss buff", fallDamageModifier, new("",
            new AcceptableValueRange<float>(-5, 5), new ConfigurationManagerAttributes() { }));

        damageModifierConfig = config(name, "Damage modifier applied by boss buff", damageModifier, new("",
            new AcceptableValueRange<float>(-5, 5), new ConfigurationManagerAttributes() { }));

        speedModifierConfig = config(name, "Speed modifier applied by boss buff", speedModifier, new("",
            new AcceptableValueRange<float>(-2, 2), new ConfigurationManagerAttributes() { }));

        healthRightBiomeConfig = config(name, "Health after dying in best biome", healthRightBiome, new("",
            new AcceptableValueRange<int>(0, 1000), new ConfigurationManagerAttributes() { }));

        bossBuffTtlConfig = config(name, "Boss buff time", bossBuffTtl, new("",
            new AcceptableValueRange<int>(1, 30), new ConfigurationManagerAttributes() { }));

        healthWrongBiomeConfig = config(name, "Health after dying in other biome", healthWrongBiome, new("",
            new AcceptableValueRange<int>(0, 1000), new ConfigurationManagerAttributes() { }));

        staminaRightBiomeConfig = config(name, "Stamina after dying in best biome", staminaRightBiome, new("",
            new AcceptableValueRange<int>(0, 1000), new ConfigurationManagerAttributes() { }));

        staminaWrongBiomeConfig = config(name, "Stamina after dying in other biome", staminaWrongBiome, new("",
            new AcceptableValueRange<int>(0, 1000), new ConfigurationManagerAttributes() { }));

        mainBiomeConfig = config(name, "Best biome", bestBiome.ToString(), 
            new("Related to healthBestBiome, healthWrongBiome, staminaBestBiome etc.",
                new AcceptableValueList<string>(AllBiomesStrings)));

        badBiomeConfig = config(name, "Bad biome", badBiome.ToString(), new("Totem wouldn't work in this biome",
            new AcceptableValueList<string>(AllBiomesStrings)));

        additionalBiomesConfig = config(name, "Additional biomes", "",
            new(
                "The player will receive the same stats as when activated in the best biome multiplied by additionalBiomeStatsModifier"));

        allBiomesConfig = config(name, "Work in all biomes", allBiomes, new(""));

        buffsConfig = config(name, "Buffs", "",
            new("The effects that the player will receive when activating the totem of the best biome."));

        chanceToActivateBuffInAdditionalBiomeConfig = config(name, "Chance to activate buf in additional biome",
            chanceToActivateBufInAdditionalBiome,
            new(
                "The chance that the player will receive buffs when activated in an additional biome.",
                new AcceptableValueRange<int>(0, 100), new ConfigurationManagerAttributes() { }));

        additionalBiomeStatsModifierConfig = config(name, "Additional biome stats modifier",
            additionalBiomeStatsModifier,
            new(
                "When activating a totem in an additional biome, the player will receive the same characteristics as when activated in the best biome, but multiplied by this value.",
                new AcceptableValueRange<float>(0.05f, 5f), new ConfigurationManagerAttributes() { }));
    }
}