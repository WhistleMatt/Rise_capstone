using UnityEngine;

public class SetActive_Child_callable : MonoBehaviour
{
    public void ChildCallSetActive()
    {
        gameObject.SetActive(false);
    }
}
