using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    private Slider slider;

    float currentValue;
    float damagePerBullet = 0.01f;

    private void Start()
    {
        slider = GetComponent<Slider>();
    }


    public void DeductHealthBar()
    {
        currentValue = slider.value;
        slider.value = currentValue - damagePerBullet;

        if (slider.value <= 0)
        {
            PlayerDied();
        }

    }

    private void PlayerDied()
    {
        Debug.Log("Unfortunately, the player character died, exiting gameplay! ");
        Application.Quit();

        //In future adding code to navigate to main menu instead

    }

}
