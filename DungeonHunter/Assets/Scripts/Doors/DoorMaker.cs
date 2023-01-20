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

    [Header ("Options:")]
    [SerializeField]
    private bool hideUp;
    [SerializeField]
    private bool hideDown;
    [SerializeField]
    private bool hideLeft;
    [SerializeField]
    private bool hideRight;

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

                    // Hide Door Sprite

                    if (hideUp) newDoor.GetComponent<SpriteRenderer> ().enabled = false;
                }
                if (doorInfo.down != "")
                {
                    doorPos = new Vector2 (0, sceneBounds.BtmLeft().y - (1f / 32f * 12f));
                    GameObject newDoor = Instantiate(doorPrefab, doorPos, Quaternion.identity, transform);
                    newDoor.transform.Rotate(new Vector3(0, 0, 180));
                    newDoor.name = doorInfo.down;

                    // Hide Door Sprite

                    if (hideDown) newDoor.GetComponent<SpriteRenderer> ().enabled = false;
                }
                if (doorInfo.left != "")
                {
                    doorPos = new Vector2 (sceneBounds.TopLeft().x, 0);
                    GameObject newDoor = Instantiate(doorPrefab, doorPos, Quaternion.identity, transform);
                    newDoor.transform.Rotate(new Vector3(0, 0, 90));
                    newDoor.name = doorInfo.left;

                    // Hide Door Sprite

                    if (hideLeft) newDoor.GetComponent<SpriteRenderer> ().enabled = false;
                }
                if (doorInfo.right != "")
                {
                    doorPos = new Vector2 (sceneBounds.TopRight().x, 0);
                    GameObject newDoor = Instantiate(doorPrefab, doorPos, Quaternion.identity, transform);
                    newDoor.transform.Rotate(new Vector3(0, 0, -90));
                    newDoor.name = doorInfo.right;

                    // Hide Door Sprite

                    if (hideRight) newDoor.GetComponent<SpriteRenderer> ().enabled = false;
                }
            }
		}
    }
}