using System.Collections;
using System.Collections.Generic;
using MoreMountains.Feedbacks;
using UnityEngine;

public class BatWall : MonoBehaviour
{
    [SerializeField] private MMFeedbacks hitFeedback;
    [SerializeField] private float pushForce;

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "Player")
        {
            hitFeedback.PlayFeedbacks();
            Debug.Log(other.gameObject.GetComponent<Rigidbody>());
            //other.gameObject.transform.Translate(other.gameObject.transform.forward * 2f);
        }
    }
}
