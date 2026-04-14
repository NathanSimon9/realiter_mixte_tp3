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
            if (validation[bouteillesCount] == 1) {
                bouteillesCount++;
               
                Debug.Log("RougeOk");
            } else { 
                bouteillesCount = 0;
                Debug.Log("RougePasOk");
            }
        }  

        else if (other.tag == "green")
        {
            if (validation[bouteillesCount] == 2) { bouteillesCount++;
                if (bouteillesCount == 5)
                {
                    Debug.Log("Reussite");

                }
                Debug.Log("VertOk");
            }
            else { bouteillesCount = 0;
                Debug.Log("VertPasOk");
            }
        }
    }
}
