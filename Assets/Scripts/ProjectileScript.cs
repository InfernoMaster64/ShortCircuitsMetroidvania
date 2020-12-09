using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileScript : MonoBehaviour
{
    public float speed = .1f;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(KillSelf());
    }

    // Update is called once per frame
    void Update()
    {
        if(this.transform.rotation == Quaternion.Euler(0, 0, 0))
        {
            this.transform.Translate(new Vector3(speed * Time.deltaTime, 0, 0));
        }
        else if(this.transform.rotation == Quaternion.Euler(1, 0, 0))
        {
            this.transform.Translate(new Vector3(-speed*Time.deltaTime, 0, 0));
        }
        
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.tag == "Player")
        {
            collision.transform.gameObject.GetComponent<PlayerScript>().TakeDamage(10);
            Destroy(this.gameObject);
        }

        if (collision.transform.tag == "Ground")
        {
            Destroy(this.gameObject);
        }

        Destroy(this.gameObject);
    }



    IEnumerator KillSelf()
    {
        yield return new WaitForSeconds(3.5f);
        Destroy(this.gameObject);
    }
}
