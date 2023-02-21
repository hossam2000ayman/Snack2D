using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventHandle : MonoBehaviour
{
    public GameObject Play_Panel;
    
    public void Play_Click()
    {
        GameObject.Find("SnakeHead").GetComponent<MoveSnake>().enabled = true;
        Play_Panel.SetActive(false);
    }
}
