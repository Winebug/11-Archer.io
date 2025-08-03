using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    [SerializeField] GameObject coinPrefab;
    [SerializeField] GameObject dummy;
    [SerializeField] GameObject howToMoveUI;
    [SerializeField] GameObject howToAttackUI;

    int coinObtainderNum = 0;
    bool tutorialEnd = false;
    [SerializeField] Vector2 coinSpawnPosition1;
    [SerializeField] Vector2 coinSpawnPosition2;

    void Start()
    {
        GameObject coin = Instantiate(coinPrefab, coinSpawnPosition1, Quaternion.identity);
        CoinHandler(coin);
    }

    
    void Update()
    {
        if (dummy == null && !tutorialEnd)
        {
            tutorialEnd = true;
        }
    }

    void CoinHandler(GameObject coin)
    {
        TutorialCoin tutorialCoin = coin.GetComponent<TutorialCoin>();
        tutorialCoin.coinTriggerd = CoinBehavior;
    }

    void CoinBehavior (GameObject coin)
    {
        
        coinObtainderNum++;
        if (coinObtainderNum == 1)
        {
            GameObject coin2 = Instantiate(coinPrefab, coinSpawnPosition2, Quaternion.identity);
            CoinHandler(coin2);
        }
        else if (coinObtainderNum == 2)
        {
            howToMoveUI.SetActive(false);
            SpawnDummy();
            howToAttackUI.SetActive(true);
        }
        else
            Debug.Log($"예외 상황: 얻은 코인 개수 {coinObtainderNum}");

        Destroy(coin);
    }

    void SpawnDummy()
    {
        dummy.SetActive(true);
    }


}
