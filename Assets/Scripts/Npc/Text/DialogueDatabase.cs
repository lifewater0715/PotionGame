using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DialogueDatabase", menuName = "Scriptable Objects/DialogueDatabase")]
public class DialogueDatabase : ScriptableObject
{
    [SerializeField] private List<string> sentences = new List<string>();

    public IReadOnlyList<string> Sentences => sentences;

    public bool HasSentences()
    {
        return sentences != null && sentences.Count > 0;
    }

    public string GetRandomSentence()
    {
        if (!HasSentences())
        {
            return "...";
        }

        return sentences[Random.Range(0, sentences.Count)];
    }
}
