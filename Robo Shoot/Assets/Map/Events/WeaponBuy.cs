using TMPro;
using UnityEngine;
using UnityEngine.LowLevel;
using UnityEngine.UI;

public class WeaponBuy : MonoBehaviour
{
    public GameObject choiceUI;
    public GameObject equationUI;
    public TextMeshProUGUI discountText;
    public TextMeshProUGUI equationText;
    public TextMeshProUGUI weaponPriceText;

    public GameObject storeObject;
    public GameObject storeWeaponModel;
    public GameObject gunPickupSound;
    public GameObject weaponToActivate;
    public GameObject weaponToDeactivate1;
    public GameObject weaponToDeactivate2;
    public GameObject weaponToDeactivate3;
    public GameObject weaponToDeactivate4;
    public GameObject weaponToDeactivate5;
    public GameObject weaponToDeactivate6;

    public int weaponPrice = 100;
    public int discountedWeaponPrice = 70;
    public int discountEquationResult = 5; // The result of the equation to get the discount

    private bool inStoreZone = false;
    private bool discountUnlocked = false;
    private bool discountUIOn = false;
    private bool discountActive = true;

    public bool isBuyAk;
    public bool isBuyShotgun;
    public bool isBuyGrenadeLauncher;
    public bool isBuySmg;
    public bool isBuyDmr;
    public bool isBuyRailgun;

    public string[] equations;
    public int[] equationAnswers;
    private int currentEquationIndex;

    private PlayerMoney playerMoney;
    public TMP_InputField answerInputField;

    MovementStateManager playerControl;
    AimStateManager playerLook;
    ActionStateManager playerActions;
    PlayerHealth playerHealth;

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

    private void Start()
    {
        playerMoney = GameObject.FindGameObjectWithTag("PlayerMoney").GetComponent<PlayerMoney>();
        playerControl = GameObject.FindGameObjectWithTag("Player").GetComponent<MovementStateManager>();
        playerLook = GameObject.FindGameObjectWithTag("Player").GetComponent<AimStateManager>();
        playerActions = GameObject.FindGameObjectWithTag("Player").GetComponent<ActionStateManager>();
        playerHealth = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerHealth>();
        equationUI.gameObject.SetActive(false);
        currentEquationIndex = -1;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            inStoreZone = true;
            choiceUI.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            inStoreZone = false;
            choiceUI.SetActive(false);
            equationUI.gameObject.SetActive(false);
        }
    }

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
                BuyWeapon(discountUnlocked ? discountedWeaponPrice : weaponPrice);
            }
            if (discountActive)
            {
                weaponPriceText.text = "'E' купить за " + weaponPrice + "$";
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

                    if (playerActions != null)
                    {
                        playerActions.enabled = false;
                    }
                }
            }

            if (!discountActive)
            {
                weaponPriceText.text = "'E' купить за " + discountedWeaponPrice + "$";
            }

            if ( discountUIOn == true )
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

                    if (playerActions != null)
                    {
                        playerActions.enabled = true;
                    }
                    discountUIOn = false;
                }
            }
        }
    }

    private void BuyWeapon(int price)
    {
        if (playerMoney.GetCurrentMoney() >= price)
        {
            // Deduct money from player and give them the weapon
            playerMoney.DeductMoney(price);
            storeWeaponModel.SetActive(false);
            Instantiate(gunPickupSound);
            weaponToActivate.SetActive(true);
            weaponToDeactivate1.SetActive(false);
            weaponToDeactivate2.SetActive(false);
            weaponToDeactivate3.SetActive(false);
            weaponToDeactivate4.SetActive(false);
            weaponToDeactivate5.SetActive(false);
            weaponToDeactivate6.SetActive(false);

            #region Weapon Bools
            if (isBuyAk)
            {
                playerActions.isAk = true;
                playerActions.isPistol = false;
                playerActions.isShotgun = false;
                playerActions.isGrenadeLauncher = false;
                playerActions.isSmg = false;
                playerActions.isDmr = false;
                playerActions.isRailgun = false;
            }

            if (isBuyShotgun)
            {
                playerActions.isShotgun = true;
                playerActions.isAk = false;
                playerActions.isPistol = false;
                playerActions.isGrenadeLauncher = false;
                playerActions.isSmg = false;
                playerActions.isDmr = false;
                playerActions.isRailgun = false;
            }
            if (isBuyGrenadeLauncher)
            {
                playerActions.isGrenadeLauncher = true;
                playerActions.isShotgun = false;
                playerActions.isAk = false;
                playerActions.isPistol = false;
                playerActions.isSmg = false;
                playerActions.isDmr = false;
                playerActions.isRailgun = false;
            }
            if (isBuySmg)
            {
                playerActions.isSmg = true;
                playerActions.isShotgun = false;
                playerActions.isAk = false;
                playerActions.isPistol = false;
                playerActions.isGrenadeLauncher = false;
                playerActions.isDmr = false;
                playerActions.isRailgun = false;
            }
            if (isBuyDmr)
            {
                playerActions.isDmr = true;
                playerActions.isShotgun = false;
                playerActions.isAk = false;
                playerActions.isPistol = false;
                playerActions.isGrenadeLauncher = false;
                playerActions.isSmg = false;
                playerActions.isRailgun = false;
            }
            if (isBuyRailgun)
            {
                playerActions.isRailgun = true;
                playerActions.isShotgun = false;
                playerActions.isAk = false;
                playerActions.isPistol = false;
                playerActions.isGrenadeLauncher = false;
                playerActions.isSmg = false;
                playerActions.isDmr = false;
            }
            #endregion
            // TODO: Give the player the weapon

            storeObject.SetActive(false);
            choiceUI.SetActive(false);
        }
        else
        {
            
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
                    discountUnlocked = true;
                    discountActive = false;
                    equationUI.SetActive(false);
                    discountText.gameObject.SetActive(false);
                    choiceUI.SetActive(true);

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

                    if (playerActions != null)
                    {
                        playerActions.enabled = true;
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
