using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Battle;
using Model.Boss;
using Model.Card;
using Model.Inventory;
using Model.Zone;
using Presentation.Boss;
using Presentation.Entity;
using Presentation.Inventory;
using Presentation.TotalStats;
using Presentation.Zone;
using UnityEngine;
using API;
using Presentation.Card;
using Services;

public class GameEntryPoint : MonoBehaviour
{
    [Header("All views. Container with script [name]View")]
    [SerializeField]
    private TotalCardStatsView totalCardStatsView;

    [SerializeField]
    private BossView bossView;

    [SerializeField]
    private ZoneView zoneView;

    [SerializeField]
    private InventoryView inventoryView;

    [SerializeField]
    private EntityView entityView;

    [Header("Toolbar UI")]
    [SerializeField]
    private EntityView toolbarCardPrefab;

    [SerializeField]
    private Transform cardsToolbarUI;


    private IUserService                   _userService;
    private InventoryModel                 _inventoryModel;
    private InventoryPresenter             _inventoryPresenter;
    private ZoneModel                      _zoneModel;
    private BossModel                      _bossModel;
    private CardLoaderService              _cardLoaderService;


    private TotalToolbarStatsViewModel     _statsModel;
    private TotalCardStatsPresenter        _statsPresenter;


    // [Header("Start game UI elements")]
    // public Button startGameButton;
    // public Transform gameUI;
    // public Transform cardsToolbarUI;
    // public Transform inventoryUI;


    private void Awake()
    {
        _zoneModel = new ZoneModel {CurrentZone = {Value = 1}};
        _bossModel = new BossModel
        {
            BaseHp = 20f,
            BaseAttack = 100f,
            MaxHp = {Value = 20f},
            CurrentHp = {Value = 20f},
            Attack = { Value = 10f},
            ExpReward = { Value = 20},
            GoldReward = { Value = 5}
        };
        
        _inventoryModel = new InventoryModel();
        _inventoryPresenter = new InventoryPresenter(_inventoryModel, inventoryView);

        new BossPresenter().Init(_bossModel, bossView);
        new ZonePresenter().Init(_zoneModel, zoneView);
        
        _cardLoaderService = new CardLoaderService();
        _userService       = new FakeUserService();

        _statsModel     = new TotalToolbarStatsViewModel();
        _statsPresenter = new TotalCardStatsPresenter();


        _inventoryModel.OnSelectionChanged += UpdateToolbarAndStats;
    }


    private async void Start()
    {
        var user = await _userService.GetCurrentUserAsync();
        _inventoryModel = user.InventoryModel;
        _inventoryPresenter = new InventoryPresenter(_inventoryModel, inventoryView);

        _inventoryModel.OnSelectionChanged += UpdateToolbarAndStats;


        UpdateToolbarAndStats();


        var allDomainCards = _cardLoaderService.GetDomainCards();
        StartBattle(allDomainCards);
    }


    private void StartBattle(List<CardModel> allDomainCards)
    {
        var battleScript = new BattleScript(
            _bossModel,
            _zoneModel,
            allDomainCards,
            _inventoryPresenter,
            _statsModel
        );
        StartCoroutine(StartBattleLoop(battleScript));
    }


    private void InitUISelectedCardsToolbar(List<CardModel> selectedCards)
    {
        foreach (Transform t in cardsToolbarUI)
            Destroy(t.gameObject);
        
        foreach (var card in selectedCards)
        {
            var instance = Instantiate(
                toolbarCardPrefab,
                cardsToolbarUI,
                worldPositionStays: false
            );

            instance.Init(card, _inventoryPresenter, isToolbar: true);
            var cardView = instance.GetComponent<CardView>();
            new CardPresenter().Init(card, cardView);
        }
    }


    private IEnumerator StartBattleLoop(BattleScript battleScript)
    {
        while (true)
        {
            battleScript.BattleUpdate();
            yield return new WaitForSeconds(1f);
        }
    }
    
    
    
    private void UpdateToolbarAndStats()
    {
        var toolbarCards = _inventoryModel
            .SelectedItems
            .OfType<CardModel>()
            .ToList();
        
        _statsModel.Cards = toolbarCards;
        _statsPresenter.Init(toolbarCards, totalCardStatsView);

        InitUISelectedCardsToolbar(toolbarCards);
    }
}