using UnityEngine;
using System.Collections;

public class UI_Brain : MonoBehaviour
{
	public GameObject touchToDrawButton;
	public GameObject colorToolPaletteButton;

	public GameObject invisibleButton;

	public TweenTransform toolColorPaletteTween;

	// may want to switch this to an enum later
	protected bool toolPaletteIsOpen = false;

	// Use this for initialization
	void Start() {
	
	}
	
	// Update is called once per frame
	void Update() {
	
	}

	public void TouchToDraw()
	{
		// button disappears on touch
		touchToDrawButton.gameObject.SetActive( false );
		invisibleButton.gameObject.SetActive( false );
	}

	public void InvisibleButtonTouched()
	{
		// button disappears on touch
		touchToDrawButton.gameObject.SetActive( false );
		invisibleButton.gameObject.SetActive( false );

		if ( toolPaletteIsOpen ) {

			CloseColorPalette();

		} else {

			//ColorPaletteIsOpen();

		}
	}

	public void OpenCloseToolPaletteTouched() { if ( toolPaletteIsOpen ) { CloseColorPalette(); } else { OpenColorPalette(); } }

	public void OpenColorPalette()
	{
		// the color/tool palette is open, close it and unhide the button
		colorToolPaletteButton.gameObject.SetActive( false );
		invisibleButton.gameObject.SetActive( true );

		toolColorPaletteTween.PlayReverse();

		toolPaletteIsOpen = true;
	}

	public void CloseColorPalette()
	{
		// the color/tool palette is closed, no need to re-close it
		colorToolPaletteButton.gameObject.SetActive( true );
		invisibleButton.gameObject.SetActive( false );

		toolColorPaletteTween.PlayForward();

		toolPaletteIsOpen = false;
	}
}