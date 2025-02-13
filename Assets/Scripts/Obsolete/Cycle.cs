using UnityEngine;
using UnityEngine.UI;


public class Cycle : MonoBehaviour
{
    // inputs
    [SerializeField]
    private Text secondsText = null;
    [SerializeField]
    private Image cycleImage = null;
    [SerializeField] 
    private GameManager gameManager = null;
    [SerializeField] 
    private CycleEvent cycleEvent = CycleEvent.DEFAULT;
    [SerializeField]
    private Text nextValue = null;
    [SerializeField]
    private bool infiniteCycle = false;

    private bool active = false;
    private float period;
    private float counter;

    public bool Active
    {
        get { return active; }
    }
    
    public string NextValue
    {
        get { if (nextValue != null) return nextValue.text; else return null; }
        set { if (nextValue != null) nextValue.text = value; }
    }

    void Update()
    {
        if (active)
        {
            // update the seconds
            secondsText.text = Mathf.Round(counter).ToString();
            // decrease timer
            counter -= Time.deltaTime;
            // update fillAmount
            cycleImage.fillAmount = counter / period;
            // check if reached 0
            if (counter <= 0)
            {
                counter = period;
                if (gameManager != null)
                {
                    gameManager.CallPeriod(cycleEvent);
                }
                if (!infiniteCycle)
                {
                    active = false;
                    cycleImage.fillAmount = 1;
                }
            }
        }
    }

    public void Enable(int seconds)
    {
        active = true;
        period = (float)seconds;
        counter = period;
    }

    public void Disable()
    {
        active = false;
        cycleImage.fillAmount = 1;
        secondsText.text = "0";
    }
}
