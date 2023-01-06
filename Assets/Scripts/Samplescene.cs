using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

// MonoBehaviourPunCallbacksを継承して、PUNのコールバックを受け取れるようにする
public class Samplescene : MonoBehaviourPunCallbacks
{
    List<string> room = new List<string>();

    Vector3[] playerpos = new Vector3[2];
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
    
        Dictionary<string, Vector3> Wardfpos = new Dictionary<string, Vector3>
        {
         {"江戸川区",new Vector3(1.66131926f,0.124431908f,-1.30000019f)},  
         {"葛飾区",new Vector3(1.39805746f,1.050547f,-1.30000019f)},   
         {"足立区",new Vector3(0.668040276f,1.45500994f,-1.30000019f)},   
         {"北区",new Vector3(-0.197609901f,1.19140005f,-1.30000019f)},   
         {"板橋区",new Vector3(-0.865769863f,1.26795995f,-1.30000019f)},  
         {"練馬区",new Vector3(-1.55133009f,0.942840993f,-1.30000019f)},  
         {"杉並中野",new Vector3(-1.36949968f,0.164799988f,-1.30000019f)},
         {"世田谷区",new Vector3(-1.47389984f,-0.798289895f,-1.30000019f)},
         {"大田区",new Vector3(-0.194999695f,-1.87361002f,-1.30000019f)},
         {"品川目黒",new Vector3(-0.456870079f,-0.995779991f,-1.30000019f)},
         {"港区",new Vector3(-0.170639992f,-0.59645009f,-1.30000019f)},
         {"千代田中央",new Vector3(0.168660164f,-0.151880026f,-1.30000019f)},
         {"墨田荒川台東",new Vector3(0.618450165f,0.56151998f,-1.30000019f)},
         {"江東区",new Vector3(0.816810131f,-0.3398f,-1.30000019f)},
         {"豊島文京",new Vector3(-0.322019577f,0.522369981f,-1.30000019f)},
         {"渋谷新宿",new Vector3(-0.60999012f,-0.0622699261f,-1.30000019f)},
         

        };

    Dictionary<string, Vector3> nodePos = new Dictionary<string, Vector3>  //nodeのろーかるポジション
        {
         {"千葉",new Vector3(0, 0, 0)},
         {"東京",new Vector3(0, 0, 0)},
         {"栃木",new Vector3(0, 0, 0)},
         {"埼玉",new Vector3(0, 0, 0)},
         {"茨城",new Vector3(0, 0, 0)},
         {"群馬",new Vector3(0, 0, 0)},
         {"神奈川",new Vector3(0, 0, 0)},

        };



    List<string> npcname = new List<string>();
    public Material[] materials;
    
    public Material[] fieldmaterials;

    Sprite Prefstatas;
    public Sprite[] prefsprites;

    [SerializeField]
    private Text _textCountdown;
    public GameObject textGameObject;

    



    //nodeのろーかるポジション

    private void Start()
    {
        //Titleから入る場合
        if (PhotonNetwork.IsConnected)
        {
            _textCountdown.text = "";
            StartCoroutine(CountdownCoroutine());
            
            //PhotonNetwork.OfflineMode = true;
        }
        else
        {
            SceneManager.LoadScene(0);
        }

        //PhotonServerSettingsの設定内容を使ってマスターサーバーへ接続する

        
        //PhotonNetwork.ConnectUsingSettings();



    }

    

    
    IEnumerator CountdownCoroutine()
    {
        
        _textCountdown.gameObject.SetActive(true);
        textGameObject.SetActive(true);

        _textCountdown.text = "3";
        yield return new WaitForSeconds(1.0f);

        _textCountdown.text = "2";
        yield return new WaitForSeconds(1.0f);

        _textCountdown.text = "1";
        yield return new WaitForSeconds(1.0f);

        _textCountdown.text = "GO!";
        NodeInstantiate();
        yield return new WaitForSeconds(1.0f);

        _textCountdown.text = "";
        
        _textCountdown.gameObject.SetActive(false);
        textGameObject.SetActive(false);

    }



    // マスターサーバーへの接続が成功した時に呼ばれるコールバック
    //public override void OnConnectedToMaster()
    //{
    //    //    // "Room"という名前のルームに参加する（ルームが存在しなければ作成して参加する）
    //    PhotonNetwork.JoinOrCreateRoom("Room", new RoomOptions(), TypedLobby.Default);


    //}

    private void Update()
    {
        double photonTime = PhotonNetwork.Time;
    }

    // ゲームサーバーへの接続が成功した時に呼ばれるコールバック
    //public override void OnJoinedRoom()
    //{
    //    NodeInstantiate();
        


    //}
    // ランダムな座標に自身のアバター（ネットワークオブジェクト）を生成する
    //var position = new Vector3(0,0,0);
    public void NodeInstantiate()
    {
        
        

        Dictionary<string, Material> fieldcolor = new Dictionary<string, Material>
    {
        {"Glay", fieldmaterials[0]},
        {"Red", fieldmaterials[1]},
        {"Blue", fieldmaterials[2]},


    };


        //Vector3 position = new Vector3(2.41121197f, -2.11983871f, 10.008172f);
        //if (PhotonNetwork.IsMasterClient)
        //{
        //var field = PhotonNetwork.Instantiate("千葉", position, Quaternion.identity);
        ////var node = PhotonNetwork.Instantiate("Node", position, Quaternion.identity);
        //field.GetComponent<Renderer>().material = fieldcolor["Red"];
        //Sprite sprite = Resources.Load<Sprite>("Sprites/千葉");
        //field.GetComponent<SpriteRenderer>().sprite = sprite;

        //}
        //var dic = this.Prefpos;
        ////dic.Remove("栃木");
        //dic.Remove("千葉");
        //dic.Remove("栃木");

        Vector3 wardposition = new Vector3(-1.55133009f, 0.942840993f, -1.30000019f);
        if (PhotonNetwork.IsMasterClient)
        {
            var wardfield = PhotonNetwork.Instantiate("江戸川区", wardposition, Quaternion.identity);
            //var node = PhotonNetwork.Instantiate("Node", position, Quaternion.identity);
            wardfield.GetComponent<Renderer>().material = fieldcolor["Red"];
            Sprite sprite = Resources.Load<Sprite>("WardSprites/練馬区");
            wardfield.GetComponent<SpriteRenderer>().sprite = sprite;
            wardfield.name = "練馬区";
            Node node = wardfield.transform.GetChild(0).gameObject.GetComponent<Node>();
            node.setSprite("練馬区");

        }
        var dic = this.Wardfpos;
        //dic.Remove("栃木");
        dic.Remove("練馬区");
        dic.Remove("渋谷新宿");


        //Vector3[] positions =
        //    {
        //        new Vector3(0, 0, 0),
        //        new Vector3(2.33f, -1.34f, 0),
        //        new Vector3(4.66f, 0, 0),
        //        new Vector3(-4.66f, 0, 0),
        //        new Vector3(-2.33f, -1.34f, 0),
        //        new Vector3(-2.33f, 1.34f, 0),
        //        new Vector3(2.33f, 1.34f, 0)

        //    };

        //if (PhotonNetwork.IsMasterClient)
        //{
           
        //    foreach (var (name,fieldPos) in dic)
        //    {
            
        //        //var Npcnode = PhotonNetwork.InstantiateRoomObject("Node", fieldPos, Quaternion.identity);
        //        //Vector3 localPos = nodePos[name];
        //        var NpcField = PhotonNetwork.InstantiateRoomObject("千葉", fieldPos, Quaternion.identity);
        //        NpcField.GetComponent<Renderer>().material = fieldcolor["Glay"];
        //        Sprite fieldsprite = Resources.Load<Sprite>("Sprites/" + name);
        //        NpcField.GetComponent<SpriteRenderer>().sprite = fieldsprite;
        //        NpcField.name = name;
        //        Node node = NpcField.transform.GetChild(0).gameObject.GetComponent<Node>();
        //        node.setSprite(name);
        //        npcname.Add(name);

        //    }
            

        //}
        
        if (PhotonNetwork.IsMasterClient)
        {
           
            foreach (var (name,fieldPos) in dic)
            {
            
                //var Npcnode = PhotonNetwork.InstantiateRoomObject("Node", fieldPos, Quaternion.identity);
                //Vector3 localPos = nodePos[name];
                var NpcField = PhotonNetwork.InstantiateRoomObject("江戸川区", fieldPos, Quaternion.identity);
                NpcField.GetComponent<Renderer>().material = fieldcolor["Glay"];
                Sprite fieldsprite = Resources.Load<Sprite>("WardSprites/" + name);
                NpcField.GetComponent<SpriteRenderer>().sprite = fieldsprite;
                NpcField.name = name;
                Node node = NpcField.transform.GetChild(0).gameObject.GetComponent<Node>();
                node.setSprite(name);
                npcname.Add(name);

            }
            

        }

        if (PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.CurrentRoom.SetStartTime(PhotonNetwork.ServerTimestamp);
        }
        else
        {
            Prefpos.Add("渋谷新宿", new Vector3(-0.60999012f, -0.0622699261f, -1.30000019f));
            //Debug.Log("栃木");
            var myPos = Prefpos["渋谷新宿"];
            var field = PhotonNetwork.Instantiate("江戸川区", myPos, Quaternion.identity);
            field.GetComponent<Renderer>().material = fieldcolor["Red"];
            Sprite sprite = Resources.Load<Sprite>("WardSprites/渋谷新宿");
            field.GetComponent<SpriteRenderer>().sprite = sprite;
            Node node = field.transform.GetChild(0).gameObject.GetComponent<Node>();
            node.setSprite("渋谷新宿");
            if (PhotonNetwork.IsMasterClient)
            {
                Destroy(field.gameObject);
            }
            
        }
    }

       
    public Vector3 PlayerPoint()
    {
        playerpos[0] = new Vector3(2.41121197f, -2.11983871f, 10.008172f);
        //playerpos[1] = new Vector3(-1, -1, 0);
        return playerpos[Random.Range(0, playerpos.Length)];
    }

}