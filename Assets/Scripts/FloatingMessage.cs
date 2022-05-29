using TMPro;
using UnityEngine;

internal class FloatingMessage : MonoBehaviour
{
    [SerializeField] float _floatSpeed = 5f;
    Vector3 direction;

    public void SetValues(string message, Color32 color, int fontSize)
    {
        var text = GetComponent<TMP_Text>();
        text.SetText(message);
        text.color = color;
        text.fontSize = fontSize;
        Destroy(gameObject, 2f);
        System.Random r = new System.Random();
        direction = Quaternion.Euler(0, (float)r.NextDouble() * 60 - 30, 0) * transform.up;
    }
    private void Update()
    {
       transform.position += direction * Time.deltaTime * _floatSpeed;
    }
}