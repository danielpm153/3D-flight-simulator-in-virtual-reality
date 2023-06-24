using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveController : MonoBehaviour
{
    public List<GameObject> Cubos;

    public void EnableCube(string name)
    {
        for(int i = 0; i < Cubos.Count; i++)
        {
            if(Cubos[i].name != name)
            {
                Cubos[i].GetComponent<MoveCube>().UnsetMove();
            }
            else
            {
                Cubos[i].GetComponent<MoveCube>().SetMove();
            }
        }
    }
}
