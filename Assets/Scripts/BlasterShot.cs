using UnityEngine;

public class BlasterShot : MonoBehaviour
{
    public float Speed;
    public int ShotPower;
    public bool Crit;
    public bool Stagger;
    public int Ricochet;
    public float Amplify;
    public float Incendiary;
    public float ArmorShred;
    public float CQC;
    public Transform Origination;
    
    public void Launch(Vector3 direction, float speed, int shotPower, bool crit, bool stagger, int ricochet, float amplify, float incendiary, float armorShred, float cqc, Transform origination)
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
        CQC = cqc;
        Origination = origination;
        GetComponent<Rigidbody>().velocity = direction * speed;
    }

    void OnCollisionEnter(Collision collision)
    {
        var zombie = collision.collider.GetComponent<Zombie>();
        if (Ricochet <= 0 || zombie != null)
        {
            Destroy(gameObject);
        }
        else
        {
            GetComponent<Rigidbody>().velocity = Vector3.Reflect(transform.up, collision.contacts[0].normal) * Speed;
            Ricochet = Ricochet - 1;
        }
    }

    void Start()
    {
        Destroy(gameObject, 5f);
    }
}
