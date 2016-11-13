using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ChangeImage : MonoBehaviour {
		
	[SerializeField] private Texture texture1;
	private Texture texture2;

	// Use this for initialization
	void Start () {
		texture2 = gameObject.GetComponent<RawImage> ().texture;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	public void SwitchImage(bool selected)
	{
		if (selected) 
		{
			gameObject.GetComponent<RawImage> ().texture = texture1;
		} 
		else
		{
			gameObject.GetComponent<RawImage> ().texture = texture2;
		}
	}
}
