using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu(fileName = "GivePuppyAction", menuName = "Data/Actions/Give Puppy", order = 4)]
public class GivePuppyAction : BaseAction
{
    public override void ExecuteAction(Action onFinish)
    {
        UIManager.Instance.DialogueView.ShowText("You have found a puppy! She's almost frozen but looks good", () =>
        {
            UIManager.Instance.DialogueView.ShowText("You put her inside your backpack. There are more to find out there!", () =>
            {
                UIManager.Instance.DialogueView.Hide();

                int currentPuppies = GameManager.Instance.Player.PuppiesCount;
                GameManager.Instance.Player.SetPuppiesCount(currentPuppies + 1);

                if (onFinish != null)
                {
                    onFinish();
                }

            });
        });
    }
}
