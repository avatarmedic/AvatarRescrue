using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Hologram
{
    public GameObject gameObject;
    public bool wasActiveLastFrame = false;
}
public class HologramController : MonoBehaviour
{
    public Hologram[] holograms;
    int currentFocus = 0;

    void FixedUpdate()
    {
        for(int i = 0; i < holograms.Length; i++)
        {
            bool isActiveThisFrame = holograms[i].gameObject.activeInHierarchy;
            if(isActiveThisFrame && !holograms[i].wasActiveLastFrame)
            {
                currentFocus = i;
                StartCoroutine(KillAllHolograms());
            }
            holograms[i].wasActiveLastFrame = holograms[i].gameObject.activeInHierarchy;
        }
    }
    IEnumerator KillAllHolograms()
    {
        yield return new WaitForSeconds(44);
        for (int i = 0; i < holograms.Length; i++)
        {
            if( i != currentFocus)
                holograms[i].gameObject.SetActive(false);
        }
    }
}
