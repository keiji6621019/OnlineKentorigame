using System.Collections;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using ExitGames.Client.Photon;
using TMPro;
using System.Collections.Generic;
using System;
using UnityEngine.UI;

public enum Fraction
{
    NPC,
    ENEMY,
    PLAYER
}

public class Node : MonoBehaviourPunCallbacks, IPunObservable, IPunInstantiateMagicCallback
{
    public static Node instance;
    int maxAmount;
    int startAmount = 10;
    public int currentAmount;
    int numberOfUnits;

    public Material[] materials;
    public Material[] fieldmaterials;

    
    //追加
    //public Fieldcolor[] fieldcolors;
    //public List<Fieldcolor> fields = new List<Fieldcolor>();
    //追加　フィールドオブジェクトのマテリアル変更
    //public GameObject fieldObject;



    public TMP_Text amountText;
    public enum NodeType
    {
        BOSS_CITY,
        BIG_CITY,
        MEDIUM_CITY,
        SMALL_CITY
    }

    public Fraction fraction;

    public NodeType type;

    public GameObject unitPrefab;
    public GameObject arrowPrefab;
    public GameObject DamageTextPrefab;

    private Vector3 Pos;
    private Vector3 FieldPos;
    private Vector3 Playerpos;
    private Vector3 Enemypos;

    Plane plane = new Plane();
    float distance = 0;
    private Vector3 mouse;



    private void Awake()
    {

        instance = this;

    }

    void Start()
    {
        plane.SetNormalAndPosition(Vector3.back, transform.localPosition);

        if (PhotonNetwork.IsMasterClient)
        {

            this.name = UnityEngine.Random.Range(0, 20000).ToString();

        }

        if (photonView.IsRoomView)
        {
            Sprite fieldStrong = transform.parent.GetComponent<SpriteRenderer>().sprite;
            GetComponent<SpriteRenderer>().material = materials[0];
            if (fieldStrong.name == "大田区")
            {
                Debug.Log("ルーム変更");
                transform.parent.GetComponent<Renderer>().material = fieldmaterials[3];
            }
            else
            {
                transform.parent.GetComponent<Renderer>().material = fieldmaterials[0];
            }
            
            this.fraction = Fraction.NPC;
            this.type = NodeType.SMALL_CITY;
            amountText.text = "10/10";


        }


        currentAmount = startAmount;

        switch (type)

        {
            case NodeType.SMALL_CITY:
                {
                    maxAmount = 10;
                }
                break;

            case NodeType.MEDIUM_CITY:
                {
                    maxAmount = 20;
                }
                break;

            case NodeType.BIG_CITY:
                {
                    maxAmount = 50;
                }
                break;

            case NodeType.BOSS_CITY:
                {
                    maxAmount = 100;
                }
                break;
        }

        if (this.fraction == Fraction.PLAYER)
        {
            StartCoroutine(Produce());
            UpdateAmountText();
        }

    }

    //private void Update()
    //{
    //    elapsedTime += Time.deltaTime;
    //}


    void IPunObservable.OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        PhotonNetwork.SendRate = 100; // 1秒間にメッセージ送信を行う回数
        PhotonNetwork.SerializationRate = 50; // 1秒間にオブジェクト同期を行う回数
        //&& this.photonView.IsMine
        //Debug.Log("発生");
        if (stream.IsWriting)
        {
            stream.SendNext(this.name);
            stream.SendNext(currentAmount);         

        }
        else
        {   

            this.name = (string)stream.ReceiveNext();
            currentAmount = (int)stream.ReceiveNext();

            
            
          UpdateAmountText();

        }
    }

    public void ArrowStart()
    {
        Pos = this.transform.position;
        
        GameObject newArrow = Instantiate(arrowPrefab, Pos, Quaternion.identity);
        newArrow.GetComponent<CursorDirection>().Setcursor(Pos);


    }

    void Produceunit()
    {
        //Debug.Log("数を増やします");
        currentAmount++;
        if (currentAmount > maxAmount)
        {
            currentAmount = maxAmount;
        }

        UpdateAmountText();

    }


    IEnumerator Produce()
    {
        while (true)
        {
            yield return new WaitForSeconds(1);
            Produceunit();
        }
    }

    void UpdateAmountText()
    {
        // Debug.Log("玉の情報を更新します。");
        amountText.text = currentAmount + "|" + maxAmount;
    }

    IEnumerator SendALLunits(string _tag,int numberOfUnits)
    {
        Node goalNode = GameObject.Find(_tag).GetComponent<Node>();
        
        while (numberOfUnits > 0)
        {           
            int unitsPerSend = numberOfUnits <= 5 ? numberOfUnits : 5;
            //Debug.Log(unitsPerSend);
            AllUnit(goalNode, unitsPerSend);
            numberOfUnits -= unitsPerSend;
            //Unitpersend(unitsPerSend);
            yield return new WaitForSeconds(0.5f);
            UpdateAmountText();
            if (photonView.IsMine)
            {
                currentAmount -= unitsPerSend;

            }
        }



    }

    public void AllUnit(Node goalNode, int numberOfUnits)
    {
        for (int i = 0; i < numberOfUnits; i++)
        {
            //Debug.Log(i);
            Pos = this.transform.position;
            GameObject newUnit = Instantiate(unitPrefab, Pos, Quaternion.identity);
            Unit unit = newUnit.GetComponent<Unit>();
            unit.senderID = photonView.OwnerActorNr;
            float intervalDeg = 10f;
            float minDeg = -(numberOfUnits - 1) * intervalDeg / 2;
            float degree = (minDeg + intervalDeg * (float)i) / 180f * Mathf.PI;
            newUnit.GetComponent<Unit>().SetUnit(fraction, goalNode, GetComponent<SpriteRenderer>().material, transform.parent.GetComponent<Fieldcolor>(), degree);

        }

    }
    [PunRPC]
    public void SendUnits(string tag, int currentAmount)
    {
        if (photonView.IsMine)
        {

            photonView.RPC(nameof(SendUnits), RpcTarget.Others,tag,currentAmount);
            
        }
        StartCoroutine(SendALLunits(tag, currentAmount));



    }



    public void HandleIncomingUnit(Fraction f, int senderID, Fieldcolor _fieldcolor)
    {
        //Debug.Log(f);
        if (f == fraction)
        {
            //Debug.Log("Nodeの情報を変更します");
            Produceunit();
            return;
        }
        else
        {
            DestroyUnit(f, senderID, _fieldcolor);
        }
    }

    void DestroyUnit(Fraction f, int senderID, Fieldcolor _fieldcolor)
    {
        UpdateAmountText();
        Sprite fieldStrong = transform.parent.GetComponent<SpriteRenderer>().sprite;
        var fieldcolorPos = this.transform.position;

        //攻撃
        //switch (_fieldcolor.name)
        //{
        //    case "渋谷新宿":
        //        currentAmount -= 2;
        //        break;

        //    case "練馬区":
        //        currentAmount -= 2;
        //        break;

        //    default:
        //        Debug.Log("何もない");
        //        break;
        //}




        //防御力
        switch (fieldStrong.name)
        {
            //case "渋谷新宿":
            //    currentAmount = currentAmount - 1 / 2;
            //    break;

            //case "練馬区":
            //    currentAmount = currentAmount - 1 / 2;
            //    break;

            case "江戸川区":
                
                var Range = UnityEngine.Random.Range(1, 3);
                currentAmount = currentAmount - Range;

                TextShowUp(Range, fieldcolorPos);
                
                
                break;

            case "千代田中央":
                var Range1 = UnityEngine.Random.Range(1, 3);
                currentAmount = currentAmount - Range1;
                TextShowUp(Range1, fieldcolorPos);
                break;



            default:
                if (photonView.IsMine) { 
                currentAmount--;
        }
                break;




        }


        if (currentAmount <= 0 && photonView.IsMine)
        {
            currentAmount = 0;
            //photonView.RPC("FractionHander", RpcTarget.All, f, senderID);
            FractionHander(f, senderID);
            UpdateAmountText();


        }
    }

    public void TextShowUp(int range, Vector3 fieldcolorPos)
    {
        if (range > 1)
        {
            GameObject canvas = GameObject.Find("TopCanvas");
            GameObject prefab1 = Instantiate(DamageTextPrefab, canvas.transform);
            prefab1.GetComponent<Text>().text = "-" + range;
            //prefab1.transform.SetParent(canvas.transform);
            prefab1.transform.position = fieldcolorPos;

        }

    }



    [PunRPC]
    void FractionHander(Fraction f, int senderID)
    {
        fraction = f;
        switch (fraction)
        {
            case Fraction.PLAYER: //相手のオブジェクトが自分のになるとき
                if (photonView.IsMine)
                {

                    photonView.RPC("FractionHander", RpcTarget.Others, Fraction.ENEMY,senderID);
                }
                //Debug.Log("自分に代わるよ");
                PlayerField();
                StartCoroutine(Produce());
                photonView.RequestOwnership();
                break;

            case Fraction.ENEMY:
                if (photonView.IsMine)
                {

                    photonView.RPC("FractionHander", RpcTarget.Others, Fraction.PLAYER,senderID);
                }
                EnemyField();
                StopCoroutine(Produce());
                photonView.TransferOwnership(senderID);

                break;
        }
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        throw new System.NotImplementedException();
    }

    void IPunInstantiateMagicCallback.OnPhotonInstantiate(PhotonMessageInfo info)
    {
        if (!photonView.IsRoomView)
        {
            if (info.Sender.IsLocal)
            {
                Debug.Log("自身がネットワークオブジェクトを生成しました");
                this.fraction = Fraction.PLAYER;
            }
            else
            {
                Debug.Log("他プレイヤーがネットワークオブジェクトを生成しました");

                this.fraction = Fraction.ENEMY;

            }
        }


    }
    public void NPCField()
    {
        transform.parent.GetComponent<Renderer>().material = fieldmaterials[0];


    }
    [PunRPC]
    public void PlayerField()
    {
        transform.parent.GetComponent<Renderer>().material = fieldmaterials[1];
        GetComponent<SpriteRenderer>().material = materials[1];

    }
    [PunRPC]
    public void EnemyField()
    {
        transform.parent.GetComponent<Renderer>().material = fieldmaterials[2];
        GetComponent<SpriteRenderer>().material = materials[2];
    }

    [PunRPC]
    public void setSprite(string name)
    {
        if (photonView.IsMine)
        {
            //Debug.Log("isMine");
            photonView.RPC("setSprite", RpcTarget.OthersBuffered, name);
        }
        else
        {

            //Debug.Log(name);
            this.transform.parent.name = name;
            Sprite fieldsprite = Resources.Load<Sprite>("WardSprites/" + name);
            this.transform.parent.GetComponent<SpriteRenderer>().sprite = fieldsprite;
        }


    }

}
