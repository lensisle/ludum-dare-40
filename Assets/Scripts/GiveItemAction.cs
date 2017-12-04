using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GiveItemAction", menuName = "Data/Actions/Give Item", order = 3)]
public class GiveItemAction : BaseAction
{
    public ItemRatity ItemRatity;
    public string ForceItemName;
    private string _givenItem = "";

    public override void ExecuteAction(System.Action onFinish)
    {
        if (!string.IsNullOrEmpty(ForceItemName))
        {
            Item target = GameManager.Instance.ItemDatabase.Find(x => x.Name.Equals(ForceItemName));
            if (target == null)
            {
                Debug.Log("Item not found");
                return;
            }

            bool itemReceived = GameManager.Instance.Player.Inventory.AddItem(target);
            _givenItem = target.DisplayName + (itemReceived == false ? " but you already have one." : "");
        }
        else
        {
            if (GameManager.Instance.ItemDatabase == null || GameManager.Instance.ItemDatabase.Count < 1)
            {
                Debug.Log("Item database empty");
                return;
            }

            List<Item> targets = GameManager.Instance.ItemDatabase.FindAll(x => x.Rarity.Equals(ItemRatity));
            Item target = targets[Random.Range(0, targets.Count)];
            if (target == null)
            {
                Debug.Log("Item not found");
                return;
            }

            bool itemReceived =  GameManager.Instance.Player.Inventory.AddItem(target);

            _givenItem = target.DisplayName + (itemReceived == false ? " but you already have one." : "");
        }

        if(!string.IsNullOrEmpty(_givenItem))
        {
            UIManager.Instance.DialogueView.ShowText("You found: " + _givenItem, () =>
            {
                UIManager.Instance.DialogueView.Hide();
                if (onFinish != null)
                {
                    onFinish();
                }

            });
        }
    }
}
