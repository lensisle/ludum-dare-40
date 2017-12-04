using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionEvent : MonoBehaviour
{
    [SerializeField]
    private BaseAction _action;

    [SerializeField]
    private GameObject _container;

    [SerializeField]
    private bool _destroyOnTrigger;

    private bool _canPressAction;
    private string _colliderName;

    void OnTriggerEnter2D(Collider2D other)
    {
        _canPressAction = true;
        _colliderName = other.name;
        GameManager.Instance.Player.Exclamation.gameObject.SetActive(true);
    }

    private void Update()
    {
        if (_canPressAction)
        {
            if (Input.GetKeyDown(KeyCode.Z) && _colliderName == "Player")
            {
                if (_destroyOnTrigger)
                {
                    _action.ExecuteAction(() => {

                        _container.SetActive(false);
                        GameManager.Instance.Player.Exclamation.gameObject.SetActive(false);

                    });
                }
                else
                {
                    _action.ExecuteAction();
                }
            }
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        _canPressAction = false;
        _colliderName = other.name;
        GameManager.Instance.Player.Exclamation.gameObject.SetActive(false);
    }
}
