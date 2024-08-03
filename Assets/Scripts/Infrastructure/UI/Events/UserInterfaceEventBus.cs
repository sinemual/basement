using Client.Infrastructure;
using UnityEngine.Events;

public class UserInterfaceEventBus
{
    //output:
    public readonly LevelCompleteScreenEvents LevelCompleteScreen = new LevelCompleteScreenEvents();
    public readonly TntPickaxeBoosterScreenEvents TntPickaxeBoosterScreen = new TntPickaxeBoosterScreenEvents();
    public readonly GlobalMapScreenEvents GlobalMapScreen = new GlobalMapScreenEvents();
    public readonly BuildScreenEvents BuildScreen = new BuildScreenEvents();
    public readonly ExitFromLevelScreenEvents ExitFromLevelScreen = new ExitFromLevelScreenEvents();
    public readonly VillageScreenEvents VillageScreen = new VillageScreenEvents();
    public readonly GameProgressScreenEvents GameProgressScreen = new GameProgressScreenEvents();
    public readonly LevelFailedScreenEvents LevelFailedScreen = new LevelFailedScreenEvents();
    public readonly SettingScreenEvents SettingScreen = new SettingScreenEvents();
    public readonly CraftScreenEvents CraftScreen = new CraftScreenEvents();
    public readonly ChangeItemScreenEvents ChangeItemScreen = new ChangeItemScreenEvents();
    public readonly NoAdsIapScreenEvents NoAdsIapScreen = new NoAdsIapScreenEvents();
    public readonly GameScreenEvents GameScreen = new GameScreenEvents();
    public readonly LevelGoalScreenEvents LevelGoalScreen = new LevelGoalScreenEvents();
    public readonly CameraControlScreenEvents CameraControlScreen = new CameraControlScreenEvents();
    public readonly CurrentHandItemScreenEvents CurrentHandItemScreen = new CurrentHandItemScreenEvents();
    public readonly InventoryScreenEvents InventoryScreen= new InventoryScreenEvents();
    public readonly GetItemScreenEvents GetItemScreen = new GetItemScreenEvents();
    public readonly OpenChestScreenEvents OpenChestScreen = new OpenChestScreenEvents();
    public readonly OpenGameProgressScreenEvents OpenGameProgressScreen = new OpenGameProgressScreenEvents();
//output:
    public readonly ExperienceEvents Experience = new ExperienceEvents();
    public readonly ResourcesEvents Resources = new ResourcesEvents();
    public readonly SoftCurrencyEvents SoftCurrency = new SoftCurrencyEvents();
    public readonly PlayerDamageEvents PlayerDamage = new PlayerDamageEvents();
}