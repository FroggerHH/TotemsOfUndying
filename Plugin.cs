using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using BepInEx;
using BepInEx.Configuration;
using Extensions;
using HarmonyLib;
using ItemManager;
using PieceManager;
using ServerSync;
using UnityEngine;
using StatusEffectManager;
using LocalizationManager;
using static Heightmap;
using static Heightmap.Biome;

namespace TotemsOfUndying;

[BepInPlugin(ModGUID, ModName, ModVersion)]
internal class Plugin : BaseUnityPlugin
{
    #region values

    internal const string ModName = "Frogger.TotemsOfUndying", ModVersion = "1.1.0", ModGUID = "com." + ModName;

    internal static Plugin _self;

    #endregion

    internal static Totem EikthyrTotem;
    internal static Totem TheElderTotem;
    internal static Totem BonemassTotem;
    internal static Totem ModerTotem;
    internal static Totem YagluthTotem;
    internal static AssetBundle bundle;

    private void Awake()
    {
        _self = this;

        Config.SaveOnConfigSet = false;
        relocateIntervalConfig = config("Merchant", "RelocateInterval", relocateInterval,
            "Number of days before merchant relocates. Sit to 0 to disable relocation.");

        SetupWatcher();
        Config.ConfigReloaded += (_, _) => UpdateConfiguration();
        Config.SaveOnConfigSet = true;
        Config.Save();

        Harmony.CreateAndPatchAll(Assembly.GetExecutingAssembly(), ModGUID);
        bundle = PrefabManager.RegisterAssetBundle("totems");

        #region Totems

        Item totemOfEikthyr = new Item(bundle, "TotemOfEikthyr");
        totemOfEikthyr.Name
            .English("Totem Of Eikthyr")
            .Swedish("Totem av Eikthyr")
            .French("Totem d'Eikthyr")
            .Italian("Totem di Eikthyr")
            .German("Totem von Eikthyr")
            .Spanish("Tótem de Eikthyr")
            .Russian("Тотем Эйктюра")
            .Romanian("Totemul lui Eikthyr")
            .Bulgarian("Тотемът на Ейктир")
            .Macedonian("Тотем на Ејтир")
            .Finnish("Eikthyrin toteemi")
            .Danish("Totem af Eikthyr")
            .Norwegian("Totem av Eikthyr")
            .Icelandic("Tótem frá Eikthyr")
            .Turkish("Eikthyr Totemi")
            .Lithuanian("Eikthyro totemas")
            .Czech("Totem Eikthyr")
            .Hungarian("Eikthyr Totem")
            .Slovak("Totem Eikthyr")
            .Polish("Totem Eikthyru")
            .Dutch("Totem van Eikthyr")
            .Chinese("艾克瑟尔图腾")
            .Japanese("エイクシルのトーテム")
            .Korean("에이크티르의 토템")
            .Hindi("इकथिर का कुलदेवता")
            .Thai("Totem ของ Eikthyr")
            .Croatian("Totem Eikthyra")
            .Georgian("ეიქტირის ტოტემი")
            .Greek("Τοτέμ του Eikthyr")
            .Serbian("Тотем Еиктира")
            .Ukrainian("Тотем Ейктір");
        totemOfEikthyr.Description
            .English("In case of your death, Eikthyr will save you and give you a part of his powers")
            .Swedish("I händelse av din död kommer Eikthyr att rädda dig och ge dig en del av sina krafter")
            .French("En cas de votre mort, Eikthyr vous sauvera et vous donnera une partie de ses pouvoirs")
            .Italian("In caso di tua morte, Eikthyr ti salverà e ti darà una parte dei suoi poteri")
            .German("Im Falle deines Todes wird Eikthyr dich retten und dir einen Teil seiner Kräfte geben")
            .Spanish("En caso de tu muerte, Eikthyr te salvará y te dará una parte de sus poderes.")
            .Russian("В случае вашей смерти Эйктир спасет вас и передаст вам часть своих сил.")
            .Romanian("În cazul morții tale, Eikthyr te va salva și îți va oferi o parte din puterile lui")
            .Bulgarian("В случай на вашата смърт, Eikthyr ще ви спаси и ще ви даде част от силите си")
            .Macedonian("Во случај на твоја смрт, Еиктир ќе те спаси и ќе ти даде дел од неговите моќи")
            .Finnish("Jos kuolet, Eikthyr pelastaa sinut ja antaa sinulle osan voimistaan")
            .Danish("I tilfælde af din død, vil Eikthyr redde dig og give dig en del af sine kræfter")
            .Norwegian("I tilfelle du dør, vil Eikthyr redde deg og gi deg en del av kreftene sine")
            .Icelandic("Ef þú deyrð mun Eikthyr bjarga þér og gefa þér hluta af krafti sínum")
            .Turkish("Ölümün durumunda Eikthyr seni kurtaracak ve güçlerinin bir kısmını sana verecek")
            .Lithuanian("Jūsų mirties atveju Eikthyr jus išgelbės ir suteiks jums dalį savo galių")
            .Czech("V případě tvé smrti tě Eikthyr zachrání a dá ti část svých sil")
            .Hungarian("Halálod esetén Eikthyr megment, és hatalmának egy részét átadja neked")
            .Slovak("V prípade vašej smrti vás Eikthyr zachráni a dá vám časť svojich schopností")
            .Polish("W przypadku twojej śmierci Eikthyr cię uratuje i odda część swoich mocy")
            .Dutch("In het geval van je dood zal Eikthyr je redden en je een deel van zijn krachten geven")
            .Chinese("如果你死了，艾克瑟尔会拯救你并给予你他的一部分力量")
            .Japanese("あなたが死んだ場合、エイクシルはあなたを救い、彼の力の一部をあなたに与えます")
            .Korean("당신이 죽을 경우, Eikthyr는 당신을 구하고 그의 힘의 일부를 당신에게 줄 것입니다")
            .Hindi("आपकी मृत्यु के मामले में, इकथिर आपको बचाएगा और आपको अपनी शक्तियों का एक हिस्सा देगा")
            .Thai("ในกรณีที่คุณเสียชีวิต Eikthyr จะช่วยคุณและมอบพลังส่วนหนึ่งให้กับคุณ")
            .Croatian("U slučaju vaše smrti, Eikthyr će vas spasiti i dati vam dio svojih moći")
            .Georgian("თქვენი სიკვდილის შემთხვევაში, ეიკთირი გადაგარჩენს და მოგცემთ თავისი ძალების ნაწილს")
            .Greek("Σε περίπτωση θανάτου σας, ο Eikthyr θα σας σώσει και θα σας δώσει ένα μέρος των δυνάμεών του")
            .Serbian("У случају ваше смрти, Еиктхир ће вас спасити и дати вам део својих моћи")
            .Ukrainian("У разі вашої смерті Ейктір врятує вас і віддасть вам частину своїх сил");
        totemOfEikthyr.Crafting.Add("Altar", 1);
        totemOfEikthyr.RequiredItems.Add("GreydwarfEye", 10);
        totemOfEikthyr.RequiredItems.Add("SurtlingCore", 7);
        totemOfEikthyr.RequiredItems.Add("TrophyEikthyr", 1);
        totemOfEikthyr.CraftAmount = 1;

        Item totemOfTheElder = new Item(bundle, "TotemOfTheElder");
        totemOfTheElder.Name
            .English("Totem Of TheElder")
            .Swedish("Totem Of TheElder")
            .French("Totem de l'Ancien")
            .Italian("Totem dell'Anziano")
            .German("Totem des Ältesten")
            .Spanish("Tótem del anciano")
            .Russian("Тотем Древнего")
            .Romanian("Totemul Bătrânului")
            .Bulgarian("Тотемът на Стария")
            .Macedonian("Тотем на постариот")
            .Finnish("Totem Of The Elder")
            .Danish("Totem Of TheElder")
            .Norwegian("Totem Of TheElder")
            .Icelandic("Totem Of TheElder")
            .Turkish("Yaşlıların Totemi")
            .Lithuanian("Vyresniojo totemas")
            .Czech("Totem The Elder")
            .Hungarian("Totem Of The Elder")
            .Slovak("Totem The Elder")
            .Polish("Totem Starszego")
            .Dutch("Totem van de Oudere")
            .Chinese("长者图腾")
            .Japanese("エルダーのトーテム")
            .Korean("장로의 토템")
            .Hindi("बड़ों का कुलदेवता")
            .Thai("Totem ของผู้เฒ่า")
            .Croatian("Totem Starijeg")
            .Georgian("უფროსის ტოტემი")
            .Greek("Totem Of The Elder")
            .Serbian("Тотем оф Тхе Елдер")
            .Ukrainian("Тотем Старшого");
        totemOfTheElder.Description
            .English("In case of your death, TheElder will save you and give you a part of his powers")
            .Swedish("Om du dör, kommer TheElder att rädda dig och ge dig en del av sina krafter")
            .French("En cas de décès, TheElder vous sauvera et vous donnera une partie de ses pouvoirs")
            .Italian("In caso di tua morte, TheElder ti salverà e ti darà una parte dei suoi poteri")
            .German("Im Falle Ihres Todes wird The Elder Sie retten und Ihnen einen Teil seiner Kräfte geben")
            .Spanish("En caso de tu muerte, TheElder te salvará y te dará una parte de sus poderes.")
            .Russian("В случае вашей смерти TheElder спасет вас и передаст вам часть своих сил.")
            .Romanian("În cazul morții tale, The Elder te va salva și îți va oferi o parte din puterile sale")
            .Bulgarian("В случай на твоята смърт TheElder ще те спаси и ще ти даде част от силите си")
            .Macedonian("Во случај на твоја смрт, Старецот ќе те спаси и ќе ти даде дел од неговите моќи")
            .Finnish("Jos kuolet, Elder pelastaa sinut ja antaa sinulle osan voimistaan")
            .Danish("I tilfælde af din død, vil TheElder redde dig og give dig en del af hans kræfter")
            .Norwegian("I tilfelle du dør, vil TheElder redde deg og gi deg en del av kreftene hans")
            .Icelandic("Ef þú deyrð mun TheElder bjarga þér og gefa þér hluta af krafti hans")
            .Turkish("Ölümünüz durumunda The Elder sizi kurtaracak ve güçlerinin bir kısmını size verecek")
            .Lithuanian("Jūsų mirties atveju Vyresnysis jus išgelbės ir suteiks jums dalį savo galių")
            .Czech("V případě vaší smrti vás TheElder zachrání a dá vám část svých schopností")
            .Hungarian("Halálod esetén TheElder megment, és hatalmának egy részét átadja neked")
            .Slovak("V prípade vašej smrti vás TheElder zachráni a dá vám časť svojich schopností")
            .Polish("W przypadku twojej śmierci TheElder cię uratuje i przekaże część swoich mocy")
            .Dutch("In het geval van je dood zal TheElder je redden en je een deel van zijn krachten geven")
            .Chinese("如果你死了，长老会救你并给你他的一部分力量")
            .Japanese("あなたが死んだ場合、TheElder はあなたを救い、彼の力の一部をあなたに与えます")
            .Korean("당신이 죽을 경우, TheElder는 당신을 구하고 그의 능력의 일부를 당신에게 줄 것입니다")
            .Hindi("आपकी मृत्यु के मामले में, द एल्डर आपको बचाएगा और आपको अपनी शक्तियों का एक हिस्सा देगा")
            .Thai("ในกรณีที่คุณเสียชีวิต TheElder จะช่วยคุณและมอบพลังส่วนหนึ่งให้กับคุณ")
            .Croatian("U slučaju vaše smrti, TheElder će vas spasiti i dati vam dio svojih moći")
            .Georgian("თქვენი სიკვდილის შემთხვევაში, უხუცესი გადაგარჩენთ და მოგცემთ მისი ძალაუფლების ნაწილს")
            .Greek("Σε περίπτωση θανάτου σας, ο TheElder θα σας σώσει και θα σας δώσει ένα μέρος των δυνάμεών του")
            .Serbian("У случају ваше смрти, Старешина ће вас спасити и дати вам део својих моћи")
            .Ukrainian("У разі вашої смерті Старійшина врятує вас і віддасть вам частину своїх сил");
        totemOfTheElder.Crafting.Add("Altar", 1);
        totemOfTheElder.RequiredItems.Add("GreydwarfEye", 10);
        totemOfTheElder.RequiredItems.Add("SurtlingCore", 7);
        totemOfTheElder.RequiredItems.Add("TrophyTheElder", 1);
        totemOfTheElder.CraftAmount = 1;

        Item totemOfBonemass = new Item(bundle, "TotemOfBonemass");
        totemOfBonemass.Name
            .English("Totem Of Bonemass")
            .Swedish("Totem Av Benmassa")
            .French("Totem de masse osseuse")
            .Italian("Totem di massa ossea")
            .German("Totem der Knochenmasse")
            .Spanish("Tótem de masa ósea")
            .Russian("Тотем Костяной Массы")
            .Romanian("Totem al Masei Osoase")
            .Bulgarian("Тотем на костната маса")
            .Macedonian("Тотем на коскената маса")
            .Finnish("Totem Of Bonemass")
            .Danish("Totem af knoglemasse")
            .Norwegian("Totem av benmasse")
            .Icelandic("Totem Of Bonemass")
            .Turkish("Kemik Kütlesi Totemi")
            .Lithuanian("Kaulų masės totemas")
            .Czech("Totem Bonemass")
            .Hungarian("Bonemass Totem")
            .Slovak("Totem Bonemass")
            .Polish("Totem Masy Kości")
            .Dutch("Totem van beenmassa")
            .Chinese("骨量图腾")
            .Japanese("骨塊のトーテム")
            .Korean("뼈 덩어리의 토템")
            .Hindi("बोनमास का टोटेम")
            .Thai("โทเท็มแห่งกระดูก")
            .Croatian("Totem koštane mase")
            .Georgian("Bonemass-ის ტოტემი")
            .Greek("Totem Of Bonemass")
            .Serbian("Тотем оф Бонемасс")
            .Ukrainian("Тотем кісткової маси");
        totemOfBonemass.Description
            .English("In case of your death, Bonemass will save you and give you a part of his powers")
            .Swedish("I händelse av din död kommer Bonemass att rädda dig och ge dig en del av sina krafter")
            .French("En cas de décès, Bonemass vous sauvera et vous donnera une partie de ses pouvoirs")
            .Italian("In caso di tua morte, Bonemass ti salverà e ti darà una parte dei suoi poteri")
            .German("Im Falle deines Todes wird Bonemass dich retten und dir einen Teil seiner Kräfte geben")
            .Spanish("En caso de tu muerte, Bonemass te salvará y te dará una parte de sus poderes.")
            .Russian("В случае вашей смерти, Костяная Масса спасет вас и передаст вам часть своих сил.")
            .Romanian("În cazul morții tale, Bonemass te va salva și îți va oferi o parte din puterile lui")
            .Bulgarian("В случай на вашата смърт Bonemass ще ви спаси и ще ви даде част от силите си")
            .Macedonian("Во случај на твоја смрт, Бонемас ќе те спаси и ќе ти даде дел од неговите моќи")
            .Finnish("Jos kuolet, Bonemass pelastaa sinut ja antaa sinulle osan voimistaan")
            .Danish("I tilfælde af din død vil Bonemass redde dig og give dig en del af hans kræfter")
            .Norwegian("I tilfelle du dør, vil Bonemass redde deg og gi deg en del av kreftene hans")
            .Icelandic("Ef þú deyrð, mun Bonemass bjarga þér og gefa þér hluta af krafti hans")
            .Turkish("Ölümünüz durumunda Bonemass sizi kurtaracak ve güçlerinin bir kısmını size verecek")
            .Lithuanian("Jūsų mirties atveju Bonemass jus išgelbės ir suteiks jums dalį savo galių")
            .Czech("V případě vaší smrti vás Bonemass zachrání a dá vám část svých schopností")
            .Hungarian("Halálod esetén Bonemass megment téged, és átadja erejének egy részét")
            .Slovak("V prípade vašej smrti vás Bonemass zachráni a dá vám časť svojich schopností")
            .Polish("W przypadku twojej śmierci Bonemass cię uratuje i przekaże część swoich mocy")
            .Dutch("In het geval van je dood zal Bonemass je redden en je een deel van zijn krachten geven")
            .Chinese("如果你死了，博内玛斯会救你并给你他的一部分力量")
            .Japanese("あなたが死んだ場合、ボーンマスはあなたを救い、彼の力の一部をあなたに与えます")
            .Korean("당신이 죽을 경우, Bonemass는 당신을 구하고 그의 힘의 일부를 줄 것입니다")
            .Hindi("आपकी मृत्यु के मामले में, बोनमास आपको बचाएगा और आपको अपनी शक्तियों का एक हिस्सा देगा")
            .Thai("ในกรณีที่คุณเสียชีวิต Bonemass จะช่วยคุณและมอบพลังส่วนหนึ่งให้กับคุณ")
            .Croatian("U slučaju vaše smrti, Bonemass će vas spasiti i dati vam dio svojih moći")
            .Georgian("თქვენი სიკვდილის შემთხვევაში ბონემასი გადაგარჩენთ და მოგცემთ თავისი ძალების ნაწილს")
            .Greek("Σε περίπτωση θανάτου σας, ο Bonemass θα σας σώσει και θα σας δώσει ένα μέρος των δυνάμεών του")
            .Serbian("У случају ваше смрти, Бонемасс ће вас спасити и дати вам део својих моћи")
            .Ukrainian("У разі вашої смерті Бонемас врятує вас і віддасть вам частину своїх сил");
        totemOfBonemass.Crafting.Add("Altar", 1);
        totemOfBonemass.RequiredItems.Add("GreydwarfEye", 10);
        totemOfBonemass.RequiredItems.Add("SurtlingCore", 7);
        totemOfBonemass.RequiredItems.Add("TrophyBonemass", 1);
        totemOfBonemass.CraftAmount = 1;

        Item totemOfModer = new Item(bundle, "TotemOfModer");
        totemOfModer.Name
            .English("Totem Of Moder")
            .Swedish("Totem av Moder")
            .French("Totem de Moder")
            .Italian("Totem di moderno")
            .German("Totem von Moder")
            .Spanish("Tótem de Moder")
            .Russian("Тотем Модера")
            .Romanian("Totem al Moderului")
            .Bulgarian("Тотем на Модер")
            .Macedonian("Тотем на Модер")
            .Finnish("Totem Of Moder")
            .Danish("Totem af Moder")
            .Norwegian("Totem Of Moder")
            .Icelandic("Totem Of Moder")
            .Turkish("Moderin Totemi")
            .Lithuanian("Modernaus totemas")
            .Czech("Totem Moder")
            .Hungarian("Moder Totem")
            .Slovak("Totem Modera")
            .Polish("Totem Moderatora")
            .Dutch("Totem van Moder")
            .Chinese("现代图腾")
            .Japanese("モダーのトーテム")
            .Korean("모더의 토템")
            .Hindi("मॉडर का कुलदेवता")
            .Thai("Totem ของโมเดอร์")
            .Croatian("Totem Modera")
            .Georgian("მოდერის ტოტემი")
            .Greek("Totem Of Moder")
            .Serbian("Тотем оф Модер")
            .Ukrainian("Тотем Модера");
        totemOfModer.Description
            .English("In case of your death, Moder will save you and give you a part of his powers")
            .Swedish("Om du dör kommer Moder att rädda dig och ge dig en del av sina krafter")
            .French("En cas de mort, Moder vous sauvera et vous donnera une partie de ses pouvoirs")
            .Italian("In caso di tua morte, Moder ti salverà e ti darà una parte dei suoi poteri")
            .German("Im Falle Ihres Todes wird Moder Sie retten und Ihnen einen Teil seiner Kräfte geben")
            .Spanish("En caso de tu muerte, Moder te salvará y te dará una parte de sus poderes.")
            .Russian("В случае вашей смерти Модер спасет вас и передаст вам часть своих сил.")
            .Romanian("În cazul morții tale, Moder te va salva și îți va oferi o parte din puterile sale")
            .Bulgarian("В случай на вашата смърт, Модер ще ви спаси и ще ви даде част от силите си")
            .Macedonian("Во случај на ваша смрт, Модер ќе ве спаси и ќе ви даде дел од неговите моќи")
            .Finnish("Jos kuolet, Moder pelastaa sinut ja antaa sinulle osan voimistaan")
            .Danish("I tilfælde af din død vil Moder redde dig og give dig en del af sine kræfter")
            .Norwegian("I tilfelle du dør, vil Moder redde deg og gi deg en del av kreftene sine")
            .Icelandic("Ef þú deyrð mun Moder bjarga þér og gefa þér hluta af krafti hans")
            .Turkish("Ölümün durumunda Moder seni kurtaracak ve güçlerinin bir kısmını sana verecek")
            .Lithuanian("Jūsų mirties atveju Moderis jus išgelbės ir suteiks jums dalį savo galių")
            .Czech("V případě vaší smrti vás Moder zachrání a dá vám část svých schopností")
            .Hungarian("Halálod esetén Moder megment téged, és átadja erejének egy részét")
            .Slovak("V prípade vašej smrti vás Moder zachráni a dá vám časť svojich právomocí")
            .Polish("W przypadku Twojej śmierci Moder Cię uratuje i odda Ci część swoich mocy")
            .Dutch("In het geval van je dood zal Moder je redden en je een deel van zijn krachten geven")
            .Chinese("如果你死了，莫德会救你并给你他的一部分力量")
            .Japanese("あなたが死んだ場合、モダーはあなたを救い、彼の力の一部をあなたに与えます")
            .Korean("당신이 죽을 경우, 모더는 당신을 구하고 그의 능력의 일부를 줄 것입니다")
            .Hindi("आपकी मृत्यु के मामले में, मॉडर आपको बचाएगा और आपको अपनी शक्तियों का एक हिस्सा देगा")
            .Thai("ในกรณีที่คุณเสียชีวิต Moder จะช่วยคุณและมอบพลังส่วนหนึ่งให้กับคุณ")
            .Croatian("U slučaju vaše smrti, Moder će vas spasiti i dati vam dio svojih moći")
            .Georgian("შენი სიკვდილის შემთხვევაში მოდერი გიშველის და მოგცემს თავისი ძალების ნაწილს")
            .Greek("Σε περίπτωση θανάτου σας, ο Moder θα σας σώσει και θα σας δώσει ένα μέρος των δυνάμεών του")
            .Serbian("У случају ваше смрти, Модер ће вас спасити и дати вам део својих моћи")
            .Ukrainian("У разі вашої смерті Модер врятує вас і віддасть вам частину своїх сил");
        totemOfModer.Crafting.Add("Altar", 1);
        totemOfModer.RequiredItems.Add("GreydwarfEye", 10);
        totemOfModer.RequiredItems.Add("SurtlingCore", 7);
        totemOfModer.RequiredItems.Add("TrophyDragonQueen", 1);
        totemOfModer.CraftAmount = 1;


        Item totemOfYagluth = new Item(bundle, "TotemOfYagluth");
        totemOfYagluth.Name
            .English("Totem Of Yagluth")
            .Swedish("Totem av Yagluth")
            .French("Totem de Yagluth")
            .Italian("Totem di Yagluth")
            .German("Totem von Yagluth")
            .Spanish("Tótem de Yagluth")
            .Russian("Тотем Яглута")
            .Romanian("Totem al lui Yagluth")
            .Bulgarian("Тотемът на Яглут")
            .Macedonian("Тотем на Јаглут")
            .Finnish("Yagluthin toteemi")
            .Danish("Totem af Yagluth")
            .Norwegian("Totem av Yagluth")
            .Icelandic("Tótem frá Yagluth")
            .Turkish("Yagluth'un Totemi")
            .Lithuanian("Jagluto totemas")
            .Czech("Totem Yagluth")
            .Hungarian("Yagluth Totem")
            .Slovak("Totem Yagluth")
            .Polish("Totem Yagluth")
            .Dutch("Totem van Yagluth")
            .Chinese("亚格鲁斯图腾")
            .Japanese("ヤグルスのトーテム")
            .Korean("야글루스의 토템")
            .Hindi("यग्लुथ का कुलदेवता")
            .Thai("โทเท็มแห่งยากลัท")
            .Croatian("Totem Yaglutha")
            .Georgian("იაგლუთის ტოტემი")
            .Greek("Τοτέμ Γιαγλούθ")
            .Serbian("Тотем Јаглута")
            .Ukrainian("Тотем Яглута");
        totemOfYagluth.Description
            .English("In case of your death, Yagluth will save you and give you a part of his powers")
            .Swedish("I fall av din död kommer Yagluth att rädda dig och ge dig en del av sina krafter")
            .French("En cas de mort, Yagluth vous sauvera et vous donnera une partie de ses pouvoirs")
            .Italian("In caso di tua morte, Yagluth ti salverà e ti donerà parte dei suoi poteri")
            .German("Im Falle deines Todes wird Yagluth dich retten und dir einen Teil seiner Kräfte geben")
            .Spanish("En caso de tu muerte, Yagluth te salvará y te dará una parte de sus poderes.")
            .Russian("В случае вашей смерти Яглут спасет вас и передаст вам часть своих сил.")
            .Romanian("În cazul morții tale, Yagluth te va salva și îți va da o parte din puterile lui")
            .Bulgarian("В случай на твоята смърт Yagluth ще те спаси и ще ти даде част от силите си")
            .Macedonian("Во случај на твоја смрт, Јаглут ќе те спаси и ќе ти даде дел од неговите моќи")
            .Finnish("Jos kuolet, Yagluth pelastaa sinut ja antaa sinulle osan voimistaan")
            .Danish("I tilfælde af din død, vil Yagluth redde dig og give dig en del af sine kræfter")
            .Norwegian("I tilfelle du dør, vil Yagluth redde deg og gi deg en del av kreftene hans")
            .Icelandic("Ef þú deyrð, mun Yagluth bjarga þér og gefa þér hluta af krafti hans")
            .Turkish("Ölümün durumunda Yagluth seni kurtaracak ve güçlerinin bir kısmını sana verecek")
            .Lithuanian("Jūsų mirties atveju Yagluth jus išgelbės ir suteiks jums dalį savo galių")
            .Czech("V případě tvé smrti tě Yagluth zachrání a dá ti část svých sil")
            .Hungarian("Halálod esetén Yagluth megment, és hatalmának egy részét átadja neked")
            .Slovak("V prípade vašej smrti vás Yagluth zachráni a dá vám časť svojich schopností")
            .Polish("W przypadku twojej śmierci Yagluth cię uratuje i przekaże część swoich mocy")
            .Dutch("In het geval van je dood zal Yagluth je redden en je een deel van zijn krachten geven")
            .Chinese("如果你死了，亚格鲁斯会救你并给你他的一部分力量")
            .Japanese("あなたが死んだ場合、ヤグルスはあなたを救い、彼の力の一部をあなたに与えます")
            .Korean("당신이 죽을 경우 Yagluth는 당신을 구하고 그의 힘의 일부를 줄 것입니다")
            .Hindi("आपकी मृत्यु के मामले में, याग्लुथ आपको बचाएगा और आपको अपनी शक्तियों का एक हिस्सा देगा")
            .Thai("ในกรณีที่คุณเสียชีวิต Yagluth จะช่วยคุณและมอบพลังส่วนหนึ่งให้กับคุณ")
            .Croatian("U slučaju vaše smrti, Yagluth će vas spasiti i dati vam dio svojih moći")
            .Georgian("შენი სიკვდილის შემთხვევაში იაგლუტი გადაგარჩენს და მოგცემს თავისი ძალების ნაწილს")
            .Greek("Σε περίπτωση θανάτου σου, ο Yagluth θα σε σώσει και θα σου δώσει ένα μέρος των δυνάμεών του")
            .Serbian("У случају ваше смрти, Иаглутх ће вас спасити и дати вам део својих моћи")
            .Ukrainian("У разі вашої смерті Яглут врятує вас і віддасть вам частину своїх сил");
        totemOfYagluth.Crafting.Add("Altar", 1);
        totemOfYagluth.RequiredItems.Add("GreydwarfEye", 10);
        totemOfYagluth.RequiredItems.Add("SurtlingCore", 7);
        totemOfYagluth.RequiredItems.Add("TrophyGoblinKing", 1);
        totemOfYagluth.CraftAmount = 1;

        #endregion

        #region Altar

        BuildPiece buildPiece = new BuildPiece(bundle, "Altar");
        buildPiece.Name
            .English("Altar")
            .Swedish("Altare")
            .French("Autel")
            .Italian("Altare")
            .German("Altar")
            .Spanish("Altar")
            .Russian("Алтарь")
            .Romanian("Altar")
            .Bulgarian("Олтар")
            .Macedonian("Олтар")
            .Finnish("Alttari")
            .Danish("Alter")
            .Norwegian("Alter")
            .Icelandic("Altari")
            .Turkish("Altar")
            .Lithuanian("Altorius")
            .Czech("Oltář")
            .Hungarian("Oltár")
            .Slovak("Oltár")
            .Polish("Ołtarz")
            .Dutch("Altaar")
            .Chinese("坛")
            .Japanese("祭壇")
            .Korean("제단")
            .Hindi("वेदी")
            .Thai("แท่นบูชา")
            .Croatian("Oltar")
            .Georgian("საკურთხეველი")
            .Greek("Αγια ΤΡΑΠΕΖΑ")
            .Serbian("Олтар")
            .Ukrainian("Вівтар");
        buildPiece.Description
            .English("Call on the help of defeated bosses by sacrificing their head.")
            .Swedish("Ring på hjälp av besegrade chefer genom att offra deras huvud.")
            .French("Faites appel à l'aide des boss vaincus en sacrifiant leur tête.")
            .Italian("Invoca l'aiuto dei boss sconfitti sacrificando la loro testa.")
            .German("Rufen Sie besiegte Bosse um Hilfe, indem Sie ihren Kopf opfern.")
            .Spanish("Pide la ayuda de los jefes derrotados sacrificando sus cabezas.")
            .Russian("Взови к помощи поверженных боссов, принеся их голову в жертву.")
            .Romanian("Apelați la ajutorul șefilor învinși sacrificându-și capul.")
            .Bulgarian("Потърсете помощта на победените босове, като пожертвате главата им.")
            .Macedonian("Повикајте им помош на поразените газди со жртвување на нивната глава.")
            .Finnish("Pyydä päihitettyjen pomojen apua uhraamalla heidän päänsä.")
            .Danish("Kald på hjælp fra besejrede chefer ved at ofre deres hoved.")
            .Norwegian("Be om hjelp fra beseirede sjefer ved å ofre hodet.")
            .Icelandic("Kallaðu á hjálp ósigraðra yfirmanna með því að fórna höfði þeirra.")
            .Turkish("Başlarını feda ederek mağlup patronların yardımını çağırın.")
            .Lithuanian("Pasikvieskite nugalėtų viršininkų pagalbą, paaukodami jų galvą.")
            .Czech("Přivolejte si na pomoc poražené bossy obětováním jejich hlavy.")
            .Hungarian("Hívd segítségül a legyőzött főnököket a fejük feláldozásával.")
            .Slovak("Privolajte si na pomoc porazených bossov obetovaním ich hlavy.")
            .Polish("Wezwij pomoc pokonanych bossów, poświęcając ich głowę.")
            .Dutch("Roep de hulp in van verslagen bazen door hun hoofd op te offeren.")
            .Chinese("通过牺牲被击败的首领的头颅来寻求帮助。")
            .Japanese("倒したボスの頭を犠牲にして助けを求めます。")
            .Korean("패배한 보스의 머리를 희생하여 도움을 요청하세요.")
            .Hindi("अपने सिर का बलिदान देकर पराजित मालिकों की मदद का आह्वान करें।")
            .Thai("ขอความช่วยเหลือจากบอสที่พ่ายแพ้ด้วยการเสียสละหัวของพวกเขา")
            .Croatian("Pozovite u pomoć poražene šefove tako što ćete žrtvovati njihovu glavu.")
            .Georgian("დახმარებისთვის მიმართეთ დამარცხებულ უფროსებს მათი თავის გაწირვით.")
            .Greek("Ζητήστε τη βοήθεια των ηττημένων αφεντικών θυσιάζοντας το κεφάλι τους.")
            .Serbian("Позовите у помоћ поражене шефове жртвујући њихову главу.")
            .Ukrainian("Взивай до допомоги повалених босів, принісши їхню голову в жертву.");
        buildPiece.RequiredItems.Add("Stone", 20, true);
        buildPiece.RequiredItems.Add("SurtlingCore", 10, true);
        buildPiece.Category.Add(BuildPieceCategory.Crafting);

        #endregion

        LoadTotems();

        #region Status effects

        CustomSE SE_Yagluth = new("totemsofundying_efects", "SE_Yagluth");
        var SE_Yagluth_effect = SE_Yagluth.Effect as SE_Stats;
        SE_Yagluth_effect.m_icon = YagluthTotem.config.itemDrop.m_itemData.GetIcon();
        SE_Yagluth_effect.m_mods = new List<HitData.DamageModPair>
        {
            new HitData.DamageModPair
            {
                m_type = HitData.DamageType.Physical,
                m_modifier = HitData.DamageModifier.Resistant
            },
            new HitData.DamageModPair
            {
                m_type = HitData.DamageType.Pickaxe,
                m_modifier = HitData.DamageModifier.Resistant
            },
            new HitData.DamageModPair
            {
                m_type = HitData.DamageType.Pierce,
                m_modifier = HitData.DamageModifier.Resistant
            },
            new HitData.DamageModPair
            {
                m_type = HitData.DamageType.Poison,
                m_modifier = HitData.DamageModifier.Resistant
            },
            new HitData.DamageModPair
            {
                m_type = HitData.DamageType.Slash,
                m_modifier = HitData.DamageModifier.Resistant
            },
            new HitData.DamageModPair
            {
                m_type = HitData.DamageType.Spirit,
                m_modifier = HitData.DamageModifier.Resistant
            },
            new HitData.DamageModPair
            {
                m_type = HitData.DamageType.Elemental,
                m_modifier = HitData.DamageModifier.Resistant
            }
        };

        CustomSE SE_Moder = new("totemsofundying_efects", "SE_Moder");
        SE_Moder.IconSprite = ModerTotem.config.itemDrop.m_itemData.GetIcon();
        var SE_Moder_effect = SE_Moder.Effect as SE_Stats;
        SE_Moder_effect.m_icon = ModerTotem.config.itemDrop.m_itemData.GetIcon();
        SE_Moder_effect.m_mods = new List<HitData.DamageModPair>
        {
            new HitData.DamageModPair
            {
                m_type = HitData.DamageType.Fire,
                m_modifier = HitData.DamageModifier.Weak
            }
        };

        CustomSE SE_Bonemass = new("totemsofundying_efects", "SE_Bonemass");
        SE_Bonemass.IconSprite = BonemassTotem.config.itemDrop.m_itemData.GetIcon();
        var SE_Bonemass_effect = SE_Bonemass.Effect as SE_Stats;
        SE_Bonemass_effect.m_icon = BonemassTotem.config.itemDrop.m_itemData.GetIcon();
        SE_Bonemass_effect.m_mods = new List<HitData.DamageModPair>
        {
            new HitData.DamageModPair
            {
                m_type = HitData.DamageType.Blunt,
                m_modifier = HitData.DamageModifier.VeryResistant
            }
        };

        CustomSE SE_Eikthyr = new("totemsofundying_efects", "SE_Eikthyr");
        SE_Eikthyr.IconSprite = EikthyrTotem.config.itemDrop.m_itemData.GetIcon();
        var SE_Eikthyr_effect = SE_Eikthyr.Effect as SE_Stats;
        SE_Eikthyr_effect.m_icon = EikthyrTotem.config.itemDrop.m_itemData.GetIcon();
        SE_Eikthyr_effect.m_mods = new List<HitData.DamageModPair>
        {
            new HitData.DamageModPair
            {
                m_type = HitData.DamageType.Lightning,
                m_modifier = HitData.DamageModifier.VeryResistant
            }
        };

        CustomSE SETheElder = new("totemsofundying_efects", "SE_TheElder");
        SETheElder.IconSprite = TheElderTotem.config.itemDrop.m_itemData.GetIcon();
        var SETheElder_effect = SETheElder.Effect as SE_Stats;
        SETheElder_effect.m_icon = TheElderTotem.config.itemDrop.m_itemData.GetIcon();
        SETheElder_effect.m_modifyAttackSkill = Skills.SkillType.Axes;
        SETheElder_effect.m_mods = new List<HitData.DamageModPair>
        {
            new HitData.DamageModPair
            {
                m_type = HitData.DamageType.Chop,
                m_modifier = HitData.DamageModifier.Weak
            },
            new HitData.DamageModPair
            {
                m_type = HitData.DamageType.Pierce,
                m_modifier = HitData.DamageModifier.Resistant
            }
        };

        #endregion

        Localizer.Load();
    }

    private void LoadTotems()
    {
        EikthyrTotem = Totem.CreateInstance("TotemOfEikthyr" /*, "fx_Eikthyr"*/);
        EikthyrTotem.config.allBiomes = true;
        EikthyrTotem.config.healthRightBiome = 10;
        EikthyrTotem.config.healthWrongBiome = 10;
        EikthyrTotem.config.staminaRightBiome = 25;
        EikthyrTotem.config.staminaWrongBiome = 25;
        EikthyrTotem.config.speedModifier = 0.1f;
        EikthyrTotem.config.damageModifier = 0f;
        EikthyrTotem.config.Bind();

        TheElderTotem = Totem.CreateInstance("TotemOfTheElder" /*, "fx_TheElder"*/);
        TheElderTotem.config.bestBiome = BlackForest;
        TheElderTotem.config.aditionalBiomes.Add(Mistlands);
        TheElderTotem.config.healthRightBiome = 50;
        TheElderTotem.config.healthWrongBiome = 20;
        TheElderTotem.config.staminaRightBiome = 45;
        TheElderTotem.config.staminaWrongBiome = 20;
        TheElderTotem.config.speedModifier = 0.1f;
        TheElderTotem.config.damageModifier = 0.13f;
        TheElderTotem.config.Bind();

        BonemassTotem = Totem.CreateInstance("TotemOfBonemass" /*, "fx_Bonemass"*/);
        BonemassTotem.config.bestBiome = Swamp;
        BonemassTotem.config.badBiome = AshLands;
        BonemassTotem.config.healthRightBiome = 40;
        BonemassTotem.config.healthWrongBiome = 50;
        BonemassTotem.config.staminaRightBiome = 130;
        BonemassTotem.config.staminaWrongBiome = 45;
        BonemassTotem.config.speedModifier = 0.2f;
        BonemassTotem.config.buffs.Add("Potion_poisonresist");
        BonemassTotem.config.Bind();

        ModerTotem = Totem.CreateInstance("TotemOfModer" /*, "fx_Moder"*/);
        ModerTotem.config.bestBiome = Mountain;
        ModerTotem.config.aditionalBiomes.Add(DeepNorth);
        ModerTotem.config.healthRightBiome = 101;
        ModerTotem.config.healthWrongBiome = 30;
        ModerTotem.config.staminaRightBiome = 60;
        ModerTotem.config.staminaWrongBiome = 50;
        ModerTotem.config.speedModifier = 0.11f;
        ModerTotem.config.fallDamageModifier = -5f;
        ModerTotem.config.buffs.Add("Potion_frostresist");
        ModerTotem.config.Bind();

        YagluthTotem = Totem.CreateInstance("TotemOfYagluth" /*, "fx_Yagluth"*/);
        YagluthTotem.config.bestBiome = Plains;
        YagluthTotem.config.aditionalBiomes.Add(Swamp);
        YagluthTotem.config.aditionalBiomes.Add(Mistlands);
        YagluthTotem.config.healthRightBiome = 150;
        YagluthTotem.config.healthWrongBiome = 80;
        YagluthTotem.config.staminaRightBiome = 60;
        YagluthTotem.config.staminaWrongBiome = 50;
        YagluthTotem.config.speedModifier = 0.5f;
        YagluthTotem.config.damageModifier = 0.5f;
        YagluthTotem.config.buffs.Add("Potion_health_major");
        YagluthTotem.config.buffs.Add("Potion_barleywine");
        YagluthTotem.config.Bind();
        Debug("All Totems Loaded");
    }

    public void UseTotem(ItemDrop.ItemData itemData, string totemName)
    {
        var currentBiome = Player.m_localPlayer.GetCurrentBiome();
        GetTotem(totemName)?.Use(itemData, currentBiome);
    }

    public static Totem GetTotem(string totemName) =>
        totemName switch
        {
            "$item_TotemOfEikthyr" => EikthyrTotem,
            "$item_TotemOfTheElder" => TheElderTotem,
            "$item_TotemOfBonemass" => BonemassTotem,
            "$item_TotemOfModer" => ModerTotem,
            "$item_TotemOfYagluth" => YagluthTotem,
            _ => null
        };


    #region tools

    public static void Debug(object msg, bool showInConsole = false)
    {
        _self.Logger.LogInfo(msg);
        if (showInConsole && Console.IsVisible()) Console.instance.AddString(msg.ToString());
    }

    public static void DebugError(object msg, bool showWriteToDev = true)
    {
        if (showWriteToDev) msg += "Write to the developer and moderator if this happens often.";

        _self.Logger.LogError(msg);
    }

    public static void DebugWarning(object msg, bool showWriteToDev = false)
    {
        if (showWriteToDev) msg += "Write to the developer and moderator if this happens often.";

        _self.Logger.LogWarning(msg);
    }

    #endregion

    #region ConfigSettings

    #region tools

    private static readonly string ConfigFileName = $"{ModGUID}.cfg";
    private DateTime LastConfigChange;

    public static readonly ConfigSync configSync = new(ModName)
        { DisplayName = ModName, CurrentVersion = ModVersion, MinimumRequiredVersion = ModVersion };

    public static ConfigEntry<T> config<T>(string group, string name, T value, ConfigDescription description,
        bool synchronizedSetting = true)
    {
        var configEntry = _self.Config.Bind(group, name, value, description);

        var syncedConfigEntry = configSync.AddConfigEntry(configEntry);
        syncedConfigEntry.SynchronizedConfig = synchronizedSetting;

        return configEntry;
    }

    private ConfigEntry<T> config<T>(string group, string name, T value, string description,
        bool synchronizedSetting = true) =>
        config(group, name, value, new ConfigDescription(description), synchronizedSetting);

    private void SetupWatcher()
    {
        FileSystemWatcher mainConfigFileSystemWatcher = new(Paths.ConfigPath, ConfigFileName);
        mainConfigFileSystemWatcher.Changed += ConfigChanged;
        mainConfigFileSystemWatcher.IncludeSubdirectories = true;
        mainConfigFileSystemWatcher.SynchronizingObject = ThreadingHelper.SynchronizingObject;
        mainConfigFileSystemWatcher.EnableRaisingEvents = true;
    }


    private void ConfigChanged(object sender, FileSystemEventArgs e)
    {
        if ((DateTime.Now - LastConfigChange).TotalSeconds <= 2) return;

        LastConfigChange = DateTime.Now;
        try
        {
            Config.Reload();
        }
        catch
        {
            DebugError("Can't reload Config");
        }
    }

    internal void ConfigChanged() { ConfigChanged(null, null); }

    #endregion

    #region configs

    public static ConfigEntry<int> relocateIntervalConfig;

    public static int relocateInterval = 1;

    #endregion

    internal void UpdateConfiguration()
    {
        try
        {
            relocateInterval = relocateIntervalConfig.Value;

            Debug("Configuration Received");
        }
        catch (Exception e)
        {
            DebugError($"Configuration error: {e.Message}", false);
        }
    }

    #endregion
}