using UnityEngine;
using UnityEngine.UI;

public class TopScreenText : MonoBehaviour
{
    public Text text;
    public static Text _text;

    private void Awake()
    {
        _text = text;
    }

    public static void SetText(string content)
    {
        _text.text = content;
    }
}
