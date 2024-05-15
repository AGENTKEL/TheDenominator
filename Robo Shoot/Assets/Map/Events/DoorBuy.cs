using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DoorBuy : MonoBehaviour
{
    public Animator anim;

    public GameObject choiceUI;
    public TextMeshProUGUI buyPriceText;
    public GameObject gunPickupSound;

    public int doorPrice = 100;
    private bool inStoreZone = false;
    private bool isDoorUnlocked = false;
    public bool isFinalDoor = false;

    private PlayerMoney playerMoney;



    void Start()
    {
        playerMoney = GameObject.FindGameObjectWithTag("PlayerMoney").GetComponent<PlayerMoney>();
        anim = GetComponent<Animator>(); 
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!isDoorUnlocked)
        {
            if (other.CompareTag("Player"))
            {
                inStoreZone = true;
                choiceUI.SetActive(true);
                buyPriceText.text = "'E' открыть за " + doorPrice + "$";
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (!isDoorUnlocked)
        {
            if (other.CompareTag("Player"))
            {
                inStoreZone = false;
                choiceUI.SetActive(false);
            }
        }
        
    }

    // Update is called once per frame
    private void Update()
    {
        if (inStoreZone)
        {
            if(!isDoorUnlocked)
            {
                if (Input.GetKeyDown(KeyCode.E))
                {
                    BuyWeapon(doorPrice);
                }
            }
        }
    }

    private void BuyWeapon(int price)
    {
        if (playerMoney.GetCurrentMoney() >= price)
        {
            // Deduct money from player
            playerMoney.DeductMoney(price);
            Instantiate(gunPickupSound);
            isDoorUnlocked = true;
            anim.SetBool("Near", isDoorUnlocked);
            if (isFinalDoor)
            {
                EndGame();
            }

            choiceUI.SetActive(false);
        }
    }

    public void EndGame()
    {
        SceneManager.LoadScene("Menu");
    }


}
