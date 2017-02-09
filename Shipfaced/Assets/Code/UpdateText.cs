using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpdateText : MonoBehaviour
{
    void Update()
    {
        GetComponent<Gradient>().UpdatePlz();
    }

}
