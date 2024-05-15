using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private Image _healthBarSprite;
    [SerializeField] private float _reduceSpeed = 2;
    [SerializeField] private Gradient _healthBarGradient;
    private float _target = 1f;

    private void Start()
    {
        _healthBarSprite = GetComponent<Image>();

        CheckHealtBarGradientAmount();

    }

    public void UpdateHealthBar(float maxHealth, float currentHealth)
    {
        _target = currentHealth / maxHealth;

        CheckHealtBarGradientAmount();
    }
    private void Update()
    {
        _healthBarSprite.fillAmount = Mathf.MoveTowards(_healthBarSprite.fillAmount, _target, _reduceSpeed * Time.deltaTime);
    }

    private void CheckHealtBarGradientAmount()
    {
        _healthBarSprite.color = _healthBarGradient.Evaluate(_target);
    }
}
