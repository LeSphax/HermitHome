using UnityEngine;
using UnityEngine.UI;

public class TopScreenText : MonoBehaviour
{
    public Text text;
    public static Text _text;
    static int count = 0;

    private void Awake()
    {
        _text = text;
        count = 0;
    }

    public static void SetText(string content)
    {
        _text.text = (++count).ToString() + ", " + content;
    }
}
