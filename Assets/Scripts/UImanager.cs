using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UImanager : MonoBehaviour
{
    public GameObject startPanel;
    public GameObject runningPanel;
    public GameObject gameOverPanel;
    public GameObject controlsPanel;

    public TMP_Text monstersText;
    public TMP_Text gameOverText;

    public Texture2D cursorTexture;
    void ShowPanel(GameObject panel)
    {
        startPanel.SetActive(false); 
        runningPanel.SetActive(false);
        gameOverPanel.SetActive(false);
        if(panel != null )
        {
            panel.SetActive(true);
        }
    }

    public void UpdateMonsterText()
    {

        if (MonsterSpawner.totalMonsters != 0)
            monstersText.text="Monsters to defeat: "+MonsterSpawner.totalMonsters;
        else
        {
            if(DragonBehaviour.dragonHealth<0)
                DragonBehaviour.dragonHealth=0;
            monstersText.text="Dragon health: "+DragonBehaviour.dragonHealth;
        }
    }

   
    public void StartGame()
    {
        ShowPanel(runningPanel);
        UpdateMonsterText();
        
    }

    public void MainMenu()
    {
        ShowPanel(startPanel);
    }

    public void GameOverMenu()
    {
        if (PlayerAttributes.playerHealth>0)
            gameOverText.text="YOU HAVE DEFEATED THE DRAGON!\nCONGRATULATIONS!";
        else gameOverText.text="GAME\nOVER";
        ShowPanel(gameOverPanel);
    }
  
    

    public void ShowControls()
    {
            if (controlsPanel.activeSelf)
            { 
                controlsPanel.SetActive(false);
                Time.timeScale= 1.0f;
            }

            else
            { 
                controlsPanel.SetActive(true);
                Time.timeScale= 0f;
            }    
    }
    void Start()
    {
        Cursor.SetCursor(cursorTexture, Vector2.zero, CursorMode.Auto);
        MainMenu();
       
    }

   void Update()
   {
        if(Input.GetKeyDown(KeyCode.Escape))
            ShowControls();
       
   }
 
}
