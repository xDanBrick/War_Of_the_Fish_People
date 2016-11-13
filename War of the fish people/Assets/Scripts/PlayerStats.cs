using UnityEngine;
using System.Collections;

public class PlayerStats : MonoBehaviour {
	
	private int startAP;
	
	public int agility = 0;
	public int health = 0;
	public int AP = 0;
	public int minDamage = 0;
	public int maxDamage = 0;
	public int addValue = 0;
	public int armour = 0;
	public int team = 1;
	public bool active = true;
	[HideInInspector] public bool isAlive = true;

	public bool RemoveAp(int ammount)
	{
		if ((AP - ammount) >= 0) 
		{
			AP -= ammount;
			return true;
		}
		return false;
	}

	public void RemoveHealth(int ammount)
	{
		health -= ammount;
		if (health <= 0) {
			isAlive = false;
		}
	}
	
	public void ResetStats()
	{
		AP = startAP;
	}
	// Use this for initialization
	void Start () {
		startAP = AP;
	}
}
