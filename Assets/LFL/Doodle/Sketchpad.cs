using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Sketchpad : MonoBehaviour {

	public Transform planeObject;

	Vector3? lastPoint;
	List<ParticleSystem.Particle> pointList = new List<ParticleSystem.Particle>();
	bool particleSystemNeedsUpdate = false;
	
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
		} else {
			EndUserInput ();
		}
	}

	void ApplyUserInput () {
		// Maps to CanvasPlane layer
		var layerMask = 1 << 8;
		Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
		RaycastHit hit;
		if (Physics.Raycast (ray, out hit, 20f, layerMask)) {
			if (lastPoint.HasValue) {
				DrawLine (lastPoint.Value, hit.point);
			} else {
				DrawPoint (hit.point);
			}
			lastPoint = hit.point;
			particleSystemNeedsUpdate = true;
		}
	}

	void EndUserInput () {
		lastPoint = null;
	}

	void DrawLine (Vector3 p1, Vector3 p2) {
		var dotsPerWorldSpace = 100f;
		var num = dotsPerWorldSpace * (p1 - p2).magnitude;
		for (var i = 0; i < num; i++) {
			var p = Vector3.Lerp (p1, p2, i / num);
			DrawPoint (p);
		}
	}

	void DrawPoint (Vector3 p) {
		var particle = new ParticleSystem.Particle ();
		particle.position = p;
		particle.color = new Color (Random.Range(0.1f, 0.9f), Random.Range(0.1f, 0.9f), Random.Range(0.1f, 0.9f), Random.Range(0.8f, 1f));
		particle.size = 0.02f * Random.Range(0.8f, 2f);
		particle.rotation = Random.Range(0f, 360f);
		//particle.angularVelocity = Random.Range(0f, 360f);
		//particle.velocity = Vector3(0,10,0);
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

	public void ClearPoints () {
		pointList.Clear ();
		particleSystemNeedsUpdate = true;
	}
}
