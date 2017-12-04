using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DialogueAction", menuName = "Data/Actions/Dialogue", order = 1)]
public class DialogueAction : BaseAction
{
    [SerializeField]
    private string _text;

    [SerializeField]
    private bool _closeDialogueOnFinish;
    
    public override void ExecuteAction(System.Action onFinish)
    {
        UIManager.Instance.DialogueView.ShowText(_text, () =>
        {
            if (_closeDialogueOnFinish)
            {
                UIManager.Instance.DialogueView.Hide();
            }

            if (ChildAction != null)
            {
                ChildAction.ExecuteAction();
            }

        });
    }
}
