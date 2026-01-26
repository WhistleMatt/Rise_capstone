using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//Nicolas Chatziargiriou
public class HitBoxLogic : MonoBehaviour
{
    public HitBoxController.ePlayer id;
    [SerializeField] private PlayerStatsController playerStatsController;
    [SerializeField] private Multiplayer_Enemy_Stat_Controller playerEnemyStatsController;

    public PlayerStatsController GetSingleStats()
    {
        return playerStatsController;
    }

    public Multiplayer_Enemy_Stat_Controller GetMultiStats()
    {
        return playerEnemyStatsController;
    }


}
