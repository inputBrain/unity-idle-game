using System.Collections;
using System.Collections.Generic;
using Battle;
using Model.Boss;
using Model.Card;
using Model.Inventory;
using Model.InventoryCard;
using Model.Zone;
using Presentation.Boss;
using Presentation.Entity;
using Presentation.Inventory;
using Presentation.TotalStats;
using Presentation.Zone;
using Services;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class GameEntryPoint : MonoBehaviour
{
    // --- Ссылки на View ---
    [SerializeField] private TotalCardStatsView totalCardStatsView;
    [SerializeField] private BossView bossView;
    [SerializeField] private ZoneView zoneView;
    [SerializeField] private InventoryView inventoryView;
    [SerializeField] private EntityView entityView;
        
    // --- Сервисы ---
    private CardLoaderService _cardLoaderService;

    // --- Доменные сущности ---
    private BossModel _bossModel;
    private ZoneModel _zoneModel;

    // [Header("UI Элементы")]
    // public Button startGameButton; // Кнопка старта игры
    // public Transform gameUI; // Панель с игровым UI (босс, зона и т.д.)
    // public Transform cardsToolbarUI; // Панель для отображения выбранных карт в бою
    // public Transform inventoryUI; // Панель с UI инвентаря
        
    //доменные сущности для инвентаря
    private List<IInventoryItem> _runtimeDomainItems = new();
    private InventoryModel _inventoryModel;
    private InventoryPresenter _inventoryPresenter;

    private void Awake()
    {
        // Инициализация сервиса и загрузка доменных карт
        _cardLoaderService = new CardLoaderService();
        var cards = _cardLoaderService.GetDomainCards();

        // Создание доменных сущностей
        _zoneModel = new ZoneModel { CurrentZone = { Value = 1 } };
        _bossModel = new BossModel { CurrentHp = { Value = 100f } };
            
        _inventoryModel = new InventoryModel();

        _inventoryPresenter = new InventoryPresenter(_inventoryModel, inventoryView);

        // foreach (var card in cards)
        // {
            // _runtimeDomainItems.Add(card);
            // new ItemPresenter().Init(card, itemView);
            _runtimeDomainItems.AddRange(cards);
            _inventoryModel.LoadItems(_runtimeDomainItems);
        // }
            
        _inventoryModel.LoadItems(_runtimeDomainItems);
            
        // // 2. ИНИЦИАЛИЗАЦИЯ ДРУГИХ ПРЕЗЕНТЕРОВ И VIEW 
        InitOtherUI(_bossModel, _zoneModel);
        //
        // // 3. НАСТРОЙКА КНОПКИ СТАРТА ИГРЫ
        // startGameButton.onClick.AddListener(StartGame);
    }
    
    void Start()
    {
        var allCards = new CardLoaderService().GetDomainCards();
        var dropService = new CardDropService(allCards);
        var drop = dropService.RollDrop();

        if (drop != null)
        {
            _inventoryPresenter.AddOrStackCard(drop);
        }
        
        StartBattle(_cardLoaderService.GetDomainCards());
    }

    private void StartGame()
    {
        // Скрываем ненужный UI
        // startGameButton.gameObject.SetActive(false);
        // inventoryUI.gameObject.SetActive(false);
        //
        // // Показываем UI боя
        // gameUI.gameObject.SetActive(true);
        // cardsToolbarUI.gameObject.SetActive(true); // Показываем тулбар для карт боя


        // ПОЛУЧАЕМ ВЫБРАННЫЕ КАРТЫ ИЗ МЕНЕДЖЕРА ИНВЕНТАРЯ
        // List<Card> selectedCards = cardInventoryManager.SelectedItems.ToList();
        //
        // if (selectedCards.Count > 0)
        // {
        //     Debug.Log($"Начинаем бой с {selectedCards.Count} картами.");
        //     // Создаем UI для выбранных карт в тулбаре
        //     InitUISelectedCardsToolbar(selectedCards);
        //     // Запускаем логику боя
        //     StartBattle(selectedCards);
        // }
        // else
        // {
        //     Debug.Log("Нет выбранных карт для боя.");
        // }
    }

    private void StartBattle(List<CardModel> userCardCollection)
    {
        // Создаем и запускаем скрипт боя
        var battleScript = new BattleScript(_bossModel, _zoneModel, userCardCollection, _inventoryPresenter);
        StartCoroutine(StartBattleLoop(battleScript));
    }

    // Инициализация UI, не связанного с инвентарем напрямую
    private void InitOtherUI(BossModel bossModel, ZoneModel zoneModel)
    {
        // Инициализация презентера для общих статов карт
        // var allCards = cardInventoryManager.SelectedItems.ToList(); // Получаем выбранные карты из менеджера
        // new TotalCardStatsPresenter().Init(allCards, totalCardStatsView);
        // Инициализация презентеров для босса и зоны
        new BossPresenter().Init(bossModel, bossView);
        new ZonePresenter().Init(zoneModel, zoneView);
    }

    // Создает UI в ТУЛБАРЕ БОЯ для карт, ВЫБРАННЫХ в инвентаре
    private void InitUISelectedCardsToolbar(List<CardModel> selectedCards)
    {
        // Создаем CardView (не InventoryItemView!) для каждой выбранной карты
        foreach (var card in selectedCards)
        {
            // Используем другой префаб и другой презентер CardPresenter
            // CardView newCardView = Instantiate(cardViewPrefab, cardsToolbarUI);
            // new CardPresenter().Init(card, newCardView);
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

        Debug.Log("Бой завершен!");
    }
    
    
    private void OnDisable()
    {
        Debug.LogError($"GameEntryPoint отключён! Объект: {gameObject.name}");
        Debug.LogError(new System.Diagnostics.StackTrace());
    }

}