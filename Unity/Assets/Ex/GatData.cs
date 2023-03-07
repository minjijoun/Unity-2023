using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GatData : MonoBehaviour
{
    public Entity_GameDB_1 entity_GameDB_1;

    // Start is called before the first frame update
    void Start()
    {
        foreach (Entity_GameDB_1.Param param in entity_GameDB_1.sheets[0].list)
        {
            Debug.Log(param.index + " - " + param.character + " - " + param.Hp + " - " + param.Mp);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
