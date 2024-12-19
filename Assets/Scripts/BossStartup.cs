using System;
using Models;

public class BossStartup : Singleton<BossStartup>
{
    public BossModel Boss = new();


    private void Start()
    {
        _initBoss();
    }


    private void _initBoss()
    {
        Boss = new BossModel
        {
            CurrentHP = 250f,
            Attack = 10f,
            ExpReward = 50,
            GoldReward = 1
        };
            
        // UpdateBossStatAndRewards_UI();
        // UpdateBossSliderHP_UI();
    }
}