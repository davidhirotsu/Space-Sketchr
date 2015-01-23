using UnityEngine;
using System.Collections;

public class LoadNextScene : MonoBehaviour {

	// Use this for initialization
	void Start () {
		Invoke ( "GoToNextScene", 2.0f );
	}
	
	// Update is called once per frame
	void GoToNextScene () {
		Application.LoadLevel( "MainScene" );
	}
}
