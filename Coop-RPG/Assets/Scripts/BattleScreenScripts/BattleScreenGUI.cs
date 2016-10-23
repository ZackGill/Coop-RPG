using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class BattleScreenGUI : MonoBehaviour {

    private bool nextState = true;
    private bool currentMoveSelected = false;
    private bool battleOver = false;
    // The 4 main choice buttons.
    private Button fightButton;
    private Button skillsButton;
    private Button itemsButton;
    private Button runButton;
    private Button skillButtonOptions;
    private Text playerHealthString;
    private Text fightMessage;
    private Image playerHealthBar;
    private Image playerActiveTimerBar;
    private Image playerBattleAvatar;
    private CanvasGroup optionsPanel;
    private CanvasGroup fightButtonsPanel;
    private CanvasGroup fightTextPanel;
    // This is how we'll interact with the other scripts.
    private ActiveTime activeTime;
    private BattleScreenStates state;
    private BattleLogic battleLogic;

	void Start () {

        // Set listeners for our four main buttons.
        fightButton = transform.FindChild("FightMenu/FightButtonsPanel/FightButton").GetComponent<Button>();
        fightButton.onClick.AddListener(() => FightButtonClicked());
        skillsButton = transform.FindChild("FightMenu/FightButtonsPanel/SkillsButton").GetComponent<Button>();
        skillsButton.onClick.AddListener(() => SkillButtonClicked());
        itemsButton = transform.FindChild("FightMenu/FightButtonsPanel/ItemsButton").GetComponent<Button>();
        itemsButton.onClick.AddListener(() => ItemsButtonClicked());
        runButton = transform.FindChild("FightMenu/FightButtonsPanel/RunButton").GetComponent<Button>();
        runButton.onClick.AddListener(() => RunButtonClicked());
        fightMessage = transform.FindChild("FightMenu/FightTextPanel/FightMessage").GetComponent<Text>();

        // Get the components from our Battle Screen GUI.
        playerHealthString = transform.FindChild("PlayerInfo/HealthBar/HP").GetComponent<Text>();
        playerHealthBar = transform.FindChild("PlayerInfo/HealthBar").GetComponent<Image>();
        playerActiveTimerBar = transform.FindChild("PlayerInfo/ActiveTimeBar").GetComponent<Image>();
        playerBattleAvatar = transform.FindChild("PlayerInfo/PlayerImage").GetComponent<Image>();

        // The menus, so we can set them to visible or invisible, then the scripts.
        optionsPanel = transform.FindChild("OptionsMenu").GetComponent<CanvasGroup>();
        fightButtonsPanel = transform.FindChild("FightMenu/FightButtonsPanel").GetComponent<CanvasGroup>();
        fightTextPanel = transform.FindChild("FightMenu/FightTextPanel").GetComponent<CanvasGroup>();
        activeTime = transform.FindChild("PlayerInfo/ActiveTimeBar").GetComponent<ActiveTime>();
        state = GetComponent<BattleScreenStates>();
        battleLogic = GetComponent<BattleLogic>();

    }
	
	void Update () {

        playerHealthString.text = battleLogic.GetPlayerHP().ToString ();
        playerHealthBar.fillAmount = (battleLogic.GetPlayerHP()) /(battleLogic.GetPlayerHPMax());
        playerActiveTimerBar.fillAmount = activeTime.GetRatio();

        if (Input.GetKeyDown("space"))
            nextState = true;

        stateCheck();
    }

    // Checks the state of the battle and changes the UI accordingly.
    // TODO: I suspect that much of this would be better in battle logic, or could be handled better.
    void stateCheck()
    {
        if (battleOver)
            return;
        if (activeTime.GetEnemyRatio() == 1)
        {
            state.curState = BattleScreenStates.FightStates.ENEMYTURN;
            nextState = true;
        }
        if (activeTime.GetRatio() == 1 && currentMoveSelected)
        {
            state.curState = BattleScreenStates.FightStates.PLAYERTURN;
            nextState = true;
        }
        if (!nextState)
            return;

        fightButtonsPanel.alpha = 0;
        fightTextPanel.alpha = 1;

        switch (state.curState)
        {
            case (BattleScreenStates.FightStates.BEGINNING):
                fightMessage.text = battleLogic.getPlayerFightMessage();
                break;
            case (BattleScreenStates.FightStates.NEUTRAL):
                fightButtonsPanel.interactable = true;
                fightButtonsPanel.alpha = 1;
                fightTextPanel.alpha = 0;
                break;
            case (BattleScreenStates.FightStates.ENEMYTURN):
                battleLogic.enemyAttacks();
                fightMessage.text = battleLogic.getEnemyFightMessage();
                fightButtonsPanel.interactable = false;
                activeTime.setEnemySeconds(0);
                break;
            case (BattleScreenStates.FightStates.PLAYERTURN):
                battleLogic.meleeAttack();
                fightMessage.text = battleLogic.getPlayerFightMessage();
                currentMoveSelected = false;
                break;
            case (BattleScreenStates.FightStates.WIN):
                fightMessage.text = battleLogic.getPlayerFightMessage();
                currentMoveSelected = false;
                battleOver = true;
                break;
            case (BattleScreenStates.FightStates.LOSE):
                fightMessage.text = battleLogic.getPlayerFightMessage();
                currentMoveSelected = false;
                battleOver = true;
                break;

        }
        nextState = false;
        state.curState = BattleScreenStates.FightStates.NEUTRAL;
    }

    void SkillButtonClicked()
    {
        // Toggle the visibility of the Options Menu.
        if (optionsPanel.alpha == 0)
            optionsPanel.alpha = 1;
        else
            optionsPanel.alpha = 0;
    }

    void ItemsButtonClicked()
    {
        if (optionsPanel.alpha == 0)
            optionsPanel.alpha = 1;
        else
            optionsPanel.alpha = 0;
    }

    void FightButtonClicked()
    {
        fightButtonsPanel.interactable = false;

        // Toggle the visibility of the Options Menu.
        optionsPanel.alpha = 0;
        currentMoveSelected = true;
    }

    void RunButtonClicked()
    {
        fightButtonsPanel.interactable = false;

        // Toggle the visibility of the Options Menu.
        optionsPanel.alpha = 0;
    }

    public void skillButtonClicked()
    {
        fightButtonsPanel.interactable = false;
        optionsPanel.interactable = false;

        // Toggle the visibility of the Options Menu.
        optionsPanel.alpha = 0;
        currentMoveSelected = true;
    }
}
