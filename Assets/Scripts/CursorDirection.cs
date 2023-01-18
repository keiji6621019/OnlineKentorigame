using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


public class CursorDirection : MonoBehaviour
{
	Plane plane = new Plane();
	float distance = 0;
	private Vector3 mouse;
	private Vector3 DefaltPos;

	void Start()
	{
		plane.SetNormalAndPosition(Vector3.back, transform.localPosition);
	}

	void Update()
	{
		cursorpoint();

        if (!Input.GetMouseButton(0))
        {
			Destroy(this.gameObject);
		}
	}

	public void Setcursor(Vector3 _DefaltPos)
	{
		DefaltPos = _DefaltPos;
	}

	void cursorpoint()
    {
			var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			
			
			mouse = Input.mousePosition;
			var worldPos = Camera.main.ScreenToWorldPoint(mouse);			
			//Debug.Log("transx :" + transform.position.x);
			//Debug.Log("mousey :" + mouse.y);
			//Debug.Log("transx :" + transform.position.y);
			double x = (Math.Pow((worldPos.x - DefaltPos.x), 2) + Math.Pow((worldPos.y - DefaltPos.y), 2));
			float y = Mathf.Sqrt((float)x);
			float z = y / 1.7f;
			float xx = 1 - (z / 10);
			
			if (plane.Raycast(ray, out distance))
			{
				var lookPoint = ray.GetPoint(distance); ;
				transform.LookAt(DefaltPos + Vector3.forward, lookPoint - DefaltPos);
			}

			transform.localScale = new Vector3(0.3f, z, 1);
		

    }
}