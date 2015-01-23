using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Sketchpad : MonoBehaviour
{

	public Transform planeObject;

	protected int selectedColor = 0;
	protected Color[] allColors;
	public Color primaryColor_01 = new Color( 1f, 1f, 1f, 1f );
	public Color primaryColor_02 = new Color( .749f, .749f, .749f, 1f );
	public Color primaryColor_03 = new Color( .129f, .129f, .129f, 1f );
	public Color primaryColor_04 = new Color( .129f, .588f, .953f, 1f );
	public Color primaryColor_05 = new Color( .298f, .777f, .313f, 1f );
	public Color primaryColor_06 = new Color( 1.0f, .921f, .231f, 1f );
	public Color primaryColor_07 = new Color( 1.0f, .266f, .211f, 1f );
	public Color primaryColor_08 = new Color( .925f, .160f, .482f, 1f );
	public Color primaryColor_09 = new Color( 1.0f, .596f, 0.0f, 1f );
	public Color primaryColor_10 = new Color( .925f, .160f, .482f, 1f );

	protected int selectedMaterial;
	protected Material[] allMaterials;
	public Material basicBrushMaterial;
	public Material lflBrushMaterial;
	public Material splatterBrushMaterial;

	Vector3? lastPoint;
	List<ParticleSystem.Particle> pointList = new List<ParticleSystem.Particle>();
	bool particleSystemNeedsUpdate = false;

	void Awake()
	{
		// place all of our colors in an array for convenience
		allColors = new Color[10];
		allColors[ 0 ] = primaryColor_01;
		allColors[ 1 ] = primaryColor_02;
		allColors[ 2 ] = primaryColor_03;
		allColors[ 3 ] = primaryColor_04;
		allColors[ 4 ] = primaryColor_05;
		allColors[ 5 ] = primaryColor_06;
		allColors[ 6 ] = primaryColor_07;
		allColors[ 7 ] = primaryColor_08;
		allColors[ 8 ] = primaryColor_09;
		allColors[ 9 ] = primaryColor_10;

		// place all of our textures in an array for convenience
		allMaterials = new Material[3];
		allMaterials[ 0 ] = basicBrushMaterial;
		allMaterials[ 1 ] = lflBrushMaterial;
		allMaterials[ 2 ] = splatterBrushMaterial;
	}

	void Update()
	{
		CheckUserInput();

		if ( particleSystemNeedsUpdate ) {
			UpdateParticles ();
			particleSystemNeedsUpdate = false;
		}
	}

	public void SetSelectedColor( int newSelectedColor ) { selectedColor = newSelectedColor; }

	public void SetSelectedTexture( int newSelectedTexture ) { selectedMaterial = newSelectedTexture; }

	public void PickRandomColor( Color baseColor )
	{
		// pull the RGB values from baseColor
		float red = baseColor.r;
		float green = baseColor.g;
		float blue = baseColor.b;

		// randomize the colors based on color picker concept

	}

	void CheckUserInput()
	{
		if (Input.GetMouseButton( 0 )) {
			ApplyUserInput();
		} else {
			EndUserInput();
		}
	}

	void ApplyUserInput()
	{
		// Maps to CanvasPlane layer
		var layerMask = 1 << 8;
		Ray ray = Camera.main.ScreenPointToRay( Input.mousePosition );
		RaycastHit hit;

		if ( Physics.Raycast(ray, out hit, 20f, layerMask) ) {

			if ( lastPoint.HasValue ) {
				DrawLine( lastPoint.Value, hit.point );
			} else {
				DrawPoint( hit.point );
			}

			lastPoint = hit.point;
			particleSystemNeedsUpdate = true;

		}
	}

	void EndUserInput() { lastPoint = null; }

	void DrawLine( Vector3 p1, Vector3 p2 )
	{
		var dotsPerWorldSpace = 100f;
		var num = dotsPerWorldSpace * (p1 - p2).magnitude;

		for (var i = 0; i < num; i++) {
			var p = Vector3.Lerp (p1, p2, i / num);
			DrawPoint (p);
		}
	}

	void DrawPoint( Vector3 p )
	{
		var particle = new ParticleSystem.Particle ();

		particle.position = p;

		particle.color = allColors[selectedColor];
		particle.size = 0.02f * Random.Range(0.8f, 2f);
		particle.rotation = Random.Range(0f, 360f);

		//particle.angularVelocity = Random.Range(0f, 360f);
		//particle.velocity = Vector3(0,10,0);

		pointList.Add (particle);
	}
	
	void UpdateParticles()
	{
		// Note: this may not be the most efficient way to do this, if we hit performance issues start here
		var asArray = pointList.ToArray();
		particleSystem.SetParticles( asArray, asArray.Length );
		particleSystem.renderer.material = allMaterials[ selectedMaterial ];
	}

	public void ClearPoints()
	{
		pointList.Clear();
		particleSystemNeedsUpdate = true;
	}
}