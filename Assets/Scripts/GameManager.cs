using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    // refer to all cycles
    [SerializeField]
    private CycleButton FoodProduction = null;
    [SerializeField]
    private CycleButton FoodConsumption = null;
    [SerializeField]
    private CycleButton WaveSpawn = null;
    [SerializeField]
    private CycleButton WorkerHiring = null;
    [SerializeField]
    private CycleButton WarriorHiring = null;

    // time periods
    [SerializeField]
    private int TimeToProduceFood = 8;
    [SerializeField]
    private int TimeToConsumeFood = 5;
    [SerializeField]
    private int TimeToWaveSpawn = 30;
    [SerializeField]
    private int WorkerHireTime = 10;
    [SerializeField]
    private int WarriorHireTime = 20;

    // gain numbers
    [SerializeField]
    private int WorkerHireCount = 1;
    [SerializeField]
    private int WarriorHireCount = 1;
    [SerializeField]
    private int FoodProducePerWorker = 2;
    [SerializeField]
    private int FoodConsumedPerWarrior = 1;

    // starting stock
    [SerializeField]
    private int FoodCountAtStart = 20;
    [SerializeField]
    private int WorkersCountAtStart = 3;
    [SerializeField]
    private int WarriorsCountAtStart = 5;

    // hire costs
    [SerializeField]
    private int WarriorHireCost = 3;
    [SerializeField]
    private int WorkerHireCost = 1;

    // hire cost labels
    [SerializeField]
    private Text WarriorHireCostText = null;
    [SerializeField]
    private Text WorkerHireCostText = null;

    // refer to all resources
    [SerializeField]
    private Resource FoodResource = null;
    [SerializeField]
    private Resource WorkersResource = null;
    [SerializeField]
    private Resource ArmyResource = null;

    // enemy count management
    [SerializeField]
    private int firstWaveCount = 3;
    [SerializeField]
    private int startWavesAfter = 2;
    [SerializeField]
    private int nextWaveIncrease = 3;

    // refer to game on hold canvas and its dashboard
    [SerializeField]
    private GameObject gameOnHold = null;
    [SerializeField]
    private GameObject dashBoard = null;
    [SerializeField]
    private Button startGameButton = null;
    [SerializeField]
    private Button resumeGameButton = null;
    [SerializeField]
    private Button restartGameButton = null;
    [SerializeField]
    private Text gameOnHoldMessage = null;
    [SerializeField]
    private Text gameOnHildSubMessage = null;
    [SerializeField]
    private Text wavesSurvivedText = null;
    [SerializeField]
    private Text foodProducedText = null;
    [SerializeField]
    private Text warriorsHiredText = null;
    [SerializeField]
    private Text workersHiredText = null;

    // game goal
    [SerializeField]
    private int warriorsCountGoal = 35;
    [SerializeField]
    private int workersCountGoal = 15;
    [SerializeField]
    private int foodCountGoal = 150;
    [SerializeField]
    private Text warriorsCountGoalText = null;
    [SerializeField]
    private Text workersCountGoalText = null;
    [SerializeField]
    private Text foodCountGoalText = null;

    // sounds
    [SerializeField]
    private AudioSource enemyIncomingAudio = null;
    [SerializeField]
    private AudioSource foodGatheredAudio = null;
    [SerializeField]
    private AudioSource foodComsumedAudio = null;
    [SerializeField]
    private AudioSource workerHiredAudio = null;
    [SerializeField]
    private AudioSource warriorHiredAudio = null;

    // priave variables
    private int foodProduced = 0;
    private int workersHired = 0;
    private int warriorsHired = 0;
    private int wavesSurvived = 0;
    private int currentWaveEnemyCount;
    private int currentFoodProduceAmount;
    private int currentFoodConsumeAmount;

    void Start()
    {
        gameOnHold.SetActive(true);
    }

    public void StartGame()
    {
        // make sure time scale is 1
        Time.timeScale = 1;

        // refresh stats
        foodProduced = 0;
        workersHired = 0;
        warriorsHired = 0;
        wavesSurvived = 0;

        // hide on hold menu
        gameOnHold.SetActive(false);

        // update price tag for hire costs
        WarriorHireCostText.text = WarriorHireCost.ToString();
        WorkerHireCostText.text = WorkerHireCost.ToString();

        // start the permanent cycles
        FoodProduction.EnableCycle(TimeToProduceFood);
        FoodConsumption.EnableCycle(TimeToConsumeFood);
        WaveSpawn.EnableCycle(TimeToWaveSpawn);

        // Set starting resources
        FoodResource.Enable(FoodCountAtStart);
        WorkersResource.Enable(WorkersCountAtStart);
        ArmyResource.Enable(WarriorsCountAtStart);

        // reset hire buttons
        WorkerHiring.HireStatus = HireActionStatus.READY;
        WarriorHiring.HireStatus = HireActionStatus.READY;

        // set the periods for hire buttons
        WorkerHiring.SetCycleSeconds(WorkerHireTime);
        WarriorHiring.SetCycleSeconds(WarriorHireTime);

        // set starting enemy count
        currentWaveEnemyCount = firstWaveCount;

        // set goal texts
        warriorsCountGoalText.text = warriorsCountGoal.ToString();
        workersCountGoalText.text = workersCountGoal.ToString();
        foodCountGoalText.text = foodCountGoal.ToString();

        // set next values
        RefreshFoodConsumptionValues();

        // refresh next values
        RefreshSplinters();
    }

    public void CallPeriod(CycleEvent cycleEvent)
    {
        RefreshFoodConsumptionValues();
        switch (cycleEvent)
        {
            case CycleEvent.FOOD_PRODUCED:
                FoodResource.Count += currentFoodProduceAmount;
                foodProduced += currentFoodProduceAmount;
                if (foodGatheredAudio != null)
                    foodGatheredAudio.Play();
                break;
            case CycleEvent.FOOD_CONSUMED:
                FoodResource.Count -= currentFoodConsumeAmount;
                if (foodComsumedAudio != null)
                    foodComsumedAudio.Play();
                break;
            case CycleEvent.WAVE_SPAWNED:
                if (startWavesAfter <= 0)
                {
                    ArmyResource.Count -= currentWaveEnemyCount;
                    currentWaveEnemyCount += nextWaveIncrease;
                    if (enemyIncomingAudio != null)
                        enemyIncomingAudio.Play();
                }
                else
                {
                    startWavesAfter--;
                }
                break;
            case CycleEvent.WORKER_HIRED:
                WorkersResource.Count += WorkerHireCount;
                WorkerHiring.ResetButton();
                workersHired++;
                if (workerHiredAudio != null)
                    workerHiredAudio.Play();
                break;
            case CycleEvent.WARRIOR_HIRED:
                ArmyResource.Count += WarriorHireCount;
                WarriorHiring.ResetButton();
                warriorsHired++;
                if (warriorHiredAudio != null)
                    warriorHiredAudio.Play();
                break;
            default: break;
        }
        bool result = CheckGameStatus();
        if (cycleEvent == CycleEvent.WAVE_SPAWNED && result && startWavesAfter <= 0)
            wavesSurvived++;
        RefreshFoodConsumptionValues();
        RefreshSplinters();
        CheckForInsufficientResources();
    }

    void Update()
    {
        //CheckForInsufficientResources();
    }

    private void RefreshFoodConsumptionValues()
    {
        // current values
        currentFoodConsumeAmount = ArmyResource.Count * FoodConsumedPerWarrior;
        currentFoodProduceAmount = WorkersResource.Count * FoodProducePerWorker;
    }

    private void RefreshSplinters()
    {
        // update next value splinters
        WaveSpawn.NextValue = currentWaveEnemyCount.ToString();
        FoodProduction.NextValue = currentFoodProduceAmount.ToString();
        FoodConsumption.NextValue = currentFoodConsumeAmount.ToString();
        WaveSpawn.NextValue = currentWaveEnemyCount.ToString();
    }

    private void CheckForInsufficientResources()
    {
        if (FoodResource.Count < WarriorHireCost)
        {
            WarriorHiring.SetDisabledButtonDueToInsufficientResources();
        }
        else
        {
            WarriorHiring.ClearButtonDisabled();
        }

        if (FoodResource.Count < WorkerHireCost)
        {
            WorkerHiring.SetDisabledButtonDueToInsufficientResources();
        }
        else
        {
            WorkerHiring.ClearButtonDisabled();
        }
    }

    private bool CheckGameStatus()
    {
        bool win = true;
        // lose if not sufficient food for consume
        if (FoodResource.Count < 0)
        {
            PutGameOnHold("Войско ушло из за недостатка еды" + System.Environment.NewLine + "Вы проиграли!", "", false, true);
            win = false;
        }

        // lose if no warriors left
        if (ArmyResource.Count < 0)
        {
            PutGameOnHold("Войско разгромлено" + System.Environment.NewLine + "Вы проиграли!", "", false, true);
            win = false;
        }

        // win if goal reached
        if (ArmyResource.Count >= warriorsCountGoal && WorkersResource.Count >= workersCountGoal && FoodResource.Count >= foodCountGoal)
        {
            PutGameOnHold("Вы достигли цели" + System.Environment.NewLine + "Вы выиграли!", "", false, true);
        }
        return win;
    }

    public void HireButtonPressed(HireActionType hireActionType)
    {
        switch (hireActionType)
        {
            case HireActionType.HIRE_WORKER:
                FoodResource.Count -= WorkerHireCost;
                break;
            case HireActionType.HIRE_WARRIOR:
                FoodResource.Count -= WarriorHireCost;
                break;
            default: break;
        }
        CheckGameStatus();
        RefreshFoodConsumptionValues();
        RefreshSplinters();
        CheckForInsufficientResources();
    }

    private void PutGameOnHold(string _gameOnHoldMessage, string _gameOnHildSubMessage, bool pause = false, bool activateDash = false)
    {
        if (pause)
        {
            Time.timeScale = 0f;
        }
        else
        {
            FoodProduction.DisableCycle();
            FoodConsumption.DisableCycle();
            WaveSpawn.DisableCycle();
            WorkerHiring.DisableCycle();
            WarriorHiring.DisableCycle();
            restartGameButton.gameObject.SetActive(true);
            startGameButton.gameObject.SetActive(false);
            resumeGameButton.gameObject.SetActive(false);
            WorkerHiring.ResetButton();
            WarriorHiring.ResetButton();
        }
        gameOnHold.SetActive(true);
        gameOnHoldMessage.text = _gameOnHoldMessage;
        gameOnHildSubMessage.text = _gameOnHildSubMessage;
        wavesSurvivedText.text = wavesSurvived.ToString();
        foodProducedText.text = foodProduced.ToString();
        warriorsHiredText.text = warriorsHired.ToString();
        workersHiredText.text = workersHired.ToString();
        dashBoard.SetActive(activateDash);
    }

    public void PauseTheGame()
    {
        PutGameOnHold("ПАУЗА", "", true, true);
        startGameButton.gameObject.SetActive(false);
        resumeGameButton.gameObject.SetActive(true);
        dashBoard.gameObject.SetActive(false);
        restartGameButton.gameObject.SetActive(false);
    }

    public void ResumeTheGame()
    {
        Time.timeScale = 1;
        gameOnHold.SetActive(false);
    }

    public void RestartTheGame()
    {
        gameOnHold.SetActive(false);
        StartGame();
    }
}
