using System.Collections;
using System.Collections.Generic;
using UnityEngine;



[System.Serializable]
public struct Resources
{
    public int copper;
    public int citizen;
    public int food;
    public int gold;
    public int stone;
    public int wood;

    public Resources(int copper, int citizen, int food, int gold, int stone, int wood)
    {
        this.copper = copper;
        this.citizen = citizen;
        this.food = food;
        this.gold = gold;
        this.stone = stone;
        this.wood = wood;
    }



    public static bool operator ==(Resources r1, Resources r2)
    {
        return r1.Equals(r2);
    }
    public static bool operator !=(Resources r1, Resources r2)
    {
        return !r1.Equals(r2);
    }
    public static bool operator >=(Resources r1, Resources r2)
    {
        bool isBigger = false;
        if
        (
            r1.copper >= r2.copper &&
            r1.citizen >= r2.citizen &&
            r1.food >= r2.food &&
            r1.gold >= r2.gold &&
            r1.stone >= r2.stone &&
            r1.wood >= r2.wood
        )
        {
            isBigger = true;
        }
        return isBigger;
    }

    public static bool operator >(Resources r1, Resources r2)
    {
        bool isBigger = false;
        if
        (
            r1.copper > r2.copper &&
            r1.citizen > r2.citizen &&
            r1.food > r2.food &&
            r1.gold > r2.gold &&
            r1.stone > r2.stone &&
            r1.wood > r2.wood
        )
        {
            isBigger = true;
        }
        return isBigger;
    }

    public void AddResource(ResourceType res, int amount)
    {
        if (res == ResourceType.citizen)
        {
            citizen += amount;
        }
        if (res == ResourceType.copper)
        {
            copper += amount;
        }
        if (res == ResourceType.food)
        {
            food += amount;
        }
        if (res == ResourceType.gold)
        {
            gold += amount;
        }
        if (res == ResourceType.stone)
        {
            stone += amount;
        }
        if (res == ResourceType.wood)
        {
            wood += amount;
        }
    }

    public static bool operator <(Resources r1, Resources r2)
    {
        bool isBigger = false;
        if
        (
            r1.copper < r2.copper &&
            r1.citizen < r2.citizen &&
            r1.food < r2.food &&
            r1.gold < r2.gold &&
            r1.stone < r2.stone &&
            r1.wood < r2.wood
        )
        {
            isBigger = true;
        }
        return isBigger;
    }

    public static bool operator <=(Resources r1, Resources r2)
    {
        bool isLess = false;
        if
        (
            r1.copper <= r2.copper &&
            r1.citizen <= r2.citizen &&
            r1.food <= r2.food &&
            r1.gold <= r2.gold &&
            r1.stone <= r2.stone &&
            r1.wood <= r2.wood
        )
        {
            isLess = true;
        }
        return isLess;
    }

    public static Resources operator +(Resources r1, Resources r2)
    {
        Resources res = new Resources 
        (
            r1.copper + r2.copper,
            r1.citizen + r2.citizen,
            r1.food + r2.food,
            r1.gold + r2.gold,
            r1.stone + r2.stone,
            r1.wood + r2.wood
        );
        
        return res;
    }

    public static Resources operator -(Resources r1, Resources r2)
    {
        r1.copper -= r2.copper;
        r1.citizen -= r2.citizen;
        r1.food -= r2.food;
        r1.gold -= r2.gold;
        r1.stone -= r2.stone;
        r1.wood -= r2.wood;
        return r1;
    }    
}
