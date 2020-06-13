using UnityEngine;


public class TBC : MonoBehaviour
{
    [SerializeField] GameObject[] exitObjects;
    [SerializeField] GameObject dialogue, Leftwall;
    public void ActivateExitObjects()
    {
        Destroy(Leftwall);
        for (int i = 0; i < exitObjects.Length; i++)
        {
            exitObjects[i].SetActive(true);

        }
        enabled = false;
    }

}
