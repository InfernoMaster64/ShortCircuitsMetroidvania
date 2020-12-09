using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlameBall : MonoBehaviour
{
    public float speed = 2;
    public int damage = 5;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(KillSelf());
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(new Vector2(0, -speed * Time.deltaTime));
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            collision.gameObject.GetComponent<PlayerScript>().TakeDamage(damage);
            Destroy(this.gameObject);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            collision.gameObject.GetComponent<PlayerScript>().TakeDamage(damage);
            Destroy(this.gameObject);
        }
    }

    IEnumerator KillSelf()
    {
        yield return new WaitForSeconds(5);
        Destroy(this.gameObject);
    }
}
