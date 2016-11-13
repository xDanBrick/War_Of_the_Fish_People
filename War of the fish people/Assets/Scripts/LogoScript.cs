using UnityEngine;
using System.Collections;

public class LogoScript : MonoBehaviour {

	private float timer = 3;
	private float counter = 0;
	public int level = 3;

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		counter += Time.deltaTime;
		if(counter > timer)
		{
			Application.LoadLevel(level);
		}
	}
}
