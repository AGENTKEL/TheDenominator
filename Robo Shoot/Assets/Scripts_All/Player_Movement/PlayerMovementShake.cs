using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementShake : MonoBehaviour
{
    public Transform recoilFollowPos;
    [SerializeField] float kickBackAmount = -1;
    [SerializeField] float kickBackSpeed = 10, returnSpeed = 20;
    float currentRecoilPosition, finalRecoilPosition;


    // Update is called once per frame
    void Update()
    {
        currentRecoilPosition = Mathf.Lerp(currentRecoilPosition, 0, returnSpeed * Time.deltaTime);
        finalRecoilPosition = Mathf.Lerp(finalRecoilPosition, currentRecoilPosition, kickBackSpeed * Time.deltaTime);
        recoilFollowPos.localPosition = new Vector3(0, finalRecoilPosition, 0);
    }
    public void TriggerMovementShake()
    {
        currentRecoilPosition += kickBackAmount;
    }

}
