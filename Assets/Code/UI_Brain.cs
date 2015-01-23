using UnityEngine;
using System.Collections;

public class UI_Brain : MonoBehaviour
{
	public static UI_Brain _instance;
	public bool hasInitialized = false;

	// the UI_Brain object recieves events from various buttons within the UI and responds accordingly
	// most of the UI must be manually linked to this object from within the Unity authoring environment

	public GameObject touchToDrawButton;
	public GameObject touchToDrawLabel;
	public GameObject colorToolPaletteButton;

	public GameObject invisibleButton;

	public TweenTransform toolColorPaletteTween;

	// may want to switch this to an enum later
	public bool toolPaletteIsOpen = true;
	
	public UIButton[] texturePaletteButtons;
	public UIButton[] colorPaletteButtons;

	public UISlider brushSizeSlider;
	
	// ---------------------------------------------------------------------------------------------

	void Awake()
	{
		_instance = this;
	}
	
	void Start()
	{
		// doing nothing for now, wait until Sketchpad comes alive and initializes UI_Brain remotely
	}

	public void Initialize()
	{
		// set the initial texture to the basic brush and the initial color to white
		Texture_01_Selected();
		Color_01_Selected();

		hasInitialized = true;
		brushSizeSlider.value = Sketchpad._instance.brushSize;
	}

	// ---------------------------------------------------------------------------------------------
	
	public void OnBrushSizeSliderChange()
	{
		if ( hasInitialized ) Sketchpad._instance.brushSize = brushSizeSlider.value;
	}

	public void OnClearButtonPressed()
	{
		if ( hasInitialized ) Sketchpad._instance.ClearPoints();
	}
	
	// ---------------------------------------------------------------------------------------------

	public void Texture_01_Selected()
	{
		if ( hasInitialized ) {
			ResetTextureSwatches();
			Color swatchColor = texturePaletteButtons[ 0 ].defaultColor;
			texturePaletteButtons[ 0 ].defaultColor = new Color( swatchColor.r, swatchColor.g, swatchColor.b, 1f );

			Sketchpad._instance.SetSelectedTexture( 0 );
			Sketchpad._instance.UpdateParticles();
		}
	}

	public void Texture_02_Selected()
	{
		if ( hasInitialized ) {
			ResetTextureSwatches();
			Color swatchColor = texturePaletteButtons[ 1 ].defaultColor;
			texturePaletteButtons[ 1 ].defaultColor = new Color( swatchColor.r, swatchColor.g, swatchColor.b, 1f );
			
			Sketchpad._instance.SetSelectedTexture( 1 );
			Sketchpad._instance.UpdateParticles();
		}
	}

	public void Texture_03_Selected()
	{
		if ( hasInitialized ) {
			ResetTextureSwatches();
			Color swatchColor = texturePaletteButtons[ 2 ].defaultColor;
			texturePaletteButtons[ 2 ].defaultColor = new Color( swatchColor.r, swatchColor.g, swatchColor.b, 1f );
			
			Sketchpad._instance.SetSelectedTexture( 2 );
			Sketchpad._instance.UpdateParticles();
		}
	}

	// ---------------------------------------------------------------------------------------------

	public void Color_01_Selected()
	{
		if ( hasInitialized ) {
			ResetColorSwatches();
			Color swatchColor = colorPaletteButtons[ 0 ].defaultColor;
			colorPaletteButtons[ 0 ].defaultColor = new Color( swatchColor.r, swatchColor.g, swatchColor.b, 1f );

			Sketchpad._instance.SetSelectedColor( 0 );
		}
	}
	public void Color_02_Selected()
	{
		if ( hasInitialized ) {
			ResetColorSwatches();
			Color swatchColor = colorPaletteButtons[ 1 ].defaultColor;
			colorPaletteButtons[ 1 ].defaultColor = new Color( swatchColor.r, swatchColor.g, swatchColor.b, 1f );
			
			Sketchpad._instance.SetSelectedColor( 1 );
		}
	}
	public void Color_03_Selected()
	{
		if ( hasInitialized ) {
			ResetColorSwatches();
			Color swatchColor = colorPaletteButtons[ 2 ].defaultColor;
			colorPaletteButtons[ 2 ].defaultColor = new Color( swatchColor.r, swatchColor.g, swatchColor.b, 1f );
			
			Sketchpad._instance.SetSelectedColor( 2 );
		}
	}
	public void Color_04_Selected()
	{
		if ( hasInitialized ) {
			ResetColorSwatches();
			Color swatchColor = colorPaletteButtons[ 3 ].defaultColor;
			colorPaletteButtons[ 3 ].defaultColor = new Color( swatchColor.r, swatchColor.g, swatchColor.b, 1f );
			
			Sketchpad._instance.SetSelectedColor( 3 );
		}
	}
	public void Color_05_Selected()
	{
		if ( hasInitialized ) {
			ResetColorSwatches();
			Color swatchColor = colorPaletteButtons[ 4 ].defaultColor;
			colorPaletteButtons[ 4 ].defaultColor = new Color( swatchColor.r, swatchColor.g, swatchColor.b, 1f );
			
			Sketchpad._instance.SetSelectedColor( 4 );
		}
	}
	public void Color_06_Selected()
	{
		if ( hasInitialized ) {
			ResetColorSwatches();
			Color swatchColor = colorPaletteButtons[ 5 ].defaultColor;
			colorPaletteButtons[ 5 ].defaultColor = new Color( swatchColor.r, swatchColor.g, swatchColor.b, 1f );
			
			Sketchpad._instance.SetSelectedColor( 5 );
		}
	}
	public void Color_07_Selected()
	{
		if ( hasInitialized ) {
			ResetColorSwatches();
			Color swatchColor = colorPaletteButtons[ 6 ].defaultColor;
			colorPaletteButtons[ 6 ].defaultColor = new Color( swatchColor.r, swatchColor.g, swatchColor.b, 1f );
			
			Sketchpad._instance.SetSelectedColor( 6 );
		}
	}
	public void Color_08_Selected()
	{
		if ( hasInitialized ) {
			ResetColorSwatches();
			Color swatchColor = colorPaletteButtons[ 7 ].defaultColor;
			colorPaletteButtons[ 7 ].defaultColor = new Color( swatchColor.r, swatchColor.g, swatchColor.b, 1f );
			
			Sketchpad._instance.SetSelectedColor( 7 );
		}
	}
	public void Color_09_Selected()
	{
		if ( hasInitialized ) {
			ResetColorSwatches();
			Color swatchColor = colorPaletteButtons[ 8 ].defaultColor;
			colorPaletteButtons[ 8 ].defaultColor = new Color( swatchColor.r, swatchColor.g, swatchColor.b, 1f );
			
			Sketchpad._instance.SetSelectedColor( 8 );
		}
	}
	public void Color_10_Selected()
	{
		if ( hasInitialized ) {
			ResetColorSwatches();
			Color swatchColor = colorPaletteButtons[ 9 ].defaultColor;
			colorPaletteButtons[ 9 ].defaultColor = new Color( swatchColor.r, swatchColor.g, swatchColor.b, 1f );
			
			Sketchpad._instance.SetSelectedColor( 9 );
		}
	}

	// ---------------------------------------------------------------------------------------------

	protected void ResetTextureSwatches()
	{
		if ( hasInitialized ) {
			// go through the whole array of swatches and de-select them all
			for ( int index = 0; index < texturePaletteButtons.Length; index++ ) {
				// set this swatch back to deselected
				Color swatchColor = texturePaletteButtons[ index ].defaultColor;
				texturePaletteButtons[ index ].defaultColor = new Color( swatchColor.r, swatchColor.g, swatchColor.b, 0.5f );
			}
		}
	}

	protected void ResetColorSwatches()
	{
		if ( hasInitialized ) {
			// go through the whole array of swatches and de-select them all
			for ( int index = 0; index < colorPaletteButtons.Length; index++ ) {
				// set this swatch back to deselected
				Color swatchColor = colorPaletteButtons[ index ].defaultColor;
				colorPaletteButtons[ index ].defaultColor = new Color( swatchColor.r, swatchColor.g, swatchColor.b, 0.5f );
			}
		}
	}

	// ---------------------------------------------------------------------------------------------

	public void TouchToDraw()
	{
		// button disappears on touch
		SetGroupAsInactive();
	}

	public void InvisibleButtonTouched()
	{
		// button disappears on touch
		SetGroupAsInactive();

		if ( toolPaletteIsOpen ) { CloseColorPalette(); }
	}

	// ---------------------------------------------------------------------------------------------

	public void OpenCloseToolPaletteTouched() { if ( toolPaletteIsOpen ) { CloseColorPalette(); } else { OpenColorPalette(); } }

	public void OpenColorPalette()
	{
		// the color/tool palette is open, close it and unhide the button
		colorToolPaletteButton.gameObject.SetActive( false );
		invisibleButton.gameObject.SetActive( true );

		toolColorPaletteTween.PlayForward();

		toolPaletteIsOpen = true;
	}

	public void CloseColorPalette()
	{
		// the color/tool palette is closed, no need to re-close it
		colorToolPaletteButton.gameObject.SetActive( true );
		invisibleButton.gameObject.SetActive( false );

		toolColorPaletteTween.PlayReverse();

		toolPaletteIsOpen = false;
	}

	protected void SetGroupAsInactive()
	{
		// there are a few functions that turn off these items, so it's been consolidated to avoid duplication
		touchToDrawButton.gameObject.SetActive( false );
		touchToDrawLabel.gameObject.SetActive( false );
		invisibleButton.gameObject.SetActive( false );
	}

	// ---------------------------------------------------------------------------------------------
}