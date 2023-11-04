using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurrencyManager : MonoBehaviour
{  
     CurrencyManager instance;

    [SerializeField] int coins;

    void Awake()
    {
        RunSingelton();      
        LoadCurrencyData();
    }

    void RunSingelton()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }

    }

    public CurrencyManager GetInstance()
    {
        return instance;
    }

    public int GetCurrentCoins()
    {
        return coins;   
    }


    public void IncreaseCoins(int amountToIncrease)
    {
      coins += amountToIncrease;
       
    }

    public void DecreaseCoins(int amountToDecrease)
    {
        coins -= amountToDecrease;
       
    }

    public void SaveCurrencyData()
    {
        SaveSystem.SaveData(this);
    }

    public void LoadCurrencyData()
    {
        CurrencyData data =  SaveSystem.LoadData();
        coins = data.coins;

    }

    



}
