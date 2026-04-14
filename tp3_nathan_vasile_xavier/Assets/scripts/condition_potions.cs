using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class condition_potions : MonoBehaviour
{
    private int[] validation = { 1, 2, 1, 1, 2 };
    private int bouteillesCount;
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "red") {
            if (validation[bouteillesCount] == 1) { bouteillesCount++; }
            else { bouteillesCount = 0; }
        }  

        else if (other.tag == "green")
        {
            if (validation[bouteillesCount] == 2) { bouteillesCount++; }
            else { bouteillesCount = 0; }
        }
    }
}
