using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;
using System.Collections;

public class BulletScript : MonoBehaviour {
	
	private bool isFired = false;
	private int team = 1;
	private int counter = 0;
	private GameObject thisObject;
	
	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void FireBullet(int theTeam, GameObject player)
	{
		team = theTeam;
		isFired = true;
		thisObject = player;
	}

	void OnCollisionEnter2D(Collision2D col)
	{
		if(isFired)
		{
			counter++;
			if (col.gameObject.tag == "Ground") {
				Destroy (gameObject);
			} 
			else if (col.gameObject.tag == "Troop" && col.gameObject != thisObject) 
			{
				PlayerStats stats = col.gameObject.GetComponent<PlayerStats>();
				if(team != stats.team && stats.active)
				{
					if(Random.Range(1, 20) - stats.agility < 16)
					{
						int damage = Random.Range(stats.minDamage, stats.maxDamage) + stats.addValue;
						damage -= stats.armour;
						stats.RemoveHealth(damage);
					}
				}
				Destroy (gameObject);
			}
		}
	}


}
