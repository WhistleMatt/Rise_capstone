using UnityEngine;
using Unity.Netcode.Components;

public class Network_Sync_Animator : NetworkAnimator
{
    protected override bool OnIsServerAuthoritative()
    {
        return false;
    }
}
