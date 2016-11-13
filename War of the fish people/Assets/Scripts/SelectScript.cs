using UnityEngine;
using UnityEngine.UI;
using UnityStandardAssets.CrossPlatformInput;
using System.Collections;
using System.Collections.Generic;

public class SelectScript : MonoBehaviour {

	[SerializeField] private GameObject scout;
	[SerializeField] private GameObject troop;
	[SerializeField] private GameObject heavy;
	[SerializeField] private GameObject miniScout;
	[SerializeField] private GameObject miniTroop;
	[SerializeField] private GameObject miniHeavy;
	[SerializeField] private GameObject button;
	[SerializeField] private Text pointsRemainingText;
	[SerializeField] private string playerNumber = "";
	[SerializeField] private GameObject selected;
	private int pointsRemaining = 30;
	private bool deployed = false;
	private bool isPressed = false;
	private int position = 2;
	private float scoutStartX = -368.0f;
	private float troopStartX = -368.0f;
	private float heavyStartX = -368.0f;
	private List<GameObject> heavyMinis= new List<GameObject>();
	private List<GameObject> troopMinis= new List<GameObject>();
	private List<GameObject> scoutMinis= new List<GameObject>();

	// Use this for initialization
	void Start () {
		selected = troop;
		if (playerNumber == "1") {
			playerNumber = "";
		}
		if (playerNumber == "2") {
			scoutStartX = 210.0f;
			troopStartX = 210.0f;
			heavyStartX = 210.0f;
		}
	}

	private void AddTroop()
	{
		if (position == 0) {
			if(TakeOffPoints(3))
			{
				scoutMinis.Add((GameObject) Instantiate(miniScout, new Vector2(scoutStartX, -62.5f), transform.rotation));
				scoutMinis[scoutMinis.Count-1].transform.SetParent(transform, false);
				if(playerNumber == "")
				{
					PlayerSelection.player1Scout++;
				}
				else
				{
					PlayerSelection.player2Scout++;
				}
				scoutStartX += 15.0f;
			}

		} else if (position == 1) {
			if(TakeOffPoints(5))
			{
				troopMinis.Add((GameObject) Instantiate(miniTroop, new Vector2(troopStartX, -83.0f), transform.rotation));
				troopMinis[troopMinis.Count-1].transform.SetParent(transform, false);
				if(playerNumber == "")
				{
					PlayerSelection.player1Trooper++;
				}
				else
				{
					PlayerSelection.player2Trooper++;
				}
				troopStartX += 15.0f;
			}
		} else if (position == 2) {
			if(TakeOffPoints(7))
			{
				heavyMinis.Add((GameObject) Instantiate(miniHeavy, new Vector2(heavyStartX, -103.5f), transform.rotation));
				heavyMinis[heavyMinis.Count-1].transform.SetParent(transform, false);
				if(playerNumber == "")
				{
					PlayerSelection.player1Heavy++;
				}
				else
				{
					PlayerSelection.player2Heavy++;
				}
				heavyStartX += 15.0f;
			}
		}
	}

	private void RemoveTroops()
	{
		if(position == 0 && scoutMinis.Count > 0)
		{
			Destroy(scoutMinis[scoutMinis.Count-1].gameObject);
			scoutMinis.RemoveAt(scoutMinis.Count -1);
			if(playerNumber == "")
			{
				PlayerSelection.player1Scout--;
			}
			else
			{
				PlayerSelection.player2Scout--;
			}
			pointsRemaining+= 3;
			scoutStartX-= 15.0f;
		}
		else if(position == 1 && troopMinis.Count > 0)
		{
			Destroy(troopMinis[troopMinis.Count-1].gameObject);
			troopMinis.RemoveAt(troopMinis.Count - 1);
			if(playerNumber == "")
			{
				PlayerSelection.player1Trooper--;
			}
			else
			{
				PlayerSelection.player2Trooper--;
			}
			pointsRemaining+= 5;
			troopStartX-= 15.0f;
		}
		else if(position == 2 && heavyMinis.Count > 0)
		{
			Destroy(heavyMinis[heavyMinis.Count-1].gameObject);
			heavyMinis.RemoveAt(heavyMinis.Count - 1);
			if(playerNumber == "")
			{
				PlayerSelection.player1Heavy--;
			}
			else
			{
				PlayerSelection.player2Heavy--;
			}
			pointsRemaining += 7;
			heavyStartX-= 15.0f;
		}
		
	}

	private void CycleCard(int num)
	{
		selected.GetComponent<ChangeImage> ().SwitchImage (false);
		if (position + num < 0) {
			position = 2;
		} else if (position + num > 2) {
			position = 0;
		} else {
			position += num;
		}
		isPressed = true;
		SelectPlayer ();
	}

	private void SelectPlayer()
	{
		if (position == 0) 
		{
			selected = scout;
		}
		else if (position == 1) 
		{
			selected = troop;
		} 
		else if(position == 2)
		{
			selected = heavy;
		}
		selected.GetComponent<ChangeImage> ().SwitchImage (true);
	}

	private void UpdateSelection()
	{
		pointsRemainingText.text = pointsRemaining.ToString();
		MoveThroughCards ();
		if(CrossPlatformInputManager.GetButtonDown("Pause" + playerNumber) && !deployed)
		{
			deployed = true;
			Statics.deployer++;
			button.GetComponent<ChangeImage> ().SwitchImage (true);
		}
		if(CrossPlatformInputManager.GetButtonDown("Confirm" + playerNumber))
		{
			AddTroop();
		}
	}

	// Update is called once per frame
	void Update () {
		
		if (CrossPlatformInputManager.GetButtonDown ("Cancel" + playerNumber)) {
			if(deployed)
			{
				deployed = false;
				Statics.deployer--;
				button.GetComponent<ChangeImage>().SwitchImage(false);
			}
			else
			{
				RemoveTroops();
			}	
		}
		if(!deployed)
		{
			UpdateSelection();
		}
		if (Statics.deployer == 2) {
			Application.LoadLevel(2);
		}
	}

	public bool TakeOffPoints(int number)
	{			
		if (pointsRemaining - number >= 0) {
				pointsRemaining -= number;
			return true;
			}
		return false;
		
	}
	private void MoveThroughCards()
	{
		float movement = CrossPlatformInputManager.GetAxis ("CycleCard" + playerNumber);
		if (movement > 0.0f && !isPressed) {
			CycleCard(1);
		} else if (movement < 0.0f && !isPressed) 
		{
			CycleCard(-1);
		} 
		else if (movement == 0.0f) 
		{
			isPressed = false;
		}
	}
}
