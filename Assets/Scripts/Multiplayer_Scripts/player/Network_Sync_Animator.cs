using UnityEngine;
using Unity.Netcode.Components;

public class Network_Sync_Animator : NetworkAnimator
{
    private bool Enable = true;

    protected override bool OnIsServerAuthoritative()
    {
        return false;
    }

    public void DisableForWalking()
    {
    }

    public void Renable()
    {
        Enable = true;
    }

}
