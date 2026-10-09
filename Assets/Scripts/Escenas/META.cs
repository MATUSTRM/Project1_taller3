using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class META : MonoBehaviour
{
    public GameObject Player;
    public GameObject Menu_Completed;
    // Start is called before the first frame update

    public void Avanzar_player()
    {
        Player.SetActive(false);
    }
    public void Active_Menu_Completed()
    {
        Menu_Completed.SetActive(true);
    }
}
