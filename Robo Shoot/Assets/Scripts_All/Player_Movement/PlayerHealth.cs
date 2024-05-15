using UnityEngine;
using UnityEngine.ProBuilder;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using Unity.VisualScripting;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 100;
    public int currentHealth;
    Ragdoll ragdoll;
    public bool playerIsAlive = true;
    CameraManager cameraManager;
    HUD gameOverScreen;
    Animator anim;

    public AudioSource playerDeathSound;
    public List<AudioClip> playerSounds;
    public int pos;
    SoundManager playerDamageSound;

    [SerializeField] private HealthBar _healthBar;

    PlayerShake cameraShake;


    private void Start()
    {
        playerDamageSound = GetComponent<SoundManager>();
        cameraShake = GetComponentInChildren<PlayerShake>();
        anim = GetComponent<Animator>();
        ragdoll = GetComponent<Ragdoll>();
        currentHealth = maxHealth;
        cameraManager = FindObjectOfType<CameraManager>();
        gameOverScreen = FindObjectOfType<HUD>();
        playerDeathSound = GetComponent<AudioSource>();
        _healthBar.UpdateHealthBar(maxHealth, currentHealth);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Projectile"))
        {
            TakeDamage(other.GetComponent<Projectile>().damage);
            _healthBar.UpdateHealthBar(maxHealth, currentHealth);
            anim.SetTrigger("TakeDamage");
        }

        if (other.CompareTag("MonsterClaw"))
        {
            TakeDamage(other.GetComponent<MonsterProjectile>().damage);
            _healthBar.UpdateHealthBar(maxHealth, currentHealth);
            anim.SetTrigger("TakeDamage");
        }

        if (other.CompareTag("Medkit"))
        {
            
            _healthBar.UpdateHealthBar(maxHealth, currentHealth);
        }
    }

    public void TakeDamage(int damage)
    {
        if(currentHealth > 0)
        {
            currentHealth -= damage;
            cameraShake.ShakeCamera();
            playerDamageSound.PlaySound2();

            if (currentHealth <= 0)
            {
                Die();
            }
        }
    }

    public void Heal(int amount)
    {
        currentHealth += amount;
        _healthBar.UpdateHealthBar(maxHealth, currentHealth);

        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }
    }

    private void Die()
    {
        playerIsAlive = false;
        ragdoll.ActivateRagdoll();
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Confined;
        PlayerDeathSound();


        MovementStateManager playerControl = GetComponent<MovementStateManager>();
        AimStateManager playerLook = GetComponent<AimStateManager>();
        WeaponManager playerShoot = GetComponentInChildren<WeaponManager>();
        if (playerControl && playerLook != null)
        {
            playerLook.enabled = false;
            playerControl.enabled = false;
        }
        if (playerShoot != null)
        {
            playerShoot.enabled = false;
        }


        cameraManager.EnableKillCam();

        gameOverScreen.gameOver();
    }

    public void PlayerDeathSound()
    {
        pos = (int)Mathf.Floor(Random.Range(0, playerSounds.Count));
        playerDeathSound.PlayOneShot(playerSounds[pos]);
    }
}
