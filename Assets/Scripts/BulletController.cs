using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    public List<string> HitableTags;
    public List<string> DestroyOnHitTags;
    public List<string> PenatrableTags;
    public Rigidbody2D rigidbody;
    public GameObject BulletHit;
    public float speed;
    public int hitPoints;

    private GameManager gameManager;

    private void Awake()
    {
        gameManager = GameObject.FindObjectOfType<GameManager>();
        HitableTags = HitableTags.ConvertAll(s => s.ToUpper());
        PenatrableTags = PenatrableTags.ConvertAll(s => s.ToUpper());
        DestroyOnHitTags = DestroyOnHitTags.ConvertAll(s => s.ToUpper());
    }

    void Start()
    {        
        rigidbody = GetComponent<Rigidbody2D>();
        Physics2D.IgnoreLayerCollision(9,10);
        Shoot();
    }

    void Shoot()
    {
        rigidbody.AddForce(Vector2.up * speed);
        rigidbody.velocity = transform.up * speed;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        var tag = other.tag.ToUpper();
        Instantiate(BulletHit, transform.position, transform.rotation);
        var otherCharacter = other.gameObject.GetComponent<Character>();
        if (otherCharacter != null)
        {
            otherCharacter.UpdateHealthValue(-hitPoints);
            Destroy(gameObject);
        }
        if (PenatrableTags.Contains(tag))
        {
            return;
        }
        else if(!tag.Equals("UNTAGGED"))
        {
            Destroy(gameObject);
        }
    }
}
