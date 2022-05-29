using UnityEngine;

public class BlasterShot : MonoBehaviour
{
    public float Speed;
    public int ShotPower;
    public bool Crit;
    public bool Stagger;
    public bool Ricochet;
    public bool Amplify;
    public float Incendiary;
    public float ArmorShred;
    
    public void Launch(Vector3 direction, float speed, int shotPower, bool crit, bool stagger, bool ricochet, bool amplify, float incendiary, float armorShred)
    {
        Speed = speed;
        direction.Normalize();
        transform.up = direction;
        ShotPower = shotPower;
        Crit = crit;
        Stagger = stagger;
        Ricochet = ricochet;
        Amplify = amplify;
        Incendiary = incendiary;
        ArmorShred = armorShred;
        GetComponent<Rigidbody>().velocity = direction * speed;
    }

    void OnCollisionEnter(Collision collision)
    {
        var zombie = collision.collider.GetComponent<Zombie>();
        if (!Ricochet || zombie != null)
        {
            Destroy(gameObject);
        }
        else
        {
            GetComponent<Rigidbody>().velocity = Vector3.Reflect(transform.up, collision.contacts[0].normal) * Speed;
            Ricochet = false;
        }
    }

    void Start()
    {
        Destroy(gameObject, 5f);
    }
}
