using UnityEngine;

public interface IProperties
{

}

public class MonoProperties : MonoBehaviour, IProperties
{
    public virtual int GetMaxHP()
    {
        return 12;
    }
    public virtual int GetCurrentHP()
    {
        return 12;
    }
    public virtual int GetMaxStamina()
    {
        return 12;
    }
    public virtual int GetCurrentStamina()
    {
        return 12;
    }
    public virtual float GetStaminaRegen()
    {
        return 12f;
    }

    public virtual int Heal(int _recover)
    {
        return 0;
    }



    public virtual void SetCurrentStamina(int _stam)
    {

    }

    public virtual void Reset()
    {

    }
}
