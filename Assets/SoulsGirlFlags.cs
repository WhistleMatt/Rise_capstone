using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoulsGirlFlags : MonoBehaviour
{
    private bool beforeDialogueEvent = false;
    private bool afterDialogueEvent = false;
   
    

   public void setFlagBDETrue()
    {
        beforeDialogueEvent = true;
    }
    public void setFlagADETrue()
    {
        afterDialogueEvent = true;
    }

    public void setFlagBDEfalse()
    {
        beforeDialogueEvent = false;
    }

    public void setFlagADEfalse()
    {
        afterDialogueEvent = false;
    }

    public bool getFlagBDE()
    {
        return beforeDialogueEvent;
    }
    public bool getFlagADE()
    {
        return afterDialogueEvent;
    }
}
