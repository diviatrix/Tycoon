using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public enum ResourceType
{
	gold,
	wood,
	stone,
	food,
	copper,
	citizen,
	maxCitizen
}

[System.Serializable]
public struct Resource
{
	public ResourceType type;
	public int amount;

	public Resource(ResourceType type, int amount)
	{
		this.type = type;
		this.amount = amount;
	}

	public static bool operator ==(Resource r1, Resource r2)
	{
		return r1.Equals(r2);
	}
	public static bool operator !=(Resource r1, Resource r2)
	{
		return !r1.Equals(r2);
	}

	public static Resource operator +(Resource r1, Resource r2)
	{
		 r1.amount+=r2.amount;
		return r1;
	}

	public static Resource operator -(Resource r1, Resource r2)
	{
		r1.amount -= r2.amount;
		return r1;
	}

	public static bool operator >=(Resource r1, Resource r2)
	{
		bool isBigger = false;

		if(r1.amount >= r2.amount)
		{
			isBigger = true;
		}
		return isBigger;
	}

	public static bool operator <=(Resource r1, Resource r2)
	{
		bool isBigger = false;

		if (r1.amount >= r2.amount)
		{
			isBigger = true;
		}
		return isBigger;
	}
}


