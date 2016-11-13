using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CycleActions : MonoBehaviour {

	[SerializeField] private GameObject image1, image2, image3;
	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	
	private void ResetImages()
	{
		image1.GetComponent<ChangeImage>().SwitchImage(false);
		image2.GetComponent<ChangeImage>().SwitchImage(false);
		image3.GetComponent<ChangeImage>().SwitchImage(false);
	}
	
	public void UpdateImage(int action)
	{
		ResetImages();
		if(action == 0)
		{
			image1.GetComponent<ChangeImage>().SwitchImage(true);
		}
		else if(action == 1)
		{
			image2.GetComponent<ChangeImage>().SwitchImage(true);
		}
		else if(action == 2)
		{
			image3.GetComponent<ChangeImage>().SwitchImage(true);
		}
	}
}
