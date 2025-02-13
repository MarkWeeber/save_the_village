using UnityEngine.UI;
using UnityEngine;

// ENUMS
public enum HireActionStatus : ushort
{
    READY = 0,
    NOT_ENOUGH_RESOURCES = 1,
    WORKING = 2
}

public enum HireActionType : ushort
{
    DEFAULT = 0,
    HIRE_WORKER = 1,
    HIRE_WARRIOR = 2
}
public enum CycleEvent : ushort
{
    DEFAULT = 0,
    FOOD_PRODUCED = 1,
    FOOD_CONSUMED = 2,
    WAVE_SPAWNED = 3,
    WORKER_HIRED = 4,
    WARRIOR_HIRED = 5
}


public class CycleButton : MonoBehaviour
{
    // inputs Cycle related
    [SerializeField]
    private Text cycleSecondsText = null;
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

    public string NextValue
    {
        get { if (nextValue != null) return nextValue.text; else return null; }
        set { if (nextValue != null) nextValue.text = value; }
    }

    // inputs button related
    [SerializeField]
    private string buttonOriginalText = string.Empty;
    [SerializeField]
    private string buttonBusyText = string.Empty;
    [SerializeField]
    private string buttonInsufficientFoodText = string.Empty;
    [SerializeField]
    private Text buttonText = null;
    [SerializeField]
    private Button button = null;
    [SerializeField]
    private Image buttonDisabledTint = null;
    [SerializeField]
    private HireActionType hireActionType = HireActionType.DEFAULT;

    // private varialbes
    private int period;
    private HireActionStatus hireStatus = HireActionStatus.READY;
    private bool cycleIsActive = false;
    private float cyclePeriod;
    private float cycleCounter;

    // properties
    public HireActionStatus HireStatus
    {
        get { return hireStatus; }
        set
        {
            switch (value)
            {
                case HireActionStatus.READY:
                    button.enabled = true;
                    buttonText.text = buttonOriginalText;
                    buttonDisabledTint.color = new Color(buttonDisabledTint.color.r, buttonDisabledTint.color.g, buttonDisabledTint.color.b, 0f);
                    break;
                case HireActionStatus.NOT_ENOUGH_RESOURCES:
                    button.enabled = false;
                    buttonText.text = buttonInsufficientFoodText;
                    buttonDisabledTint.color = new Color(buttonDisabledTint.color.r, buttonDisabledTint.color.g, buttonDisabledTint.color.b, 0.6f);
                    break;
                case HireActionStatus.WORKING:
                    button.enabled = false;
                    buttonText.text = buttonBusyText;
                    buttonDisabledTint.color = new Color(buttonDisabledTint.color.r, buttonDisabledTint.color.g, buttonDisabledTint.color.b, 0.6f);
                    this.EnableCycle(period);
                    gameManager.HireButtonPressed(hireActionType);
                    break;
                default:
                    break;
            }
            hireStatus = value;
        }
    }

    // Start
    void Start()
    {
        if(button != null)
            buttonOriginalText = buttonText.text;
    }

    // Update
    void Update()
    {
        if (cycleIsActive)
        {
            cycleSecondsText.text = Mathf.Round(cycleCounter).ToString();
            cycleCounter -= Time.deltaTime;
            cycleImage.fillAmount = cycleCounter / cyclePeriod;
            if (cycleCounter <= 0)
            {
                cycleCounter = cyclePeriod;
                if (gameManager != null)
                {
                    gameManager.CallPeriod(cycleEvent);
                }
                if (!infiniteCycle)
                {
                    cycleIsActive = false;
                    cycleImage.fillAmount = 1;
                }
            }
        }
    }

    // Cycle functions
    public void SetCycleSeconds(int seconds)
    {
        period = seconds;
    }

    public void EnableCycle(int seconds)
    {
        cycleIsActive = true;
        cyclePeriod = (float)seconds;
        cycleCounter = cyclePeriod;
    }

    public void DisableCycle()
    {
        cycleIsActive = false;
        cycleImage.fillAmount = 1;
        cycleSecondsText.text = "0";
    }

    // Button functions
    public void ToggleButton()
    {
        if (HireStatus == HireActionStatus.READY)
        {
            HireStatus = HireActionStatus.WORKING;
        }
    }

    public void ResetButton()
    {
        if (HireStatus == HireActionStatus.WORKING)
        {
            HireStatus = HireActionStatus.READY;
        }
    }

    public void SetDisabledButtonDueToInsufficientResources()
    {
        if (HireStatus == HireActionStatus.READY)
        {
            HireStatus = HireActionStatus.NOT_ENOUGH_RESOURCES;
        }
    }

    public void ClearButtonDisabled()
    {
        if (HireStatus == HireActionStatus.NOT_ENOUGH_RESOURCES)
        {
            if (cycleIsActive)
            {
                HireStatus = HireActionStatus.WORKING;
            }
            else
            {
                HireStatus = HireActionStatus.READY;
            }
        }
    }


}
