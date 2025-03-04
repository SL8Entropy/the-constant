using UnityEngine;

public class WInningFlag:MonoBehaviour
{
    public GameObject player;
    public GameObject winScreen;
    public GameObject winText;
    void Awake(){
        winScreen.SetActive(false);
        winText.SetActive(false);
    }
    void OnCollisionEnter2D(Collision2D col){
        if(col.gameObject == player){
            Win();
        }
    }
    void Win(){
        winScreen.SetActive(true);
        winText.SetActive(true);
    }
}
