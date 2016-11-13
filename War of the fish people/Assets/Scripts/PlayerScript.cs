using UnityEngine;
using UnityEngineInternal;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityStandardAssets.CrossPlatformInput;

public class PlayerScript : MonoBehaviour {

	private const int moveAPammount = 1;
	private const int attackAPammount = 4;

	[SerializeField] private GameObject actionPannel;
	[SerializeField] private GameObject activateBossBox;
	[SerializeField] private GameObject activateBossYes;
	[SerializeField] private GameObject activateBossNo;
	[SerializeField] private GameObject light;
	[SerializeField] private GameObject medium;
	[SerializeField] private GameObject heavy;
	[SerializeField] private GameObject boss;
	[SerializeField] private Sprite bossSprite;
	[SerializeField] private Text messageText, healthText, ApText;
	
	private List<GameObject> players = new List<GameObject>();
	private enum STATES{AIM, IDLE, MOVE, SELECT_ACTION, ACTIVATE_BOSS};
	private STATES state = STATES.IDLE;
	private enum ACTIONS{MOVE, FIRE, CANCEL};
	private int playerNumber = 1;
	private ACTIONS actionNumber = ACTIONS.MOVE;
	private int playerSelected = 0;
	private bool canMove = true;
	private bool changeAction = false;
	private Vector2 startPosition = new Vector2 (-35.0f, 10.0f);
	private string playerNum = "";
	private int mechCounter = 0;
	private bool isNo = true;
	private bool bossActivated = false;

	// Use this for initialization
	void Start()
	{
		if(gameObject.name == "Player 2") 
		{
			playerNumber = 2;
			playerNum = "2";
			startPosition.Set(20.0f, 10.0f);
		}
		else
		{
			boss.GetComponent<Animator>().SetBool("StartUp", true);
		}
		AddTroops ();
		Debug.Log (players.Count);
		players [playerSelected].GetComponent<PlatformerCharacter2D> ().SelectPlayer (true);
	}

	private void AddTroops()
	{
		//Adds the player you have selected to the game
		if (playerNumber == 1) {
			for (int i = 0; i < PlayerSelection.player1Heavy; i++) {
				GameObject newPlayer = (GameObject)Instantiate (heavy, startPosition, transform.rotation); 
				newPlayer.GetComponent<PlatformerCharacter2D>().Flip();
				players.Add (newPlayer);
				startPosition.x+= 3.0f;
			}
			for (int i = 0; i < PlayerSelection.player1Trooper; i++) {
				GameObject newPlayer = (GameObject)Instantiate (medium, startPosition, transform.rotation); 
				newPlayer.GetComponent<PlatformerCharacter2D>().Flip();
				players.Add (newPlayer);
				startPosition.x+= 3.0f;
			}
			for (int i = 0; i < PlayerSelection.player1Scout; i++) {
				GameObject newPlayer = (GameObject)Instantiate (light, startPosition, transform.rotation); 
				newPlayer.GetComponent<PlatformerCharacter2D>().Flip();
				players.Add (newPlayer);
				startPosition.x+= 3.0f;
			}
		} 
		else 
		{
			for (int i = 0; i < PlayerSelection.player2Scout; i++) {
				GameObject newPlayer = (GameObject)Instantiate (light, startPosition, transform.rotation); 
				newPlayer.GetComponent<PlatformerCharacter2D>().dir = -1.0f;
				players.Add (newPlayer);
				startPosition.x-= 3.0f;
			}
			for (int i = 0; i < PlayerSelection.player2Trooper; i++) {
				GameObject newPlayer = (GameObject)Instantiate (medium, startPosition, transform.rotation); 
				newPlayer.GetComponent<PlatformerCharacter2D>().dir = -1.0f;
				players.Add (newPlayer);
				startPosition.x-= 3.0f;
			}
			for (int i = 0; i < PlayerSelection.player2Heavy; i++) {
				GameObject newPlayer = (GameObject)Instantiate (heavy, startPosition, transform.rotation); 
				players.Add (newPlayer);
				newPlayer.GetComponent<PlatformerCharacter2D>().dir = -1.0f;
				startPosition.x-= 3.0f;
			}
		}
	}
	
	private void UpdatePlayer()
	{
		//If Cancel is pressed
		if(CrossPlatformInputManager.GetButtonDown ("Cancel"+playerNum))
		{
			if(state == STATES.AIM)
			{
				players [playerSelected].GetComponent<PlatformerCharacter2D> ().AimWeapon(false);
			}
			messageText.text = "";
			state = STATES.IDLE;
			actionPannel.SetActive (false);			
		}
		//If Confirm is pressed
		else if (CrossPlatformInputManager.GetButtonDown ("Confirm"+playerNum))
		{
			if(state == STATES.AIM)
			{
				if(players [playerSelected].GetComponent<PlayerStats> ().RemoveAp(attackAPammount))
				{
					players [playerSelected].GetComponent<PlatformerCharacter2D> ().SelectState(PlatformerCharacter2D.State.attack);
				}
				else{messageText.text = "Not enough AP";}
				state = STATES.IDLE;
				players [playerSelected].GetComponent<PlatformerCharacter2D> ().AimWeapon(false);
			}
			else if(state == STATES.SELECT_ACTION)
			{
				SelectAction();
			}
			else if(Statics.isWalking)
			{
				players [playerSelected].GetComponent<PlatformerCharacter2D> ().jump = true;
			}
			else if(state == STATES.ACTIVATE_BOSS)
			{
				if(!isNo)
				{
					if(playerNumber == 1)
					{
						boss.GetComponent<Animator>().SetFloat("Speed", 1.0f);
						mechCounter++;
						
					}
					else
					{
						state = STATES.IDLE;
						players.Add(boss);
					}
					boss.GetComponent<PlayerStats>().active = true;
				}
				else
				{
					state = STATES.IDLE;
				}
				activateBossBox.SetActive(false);
			}
		}
		CheckPlayers ();
	}

	private void UpdateStates()
	{
		float x = CrossPlatformInputManager.GetAxis ("xMovement"+playerNum);	
		switch(state) 
		{
			case STATES.MOVE:
			{
				if (x > 0.0f && canMove) {
					if (players [playerSelected].GetComponent<PlayerStats> ().RemoveAp (attackAPammount)) 
					{
						canMove = false;
						Statics.isWalking = true;
						players [playerSelected].GetComponent<PlatformerCharacter2D> ().SelectState(PlatformerCharacter2D.State.moveRight);
					} 
					else {
						messageText.text = "Not enough AP";
					}	
				} 
				else if (x < 0.0f && canMove) {
					if (players [playerSelected].GetComponent<PlayerStats> ().RemoveAp (attackAPammount)) 
					{
						canMove = false;
						state = STATES.IDLE;
						players [playerSelected].GetComponent<PlatformerCharacter2D> ().SelectState(PlatformerCharacter2D.State.moveLeft);
					} 
					else {
						messageText.text = "Not enough AP";
					}
					
				}else if (x == 0) {
					canMove = true;
				}
				break;
			}
			case STATES.AIM:
			{
				players [playerSelected].GetComponent<PlatformerCharacter2D> ().AimBullet(-x);
				break;
			}
			case STATES.ACTIVATE_BOSS:
			{
				float xMov = CrossPlatformInputManager.GetAxis ("CycleCard" + playerNum);
				if(isNo && xMov < 0.0f)
				{
					isNo = false;
					activateBossYes.GetComponent<ChangeImage>().SwitchImage(true);
					activateBossNo.GetComponent<ChangeImage>().SwitchImage(false);
				}
				else if(!isNo && xMov > 0.0f)
				{
					isNo = true;
					activateBossYes.GetComponent<ChangeImage>().SwitchImage(false);
					activateBossNo.GetComponent<ChangeImage>().SwitchImage(true);
				}
				break;
			}
			case STATES.IDLE:
			{
				//Checks to see if the player is trying to change player
				SelectPlayer();
				//If the actions button is pressed
				if (CrossPlatformInputManager.GetButtonDown ("Actions"+playerNum)) {
					state = STATES.SELECT_ACTION;
					actionPannel.SetActive (true);
				}
				break;
			}
			case STATES.SELECT_ACTION:
			{
				float y = CrossPlatformInputManager.GetAxis("CycleAction" + playerNum);
				int action;
				if(y > 0.0f && !changeAction)
				{
					if(actionNumber == ACTIONS.MOVE)
					{
						actionNumber = ACTIONS.CANCEL;
					}
					else 
					{
						actionNumber--;
					}
					changeAction = true;
					action = (int)actionNumber;
					actionPannel.GetComponent<CycleActions>().UpdateImage(action);
				}
				else if(y < 0.0f && !changeAction)
				{
					if(actionNumber == ACTIONS.CANCEL)
					{
						actionNumber = ACTIONS.MOVE;
					}
					else
					{ 
						actionNumber++;
					}
					changeAction = true;
					action = (int)actionNumber;
					actionPannel.GetComponent<CycleActions>().UpdateImage(action);
				}
				if (y == 0.0f)
				{
					changeAction = false;
				}
				break;
			}
		} 
	}

	private void CheckPlayers()
	{
		if (players.Count != 0) {
			//For all the players in play
			for (int i = 0; i < players.Count; i++) {
				//If the player is no longer alive
				if (!players [i].GetComponent<PlayerStats> ().isAlive) {
					Destroy (players [i], 3);
					players[i].GetComponent<PlatformerCharacter2D>().SelectState(PlatformerCharacter2D.State.die);
					players.RemoveAt(i);
				}
			}
		}
	}

	private void SelectPlayer()
	{
		//If the left bumper is pressed
		if(CrossPlatformInputManager.GetButtonDown("CyclePlayerRight" + playerNum))
		{
			//Deselects the current player
			players[playerSelected].GetComponent<PlatformerCharacter2D> ().SelectPlayer(false);
			//Rotates the selected player 1 to the left
			if (playerSelected == 0) {
				playerSelected = players.Count-1;
			} 
			else 
			{
				playerSelected -= 1;
			}
			//Selects the new player
			players[playerSelected].GetComponent<PlatformerCharacter2D> ().SelectPlayer(true);
		}
		//If the right bumper is pressed
		else if(CrossPlatformInputManager.GetButtonDown("CyclePlayerLeft" + playerNum))
		{
			//Deselects the current player
			players[playerSelected].GetComponent<PlatformerCharacter2D> ().SelectPlayer(false);
			//Rotates the selected player 1 to the right
			if (playerSelected == players.Count - 1) {
				playerSelected = 0;
			}
			else 
			{
				playerSelected += 1;
			}
			//Selects the new player
			players[playerSelected].GetComponent<PlatformerCharacter2D>().SelectPlayer(true);
		}
	}

	private void SelectAction()
	{
		//Select Action
		switch(actionNumber)
		{
			case ACTIONS.MOVE:
			{
				state = STATES.MOVE;
				break;
			}
			case ACTIONS.FIRE:
			{
				state = STATES.AIM;
				players[playerSelected].GetComponent<PlatformerCharacter2D>().AimWeapon(true);
				break;
			}
			case ACTIONS.CANCEL:
			{
				Statics.gameTime = 0.0f;
				break;
			}
		}
		actionPannel.SetActive (false);
	}

	// Update is called once per frame
	void Update () {
	
		if(players.Count == 0)
		{
			
		}
		//If it is this players turn
		if (playerNumber == Statics.playerTurn) 
		{
			if(Statics.turns > 1 && !bossActivated)
			{
				messageText.text = "Start to Activate Boss";
				if(CrossPlatformInputManager.GetButtonDown("Pause" + playerNum))
				{
					state = STATES.ACTIVATE_BOSS;
					bossActivated = true;
					activateBossBox.SetActive(true);
					if(playerNumber == 2)
					{
						players.Add(boss);
					}
				}
			}
			if(mechCounter > 0)
			{
				mechCounter++;
				if(mechCounter > 80)
				{
					boss.GetComponent<Animator>().SetBool("StartUp", false);
					boss.GetComponent<BoxCollider2D>().size = new Vector2(boss.GetComponent<BoxCollider2D>().size.x, 1.9f);
					boss.GetComponent<BoxCollider2D>().offset = new Vector2(boss.GetComponent<BoxCollider2D>().offset.x, -0.015f);
					state = STATES.IDLE;
					players.Add(boss);
					mechCounter = 0;
				}
			}
			UpdateStates();
			UpdatePlayer();
			healthText.text = players [playerSelected].GetComponent<PlayerStats> ().health.ToString();
			ApText.text = players [playerSelected].GetComponent<PlayerStats> ().AP.ToString();
		}
		else
		{
			if(state == STATES.AIM)
			{
				players[playerSelected].GetComponent<PlatformerCharacter2D>().AimWeapon(false);
				state = STATES.IDLE;
			}
			for(int i = 0; i < players.Count; i++)
			{
				players[i].GetComponent<PlayerStats>().ResetStats();
			}
		}
	}
}
