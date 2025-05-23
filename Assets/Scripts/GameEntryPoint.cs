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

    private IUserService _userService;

    private InventoryModel _inventoryModel;

    private InventoryPresenter _inventoryPresenter;

    private ZoneModel _zoneModel;

    private BossModel _bossModel;


    // [Header("Start game UI elements")]
    // public Button startGameButton;
    // public Transform gameUI;
    // public Transform cardsToolbarUI;
    // public Transform inventoryUI;


    private void Awake()
    {
        _zoneModel = new ZoneModel {CurrentZone = {Value = 1}};
        _bossModel = new BossModel {CurrentHp = {Value = 2000f}};
        
        _inventoryModel = new InventoryModel();
        _inventoryPresenter = new InventoryPresenter(_inventoryModel, inventoryView);

        new BossPresenter().Init(_bossModel, bossView);
        new ZonePresenter().Init(_zoneModel, zoneView);

        _userService = new FakeUserService();
    }


    private async void Start()
    {
        var user = await _userService.GetCurrentUserAsync();
        _inventoryModel.LoadItems(user.InventoryModel.Items);

        Debug.Log($"User -- {user.Nickname} --  has {_inventoryModel.Items.Count} cards");

        var cardsForBattle = _inventoryModel.Items.OfType<CardModel>().ToList();

        InitUISelectedCardsToolbar(cardsForBattle);

        StartBattle(cardsForBattle);
    }


    private void StartBattle(List<CardModel> userCardCollection)
    {
        var battleScript = new BattleScript(_bossModel, _zoneModel, userCardCollection, _inventoryPresenter);
        StartCoroutine(StartBattleLoop(battleScript));
    }


    private void InitUISelectedCardsToolbar(List<CardModel> selectedCards)
    {
        foreach (Transform t in cardsToolbarUI)
            Destroy(t.gameObject);


        foreach (var card in selectedCards)
        {
            var instance = Instantiate(toolbarCardPrefab, cardsToolbarUI, worldPositionStays: false);

            instance.Init(card, _inventoryPresenter, isToolbar: true);

            var cardView = instance.GetComponent<CardView>();
            new CardPresenter().Init(card, cardView);
        }
    }


    private IEnumerator StartBattleLoop(BattleScript battleScript)
    {
        //ToDo: battleScript.IsBattleOngoing()
        while (true)
        {
            battleScript.BattleUpdate();
            yield return new WaitForSeconds(1f);
        }
    }
}