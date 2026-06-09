using UnityEngine;
using UnityEngine.Rendering.Universal;

public class RandomLight : MonoBehaviour
{
    [SerializeField] private GameObject lightTarget;
    private Light2D lights;
    [SerializeField] private float lightPower;
    private float Orginlight;

    void Start()
    {
        if (lightTarget.GetComponent<Light2D>() != null)
        {
            lights = lightTarget.GetComponent<Light2D>();
            Orginlight = lights.intensity;
        }
    }

    void FixedUpdate()
    {
        lights.intensity = Orginlight + UnityEngine.Random.Range(-lightPower, lightPower);
    }
}
