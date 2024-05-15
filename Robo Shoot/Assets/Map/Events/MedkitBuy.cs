using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MedkitBuy : MonoBehaviour
{
    public GameObject choiceUI;
    public GameObject equationUI;
    public TextMeshProUGUI buyPriceText;
    public TextMeshProUGUI equationText;
    public GameObject healSound;

    public int healPrice = 100;
    private bool inStoreZone = false;
    private bool discountUnlocked = false;
    private bool discountUIOn = false;
    private bool discountActive = true;

    public string[] equations;
    public int[] equationAnswers;
    private int currentEquationIndex;

    private PlayerMoney playerMoney;
    public TMP_InputField answerInputField;
    ActionStateManager playerWeaponState;
    PlayerHealth playerHealth;
    MovementStateManager playerControl;
    AimStateManager playerLook;

    public WeaponManager pistolManager;
    public WeaponManager akManager;
    public WeaponManager shotgunManager;
    public WeaponManager grenadeLauncherManager;
    public WeaponManager smgManager;
    public WeaponManager dmrManager;
    public WeaponManager railgunManager;


    private void Awake()
    {
        pistolManager = GameObject.Find("Pistol").GetComponentInChildren<WeaponManager>();
        akManager = GameObject.Find("AkRifle").GetComponentInChildren<WeaponManager>();
        shotgunManager = GameObject.Find("Shotgun").GetComponentInChildren<WeaponManager>();
        grenadeLauncherManager = GameObject.Find("Grenade Launcher").GetComponentInChildren<WeaponManager>();
        smgManager = GameObject.Find("SMG").GetComponentInChildren<WeaponManager>();
        dmrManager = GameObject.Find("DMR").GetComponentInChildren<WeaponManager>();
        railgunManager = GameObject.Find("Railgun").GetComponentInChildren<WeaponManager>();
    }

    void Start()
    {
        playerMoney = GameObject.FindGameObjectWithTag("PlayerMoney").GetComponent<PlayerMoney>();
        playerWeaponState = GameObject.FindGameObjectWithTag("Player").GetComponent<ActionStateManager>();
        playerControl = GameObject.FindGameObjectWithTag("Player").GetComponent<MovementStateManager>();
        playerLook = GameObject.FindGameObjectWithTag("Player").GetComponent<AimStateManager>();
        playerHealth = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerHealth>();
    }

    private void OnTriggerEnter(Collider other)
    {

        if (other.CompareTag("Player"))
        {
            inStoreZone = true;
            choiceUI.SetActive(true);
            buyPriceText.text = "'E' купить за " + healPrice + "$";
        }
    }

    private void OnTriggerExit(Collider other)
    {


        if (other.CompareTag("Player"))
        {
            inStoreZone = false;
            choiceUI.SetActive(false);
        }


    }

    // Update is called once per frame
    private void Update()
    {
        if (playerHealth.playerIsAlive == false)
        {
            enabled = false;
        }
        if (inStoreZone)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                if (playerHealth.currentHealth < playerHealth.maxHealth)
                {
                    BuyHealth(healPrice);
                }
            }

            if (discountActive)
            {
                if (Input.GetKeyDown(KeyCode.T))
                {
                    discountUIOn = true;
                    OpenDiscountWindow();
                    Cursor.visible = true;
                    Cursor.lockState = CursorLockMode.Confined;
                    if (playerControl && playerLook != null)
                    {
                        playerLook.enabled = false;
                        playerControl.enabled = false;
                    }
                    if (pistolManager != null)
                        pistolManager.enabled = false;
                    if (akManager != null)
                        akManager.enabled = false;
                    if (shotgunManager != null)
                        shotgunManager.enabled = false;
                    if (grenadeLauncherManager != null)
                        grenadeLauncherManager.enabled = false;
                    if (smgManager != null)
                        smgManager.enabled = false;
                    if (dmrManager != null)
                        dmrManager.enabled = false;
                    if (railgunManager != null)
                        railgunManager.enabled = false;

                    if (playerWeaponState != null)
                    {
                        playerWeaponState.enabled = false;
                    }
                }
            }

            if (discountUIOn == true)
            {
                if (Input.GetKeyDown(KeyCode.Escape))
                {

                    choiceUI.SetActive(true);
                    equationUI.gameObject.SetActive(false);
                    Cursor.visible = false;
                    Cursor.lockState = CursorLockMode.Locked;

                    if (playerControl && playerLook != null)
                    {
                        playerLook.enabled = true;
                        playerControl.enabled = true;
                    }
                    if (pistolManager != null)
                        pistolManager.enabled = true;
                    if (akManager != null)
                        akManager.enabled = true;
                    if (shotgunManager != null)
                        shotgunManager.enabled = true;
                    if (grenadeLauncherManager != null)
                        grenadeLauncherManager.enabled = true;
                    if (smgManager != null)
                        smgManager.enabled = true;
                    if (dmrManager != null)
                        dmrManager.enabled = true;
                    if (railgunManager != null)
                        railgunManager.enabled = true;

                    if (playerWeaponState != null)
                    {
                        playerWeaponState.enabled = true;
                    }
                    discountUIOn = false;
                }
            }
        }
    }

    private void BuyHealth(int price)
    {
        if (playerMoney.GetCurrentMoney() >= price)
        {
            // Deduct money from player
            playerMoney.DeductMoney(price);
            Instantiate(healSound);
            playerHealth.Heal(20);
            


        }
    }

    private void OpenDiscountWindow()
    {
        if (!discountUnlocked)
        {
            currentEquationIndex = Random.Range(0, equations.Length); // Select a random equation
            equationText.text = $"Решите: {equations[currentEquationIndex]} = ?";
            choiceUI.SetActive(false);
            equationUI.gameObject.SetActive(true);
        }
    }

    public void CheckDiscountEquation()
    {


        string answer = answerInputField.text; // Get the text from the input field
        int parsedAnswer;

        if (currentEquationIndex >= 0 && currentEquationIndex < equationAnswers.Length)
        {
            if (int.TryParse(answer, out parsedAnswer))
            {
                if (parsedAnswer == equationAnswers[currentEquationIndex])
                {
                    equationUI.SetActive(false);
                    choiceUI.SetActive(true);
                    Instantiate(healSound);
                    if (playerHealth.currentHealth < playerHealth.maxHealth)
                    {
                        playerHealth.Heal(20);
                    }


                    Cursor.visible = false;
                    Cursor.lockState = CursorLockMode.Locked;

                    if (playerControl && playerLook != null)
                    {
                        playerLook.enabled = true;
                        playerControl.enabled = true;
                    }
                    if (pistolManager != null)
                        pistolManager.enabled = true;
                    if (akManager != null)
                        akManager.enabled = true;
                    if (shotgunManager != null)
                        shotgunManager.enabled = true;
                    if (grenadeLauncherManager != null)
                        grenadeLauncherManager.enabled = true;
                    if (smgManager != null)
                        smgManager.enabled = true;
                    if (dmrManager != null)
                        dmrManager.enabled = true;
                    if (railgunManager != null)
                        railgunManager.enabled = true;

                    if (playerWeaponState != null)
                    {
                        playerWeaponState.enabled = true;
                    }
                    discountUIOn = false;
                }
                else
                {
                    // TODO: Display a message indicating the answer is incorrect
                }
            }
        }



    }
}
