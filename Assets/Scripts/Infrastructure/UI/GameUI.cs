using System;
using System.Collections.Generic;
using Client;
using Client.Data;
using Client.Data.Core;
using Client.Infrastructure.Services;
using UnityEngine;

public class GameUI : MonoBehaviour
{
    [Header("Joystick")] 
    public Joystick AimJoystick;
    [Header("Screens")] 
    public OpenSettingScreen OpenSettingScreen;
    public OpenCraftScreen OpenCraftScreen;
    public OpenLevelCompleteScreen OpenLevelCompleteScreen;
    public BoosterFeelingScreen BoosterFeelingScreen;
    public OpenTntPickaxeBoosterScreen OpenTntPickaxeBoosterScreen;
    public TntPickaxeBoosterScreen TntPickaxeBoosterScreen;
    public SettingScreen SettingScreen;
    public GoalScreen GoalScreen;
    public CheatScreen CheatScreen;
    //public LoadingScreen LoadingScreen;
    //public OpenInventoryScreen OpenInventoryScreen;
    public InventoryScreen InventoryScreen;
    public ResourceScreen ResourceScreen;
    public PlayerDamageFeelingScreen PlayerDamageFeelingScreen;
    public OnLevelScreen OnLevelScreen;
    public VillageScreen VillageScreen;
    public GlobalMapScreen GlobalMapScreen;
    public HandItemScreen HandItemScreen;
    public CraftScreen CraftScreen;
    public ChooseItemScreen ChooseItemScreen;
    public BuildScreen BuildScreen;
    //public GetItemScreen GetItemScreen;
    public ChestScreen ChestScreen;
    
    [Header("Tutorials")] 
    public TutorialScreens Tutorials;

    private List<BaseScreen> _screens;
    private List<BaseScreen> _worldUiScreens;
    
    // Services:
    [HideInInspector] public UserInterfaceEventBus EventBus;
    [HideInInspector] public TimeManagerService TimeManagerService;
    [HideInInspector] public AudioService AudioService;
    [HideInInspector] public AnalyticService AnalyticService;
    [HideInInspector] public AdsService AdsService;

    private bool IsUiEnabled;
    private bool IsWorldUiEnabled;

    public void Inject(
        SharedData sharedData,
        UserInterfaceEventBus uiEventBus,
        TimeManagerService timeManagerService,
        AudioService audioService,
        AnalyticService analyticService,
        AdsService adsService)
    {
        EventBus = uiEventBus;
        TimeManagerService = timeManagerService;
        AudioService = audioService;
        AnalyticService = analyticService;
        AdsService = adsService;

        _screens = new List<BaseScreen>();
        _screens.AddRange(GetComponentsInChildren<BaseScreen>(true));

        foreach (var screen in _screens)
        {
            screen.gameObject.SetActive(true);
            screen.Inject(sharedData, this);
            screen.Init();
            screen.gameObject.SetActive(false);
        }

        IsUiEnabled = true;
    }

    public void TriggerShowStateAllScreen()
    {
        IsUiEnabled = !IsUiEnabled;
        foreach (var screen in _screens)
            screen.gameObject.SetActive(IsUiEnabled);
    }
    
    public void TriggerShowStateAllWorldUiScreen()
    {
        IsWorldUiEnabled = !IsWorldUiEnabled;
        foreach (var screen in _worldUiScreens)
            screen.gameObject.SetActive(IsWorldUiEnabled);
    }
    
    public void AddScreenToWorldUiScreens(BaseScreen worldUiScreen)
    {
        _worldUiScreens.Add(worldUiScreen);
    }

    [Serializable]
    public class TutorialScreens : SerializedDictionary<TutorialStep, TutorialBaseScreen>
    {
    }
}