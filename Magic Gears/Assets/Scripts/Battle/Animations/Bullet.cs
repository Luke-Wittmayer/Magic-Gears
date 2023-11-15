using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public int power;

    // Start is called before the first frame update
    void Start()
    {
        // Disable the bullet initially
        gameObject.SetActive(false);
    }

    // Method to initiate the bullet movement
    public void Activate()
    {
        gameObject.SetActive(true);

        Rigidbody rb = GetComponent<Rigidbody>();

        if (rb != null)
        {
            Debug.Log("WORKNIG");
            rb.AddForce(transform.forward * power, ForceMode.Impulse);

            rb.constraints = RigidbodyConstraints.None;
        }
        float timeScale = Time.timeScale;
        Debug.Log("Time Scale: " + timeScale);
    }
}
