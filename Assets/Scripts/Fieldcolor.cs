using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Fieldcolor : MonoBehaviourPunCallbacks, IPunInstantiateMagicCallback
{
    public GameObject parentObject;
    public Material[] fieldmaterials;

    Dictionary<string, Vector3> Prefpos = new Dictionary<string, Vector3>
        {
         {"千葉",new Vector3(2.21121192f,1.52016139f,10.008172f)},
         {"東京",new Vector3(-0.488788128f,-1.24983859f,10.008172f)},
         {"栃木",new Vector3(-1.87878823f,2.19016147f,10.008172f)},
         {"埼玉",new Vector3(-0.958788157f,0.0571363568f,10.008172f)},
         {"茨城",new Vector3(2.21121192f,1.52016139f,10.008172f)},
         {"群馬",new Vector3(0.741211891f,2.7601614f,10.008172f)},
         {"神奈川",new Vector3(-0.784505904f,-2.50983858f,10.008172f)},


        };
    // Start is called before the first frame update

    void IPunInstantiateMagicCallback.OnPhotonInstantiate(PhotonMessageInfo info)
    {
        if (!photonView.IsRoomView)
        {
            if (info.Sender.IsLocal)
            {
                Debug.Log("自身がネットワークオブジェクトを生成しました");
                transform.GetChild(0).GetComponent<Renderer>().material = Node.instance.materials[1];
                GetComponent<Renderer>().material = Node.instance.fieldmaterials[1];

                transform.GetChild(0).GetComponent<Node>().fraction = Fraction.PLAYER;

            }
            else
            {
                Debug.Log("他プレイヤーがネットワークオブジェクトを生成しました");
                transform.GetChild(0).GetComponent<Renderer>().material = Node.instance.materials[2];
                GetComponent<Renderer>().material = Node.instance.fieldmaterials[2];
                //追加
                //Fieldsetting();

                //追加
                //transform.parent.GetComponent<Renderer>().material = fieldmaterials[2];
                //EnemyField();
                transform.GetChild(0).GetComponent<Node>().fraction = Fraction.ENEMY;

                //追加　位置変更
                //var enemyposition = new Vector3(0,6,0);
                //Enemypos = enemyposition;
                //this.Pos = new Vector3(0, -5, 0);


            }
        }
        //else
        //{
            

            //if (this.photonView.IsRoomView) //masterはなにもしない、othersは一つの県を生成、もうすでに生成したかどうかを調べる
            //{
                
            //    var myPos = Prefpos["栃木"];
            //    var field = PhotonNetwork.Instantiate("千葉", myPos, Quaternion.identity);
            //    //field.GetComponent<Renderer>().material = fieldcolor["Red"];
            //    Sprite sprite = Resources.Load<Sprite>("Sprites/栃木");
            //    field.GetComponent<SpriteRenderer>().sprite = sprite;
            //    field.name = "栃木";
            //        Debug.Log("分岐");
            //        Destroy(this);
               
                

            //}

            
        //}
        


    }

    
}

