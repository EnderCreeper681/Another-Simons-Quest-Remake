using System;

[Serializable]
public class PlayerData
{
    public int currentHearts;
    public string currentScene;
    public int level;
    public float[] position;
    public float strength;
    public int constitution;
    public float intelligence;
    public float currentExperience;
    public float nextExperience;
    public int gold;
    public int maxHearts;
    public float maxHealth;
    public string[] relics;
    public string[] subweapons;
    public string[] pickups;
    public string[] defeatedBosses;


    public PlayerData(Stats stats, Attacking attacking, Saving saving)
    {
        level = stats.level;
        currentExperience = stats.currentExperience;
        nextExperience = stats.nextExperience;

        strength = stats.strength;
        constitution = stats.constitution;
        intelligence = stats.intelligence;
        
        gold = stats.gold;

        maxHealth = stats.maxHealth;

        currentHearts = stats.currentHearts;
        maxHearts = stats.maxHearts;

        currentScene = stats.currentScene;

        relics = stats.relics.ToArray();
        subweapons = attacking.subweapons.ToArray();
        pickups = stats.pickups.ToArray();

        defeatedBosses = saving.defeatedBosses.ToArray();
        
        position = new float[3];
        position[0] = stats.transform.position.x;
        position[1] = stats.transform.position.y;
        position[2] = 0;
    }
}
