using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorMaker : MonoBehaviour
{
    [Header("Components:")]

    [SerializeField]
    private DoorInfo_SO doorInfoSO;
    [SerializeReference]
    private GameObject doorPrefab;

    [Header ("Scripts:")]

    [SerializeReference]
    private SceneBounds sceneBounds;

    public void PlaceDoors (string room)
    {
        foreach (DoorInfo doorInfo in doorInfoSO.doorInfos)
		{
            if (doorInfo.roomName == room)
			{
                Vector2 doorPos = new Vector2();

                if (doorInfo.up != "")
                {
                    doorPos = new Vector2 (0, sceneBounds.TopLeft().y);
                    GameObject newDoor = Instantiate(doorPrefab, doorPos, Quaternion.identity, transform);
                    newDoor.name = doorInfo.up;
                }
                if (doorInfo.down != "")
                {
                    doorPos = new Vector2 (0, sceneBounds.BtmLeft().y);
                    GameObject newDoor = Instantiate(doorPrefab, doorPos, Quaternion.identity, transform);
                    newDoor.name = doorInfo.down;
                }
                if (doorInfo.left != "")
                {
                    doorPos = new Vector2 (sceneBounds.TopLeft().x, 0);
                    GameObject newDoor = Instantiate(doorPrefab, doorPos, Quaternion.identity, transform);
                    newDoor.transform.Rotate(new Vector3(0, 0, 90));
                    newDoor.name = doorInfo.left;
                }
                if (doorInfo.right != "")
                {
                    doorPos = new Vector2 (sceneBounds.TopRight().x, 0);
                    GameObject newDoor = Instantiate(doorPrefab, doorPos, Quaternion.identity, transform);
                    newDoor.transform.Rotate(new Vector3(0, 0, -90));
                    newDoor.name = doorInfo.right;
                }
            }
		}
    }
}