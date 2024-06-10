using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

public class LobbbyManager : MonoBehaviourPunCallbacks
{
    // ���� ���� �񱳿�
    private string gameVersion = "1.0";     
    // ��Ʈ��ũ ���� ���� ������ ǥ���� �ؽ�Ʈ
    public Text connText;
    // �� ���� ��ư
    public Button joinButton;              

    // Start is called before the first frame update
    void Start()
    {
        //���ӿ� �ʿ��� ����(���ӹ���) ����
        PhotonNetwork.GameVersion = gameVersion;
        //������ ������ ���濡�� �����ϴ� ������ ���� ���� �õ�
        PhotonNetwork.ConnectUsingSettings();

        //���� �����ͼ����� ������ �Ϸ�� ���°� �ƴϱ⋚���� �� ���� ��ư�� ��� ��Ȱ��ȭ
        joinButton.interactable = false;
        //������ ���� ���� �õ� ������ �ؽ�Ʈ�� ǥ��
        connText.text = "������ ������ ���� ��...";
    }
    //ConnectUsingSettings()�Լ��� ȣ������ �� ������ ���� ���� ������ �ڵ����� ����Ǵ� �Լ�
    public override void OnConnectedToMaster()
    {
        //�� ���� ��ư Ȱ��ȭ
        joinButton.interactable = true;
        //���� ���� ǥ��
        connText.text = "�¶��� : ������ ������ �����";
    }

    //������ ���� ���� ���� �� �ڵ� ����Ǵ� �Լ�
    //������ ������ ������ �Ǿ��ִٰ� ������ ��������� ��������
    public override void OnDisconnected(DisconnectCause cause)
    {
        //�� ���� ��ư ��Ȱ��ȭ
        joinButton.interactable = false;
        //���� ���� ǥ��
        connText.text = "�������� :  ������ ������ ������� ����\n���� ��õ� ��... ";

        //������ �������� ������ �õ�
        PhotonNetwork.ConnectUsingSettings();    
    }

    public void Connect()
    {
        joinButton.interactable = false;

        if(PhotonNetwork.IsConnected)
        {
            connText.text = "�뿡 ���� ...";
            PhotonNetwork.JoinRandomRoom();
        }
        else
        {
            //������ ������ ���� ���� �ƴ϶�� ������ ������ ���� �õ�
            connText.text = "�������� :  ������ ������ ������� ����\n���� ��õ� ��... ";
            //������ �������� ������ �õ�
            PhotonNetwork.ConnectUsingSettings();    
        }
    }
    //�� ���� ������
    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        //���� ���� ǥ��
        connText.text = "�� ���� ����,���ο� �� ����...";
        //�ִ� 4���� ���� ������ �� �� ����
        PhotonNetwork.CreateRoom(null, new RoomOptions() { MaxPlayers = 4 });    
    }
    //�� ���� ������
    public override void OnJoinedRoom()
    {
        //���� ���� ǥ��
        connText.text = "�� ���� ����";
        //��� �� �����ڰ� Main ���� �ε��ϰ���
        PhotonNetwork.LoadLevel("ZombieMain");
    }
}
