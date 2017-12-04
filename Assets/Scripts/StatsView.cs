using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatsView : MonoBehaviour
{
    [SerializeField]
    private GameObject _playerStatsContainer;
    public GameObject PlayerStatsContainer
    {
        get
        {
            return _playerStatsContainer;
        }
    }

    [SerializeField]
    private Text _stat1Value;

    [SerializeField]
    private Text _stat2Value;

    [SerializeField]
    private Text _stat3Value;

    [SerializeField]
    private Image _fillBar;

    [SerializeField]
    private Text puppyCounter;

    public void FillColdBar(float fillValue)
    {
        _fillBar.fillAmount = fillValue;
    }

    public void SetPuppyCounter(int counter)
    {
        puppyCounter.text = (GameManager.Instance.PuppiesGoal - counter).ToString() + " left";
    }

    public void UpdateStat(int number, int value)
    {
        if (number == 1)
        {
            _stat1Value.text = "+ " + value.ToString();
        }
        else if (number == 2)
        {
            _stat2Value.text = "+ " +  value.ToString();
        }
        else if (number == 3)
        {
            _stat3Value.text = "+ " +  value.ToString();
        }
    }
}
