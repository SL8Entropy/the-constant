using UnityEngine;

public class WInningFlag:MonoBehaviour
{
    public GameObject player;
    public GameObject winScreen;
    public GameObject winText;
    public GameObject timer;
    private timerScript timerScriptref;
    void Awake(){
        winScreen.SetActive(false);
        winText.SetActive(false);
        timerScriptref = timer.GetComponent<timerScript>(); // Get the timer script reference

    }
    void OnCollisionEnter2D(Collision2D col){
        if(col.gameObject == player){
            Win();
        }
    }
    void Win(){
        winScreen.SetActive(true);
        winText.SetActive(true);
        timerScriptref.StopTimer();
    }
}
