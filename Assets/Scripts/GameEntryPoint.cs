using System.Collections;
using System.Collections.Generic;
using Battle;
using Domain.Entities;
using Domain.Interfaces;
using Presentation.Boss;
using Presentation.Inventory;
using Presentation.TotalStats;
using Presentation.Zone;
using Services;
using UnityEngine;
using UnityEngine.UI;

public class GameEntryPoint : MonoBehaviour
{
    // --- Ссылки на View ---
    [SerializeField] private TotalCardStatsView totalCardStatsView;
    [SerializeField] private BossView bossView;
    [SerializeField] private ZoneView zoneView;
    [SerializeField] private InventoryView inventoryView;
    [SerializeField] private ItemView itemView;
        
    // --- Сервисы ---
    private CardLoaderService _cardLoaderService;

    // --- Доменные сущности ---
    private Boss _boss;
    private Zone _zone;

    [Header("UI Элементы")]
    public Button startGameButton; // Кнопка старта игры
    public Transform gameUI; // Панель с игровым UI (босс, зона и т.д.)
    public Transform cardsToolbarUI; // Панель для отображения выбранных карт в бою
    public Transform inventoryUI; // Панель с UI инвентаря
        
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
        _zone = new Zone { CurrentZone = { Value = 1 } };
        _boss = new Boss { CurrentHp = { Value = 100f } };
            
        _inventoryModel = new InventoryModel();

        _inventoryPresenter = new InventoryPresenter(_inventoryModel, inventoryView);

        foreach (var card in cards)
        {
            _runtimeDomainItems.Add(card);
            new ItemPresenter().Init(card, itemView);

        }
            
        _inventoryModel.LoadItems(_runtimeDomainItems);
            
        // // 2. ИНИЦИАЛИЗАЦИЯ ДРУГИХ ПРЕЗЕНТЕРОВ И VIEW 
        InitOtherUI(_boss, _zone);
        //
        // // 3. НАСТРОЙКА КНОПКИ СТАРТА ИГРЫ
        startGameButton.onClick.AddListener(StartGame);
    }

    private void StartGame()
    {
        // Скрываем ненужный UI
        startGameButton.gameObject.SetActive(false);
        inventoryUI.gameObject.SetActive(false);

        // Показываем UI боя
        gameUI.gameObject.SetActive(true);
        cardsToolbarUI.gameObject.SetActive(true); // Показываем тулбар для карт боя


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

    private void StartBattle(List<Card> selectedCards)
    {
        // Создаем и запускаем скрипт боя
        var battleScript = new BattleScript(_boss, _zone, selectedCards);
        StartCoroutine(StartBattleLoop(battleScript));
    }

    // Инициализация UI, не связанного с инвентарем напрямую
    private void InitOtherUI(Boss boss, Zone zone)
    {
        // Инициализация презентера для общих статов карт
        // var allCards = cardInventoryManager.SelectedItems.ToList(); // Получаем выбранные карты из менеджера
        // new TotalCardStatsPresenter().Init(allCards, totalCardStatsView);
        // Инициализация презентеров для босса и зоны
        new BossPresenter().Init(boss, bossView);
        new ZonePresenter().Init(zone, zoneView);
    }

    // Создает UI в ТУЛБАРЕ БОЯ для карт, ВЫБРАННЫХ в инвентаре
    private void InitUISelectedCardsToolbar(List<Card> selectedCards)
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
}