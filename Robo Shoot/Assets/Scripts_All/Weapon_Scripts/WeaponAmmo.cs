using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class WeaponAmmo : MonoBehaviour
{
    public int clipSize;
    public int extraAmmo;
    public int currentAmmo;

    public AudioClip magInSound;
    public AudioClip magOutSound;
    public AudioClip releaseSlideSound;

    public TextMeshProUGUI ammoCounterText;


    void Start()
    {
        currentAmmo = clipSize;
    }
    private void Update()
    {
        UpdateAmmoCounter();
    }


    public void Reload()
    {
        if(extraAmmo >= clipSize)
        {
            int ammoToReload = clipSize - currentAmmo;
            extraAmmo -= ammoToReload;
            currentAmmo += ammoToReload;
        }
        else if(extraAmmo > 0)
        {
            if(extraAmmo + currentAmmo > clipSize)
            {
                int leftOverAmmo = extraAmmo  + currentAmmo - clipSize;
                extraAmmo = leftOverAmmo;
                currentAmmo = clipSize;
            }
            else
            {
                currentAmmo += extraAmmo;
                extraAmmo = 0;
            }
        }
    }

    public void UpdateAmmoCounter()
    {
        ammoCounterText.text = $"{currentAmmo} / {extraAmmo}";
    }

}
