using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Manager : MonoBehaviour
{
    private Frogger frogger;
    private Home[] homes;
    private int score;
    private int lives;
    private int time;

    public GameObject gameOverMenu;
    public Text scoreText;
    public Text timeText;
    public Text livesText;

    void Awake() 
    {
        homes = FindObjectsOfType<Home>();
        frogger = FindObjectOfType<Frogger>();
    }

    void Start()
    {
        NewGame();
    }
   
   
   private void NewGame()
   {
        gameOverMenu.SetActive(false);
        SetScore(0);
        SetLives(3);
        NewLevel();
   }

   private void NewLevel()
   {
        for(int i=0;i<homes.Length;i++) {
            homes[i].enabled = false;
        }

        Respawn();
   }

   private void Respawn()
   {
       frogger.Respawn();
       StopAllCoroutines();
       StartCoroutine(Timer(30));
   }

   private IEnumerator Timer(int duration)
   {
        time = duration;
        timeText.text = time.ToString();

        while(time > 0){
            yield return new WaitForSeconds(1);
            time--;
            timeText.text = time.ToString();
        }

        frogger.Death(); 
   }

   private void SetScore(int score) 
   {    
        this.score = score;
        scoreText.text = score.ToString();
   }

   private void SetLives(int lives)
   {
        this.lives = lives;
        livesText.text = lives.ToString();
   }

   private bool IsCleared()
   {
        for(int i=0;i<homes.Length;i++) {
            if(!homes[i].enabled){
                return false;
            }
        }

        return true;
   }

   private void GemeOver()
   {
        frogger.gameObject.SetActive(false);
        gameOverMenu.SetActive(true);
        StopAllCoroutines();
        StartCoroutine(PlayAgain());
   }

   private IEnumerator PlayAgain()
   {
        bool playAgain = false;

        while(!playAgain){
            if(Input.GetKeyDown(KeyCode.Return)){
                playAgain = true;
            }
             yield return null;
        }

        NewGame();
   }

   public void AdvancedRow()
   {
        SetScore(score + 10);
   }

   public void Died()
   {
        SetLives(lives -1 );

        if(lives > 0 ){
           Invoke(nameof(Respawn),1f);
        }else{
            Invoke(nameof(GemeOver),1f);
        }
   }

   public void HomeOccupied()
   {
        frogger.gameObject.SetActive(false);

        SetScore(score+50 + time * 20);

        if(IsCleared()){
           SetScore(score+1000);
           SetLives(lives+1);
           Invoke(nameof(NewLevel),1f);
        }else{
           Invoke(nameof(Respawn),1f);
        }
   }

}
