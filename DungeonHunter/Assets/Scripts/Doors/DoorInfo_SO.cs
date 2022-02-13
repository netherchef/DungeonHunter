using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct DoorInfo
{
    public string roomName;
    public string up, down, left, right;
}

[CreateAssetMenu (fileName = "New DoorInfo", menuName = "New DoorInfo")]
public class DoorInfo_SO : ScriptableObject
{
    public DoorInfo[] doorInfos;
}