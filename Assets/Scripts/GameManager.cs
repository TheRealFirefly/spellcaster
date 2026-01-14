using System;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class GameManager : MonoBehaviour
{
    public BoardManager BoardManager;
    public PlayerController PlayerController;
    public TickManager TickManager;
    public PlayerStats PlayerStats;
    public Boolean gameStarted = false;

    public GameObject mainMenu;
    public GameObject gameOverMenu;
    public GameObject inGameUI;
    public Transform ItemArea;
    public ActiveItemSlot prefab;
    public ActiveItemSlot[] activeItems = new ActiveItemSlot[2];
    public ActiveItemSlot consumable;
    private SpellManager spells;
    public GameObject PauseScreen;
    public GameObject LoadoutScreen;

    public static GameManager instance;
    private void Awake()
    {
        instance = this;
        BoardManager.Init();
    }

    public void StartGame()
    {
        mainMenu.SetActive(false);
        TickManager.Player = PlayerStats;
        spells = PlayerStats.instance.GetComponent<SpellManager>();
        inGameUI.SetActive(true);
        
        UpdateUI();

        WaveManager.instance.StartNextWave();

        PlayerStats.OnDeath += PlayerDeath;
        gameStarted = true;
    }

    void Update()
    {
        if (gameStarted)
        {
            UpdateUI();
        }
    }

    private void OnEnable()
    {
        TickManager.instance.OnTick += UpdateUI;
    }

    private void OnDisable()
    {
        TickManager.instance.OnTick -= UpdateUI;
    }

    private void UpdateUI()
    {
        inGameUI.transform.Find("HP").GetComponentInChildren<TMP_Text>().text = "HP: " + PlayerStats.instance.currentHP + " / " + PlayerStats.instance.maxHP + "";
        inGameUI.transform.Find("Mana").GetComponentInChildren<TMP_Text>().text = "Mana: " + PlayerStats.instance.currentMana + " / " + PlayerStats.instance.maxMana + "";
        inGameUI.transform.Find("WaveTimer").GetComponentInChildren<TMP_Text>().text = "Remaining Time: " + WaveManager.instance.waveTimer;
        inGameUI.transform.Find("CurrentWave").GetComponentInChildren<TMP_Text>().text = "Wave " + WaveManager.instance.currentWave;
        inGameUI.transform.Find("EnemiesRemaining").GetComponentInChildren<TMP_Text>().text = "Remaining Enemies: " + ((WaveManager.instance.baseEnemies + WaveManager.instance.currentWave) - WaveManager.instance.enemiesKilled) + " / " + (WaveManager.instance.baseEnemies + WaveManager.instance.currentWave);
        for (int i = 0; i < activeItems.Length; i++)
        {

            if (spells?.activeSpells[i] != null)
            {
                activeItems[i].setSpell(spells.activeSpells[i], i, spells);
            }
            else
            {
                
                activeItems[i].setSpell(null, i, spells);
            }
            activeItems[i].UpdateUI(i);
          
        }

        consumable.SetConsumable(PlayerStats.instance.consumable);
        
    }
    public void playAgain()
    {
        Time.timeScale = 1.0f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void ExitGame()
    {
        Application.Quit();
    }
    private void PlayerDeath()
    {
        if (gameStarted)
        {
            PlayerController.paused = true;
            
            Time.timeScale = 0f;

            gameOverMenu.SetActive(true);
        }
    }

    public void pause()
    {
        if(gameStarted)
        {
            PlayerController.paused = true;

            Time.timeScale = 0f;
            inGameUI.SetActive(false);
            PauseScreen.SetActive(true);
        }
    }
    public void Continue()
    {
        PlayerController.paused = false;
        Time.timeScale = 1f;
        inGameUI.SetActive(true);
        PauseScreen.SetActive(false);
    }
    public void ChangeLoadout()
    {
        LoadoutScreen.SetActive(true);
        mainMenu.SetActive(false);
    }
    public void BackToMain()
    {
        LoadoutScreen.SetActive(false);
        mainMenu.SetActive(true);
    }
}
