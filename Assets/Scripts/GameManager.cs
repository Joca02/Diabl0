using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{

    public enum GameState
    {
        mainMenu,
        running,
        over
    };

    
    public GameObject prefab;
    public static GameState gameState;
    public UImanager uiManager;
    public PlayerAttributes playerAttributes;
    MonsterSpawner monsterSpawner;
    public GameObject wall;
    public playerMove abilities;
    void Start()
    {
        
        monsterSpawner=GetComponent<MonsterSpawner>();
        Reset();
        
    }


    public void StartGame()
    {
        gameState=GameState.running;
        uiManager.StartGame();

    }
    public void GameOver()
    {
        gameState=GameState.over;
        uiManager.GameOverMenu();
    }

    void DestroyAllMonsters()
    {
        GameObject[] monsters = GameObject.FindGameObjectsWithTag("Monster");
        for (int i = 0; i < monsters.Length; i++)
        {
            Destroy(monsters[i]);
        }
        GameObject dragon = GameObject.FindWithTag("Dragon");
        if( dragon != null ) 
            Destroy(dragon);
    }

    public void Reset()//RESET IGRE
    {

        DestroyAllMonsters();
        playerAttributes.ResetPlayer();
        wall.SetActive(true);
        monsterSpawner.GenerateMonsters();
        abilities.ResetAbilityIcons();
        gameState=GameState.mainMenu;
        uiManager.MainMenu();
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
