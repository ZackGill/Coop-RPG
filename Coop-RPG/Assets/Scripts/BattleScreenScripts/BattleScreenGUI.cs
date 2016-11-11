using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using AssemblyCSharp;
public class BattleScreenGUI : MonoBehaviour
{
    private bool nextState = true;
    private bool battleOver = false;
    // The 4 main choice buttons.
    private Button fightButton;
    private Button skillsButton;
    private Button itemsButton;
    private Button runButton;
    private Button skillButtonOptions;
    private Button skillButton1;
    private Button skillButton2;
    private Button skillButton3;
    private Button skillButton4;
    private Button skillButton5;
    private Button skillButton6;
    private Button skillButton7;
    private Button skillButton8;
    Button[] skillButtons;
    private Skill[] playerSkills;
    private Text playerHealthString;
    private Text fightMessage;
    private Image playerHealthBar;
    private Image playerActiveTimerBar;
    private CanvasGroup optionsPanel;
    private CanvasGroup fightButtonsPanel;
    private CanvasGroup fightTextPanel;
    // This is how we'll interact with the other scripts.
    private ActiveTime activeTime;
    private BattleScreenStates state;
    private EnemyQuantity enemies;
    private BattleLogic battleLogic;
    private Characters character;
    // To control enemy sprites
    private SpriteRenderer enemy1;
    private SpriteRenderer enemy2;
    private SpriteRenderer enemy3;

    void Start()
    {

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

        // The menus, so we can set them to visible or invisible, then the scripts.
        optionsPanel = transform.FindChild("OptionsMenu").GetComponent<CanvasGroup>();
        fightButtonsPanel = transform.FindChild("FightMenu/FightButtonsPanel").GetComponent<CanvasGroup>();
        fightTextPanel = transform.FindChild("FightMenu/FightTextPanel").GetComponent<CanvasGroup>();
        activeTime = transform.FindChild("PlayerInfo/ActiveTimeBar").GetComponent<ActiveTime>();
        state = GetComponent<BattleScreenStates>();
        battleLogic = GetComponent<BattleLogic>();

        // The Skill Buttons.
        skillButton1 = transform.FindChild("OptionsMenu/VisibleArea/SkillsMenu/SkillsScroll/SkillButton1").GetComponent<Button>();
        skillButton2 = transform.FindChild("OptionsMenu/VisibleArea/SkillsMenu/SkillsScroll/SkillButton2").GetComponent<Button>();
        skillButton3 = transform.FindChild("OptionsMenu/VisibleArea/SkillsMenu/SkillsScroll/SkillButton3").GetComponent<Button>();
        skillButton4 = transform.FindChild("OptionsMenu/VisibleArea/SkillsMenu/SkillsScroll/SkillButton4").GetComponent<Button>();
        skillButton5 = transform.FindChild("OptionsMenu/VisibleArea/SkillsMenu/SkillsScroll/SkillButton5").GetComponent<Button>();
        skillButton6 = transform.FindChild("OptionsMenu/VisibleArea/SkillsMenu/SkillsScroll/SkillButton6").GetComponent<Button>();
        skillButton7 = transform.FindChild("OptionsMenu/VisibleArea/SkillsMenu/SkillsScroll/SkillButton7").GetComponent<Button>();
        skillButton8 = transform.FindChild("OptionsMenu/VisibleArea/SkillsMenu/SkillsScroll/SkillButton8").GetComponent<Button>();
        skillButtons = new Button[8];
        skillButtons[0] = skillButton1;
        skillButtons[1] = skillButton2;
        skillButtons[2] = skillButton3;
        skillButtons[3] = skillButton4;
        skillButtons[4] = skillButton5;
        skillButtons[5] = skillButton6;
        skillButtons[6] = skillButton7;
        skillButtons[7] = skillButton8;
        for (int i = 0; i < skillButtons.Length; i++)
        {
            skillButtons[i].enabled = false;
            skillButtons[i].GetComponent<Image>().enabled = false;
            skillButtons[i].transform.Find("Text").GetComponent<Text>().enabled = false;
        }

        // The enemy sprites.
        enemy1 = transform.FindChild("EnemyPanel/Enemy").GetComponent<SpriteRenderer>();
        enemy2 = transform.FindChild("EnemyPanel/Enemy2").GetComponent<SpriteRenderer>();
        enemy3 = transform.FindChild("EnemyPanel/Enemy3").GetComponent<SpriteRenderer>();
        enemy2.enabled = false;
        enemy3.enabled = false;

        character = GetComponent<Characters>();
        StartCoroutine(updateFromDatabase());
    }

    void Update()
    {

        playerHealthString.text = battleLogic.getPlayerHP().ToString();
        playerHealthBar.fillAmount = (battleLogic.getPlayerHP()) / (100);
        playerActiveTimerBar.fillAmount = activeTime.GetRatio();

        stateCheck();

        character = battleLogic.getCharacter();
    }

    IEnumerator updateFromDatabase()
    {
        yield return new WaitForSeconds(51);
        character = battleLogic.getCharacter();
        fillSkillButtons();
    }

    // Checks the state of the battle and changes the UI accordingly.
    // TODO: I suspect that much of this would be better in battle logic, or could be handled better.
    void stateCheck()
    {
        fightButtonsPanel.alpha = 0;
        fightTextPanel.alpha = 1;

        switch (state.curState)
        {
            case (BattleScreenStates.FightStates.BEGINNING):
                fightMessage.text = battleLogic.getFightMessage();
                break;
            case (BattleScreenStates.FightStates.NEUTRAL):
                if (!battleLogic.currentMoveSelected)
                {
                    fightButtonsPanel.interactable = true;
                    optionsPanel.interactable = true;
                }
                fightButtonsPanel.alpha = 1;
                fightTextPanel.alpha = 0;
                break;
            case (BattleScreenStates.FightStates.ENEMYTURN):
                fightMessage.text = battleLogic.getFightMessage();
                fightButtonsPanel.interactable = false;
                break;
            case (BattleScreenStates.FightStates.SECONDENEMYJOINS):
                fightMessage.text = battleLogic.getFightMessage();
                fightButtonsPanel.interactable = false;
                enemy2.enabled = true;
                break;
            case (BattleScreenStates.FightStates.THIRDENEMYJOINS):
                fightMessage.text = battleLogic.getFightMessage();
                fightButtonsPanel.interactable = false;
                enemy3.enabled = true;
                break;
            case (BattleScreenStates.FightStates.PLAYERTURN):
                fightMessage.text = battleLogic.getFightMessage();
                battleLogic.currentMoveSelected = false;
                break;
            case (BattleScreenStates.FightStates.WIN):
                fightMessage.text = battleLogic.getFightMessage();
                battleLogic.currentMoveSelected = false;
                battleOver = true;
                optionsPanel.alpha = 0;
                break;
            case (BattleScreenStates.FightStates.PICKANENEMY):
                fightMessage.text = battleLogic.getFightMessage();
                battleLogic.currentMoveSelected = false;
                optionsPanel.alpha = 0;
                break;
            case (BattleScreenStates.FightStates.LOSE):
                fightMessage.text = battleLogic.getFightMessage();
                battleLogic.currentMoveSelected = false;
                battleOver = true;
                optionsPanel.alpha = 0;
                break;
        }
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
        battleLogic.currentMoveSelected = true;
    }

    void RunButtonClicked()
    {
        fightButtonsPanel.interactable = false;

        // Toggle the visibility of the Options Menu.
        optionsPanel.alpha = 0;
    }

    public void fillSkillButtons()
    {
        playerSkills = character.getSkills();
        for (int i = 0; i < playerSkills.Length; i++)
        {
            skillButtons[i].transform.Find("Text").GetComponent<Text>().text = playerSkills[i].getName();
            skillButtons[i].interactable = true;
            skillButtons[i].enabled = true;
            skillButtons[i].GetComponent<Image>().enabled = true;
            skillButtons[i].transform.Find("Text").GetComponent<Text>().enabled = true;
        }
    }

    public void skillButtonClicked(int which)
    {
        fightButtonsPanel.interactable = false;
        optionsPanel.interactable = false;

        // Toggle the visibility of the Options Menu.
        optionsPanel.alpha = 0;
        battleLogic.whichSkill = which;
        battleLogic.currentMoveSelected = true;
    }

}
