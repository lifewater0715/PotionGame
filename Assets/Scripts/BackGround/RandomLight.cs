using UnityEngine;
using UnityEngine.Rendering.Universal;

public class RandomLight : MonoBehaviour
{
    [SerializeField] private GameObject lightTarget;
    private Light2D lights;

    void Start()
    {
        lights = lightTarget.GetComponent<Light2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
