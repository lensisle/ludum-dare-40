using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum UIManagerState
{
    ShowingText,
    Available,
    WaitingResponse,
    ShowingInventory
}

public class UIManager : MonoBehaviour
{
    private static UIManager _instance;
    public static UIManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<UIManager>();
            }

            if (_instance == null)
            {
                GameObject game = new GameObject("UIManager");
                _instance = game.AddComponent<UIManager>();
            }

            return _instance;
        }
    }

    [SerializeField]
    private UIManagerState _currentState;
    public UIManagerState CurrentState
    {
        get
        {
            return _currentState;
        }
    }

    [SerializeField]
    private InventoryView _inventoryView;
    public InventoryView InventoryView
    {
        get
        {
            return _inventoryView;
        }
    }

    [SerializeField]
    private DialogueView _dialogueView;
    public DialogueView DialogueView
    {
        get
        {
            return _dialogueView;
        }
    }

    [SerializeField]
    private StatsView _statsView;
    public StatsView StatsView
    {
        get
        {
            return _statsView;
        }
    }

    private void Start()
    {
        _currentState = UIManagerState.Available;
        _inventoryView.gameObject.SetActive(false);
        _dialogueView.gameObject.SetActive(false);
        _statsView.gameObject.SetActive(true);
    }

    public void SetState(UIManagerState state)
    {
        _currentState = state;
    }
}
