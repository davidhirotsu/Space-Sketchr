using UnityEngine;
using System.Collections;

public class LoadNextScene : MonoBehaviour
{
	public string sceneToLoad = "MainScene";
	public GameObject destroyOnLoad;

	// Use this for initialization
	void Start () {
		Invoke ( "GoToNextScene", 1.0f );
	}
	
	// Update is called once per frame
	void GoToNextScene ()
	{
		Application.LoadLevelAdditive( sceneToLoad );
		Destroy ( destroyOnLoad );
		Destroy ( this.gameObject );
	}
}
