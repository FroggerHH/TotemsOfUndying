# TotemsOfUndying

Added **5** totems, one for each boss. To create it, you will need the grey Dwarr's eyes,
the Suktling cores and the boss totem.
They are created on a special altar, you will find it in the crafting tab.

Totems prevent your death and give different effects depending on the biome.
Each totem has its own characteristics, the main ones are its favorite biome,
additional biomes, bad biome and effects. When activated in your favorite biome, the totem will
give you a certain amount of health, stamina, a unique time bonus and possibly some additional
effects.

When activated in one of the additional biomes ,
you will not get a unique bonus, with a certain chance you will get additional effects and also get
a little less health and stamina than in the best biome. In a bad biome, the totem won't work.

Localized to 31 language, thanks to Google Translate.

### <ins>If something does not work for you, you have found any bugs, there are any suggestions, then be sure to write to me!</ins>

My discord ```justafrogger```
<img alt="Cat" height="30" src="https://media.tenor.com/bWUeVRqW9-IAAAAi/fast-cat-cat-excited.gif" align="center"/>

Special thanks for the models to ```MichaelB```, discord -
@anime9896, [artstation](https://www.artstation.com/forexample)

#### All totem characteristics are configurable in the mod configuration.

Config:

````
[item_TotemOfBonemass]

Crafting Station = Custom

Custom Crafting Station = Altar

Crafting Station Level = 1

Require only one resource = Off

Quality Multiplier = 1

Crafting Costs = GreydwarfEye:10,SurtlingCore:7,TrophyBonemass:1

Drops from = 

Weight = 1

Trader Value = 0

Trader Selling = None

Trader Price = 0

Trader Stack = 1

Trader Required Global Key = 

[item_TotemOfEikthyr]

Crafting Station = Custom

Custom Crafting Station = Altar

Crafting Station Level = 1

Require only one resource = Off

Quality Multiplier = 1

Crafting Costs = GreydwarfEye:10,SurtlingCore:7,TrophyEikthyr:1

Drops from = 

Weight = 1

Trader Value = 0

Trader Selling = None

Trader Price = 0

Trader Stack = 1

Trader Required Global Key = 

[item_TotemOfModer]

Crafting Station = Custom

Custom Crafting Station = Altar

Crafting Station Level = 1

Require only one resource = Off

Quality Multiplier = 1

Crafting Costs = GreydwarfEye:10,SurtlingCore:7,TrophyDragonQueen:1

Drops from = 

Weight = 1

Trader Value = 0

Trader Selling = None

Trader Price = 0

Trader Stack = 1

Trader Required Global Key = 

[item_TotemOfTheElder]

Crafting Station = Custom

Custom Crafting Station = Altar

Crafting Station Level = 1

Require only one resource = Off

Quality Multiplier = 1

Crafting Costs = GreydwarfEye:10,SurtlingCore:7,TrophyTheElder:1

Drops from = 

Weight = 1

Trader Value = 0

Trader Selling = None

Trader Price = 0

Trader Stack = 1

Trader Required Global Key = 

[item_TotemOfYagluth]

Crafting Station = Custom

Custom Crafting Station = Altar

Crafting Station Level = 1

Require only one resource = Off

Quality Multiplier = 1

Crafting Costs = GreydwarfEye:10,SurtlingCore:7,TrophyGoblinKing:1

Drops from = 

Weight = 1

Trader Value = 0

Trader Selling = None

Trader Price = 0

Trader Stack = 1

Trader Required Global Key = 

[piece_Altar]

Build Table Category = Crafting

Custom Build Category = 

Crafting Costs = Stone:20:True,SurtlingCore:10:True

[TotemOfBonemass]

# Setting type: Int32
# Default value: 5
# Acceptable value range: From 1 to 25
Max count in inventory = 5

# Setting type: Single
# Default value: 0
# Acceptable value range: From -5 to 5
Fall damage modifier applied by boss buff = 0

# Setting type: Single
# Default value: 0
# Acceptable value range: From -5 to 5
Damage modifier applied by boss buff = 0

# Setting type: Single
# Default value: 0.2
# Acceptable value range: From -2 to 2
Speed modifier applied by boss buff = 0.2

# Setting type: Int32
# Default value: 40
# Acceptable value range: From 0 to 1000
Health after dying in best biome = 40

# Setting type: Int32
# Default value: 15
# Acceptable value range: From 1 to 30
Boss buff time = 15

# Setting type: Int32
# Default value: 50
# Acceptable value range: From 0 to 1000
Health after dying in other biome = 50

# Setting type: Int32
# Default value: 130
# Acceptable value range: From 0 to 1000
Stamina after dying in best biome = 130

# Setting type: Int32
# Default value: 45
# Acceptable value range: From 0 to 1000
Stamina after dying in other biome = 45

## Related to healthBestBiome, healthWrongBiome, staminaBestBiome etc.
# Setting type: String
# Default value: Swamp
# Acceptable values: None, Meadows, Swamp, Mountain, BlackForest, Plains, AshLands, DeepNorth, Ocean, Mistlands
Best biome = Swamp

## Totem wouldn't work in this biome
# Setting type: String
# Default value: AshLands
# Acceptable values: None, Meadows, Swamp, Mountain, BlackForest, Plains, AshLands, DeepNorth, Ocean, Mistlands
Bad biome = AshLands

## The player will receive the same stats as when activated in the best biome multiplied by additionalBiomeStatsModifier
# Setting type: String
# Default value: 
Additional biomes = 

# Setting type: Boolean
# Default value: false
Work in all biomes = false

## Is it possible to transfer an item through the portal
# Setting type: Boolean
# Default value: true
Teleportable = true

## The effects that the player will receive when activating the totem of the best biome.
# Setting type: String
# Default value: 
Buffs = 

## The chance that the player will receive buffs when activated in an additional biome.
# Setting type: Int32
# Default value: 45
# Acceptable value range: From 0 to 100
Chance to activate buf in additional biome = 45

## When activating a totem in an additional biome, the player will receive the same characteristics as when activated in the best biome, but multiplied by this value.
# Setting type: Single
# Default value: 0.6
# Acceptable value range: From 0,05 to 5
Additional biome stats modifier = 0.6

[TotemOfEikthyr]

# Setting type: Int32
# Default value: 5
# Acceptable value range: From 1 to 25
Max count in inventory = 5

# Setting type: Single
# Default value: 0
# Acceptable value range: From -5 to 5
Fall damage modifier applied by boss buff = 0

# Setting type: Single
# Default value: 0
# Acceptable value range: From -5 to 5
Damage modifier applied by boss buff = 0

# Setting type: Single
# Default value: 0.1
# Acceptable value range: From -2 to 2
Speed modifier applied by boss buff = 0.1

# Setting type: Int32
# Default value: 10
# Acceptable value range: From 0 to 1000
Health after dying in best biome = 10

# Setting type: Int32
# Default value: 15
# Acceptable value range: From 1 to 30
Boss buff time = 15

# Setting type: Int32
# Default value: 10
# Acceptable value range: From 0 to 1000
Health after dying in other biome = 10

# Setting type: Int32
# Default value: 25
# Acceptable value range: From 0 to 1000
Stamina after dying in best biome = 25

# Setting type: Int32
# Default value: 25
# Acceptable value range: From 0 to 1000
Stamina after dying in other biome = 25

## Related to healthBestBiome, healthWrongBiome, staminaBestBiome etc.
# Setting type: String
# Default value: None
# Acceptable values: None, Meadows, Swamp, Mountain, BlackForest, Plains, AshLands, DeepNorth, Ocean, Mistlands
Best biome = None

## Totem wouldn't work in this biome
# Setting type: String
# Default value: None
# Acceptable values: None, Meadows, Swamp, Mountain, BlackForest, Plains, AshLands, DeepNorth, Ocean, Mistlands
Bad biome = None

## The player will receive the same stats as when activated in the best biome multiplied by additionalBiomeStatsModifier
# Setting type: String
# Default value: 
Additional biomes = 

# Setting type: Boolean
# Default value: true
Work in all biomes = true

## Is it possible to transfer an item through the portal
# Setting type: Boolean
# Default value: true
Teleportable = true

## The effects that the player will receive when activating the totem of the best biome.
# Setting type: String
# Default value: 
Buffs = 

## The chance that the player will receive buffs when activated in an additional biome.
# Setting type: Int32
# Default value: 45
# Acceptable value range: From 0 to 100
Chance to activate buf in additional biome = 45

## When activating a totem in an additional biome, the player will receive the same characteristics as when activated in the best biome, but multiplied by this value.
# Setting type: Single
# Default value: 0.6
# Acceptable value range: From 0,05 to 5
Additional biome stats modifier = 0.6

[TotemOfModer]

# Setting type: Int32
# Default value: 5
# Acceptable value range: From 1 to 25
Max count in inventory = 5

# Setting type: Single
# Default value: -5
# Acceptable value range: From -5 to 5
Fall damage modifier applied by boss buff = -5

# Setting type: Single
# Default value: 0
# Acceptable value range: From -5 to 5
Damage modifier applied by boss buff = 0

# Setting type: Single
# Default value: 0.11
# Acceptable value range: From -2 to 2
Speed modifier applied by boss buff = 0.11

# Setting type: Int32
# Default value: 101
# Acceptable value range: From 0 to 1000
Health after dying in best biome = 101

# Setting type: Int32
# Default value: 15
# Acceptable value range: From 1 to 30
Boss buff time = 15

# Setting type: Int32
# Default value: 30
# Acceptable value range: From 0 to 1000
Health after dying in other biome = 30

# Setting type: Int32
# Default value: 60
# Acceptable value range: From 0 to 1000
Stamina after dying in best biome = 60

# Setting type: Int32
# Default value: 50
# Acceptable value range: From 0 to 1000
Stamina after dying in other biome = 50

## Related to healthBestBiome, healthWrongBiome, staminaBestBiome etc.
# Setting type: String
# Default value: Mountain
# Acceptable values: None, Meadows, Swamp, Mountain, BlackForest, Plains, AshLands, DeepNorth, Ocean, Mistlands
Best biome = Mountain

## Totem wouldn't work in this biome
# Setting type: String
# Default value: None
# Acceptable values: None, Meadows, Swamp, Mountain, BlackForest, Plains, AshLands, DeepNorth, Ocean, Mistlands
Bad biome = None

## The player will receive the same stats as when activated in the best biome multiplied by additionalBiomeStatsModifier
# Setting type: String
# Default value: 
Additional biomes = 

# Setting type: Boolean
# Default value: false
Work in all biomes = false

## Is it possible to transfer an item through the portal
# Setting type: Boolean
# Default value: true
Teleportable = true

## The effects that the player will receive when activating the totem of the best biome.
# Setting type: String
# Default value: 
Buffs = 

## The chance that the player will receive buffs when activated in an additional biome.
# Setting type: Int32
# Default value: 45
# Acceptable value range: From 0 to 100
Chance to activate buf in additional biome = 45

## When activating a totem in an additional biome, the player will receive the same characteristics as when activated in the best biome, but multiplied by this value.
# Setting type: Single
# Default value: 0.6
# Acceptable value range: From 0,05 to 5
Additional biome stats modifier = 0.6

[TotemOfTheElder]

# Setting type: Int32
# Default value: 5
# Acceptable value range: From 1 to 25
Max count in inventory = 5

# Setting type: Single
# Default value: 0
# Acceptable value range: From -5 to 5
Fall damage modifier applied by boss buff = 0

# Setting type: Single
# Default value: 0.13
# Acceptable value range: From -5 to 5
Damage modifier applied by boss buff = 0.13

# Setting type: Single
# Default value: 0.1
# Acceptable value range: From -2 to 2
Speed modifier applied by boss buff = 0.1

# Setting type: Int32
# Default value: 50
# Acceptable value range: From 0 to 1000
Health after dying in best biome = 50

# Setting type: Int32
# Default value: 15
# Acceptable value range: From 1 to 30
Boss buff time = 15

# Setting type: Int32
# Default value: 20
# Acceptable value range: From 0 to 1000
Health after dying in other biome = 20

# Setting type: Int32
# Default value: 45
# Acceptable value range: From 0 to 1000
Stamina after dying in best biome = 45

# Setting type: Int32
# Default value: 20
# Acceptable value range: From 0 to 1000
Stamina after dying in other biome = 20

## Related to healthBestBiome, healthWrongBiome, staminaBestBiome etc.
# Setting type: String
# Default value: BlackForest
# Acceptable values: None, Meadows, Swamp, Mountain, BlackForest, Plains, AshLands, DeepNorth, Ocean, Mistlands
Best biome = BlackForest

## Totem wouldn't work in this biome
# Setting type: String
# Default value: AshLands
# Acceptable values: None, Meadows, Swamp, Mountain, BlackForest, Plains, AshLands, DeepNorth, Ocean, Mistlands
Bad biome = AshLands

## The player will receive the same stats as when activated in the best biome multiplied by additionalBiomeStatsModifier
# Setting type: String
# Default value: 
Additional biomes = 

# Setting type: Boolean
# Default value: false
Work in all biomes = false

## Is it possible to transfer an item through the portal
# Setting type: Boolean
# Default value: true
Teleportable = true

## The effects that the player will receive when activating the totem of the best biome.
# Setting type: String
# Default value: 
Buffs = 

## The chance that the player will receive buffs when activated in an additional biome.
# Setting type: Int32
# Default value: 45
# Acceptable value range: From 0 to 100
Chance to activate buf in additional biome = 45

## When activating a totem in an additional biome, the player will receive the same characteristics as when activated in the best biome, but multiplied by this value.
# Setting type: Single
# Default value: 0.6
# Acceptable value range: From 0,05 to 5
Additional biome stats modifier = 0.6

[TotemOfYagluth]

# Setting type: Int32
# Default value: 5
# Acceptable value range: From 1 to 25
Max count in inventory = 5

# Setting type: Single
# Default value: 0
# Acceptable value range: From -5 to 5
Fall damage modifier applied by boss buff = 0

# Setting type: Single
# Default value: 0.5
# Acceptable value range: From -5 to 5
Damage modifier applied by boss buff = 0.5

# Setting type: Single
# Default value: 0.5
# Acceptable value range: From -2 to 2
Speed modifier applied by boss buff = 0.5

# Setting type: Int32
# Default value: 150
# Acceptable value range: From 0 to 1000
Health after dying in best biome = 150

# Setting type: Int32
# Default value: 15
# Acceptable value range: From 1 to 30
Boss buff time = 15

# Setting type: Int32
# Default value: 80
# Acceptable value range: From 0 to 1000
Health after dying in other biome = 80

# Setting type: Int32
# Default value: 60
# Acceptable value range: From 0 to 1000
Stamina after dying in best biome = 60

# Setting type: Int32
# Default value: 50
# Acceptable value range: From 0 to 1000
Stamina after dying in other biome = 50

## Related to healthBestBiome, healthWrongBiome, staminaBestBiome etc.
# Setting type: String
# Default value: Plains
# Acceptable values: None, Meadows, Swamp, Mountain, BlackForest, Plains, AshLands, DeepNorth, Ocean, Mistlands
Best biome = Plains

## Totem wouldn't work in this biome
# Setting type: String
# Default value: Ocean
# Acceptable values: None, Meadows, Swamp, Mountain, BlackForest, Plains, AshLands, DeepNorth, Ocean, Mistlands
Bad biome = Ocean

## The player will receive the same stats as when activated in the best biome multiplied by additionalBiomeStatsModifier
# Setting type: String
# Default value: 
Additional biomes = 

# Setting type: Boolean
# Default value: false
Work in all biomes = false

## Is it possible to transfer an item through the portal
# Setting type: Boolean
# Default value: true
Teleportable = true

## The effects that the player will receive when activating the totem of the best biome.
# Setting type: String
# Default value: 
Buffs = 

## The chance that the player will receive buffs when activated in an additional biome.
# Setting type: Int32
# Default value: 45
# Acceptable value range: From 0 to 100
Chance to activate buf in additional biome = 45

## When activating a totem in an additional biome, the player will receive the same characteristics as when activated in the best biome, but multiplied by this value.
# Setting type: Single
# Default value: 0.6
# Acceptable value range: From 0,05 to 5
Additional biome stats modifier = 0.6



```


### <ins>If something does not work for you, you have found any bugs, there are any suggestions, then be sure to write to me!</ins>
 
Discord ```justafrogger```