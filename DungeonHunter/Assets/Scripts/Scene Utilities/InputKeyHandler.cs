using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputKeyHandler : MonoBehaviour
{
    private static InputKeyHandler _instance;
    public static InputKeyHandler IKH_Instance { get { return _instance; } }

    private void Awake ()
    {
        // Singleton

        if (_instance != null & _instance != this) Destroy (gameObject);
        else _instance = this;

        DontDestroyOnLoad (this);
    }

    // Up

    public bool Up_Active ()
    {
        return Input.GetKey ("w") || Input.GetKey (KeyCode.UpArrow) || Input.GetAxisRaw ("Vertical") > 0f;
    }

    public bool Up_Start ()
    {
        return Input.GetKeyDown ("w") || Input.GetKeyDown (KeyCode.UpArrow) || Input.GetAxisRaw ("Vertical") > 0f;
    }

    // Down

    public bool Down_Active ()
    {
        return Input.GetKey ("s") || Input.GetKey (KeyCode.DownArrow) || Input.GetAxisRaw ("Vertical") < 0f;
    }
    
    public bool Down_Start ()
    {
        return Input.GetKeyDown ("s") || Input.GetKeyDown (KeyCode.DownArrow) || Input.GetAxisRaw ("Vertical") < 0f;
    }

    // Left

    public bool Left_Active ()
    {
        return Input.GetKey ("a") || Input.GetKey (KeyCode.LeftArrow) || Input.GetAxisRaw ("Horizontal") < 0f;
    }
    
    public bool Left_Start ()
    {
        return Input.GetKeyDown ("a") || Input.GetKeyDown (KeyCode.LeftArrow) || Input.GetAxisRaw ("Horizontal") < 0f;
    }

    // Right

    public bool Right_Active ()
    {
        return Input.GetKey ("d") || Input.GetKey (KeyCode.RightArrow) || Input.GetAxisRaw ("Horizontal") > 0f;
    }
    
    public bool Right_Start ()
    {
        return Input.GetKeyDown ("d") || Input.GetKeyDown (KeyCode.RightArrow) || Input.GetAxisRaw ("Horizontal") > 0f;
    }

    // Interact

    public bool Interact_Start ()
	{
        return Input.GetButtonDown ("Interact");
	}

    public bool Interact_Active ()
	{
        return Input.GetButton ("Interact");
	}
}
