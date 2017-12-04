using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GiveItemAction", menuName = "Data/Actions/Give Item", order = 3)]
public class GiveItemAction : BaseAction
{
    public string ForceItemName;
    private string _givenItem = "";

    public override void ExecuteAction()
    {
        if (!string.IsNullOrEmpty(ForceItemName))
        {
            Item target = GameManager.Instance.ItemDatabase.Find(x => x.Name.Equals(ForceItemName));
            if (target == null)
            {
                Debug.Log("Item not found");
                return;
            }

            GameManager.Instance.Player.Inventory.AddItem(target);
            _givenItem = target.DisplayName;
        }
        else
        {
            if (GameManager.Instance.ItemDatabase == null || GameManager.Instance.ItemDatabase.Count < 1)
            {
                Debug.Log("Item database empty");
                return;
            }

            int maxCount = GameManager.Instance.ItemDatabase.Count;
            Item target = GameManager.Instance.ItemDatabase[Random.Range(0, maxCount)];
            if (target == null)
            {
                Debug.Log("Item not found");
                return;
            }

            GameManager.Instance.Player.Inventory.AddItem(target);

            _givenItem = target.DisplayName;
        }

        if(!string.IsNullOrEmpty(_givenItem))
        {
            UIManager.Instance.DialogueView.ShowText("You found a " + _givenItem, () =>
            {
                UIManager.Instance.DialogueView.Hide();
            });
        }
    }
}
