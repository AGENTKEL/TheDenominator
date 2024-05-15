using TMPro;
using UnityEngine;

public class PlayerMoney : MonoBehaviour
{
    public int initialMoney = 500;
    public int currentMoney;

    public TextMeshProUGUI playerPistolMoneyText;
    public TextMeshProUGUI playerAKMoneyText;


    private void Awake()
    {
        currentMoney = initialMoney;
    }
    private void Update()
    {
        UpdateMoneyText();
    }

    public int GetCurrentMoney()
    {
        return currentMoney;
    }

    public void DeductMoney(int amount)
    {
        currentMoney -= amount;
    }

    public void AddMoney(int amount)
    {
        currentMoney += amount;
    }

    public void UpdateMoneyText()
    {
        if (playerPistolMoneyText != null)
        {
            playerPistolMoneyText.text = currentMoney.ToString() + "$";
        }

        if (playerAKMoneyText != null)
        {
            playerAKMoneyText.text = currentMoney.ToString() + "$";
        }
    }
}
