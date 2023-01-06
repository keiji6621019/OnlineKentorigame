using System.Collections;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using ExitGames.Client.Photon;
using TMPro;
using System.Collections.Generic;

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
    int currentAmount;
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
    private Vector3 Pos;
    private Vector3 FieldPos;
    private Vector3 Playerpos;
    private Vector3 Enemypos;

    

    private void Awake()
    {
        
            instance = this;
        
    }

    void Start()
    {
        
        if (PhotonNetwork.IsMasterClient)
        {
            //名前の生成　被りなし
            //int start = 1;
            //int end = 10;

            //List<int> numbers = new List<int>();

            //for (int i = start; i <= end; i++)
            //    {
            //        numbers.Add(i);
            //    }

            //    while (numbers.Count > 0)
            //    {

            //        name = Random.Range(0, numbers.Count).ToString();

            //        numbers.RemoveAt(name);
            //    }

            this.name = Random.Range(0, 200).ToString();

        }

        if (photonView.IsRoomView)
        {
            
            
                
                GetComponent<SpriteRenderer>().material = materials[0];
                transform.parent.GetComponent<Renderer>().material = fieldmaterials[0];
                this.fraction = Fraction.NPC;
                this.type = NodeType.SMALL_CITY;
                amountText.text = "10/10";
                //transform.GetChild(1).GetComponent<Renderer>().material = fieldcolor["Glay"];
                //Sprite fieldsprite = Resources.Load<Sprite>("Sprites/" + name);
                //transform.GetChild(1).GetComponent<SpriteRenderer>().sprite = fieldsprite;
                //追加　子オブジェクトのマテリアル変更
                //transform.GetChild(1).GetComponent<Renderer>().material = fieldmaterials[0];
                //Invoke("NPCField", 0.5f);
                //Fieldsetting();

            
            //Dictionary<string, Material> fieldcolor = new Dictionary<string, Material>
            //{
            //{"Glay", fieldmaterials[0]},
            //{"Red", fieldmaterials[1]},
            //{"Blue", fieldmaterials[2]},
            //};
            

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

        if (this.fraction == Fraction.PLAYER){
            StartCoroutine(Produce());
            UpdateAmountText();
        }

        //追加
        //foreach (Fieldcolor field in fieldcolors)
        //{
        //    fields.Add(field);
        //}
        //Instantiate(fieldObject, this.transform.position, Quaternion.identity);
        //GameObject newField = Instantiate(fieldObject, this.transform.position, Quaternion.identity);
        //Fieldcolor field = newField.GetComponent<Fieldcolor>();
        //newField.GetComponent<Fieldcolor>().Setfield(GetComponent<SpriteRenderer>().material);

        //Debug.Log(photonView.Owner.UserId);
    }


    void IPunObservable.OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        PhotonNetwork.SendRate = 50; // 1秒間にメッセージ送信を行う回数
        PhotonNetwork.SerializationRate = 10; // 1秒間にオブジェクト同期を行う回数
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

    void Produceunit()
    {
        //Debug.Log("数を増やします");
        currentAmount++;
        if(currentAmount>maxAmount)
        {
            currentAmount = maxAmount;
        }

    UpdateAmountText(); 

    } 
    IEnumerator Produce()
        {
            while(true)
            {
                yield return new WaitForSeconds(1);
                Produceunit();
            }
        }

    void UpdateAmountText()
    {
       // Debug.Log("玉の情報を更新します。");
        amountText.text = currentAmount+"|"+ maxAmount;
    }
    
    IEnumerator SendALLunits(string _tag)
    {
        Node goalNode = GameObject.Find(_tag).GetComponent<Node>();

        int unitsToSend = currentAmount;
        while(currentAmount > 0)
        {
            int unitsPerSend = currentAmount <= 5 ? currentAmount : 5;

            AllUnit(goalNode,unitsPerSend);
            currentAmount -= unitsPerSend;
            //currentAmount -= unitsPerSend;
            yield return new WaitForSeconds(0.5f);
            UpdateAmountText();
            

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
            newUnit.GetComponent<Unit>().SetUnit(fraction, goalNode, GetComponent<SpriteRenderer>().material,transform.parent.GetComponent<Fieldcolor>(),degree);

        }

    }
    [PunRPC]
    public void SendUnits(string tag)
    {
        if (photonView.IsMine)
        {
            photonView.RPC(nameof(SendUnits), RpcTarget.Others, tag);    
        }
        StartCoroutine(SendALLunits(tag));

    }

    

    public void HandleIncomingUnit(Fraction f,int senderID, Fieldcolor _fieldcolor)
    {
        //Debug.Log(f);
       if(f == fraction)
       {
        //Debug.Log("Nodeの情報を変更します");
        Produceunit();
            return;
       }
       else
       {
        DestroyUnit(f,senderID,_fieldcolor);
       }
    }

    void DestroyUnit(Fraction f, int senderID, Fieldcolor _fieldcolor)
    {
        UpdateAmountText();
        Sprite fieldStrong = transform.parent.GetComponent<SpriteRenderer>().sprite;

        switch(_fieldcolor.name)
        {
            case "渋谷新宿":
                currentAmount -= 5;
                break;
    
            case "練馬区":
                currentAmount -= 5;
                break;

            default:
                Debug.Log("何もない");
                break;
        }
        
        



        switch(fieldStrong.name)
        {
            case "渋谷新宿":
                currentAmount = currentAmount - 1 / 2;
                break;
            
            case "練馬区":
                currentAmount = currentAmount - 1 / 2;
                break;

            case "江戸川区":
                currentAmount = currentAmount - 10 / 3;
                    break;
            
            case "千代田中央":
                currentAmount = currentAmount - 10 / 3;
                    break;

                

            default:
                currentAmount--;
                break;




        }
        
        if(currentAmount<=0)
        {
            currentAmount = 0;
            FractionHander(f,senderID);
        }
    }
    void FractionHander(Fraction f, int senderID)
    {
        fraction = f;
        switch(fraction)
        {
           case Fraction.PLAYER: //相手のオブジェクトが自分のになるとき
                GetComponent<SpriteRenderer>().material = materials[1];
                //追加
                //transform.GetChild(1).GetComponent<Renderer>().material = fieldmaterials[1];
                PlayerField();

                StartCoroutine(Produce());
                photonView.RequestOwnership();


                break;
           
           case Fraction.ENEMY:
                GetComponent<SpriteRenderer>().material = materials[2];
                //追加
                //transform.GetChild(1).GetComponent<Renderer>().material = fieldmaterials[2];
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
                //GetComponent<SpriteRenderer>().material = materials[1];

                //追加
                //Fieldsetting();

                //追加
                //transform.parent.GetComponent<Renderer>().material = fieldmaterials[1];
                //PlayerField();
                this.fraction = Fraction.PLAYER;

                //追加　位置変更
                //var playerposition = new Vector3(0, -6, 0);
                //Playerpos = playerposition;
                //this.Pos = new Vector3(0, 5, 0);
            }
            else
            {
                Debug.Log("他プレイヤーがネットワークオブジェクトを生成しました");
                //GetComponent<SpriteRenderer>().material = materials[2];
                //追加
                //Fieldsetting();

                //追加
                //transform.parent.GetComponent<Renderer>().material = fieldmaterials[2];
                //EnemyField();
                this.fraction = Fraction.ENEMY;

                //追加　位置変更
                //var enemyposition = new Vector3(0,6,0);
                //Enemypos = enemyposition;
                //this.Pos = new Vector3(0, -5, 0);


            }
        }


        }
        public void NPCField()
    {
        transform.parent.GetComponent<Renderer>().material = fieldmaterials[0];
       
    }
    public void PlayerField()
    {
        transform.parent.GetComponent<Renderer>().material = fieldmaterials[1];
      
    }
    public void EnemyField()
    {
        transform.parent.GetComponent<Renderer>().material = fieldmaterials[2];
    }

    [PunRPC]
    public void setSprite(string name)
    {
        if (photonView.IsMine)
        {
            Debug.Log("isMine");
            photonView.RPC("setSprite", RpcTarget.OthersBuffered, name);
        }
        else
        {

            Debug.Log(name);
            this.transform.parent.name = name;
            Sprite fieldsprite = Resources.Load<Sprite>("WardSprites/" + name);
            this.transform.parent.GetComponent<SpriteRenderer>().sprite = fieldsprite;
        }
        

    }

    //void FieldHander()
    //{
    //    var sprite = GetComponentInParent<SpriteRenderer>().sprite;
    //    switch (sprite)
    //    {
    //        case sprite: //相手のオブジェクトが自分のになるとき
    //            GetComponent<SpriteRenderer>().material = materials[1];
    //            //追加
    //            //transform.GetChild(1).GetComponent<Renderer>().material = fieldmaterials[1];
    //            PlayerField();

    //            StartCoroutine(Produce());
    //            photonView.RequestOwnership();


    //            break;

    //        case Fraction.ENEMY:
    //            GetComponent<SpriteRenderer>().material = materials[2];
    //            //追加
    //            //transform.GetChild(1).GetComponent<Renderer>().material = fieldmaterials[2];
    //            EnemyField();

    //            StopCoroutine(Produce());
    //            photonView.TransferOwnership(senderID);

    //            break;
    //    }
    //}

    //public void swichfield()
    //{
    //    foreach (Fieldcolor field in fieldcolors)
    //    {
    //        field.gameObject.SetActive(false);
    //    }

    //}

    //追加
    //public void Fieldsetting()
    //{
    //    GameObject newField = Instantiate(fieldObject, this.transform.position, Quaternion.identity);
    //    newField.GetComponent<Fieldcolor>().Setfield(GetComponent<SpriteRenderer>().material);
    //    Debug.Log(GetComponent<SpriteRenderer>().material);
    //}
    //public void Playerposition()
    //{
    //    if (photonView.IsMine && !photonView.IsRoomView)
    //    {
    //        this.Pos = new Vector3(0, 5, 0);

    //    }
    //    else if (!photonView.IsMine && !photonView.IsRoomView)
    //    {
    //        this.Pos = new Vector3(0, -5, 0);
    //    }

    //}
}
