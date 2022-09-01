using System.Collections;
using System.Collections.Generic;
using Constants;
using Helpers;
using UnityEngine;


public class StartPosition : MonoBehaviour
{
    // Start is called before the first frame update
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(Tags.Player))
        {
            EventManager.Trigger(new PlayerAtStartEventArgs());
        }
    }
}

public class PlayerAtStartEventArgs
{

}
