using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class InputManager : MonoBehaviourPunCallbacks
{
    Node selectedNode;
    Node nodeToVisit;
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            int layerMask = 1;
            float maxDistance = 10;
            RaycastHit2D hit = Physics2D.Raycast((Vector2)ray.origin, (Vector2)ray.direction, maxDistance, layerMask);

            if (hit.collider)
            {
                Node n = hit.collider.GetComponent<Node>();

                if (n != null)
                {
                    n.ArrowStart();
                    //if (SystemInfo.supportsVibration)
                    //{
                    //    Handheld.Vibrate();
                    //}
                    GameManager.instance.HandleSelectedNodes(n);
                    return;
                }

            }
        }
        else if (Input.GetMouseButtonUp(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            int layerMask = 1;
            float maxDistance = 10;
            RaycastHit2D hit = Physics2D.Raycast((Vector2)ray.origin, (Vector2)ray.direction, maxDistance, layerMask);
            //Nullè¡Ç∑
            if (hit.collider)
            {
                Node n = hit.collider.GetComponent<Node>();

                if (n != null)
                {
                    Debug.Log("ó£ÇµÇΩ");
                    GameManager.instance.VisutToNodes(n);
                   
                }
                
            }
            //else
            //{
            //    GameManager.instance.Selectednodenull();
            //}
        }
    }
    
}
