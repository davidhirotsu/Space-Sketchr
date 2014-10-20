using UnityEngine;
using System.Collections;

public class Sketchpad : MonoBehaviour {

	public GameObject particle;

	void Start () {
	
	}

	void Update () {
		if (Input.GetMouseButton(0)) {
			int layerMask = 1 << 8; // Maps to CanvasPlane layer
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			RaycastHit hit;
			if (Physics.Raycast(ray, out hit, 20F, layerMask)) {
				Debug.Log (hit.point);
				Instantiate(particle, hit.point, transform.rotation);
			}
		}
	}
}
