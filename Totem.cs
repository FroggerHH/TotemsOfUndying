using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static Heightmap;
using static TotemsOfUndying.Plugin;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

namespace TotemsOfUndying;

public class Totem
{
    internal TotemConfig config { get; private set; }
    public Totem(string name) { config = new(this, name); }

    public string bossBuff => $"SE_{config.name.Replace("TotemOf", string.Empty)}";

    public static Totem CreateInstance(string name)
    {
        var totem = new Totem(name);
        return totem;
    }

    public void Use(ItemDrop.ItemData itemData, Heightmap.Biome biome)
    {
        Inventory inventory = Player.m_localPlayer.GetInventory();
        MessageHud.instance.ShowMessage(MessageHud.MessageType.Center, config.name, 0, null);
        if (biome == config.bestBiome || config.allBiomes)
        {
            Player.m_localPlayer.SetHealth(config.healthRightBiome);
            Player.m_localPlayer.AddStamina(config.staminaRightBiome);
            config.buffs.ForEach(x =>
                Player.m_localPlayer.m_seman.AddStatusEffect(x.GetStableHashCode(), true));
            Player.m_localPlayer.m_seman.AddStatusEffect(bossBuff.GetStableHashCode(), true);
        } else if (config.aditionalBiomes.Contains(biome))
        {
            Player.m_localPlayer.SetHealth(config.healthRightBiome * config.additionalBiomeStatsModifier);
            Player.m_localPlayer.AddStamina(config.staminaRightBiome * config.additionalBiomeStatsModifier);
            if (Random.value <= config.chanceToActivateBufInAdditionalBiome / 100 && config.buffs.Count > 0)
                config.buffs.ForEach(x =>
                    Player.m_localPlayer.m_seman.AddStatusEffect(x.GetStableHashCode(), true));
        } else if (biome != config.badBiome)
        {
            Player.m_localPlayer.SetHealth(config.healthWrongBiome);
            Player.m_localPlayer.AddStamina(config.staminaWrongBiome);
        }

        Object.Instantiate<GameObject>(config.fx,
            new Vector3(Player.m_localPlayer.transform.position.x, Player.m_localPlayer.transform.position.y + 1.4f,
                Player.m_localPlayer.transform.position.z), Quaternion.identity);
        inventory.RemoveItem(itemData, 1);
    }

    public SE_Stats GetSE() => ObjectDB.instance.GetStatusEffect(bossBuff.GetStableHashCode()) as SE_Stats;
}