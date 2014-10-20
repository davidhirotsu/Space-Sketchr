using UnityEngine;
using System.Collections;

[RequireComponent(typeof(LineRenderer))]
public class PivotHandle : MonoBehaviour
{
    private LineRenderer m_lines;

	// Use this for initialization
	void Start () 
    {
        m_lines = gameObject.GetComponent<LineRenderer>();

        m_lines.material = new Material(Shader.Find("Particles/Additive"));
        m_lines.SetWidth(0.05f, 0.05f);
        m_lines.SetVertexCount(5);
        m_lines.SetColors(Color.red, Color.blue);
	}
	
	// Update is called once per frame
	void Update ()
    {
        Vector3 origin = transform.position;

	    // draw red line (x-axis)
        Vector3 vecToRed = origin + transform.right;

        m_lines.SetPosition(0,vecToRed);
        m_lines.SetPosition(1, origin);

        // draw green line (y-axis)
        Vector3 vecToGreen = origin + transform.up;
        
        m_lines.SetPosition(2,vecToGreen);
        m_lines.SetPosition(3, origin);

        // draw blue line (z-axis)
        Vector3 vecToBlue = origin + transform.forward;
        
        m_lines.SetPosition(4,vecToBlue);
        //m_lines.SetPosition(6, origin);
	}
}
