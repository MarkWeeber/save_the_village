using UnityEngine;
using UnityEngine.UI;

public class Resource : MonoBehaviour
{
    private int count;
    [SerializeField]
    private Text text = null;
    public int Count
    {
        get { return count; }
        set { count = value; RefreshText(); }
    }

    private void RefreshText()
    {
        text.text = count.ToString();
    }

    public void Enable(int startCount)
    {
        Count = startCount;
    }
}
