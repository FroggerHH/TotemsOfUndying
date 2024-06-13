namespace TotemsOfUndying;

public class Totem
{
    public Totem(string name) { config = new TotemConfig(this, name); }
    internal TotemConfig config { get; }

    public string bossBuff => $"SE_{config.Name.Replace("TotemOf", string.Empty)}";

    public static Totem CreateInstance(string name)
    {
        var totem = new Totem(name);
        return totem;
    }

    public void Use(ItemData itemData, Biome biome)
    {
        var inventory = Player.m_localPlayer.GetInventory();
        MessageHud.instance.ShowMessage(MessageHud.MessageType.Center, config.Name);
        if (biome == config.bestBiome || config.allBiomes)
        {
            Player.m_localPlayer.SetHealth(config.healthRightBiome);
            Player.m_localPlayer.AddStamina(config.staminaRightBiome);
            Player.m_localPlayer.AddEitr(config.eitrRightBiome);
            config.buffs.ForEach(x =>
                Player.m_localPlayer.m_seman.AddStatusEffect(x.GetStableHashCode(), true));
            Player.m_localPlayer.m_seman.AddStatusEffect(bossBuff.GetStableHashCode(), true);
        }
        else if (config.additionalBiomes.Contains(biome))
        {
            Player.m_localPlayer.SetHealth(config.healthRightBiome * config.additionalBiomeStatsModifier);
            Player.m_localPlayer.AddStamina(config.staminaRightBiome * config.additionalBiomeStatsModifier);
            Player.m_localPlayer.AddEitr(config.eitrRightBiome * config.additionalBiomeStatsModifier);
            if (Random.value <= config.chanceToActivateBufInAdditionalBiome / 100 && config.buffs.Count > 0)
                config.buffs.ForEach(x =>
                    Player.m_localPlayer.m_seman.AddStatusEffect(x.GetStableHashCode(), true));
        } else if (biome != config.badBiome)
        {
            Player.m_localPlayer.SetHealth(config.healthWrongBiome);
            Player.m_localPlayer.AddStamina(config.staminaWrongBiome);
            Player.m_localPlayer.AddEitr(config.eitrWrongBiome);
        }

        Instantiate(config.fx,
            new Vector3(Player.m_localPlayer.transform.position.x, Player.m_localPlayer.transform.position.y + 1.4f,
                Player.m_localPlayer.transform.position.z), Quaternion.identity);
        inventory.RemoveItem(itemData, 1);
    }

    public SE_Stats GetSE() { return ObjectDB.instance.GetStatusEffect(bossBuff.GetStableHashCode()) as SE_Stats; }
}