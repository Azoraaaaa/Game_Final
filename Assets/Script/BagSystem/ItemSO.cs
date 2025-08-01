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
            /*
            if (PlayerHealthController.instance.currentHealth == PlayerHealthController.instance.maxHealth)
            {
                return false;
            }
            else
            {
                PlayerHealthController.instance.HealPlayer(amountToChangeStat);
                return true;
            }
            */
        }
        else if (attributeToChange == AttributeToChange.Speed)
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
        SwordDamage
    };
    public enum AttributeToChange
    {
        none,
        Speed
    };
}
