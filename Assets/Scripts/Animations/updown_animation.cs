using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class updown_animation : MonoBehaviour
{
    public float minHeight;
    public float maxHeight;   
    public float minScale;
    public float maxScale; 
    public float speed;
    static float t = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
        // animate the position of the game object...
        transform.position = new Vector3(transform.parent.position.x, Mathf.Lerp(minHeight, maxHeight, t), transform.parent.position.z);
        transform.localScale = new Vector3(Mathf.Lerp(maxScale,minScale, t), Mathf.Lerp(maxScale,minScale, t), Mathf.Lerp( maxScale,minScale, t));

        // .. and increase the t interpolater
        t += 0.5f * Time.deltaTime * speed;

        // now check if the interpolator has reached 1.0
        // and swap maximum and minimum so game object moves
        // in the opposite direction.
        if (t > 1.0f)
        {
            float temp = maxHeight;
            maxHeight = minHeight;
            minHeight = temp;

            float temp2 = maxScale;
            maxScale = minScale;
            minScale = temp2;
            t = 0.0f;
        }
    }
}
