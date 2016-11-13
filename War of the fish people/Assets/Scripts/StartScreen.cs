using UnityEngine;
using System.Collections;
using UnityStandardAssets.CrossPlatformInput;

public class StartScreen : MonoBehaviour {


	[SerializeField] private GameObject button1, button2, button3;
	private int place = 0;
	private bool move = false;
	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		if(CrossPlatformInputManager.GetAxis("CycleAction") < 0.0f && !move)
		{
			if(place == 0)
			{
				button1.GetComponent<ChangeImage>().SwitchImage(false);
				button2.GetComponent<ChangeImage>().SwitchImage(true);
				place++;
				move = true;
			}
			else if(place == 1)
			{	
				button2.GetComponent<ChangeImage>().SwitchImage(false);
				button3.GetComponent<ChangeImage>().SwitchImage(true);
				place++;
				move = true;
			}
		}
		if(CrossPlatformInputManager.GetAxis("CycleAction") > 0.0f && !move)
		{
			if(place == 1)
			{
				button2.GetComponent<ChangeImage>().SwitchImage(false);
				button1.GetComponent<ChangeImage>().SwitchImage(true);
				place--;
				move = true;
			}
			else if(place == 2)
			{	
				button3.GetComponent<ChangeImage>().SwitchImage(false);
				button2.GetComponent<ChangeImage>().SwitchImage(true);
				place--;
				move = true;
			}
		}
		if(CrossPlatformInputManager.GetAxis("CycleAction") == 0)
		{
			move = false;
		}
		if(CrossPlatformInputManager.GetButtonDown("Confirm"))
		{
			if(place == 0)
			{
				Application.LoadLevel(4);
			}
		}
	}
}
