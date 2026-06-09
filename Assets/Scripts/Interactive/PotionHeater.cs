using UnityEngine;

public class PotionHeater : MonoBehaviour
{
    [SerializeField] private GameObject heaterLight;

    void Start()
    {

    }

    void Update()
    {

    }

    private void OnTriggerStay2D(Collider2D other)
    {

        if (other.tag == "Potion")
        {
            heaterLight.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        heaterLight.SetActive(false);
    }
}
