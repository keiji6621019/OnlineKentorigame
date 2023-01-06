using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

// MonoBehaviourPunCallbacks���p�����āAPUN�̃R�[���o�b�N���󂯎���悤�ɂ���
public class Samplescene : MonoBehaviourPunCallbacks
{
    List<string> room = new List<string>();

    Vector3[] playerpos = new Vector3[2];
    Dictionary<string, Vector3> Prefpos = new Dictionary<string, Vector3>
        {
         {"��t",new Vector3(2.21121192f,1.52016139f,10.008172f)},  
         {"����",new Vector3(-0.488788128f,-1.24983859f,10.008172f)},   
         {"�Ȗ�",new Vector3(-1.87878823f,2.19016147f,10.008172f)},   
         {"���",new Vector3(-0.958788157f,0.0571363568f,10.008172f)},   
         {"���",new Vector3(2.21121192f,1.52016139f,10.008172f)},  
         {"�Q�n",new Vector3(0.741211891f,2.7601614f,10.008172f)},  
         {"�_�ސ�",new Vector3(-0.784505904f,-2.50983858f,10.008172f)},
         

        };
    
        Dictionary<string, Vector3> Wardfpos = new Dictionary<string, Vector3>
        {
         {"�]�ː��",new Vector3(1.66131926f,0.124431908f,-1.30000019f)},  
         {"������",new Vector3(1.39805746f,1.050547f,-1.30000019f)},   
         {"������",new Vector3(0.668040276f,1.45500994f,-1.30000019f)},   
         {"�k��",new Vector3(-0.197609901f,1.19140005f,-1.30000019f)},   
         {"����",new Vector3(-0.865769863f,1.26795995f,-1.30000019f)},  
         {"���n��",new Vector3(-1.55133009f,0.942840993f,-1.30000019f)},  
         {"��������",new Vector3(-1.36949968f,0.164799988f,-1.30000019f)},
         {"���c�J��",new Vector3(-1.47389984f,-0.798289895f,-1.30000019f)},
         {"��c��",new Vector3(-0.194999695f,-1.87361002f,-1.30000019f)},
         {"�i��ڍ�",new Vector3(-0.456870079f,-0.995779991f,-1.30000019f)},
         {"�`��",new Vector3(-0.170639992f,-0.59645009f,-1.30000019f)},
         {"���c����",new Vector3(0.168660164f,-0.151880026f,-1.30000019f)},
         {"�n�c�r��䓌",new Vector3(0.618450165f,0.56151998f,-1.30000019f)},
         {"�]����",new Vector3(0.816810131f,-0.3398f,-1.30000019f)},
         {"�L������",new Vector3(-0.322019577f,0.522369981f,-1.30000019f)},
         {"�a�J�V�h",new Vector3(-0.60999012f,-0.0622699261f,-1.30000019f)},
         

        };

    Dictionary<string, Vector3> nodePos = new Dictionary<string, Vector3>  //node�̂�[����|�W�V����
        {
         {"��t",new Vector3(0, 0, 0)},
         {"����",new Vector3(0, 0, 0)},
         {"�Ȗ�",new Vector3(0, 0, 0)},
         {"���",new Vector3(0, 0, 0)},
         {"���",new Vector3(0, 0, 0)},
         {"�Q�n",new Vector3(0, 0, 0)},
         {"�_�ސ�",new Vector3(0, 0, 0)},

        };



    List<string> npcname = new List<string>();
    public Material[] materials;
    
    public Material[] fieldmaterials;

    Sprite Prefstatas;
    public Sprite[] prefsprites;

    [SerializeField]
    private Text _textCountdown;
    public GameObject textGameObject;

    



    //node�̂�[����|�W�V����

    private void Start()
    {
        //Title�������ꍇ
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

        //PhotonServerSettings�̐ݒ���e���g���ă}�X�^�[�T�[�o�[�֐ڑ�����

        
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



    // �}�X�^�[�T�[�o�[�ւ̐ڑ��������������ɌĂ΂��R�[���o�b�N
    //public override void OnConnectedToMaster()
    //{
    //    //    // "Room"�Ƃ������O�̃��[���ɎQ������i���[�������݂��Ȃ���΍쐬���ĎQ������j
    //    PhotonNetwork.JoinOrCreateRoom("Room", new RoomOptions(), TypedLobby.Default);


    //}

    private void Update()
    {
        double photonTime = PhotonNetwork.Time;
    }

    // �Q�[���T�[�o�[�ւ̐ڑ��������������ɌĂ΂��R�[���o�b�N
    //public override void OnJoinedRoom()
    //{
    //    NodeInstantiate();
        


    //}
    // �����_���ȍ��W�Ɏ��g�̃A�o�^�[�i�l�b�g���[�N�I�u�W�F�N�g�j�𐶐�����
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
        //var field = PhotonNetwork.Instantiate("��t", position, Quaternion.identity);
        ////var node = PhotonNetwork.Instantiate("Node", position, Quaternion.identity);
        //field.GetComponent<Renderer>().material = fieldcolor["Red"];
        //Sprite sprite = Resources.Load<Sprite>("Sprites/��t");
        //field.GetComponent<SpriteRenderer>().sprite = sprite;

        //}
        //var dic = this.Prefpos;
        ////dic.Remove("�Ȗ�");
        //dic.Remove("��t");
        //dic.Remove("�Ȗ�");

        Vector3 wardposition = new Vector3(-1.55133009f, 0.942840993f, -1.30000019f);
        if (PhotonNetwork.IsMasterClient)
        {
            var wardfield = PhotonNetwork.Instantiate("�]�ː��", wardposition, Quaternion.identity);
            //var node = PhotonNetwork.Instantiate("Node", position, Quaternion.identity);
            wardfield.GetComponent<Renderer>().material = fieldcolor["Red"];
            Sprite sprite = Resources.Load<Sprite>("WardSprites/���n��");
            wardfield.GetComponent<SpriteRenderer>().sprite = sprite;
            wardfield.name = "���n��";
            Node node = wardfield.transform.GetChild(0).gameObject.GetComponent<Node>();
            node.setSprite("���n��");

        }
        var dic = this.Wardfpos;
        //dic.Remove("�Ȗ�");
        dic.Remove("���n��");
        dic.Remove("�a�J�V�h");


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
        //        var NpcField = PhotonNetwork.InstantiateRoomObject("��t", fieldPos, Quaternion.identity);
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
                var NpcField = PhotonNetwork.InstantiateRoomObject("�]�ː��", fieldPos, Quaternion.identity);
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
            Prefpos.Add("�a�J�V�h", new Vector3(-0.60999012f, -0.0622699261f, -1.30000019f));
            //Debug.Log("�Ȗ�");
            var myPos = Prefpos["�a�J�V�h"];
            var field = PhotonNetwork.Instantiate("�]�ː��", myPos, Quaternion.identity);
            field.GetComponent<Renderer>().material = fieldcolor["Red"];
            Sprite sprite = Resources.Load<Sprite>("WardSprites/�a�J�V�h");
            field.GetComponent<SpriteRenderer>().sprite = sprite;
            Node node = field.transform.GetChild(0).gameObject.GetComponent<Node>();
            node.setSprite("�a�J�V�h");
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