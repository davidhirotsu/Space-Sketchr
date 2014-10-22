using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Sketchpad : MonoBehaviour {

	public Transform planeObject;

	Vector3 lastPoint;
	bool isDrawing = false;
	List<ParticleSystem.Particle> pointList = new List<ParticleSystem.Particle>();
	bool particleSystemNeedsUpdate = false;
	bool togglePlaneVisibility = false;
	
	void Update () {
		CheckUserInput ();

		if (particleSystemNeedsUpdate) {
			UpdateParticles ();
			particleSystemNeedsUpdate = false;
		}
	}

	void CheckUserInput () {
		if (Input.GetMouseButton (0)) {
			ApplyUserInput ();
			isDrawing = true;
		} else {
			isDrawing = false;
		}
	}

	void ApplyUserInput () {
		int layerMask = 1 << 8;
		// Maps to CanvasPlane layer
		Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
		RaycastHit hit;
		if (Physics.Raycast (ray, out hit, 20f, layerMask)) {
			if (isDrawing) {
				DrawLine (lastPoint, hit.point);
			} else {
				DrawPoint (hit.point);
			}
			lastPoint = hit.point;
			particleSystemNeedsUpdate = true;
		}
	}

	void DrawLine (Vector3 p1, Vector3 p2) {
		float dotsPerWorldSpace = 100f;
		float num = dotsPerWorldSpace * (p1 - p2).magnitude;
		for (int i = 0; i < num; i++) {
			Vector3 p = Vector3.Lerp (p1, p2, i / num);
			DrawPoint (p);
		}
	}

	void DrawPoint (Vector3 p) {
		var particle = new ParticleSystem.Particle ();
		particle.position = p;
		particle.color = new Color (1f, 1f, 1f, 1f);
		particle.size = 0.02f * Random.Range(0.8f, 1f);
		pointList.Add (particle);
	}

	/// <remarks>
	/// definitely not most efficient thing possible, as
	/// it streams more and more points every frame. A more robust
	/// solution here would probably break the particles into chunks,
	/// or figure out a way to ONLY send new points to Shuriken.
	/// </remarks>
	void UpdateParticles () {
		var asArray = pointList.ToArray ();
		particleSystem.SetParticles (asArray, asArray.Length);
	}

	void ClearPoints () {
		pointList.Clear ();
		particleSystemNeedsUpdate = true;
	}

	void OnGUI () {
		if (GUI.Button(new Rect(10, 10, 100, 50), "Clear")) {
			ClearPoints();
		}
	}
}
