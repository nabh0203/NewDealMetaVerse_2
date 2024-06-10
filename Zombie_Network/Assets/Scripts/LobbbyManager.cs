using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

public class LobbbyManager : MonoBehaviourPunCallbacks
{
    // 게임 버전 비교용
    private string gameVersion = "1.0";     
    // 네트워크 접속 상태 정보를 표시할 텍스트
    public Text connText;
    // 룸 접속 버튼
    public Button joinButton;              

    // Start is called before the first frame update
    void Start()
    {
        //접속에 필요한 정보(게임버전) 설정
        PhotonNetwork.GameVersion = gameVersion;
        //설정한 정보로 포톤에서 제공하는 마스터 서버 접속 시도
        PhotonNetwork.ConnectUsingSettings();

        //아직 마스터서버에 접속이 완료된 상태가 아니기떄문에 룸 접속 버튼을 잠시 비활성화
        joinButton.interactable = false;
        //마스터 서버 접속 시도 중임을 텍스트로 표기
        connText.text = "마스터 서버에 접속 중...";
    }
    //ConnectUsingSettings()함수를 호출했을 때 마스터 서버 접속 성공시 자동으로 실행되는 함수
    public override void OnConnectedToMaster()
    {
        //룸 접속 버튼 활성화
        joinButton.interactable = true;
        //접속 정보 표시
        connText.text = "온라인 : 마스터 서버와 연결됨";
    }

    //마스터 서버 접속 실패 시 자동 실행되는 함수
    //마스터 서버에 접속이 되어있다가 접속이 어떠한이유로 끊겼을때
    public override void OnDisconnected(DisconnectCause cause)
    {
        //룸 접속 버튼 비활성화
        joinButton.interactable = false;
        //접속 정보 표시
        connText.text = "오프라인 :  마스터 서버와 연결되지 않음\n접속 재시도 중... ";

        //마스터 서버로의 재접속 시도
        PhotonNetwork.ConnectUsingSettings();    
    }

    public void Connect()
    {
        joinButton.interactable = false;

        if(PhotonNetwork.IsConnected)
        {
            connText.text = "룸에 접속 ...";
            PhotonNetwork.JoinRandomRoom();
        }
        else
        {
            //마스터 서버에 접속 중이 아니라면 마스터 서버에 접속 시도
            connText.text = "오프라인 :  마스터 서버와 연결되지 않음\n접속 재시도 중... ";
            //마스터 서버로의 재접속 시도
            PhotonNetwork.ConnectUsingSettings();    
        }
    }
    //빈 방이 없을시
    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        //접속 상태 표시
        connText.text = "빈 방이 없음,새로운 방 생성...";
        //최대 4명을 수용 가능한 빈 방 생성
        PhotonNetwork.CreateRoom(null, new RoomOptions() { MaxPlayers = 4 });    
    }
    //빈 방이 있을시
    public override void OnJoinedRoom()
    {
        //접속 상태 표시
        connText.text = "방 참가 성공";
        //모든 룸 참가자가 Main 신을 로드하게함
        PhotonNetwork.LoadLevel("ZombieMain");
    }
}
