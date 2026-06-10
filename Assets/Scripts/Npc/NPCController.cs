using UnityEngine;

public class NPCController : MonoBehaviour
{
    public NPCFader npcFader;
    public TextPopup textPopup;

    public void ShowNPC()
    {
        npcFader.Show();
        textPopup.Show();
    }

    public void HideNPC()
    {
        npcFader.Hide();
        textPopup.Hide();
    }
}