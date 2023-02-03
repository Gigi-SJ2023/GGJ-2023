using UnityEngine;

public class SpriteContainer : MonoBehaviour
{

    // Update is called once per frame
    void Update()
    {
        transform.rotation = Camera.main.transform.rotation;
    }
}
