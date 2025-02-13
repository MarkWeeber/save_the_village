using UnityEngine;
using UnityEngine.UI;

public class ToggleButton : MonoBehaviour
{
    // inputs
    [SerializeField]
    private string originalText;
    [SerializeField]
    private string busyText = string.Empty;
    [SerializeField] 
    private string insufficientFoodText = string.Empty;
    [SerializeField] 
    private Text buttonText = null;
    [SerializeField] 
    private Button button = null;
    [SerializeField] 
    private Cycle cycle = null;
    [SerializeField] 
    private Image disabledTint = null;
    [SerializeField] 
    private GameManager gameManager = null;
    [SerializeField] 
    private HireActionType hireActionType = HireActionType.DEFAULT;
    
    private int period;
    private HireActionStatus hireStatus = HireActionStatus.READY;
    public HireActionStatus HireStatus
    {
        get { return hireStatus; }
        set
        {
            switch (value)
            {
                case HireActionStatus.READY:
                    button.enabled = true;
                    buttonText.text = originalText;
                    disabledTint.color = new Color(disabledTint.color.r, disabledTint.color.g, disabledTint.color.b, 0f);
                    break;
                case HireActionStatus.NOT_ENOUGH_RESOURCES:
                    button.enabled = false;
                    buttonText.text = insufficientFoodText;
                    disabledTint.color = new Color(disabledTint.color.r, disabledTint.color.g, disabledTint.color.b, 0.6f);
                    break;
                case HireActionStatus.WORKING:
                    button.enabled = false;
                    buttonText.text = busyText;
                    disabledTint.color = new Color(disabledTint.color.r, disabledTint.color.g, disabledTint.color.b, 0.6f);
                    cycle.Enable(period);
                    gameManager.HireButtonPressed(hireActionType);
                    break;
                default:
                    break;
            }
            //gameManager.HireButtonPressed(hireActionType);
            hireStatus = value;
        }
    }

    void Start()
    {
        originalText = buttonText.text;
    }

    public void SetCycleSeconds(int seconds)
    {
        period = seconds;
    }

    public void ResetButton()
    {
        if (HireStatus == HireActionStatus.WORKING)
        {
            HireStatus = HireActionStatus.READY;
        }
    }

    public void Toggle()
    {
        if (HireStatus == HireActionStatus.READY)
        {
            HireStatus = HireActionStatus.WORKING;
        }
    }

    public void SetDisabledDueToInsufficientResources()
    {
        if (HireStatus == HireActionStatus.READY)
        {
            HireStatus = HireActionStatus.NOT_ENOUGH_RESOURCES;
        }
    }

    public void ClearDisabled()
    {
        if (HireStatus == HireActionStatus.NOT_ENOUGH_RESOURCES)
        {
            if (cycle.Active)
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
