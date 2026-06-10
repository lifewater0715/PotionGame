using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Audio;

public class Setting : MonoBehaviour
{
    [SerializeField] private GameObject imageObject;

    public void OnClickSettingSound()
    {
        
    }
    public void OnClickSettingOn()
    {
        Debug.Log("설정키기");
        imageObject.SetActive(!imageObject.activeSelf);
        Time.timeScale = 0f;
    }
    public void OnClickSettingOff()
    {
        Debug.Log("설정끄기");
        imageObject.SetActive(!imageObject.activeSelf);
        Time.timeScale = 1f;
    }

    public void OnClickQuit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else 

#endif
    }
}
