using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Timer : MonoBehaviour {

	[SerializeField] private Text text, player1, player2;
	[SerializeField] private GameObject button1, button2;
	[SerializeField] private GameObject actions;
	private Color origonalColor = new Color(255, 255, 255, 150);
	private Color changeColor = new Color(255, 0, 0, 255);
	
	// Use this for initialization
	void Start () {
		player1.color = changeColor;
		player2.color = origonalColor;
	}
	
	// Update is called once per frame
	void Update () {
		//Counts down a timer to 0 then switches turns
		Statics.gameTime -= Time.deltaTime;
		if (Statics.gameTime <= 0.0f) {
			Statics.SwitchTurns();
			actions.SetActive(false);
			if(player1.color == changeColor)
			{
				player1.color = origonalColor;
				player2.color = changeColor;
			}
			else
			{
				player1.color = changeColor;
				player2.color = origonalColor;
			}
		}
		if(Statics.turns == 4)
		{
			button1.SetActive(true);
		}
		else if(Statics.turns == 5)
		{
			button2.SetActive(true);
		}
		text.text = Statics.gameTime.ToString("F2");
	}
}
