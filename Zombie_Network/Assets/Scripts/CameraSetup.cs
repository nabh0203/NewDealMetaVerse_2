using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Cinemachine;
using UnityEngine;

public class CameraSetup : MonoBehaviourPun
{
    // Start is called before the first frame update
    void Start()
    {
        //���� �ڽ��� ���� �÷��̾���
        if (photonView.IsMine)
        {
            //���� �ִ� �ó׸ӽ� ���� ī�޶� ã��
            CinemachineVirtualCamera followCam = FindAnyObjectByType<CinemachineVirtualCamera>();
            //���� ī�޶��� ���� ����� �ڽ��� Ʈ���������� ����
            followCam.Follow = transform;
            followCam.LookAt = transform;
        }   
    }
}
