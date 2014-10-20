using UnityEngine;
using System.Collections;

public class Sketchpad : MonoBehaviour {

	public GameObject particle;
	public Vector3 lastPoint;
	public bool isDrawing = false;

	void Start () {
	
	}

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
			}
		} else {
			isDrawing = false;
		}
	}

	void DrawLine (Vector3 p1, Vector3 p2) {
		// TODO: use a particle system here, not spheres.
		float num = 20f; // TODO: use line length, and ignore large gaps in time / space
		for (int i=0; i<num; i++) {
			Vector3 p = Vector3.Slerp (p1, p2, i / num);
			DrawPoint (p);
		}
	}

	void DrawPoint (Vector3 p) {
		Instantiate (particle, p, Quaternion.identity);
	}
}
