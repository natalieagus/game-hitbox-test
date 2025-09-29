using UnityEngine;

public class Parallax: MonoBehaviour
{
    public float length, startpos;
    public GameObject cam;
    public float parallaxEffect;
    
    public void Start()
    {
        startpos = transform.position.x;
        length = GetComponent<SpriteRenderer>().bounds.size.x;
    }

    public void Update()
    {
        float temp = (cam.transform.position.x * (1 - parallaxEffect));
        float dist = (cam.transform.position.x * parallaxEffect);

        transform.position = new Vector3(startpos + dist, transform.position.y, transform.position.z);

        if (temp > startpos + length * 1.5) startpos += length * 3;
        else if(temp < startpos - length * 1.5) startpos -= length * 3;
    }
}
