using UnityEngine;
using UnityEngine.UI;
using Photon.Realtime;

public class Roomview  : MonoBehaviour
{
    public Text buttonText;
    private RoomInfo info;


    public void ResisterRoomDetails(RoomInfo info)
    {
        
        this.info = info;

        buttonText.text = this.info.Name;
    }


    public void OpenRoom()
    {
        PhotonManager.instance.JoinRoom(info);
    }

}
