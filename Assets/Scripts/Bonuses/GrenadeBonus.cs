using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrenadeBonus : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        GrenadeThrow gt = other.GetComponent<GrenadeThrow>();
        if (gt != null)
        {
           gt.AddGrenade();
           gameObject.SetActive(false);
        }
    }
}
