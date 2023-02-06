using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinalDoorHandler : MonoBehaviour
{
    public void IncreaseLoop ()
	{
		DataPasser.DPInstance.Loop ();
	}
}