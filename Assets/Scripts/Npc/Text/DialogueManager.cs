using UnityEngine;
using System.Collections.Generic;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager Instance { get; private set; }

    public List<string> sentences = new List<string>()
    {
        "Hello!",
        "Nice to meet you.",
        "How can I help you?"
    };

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    public string GetRandomSentence()
    {
        if (sentences.Count == 0)
            return "...";
        return sentences[Random.Range(0, sentences.Count)];
    }
}