using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class GameManager : MonoBehaviourPunCallbacks
{

    public static GameManager instance;

    void Awake() {
        {
            instance = this;
        }
    }
    public Node selectedNode;
    public Node nodeToVisit;

    public void HandleSelectedNodes(Node node)
    { 
         

        
    if(selectedNode == null)
    {
        Debug.Log("node SELECTED");
        selectedNode = node;
        return;
    }

        
    }
    [PunRPC]
    public void VisutToNodes(Node node)
    {
        if(selectedNode.fraction != Fraction.PLAYER)
        {
            selectedNode = null;
            nodeToVisit = null;
            return;
        }

        if (selectedNode == node || node == null)
        {
            selectedNode = null;
            nodeToVisit = null;

            Debug.Log("Node DESELECTED");
            return;
        }

        if (selectedNode != node && selectedNode != null)
        {
            nodeToVisit = node;
            string goalNodeTag = nodeToVisit.name;
            Debug.Log("VISIT node SELECTED");
            
           selectedNode.SendUnits(goalNodeTag);

            selectedNode = null;
            nodeToVisit = null;
        }
    }

        //public void Selectednodenull()
        //{
        //    selectedNode = null;
        //    Debug.Log("Node DESELECTED");
        //}

}
