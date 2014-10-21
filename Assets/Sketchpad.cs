using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Sketchpad : MonoBehaviour {

	public Transform planeObject;

	Vector3 lastPoint;
	bool isDrawing = false;
	List<ParticleSystem.Particle> cloud = new List<ParticleSystem.Particle>();
	bool needsUpdate = false;
	bool togglePlaneVisibility = false;
	
	void Update () {
		if (Input.GetMouseButton (0)) {
			int layerMask = 1 << 8; // Maps to CanvasPlane layer
			Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
			RaycastHit hit;
			if (Physics.Raycast (ray, out hit, 20F, layerMask)) {
				if (isDrawing) {
					DrawLine (lastPoint, hit.point);
				} else {
					DrawPoint (hit.point);
				}
				lastPoint = hit.point;
				isDrawing = true;
				needsUpdate = true;
			}
		} else {
			isDrawing = false;
		}

		if (needsUpdate) {
 			var cloudArray = cloud.ToArray(); // TODO: definitely not most efficient thing possible
			particleSystem.SetParticles (cloudArray, cloudArray.Length);
			needsUpdate = false;
		}
	}

	void DrawLine (Vector3 p1, Vector3 p2) {
		float dotsPerWorldSpace = 100f;
		float num = dotsPerWorldSpace * (p1 - p2).magnitude;
		for (int i=0; i<num; i++) {
			Vector3 p = Vector3.Lerp (p1, p2, i / num);
			DrawPoint (p);
		}
	}

	void DrawPoint (Vector3 p) {
		var particle = new ParticleSystem.Particle ();
		particle.position = p;
		particle.color = new Color (1f, 1f, 1f, 1f);
		particle.size = 0.03f;
		cloud.Add (particle);
	}

	void ClearPoints () {
		cloud.Clear ();
		needsUpdate = true;
	}

	void OnGUI () {
		if (GUI.Button(new Rect(10, 10, 100, 100), "Clear")) {
			ClearPoints();
		}

		togglePlaneVisibility = GUI.Toggle (new Rect (10, 150, 100, 100), togglePlaneVisibility, "Plane");
		planeObject.renderer.enabled = togglePlaneVisibility;
	}
}
