using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class HealthBar : MonoBehaviour
{
    private Image healthBarImage;

    void Start()
    {
        healthBarImage =  GetComponent<Image>();
    }

    public void UpdateLife(float playerCurrentLife)
    {
        healthBarImage.fillAmount = Mathf.Clamp(playerCurrentLife / 100, 0, 1f);
    }
}
