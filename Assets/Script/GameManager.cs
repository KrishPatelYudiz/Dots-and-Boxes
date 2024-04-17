using Unity.VisualScripting;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    
    public static GameManager Instance ;
    [SerializeField]GameObject _bordPrefab;

    [SerializeField]TowPlayerScreen towPlayerScreen;
    GameObject _bord;


    private void Awake() {
        Instance = this;
    }
    
    public Player GetWinner(){
        return towPlayerScreen.GetWinner();
        
    }
    public void GenerateNewBord(){
        if(_bord != null){
            Destroy( _bord );
        }
        _bord = Instantiate(_bordPrefab);
    }

}
