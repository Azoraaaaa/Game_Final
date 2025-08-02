using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[CreateAssetMenu]
public class ItemSO : ScriptableObject
{
    public string itemName;
    public StatToChange statToChange = new StatToChange();
    public int amountToChangeStat;

    public AttributeToChange attributeToChange = new AttributeToChange();
    public float amountToChangeAttribute;

    public bool UseItem()
    {
        if (statToChange == StatToChange.Health)
        {
            
            if (PlayerHealthSystem.instance.currentHealth == PlayerHealthSystem.instance.maxHealth)
            {
                return false;
            }
            else
            {
                PlayerHealthSystem.instance.currentHealth = PlayerHealthSystem.instance.currentHealth + amountToChangeStat;
                return true;
            }
            
        }
        else if (statToChange == StatToChange.maxHealth1)
        {
                PlayerHealthSystem.instance.maxHealth = amountToChangeStat;
                return true;
        }
        else if (statToChange == StatToChange.maxEnergy1)
        {
            PlayerHealthSystem.instance.maxSkillPoints = PlayerHealthSystem.instance.maxSkillPoints + amountToChangeStat;
            return true;
        }
        else if (statToChange == StatToChange.maxHealth2)
        {
            PlayerHealthSystem.instance.maxHealth = amountToChangeStat;
            return true;
        }
        else if (statToChange == StatToChange.maxEnergy2)
        {
            PlayerHealthSystem.instance.maxSkillPoints = PlayerHealthSystem.instance.maxSkillPoints + amountToChangeStat;
            return true;
        }
        else if (attributeToChange == AttributeToChange.BombThrow)
        {
            /*
            PlayerSpeedController.instance.BoostSpeed(amountToChangeAttribute, 5f);
            Debug.Log("Speed Boosted");
            return true;
            */
        }

        return false;
    }
    public enum StatToChange
    {
        none,
        Health,
        maxHealth1,
        maxEnergy1,
        maxHealth2,
        maxEnergy2,
        SwordDamage
    };
    public enum AttributeToChange
    {
        none,
        BombThrow
    };
}
