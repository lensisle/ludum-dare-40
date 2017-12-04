using UnityEngine;

public class InventoryView : MonoBehaviour
{
    private float _closeTime = 0.5f;
    private bool _canClose;
    private float _closeCounter;

    public void Show()
    {
        if (UIManager.Instance.CurrentState != UIManagerState.Available)
        {
            Debug.Log("UImanager is not available");
            return;
        }

        _closeCounter = 0;
        _canClose = false;
        UIManager.Instance.SetState(UIManagerState.ShowingInventory);
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        UIManager.Instance.SetState(UIManagerState.Available);
        gameObject.SetActive(false);
    }

    private void Update()
    {
        if (UIManager.Instance.CurrentState == UIManagerState.ShowingInventory && !_canClose)
        {
            _closeCounter += Time.deltaTime;
            if (_closeCounter >= _closeTime)
            {
                _canClose = true;
                _closeCounter = 0;
            }
        }

        if (UIManager.Instance.CurrentState == UIManagerState.ShowingInventory && Input.GetKeyDown(KeyCode.X) && _canClose)
        {
            Hide();
        }
    }
}
