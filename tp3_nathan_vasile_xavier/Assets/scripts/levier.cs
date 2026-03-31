using UnityEngine;
public class ActionLevier : MonoBehaviour
{
    public HingeJoint hinge;
    private bool dejaActive = false;
    void Update()
    {
        // Si l'angle du levier dépasse 50 degrés
        if (!dejaActive && hinge.angle > 50f)
        {
            dejaActive = true;
            Debug.Log("LEVIER ACTIVÉ !");
            // Ajoute ici ce que tu veux déclencher
        }
        else if (dejaActive && hinge.angle < 10f)
        {
            dejaActive = false;
            Debug.Log("LEVIER RÉINITIALISÉ");
        }
    }
}