using UnityEngine;

public class Heart : MonoBehaviour
{
    public PlayerHealth playerHealth; 
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    private void Awake()
    {
        //playerHealth = GetComponent<PlayerHealth>();
        gameObject.SetActive(true);
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
 
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("player"))
        {
          
            gameObject.SetActive(false);
        }
    }
}
