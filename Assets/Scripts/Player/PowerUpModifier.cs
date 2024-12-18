using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class PowerUpModifier 
{
    static string filePath;
    static string json;
    int dashes;
    float dashTime;
    float timeStop;
    int luck;
    float radious;

    public class PowerUpsData
    {
        public int dashes;
        public float dashTime;
        public float timeStop;
        public float radious;
        public int luck;        
    }

    PowerUpsData loadedData;
   
     public void Start()
    {
        dashes = KyotoManager.Instance.GetLevel(PowerUpLevel.DASHLEVEL);
        dashTime = KyotoManager.Instance.GetLevel(PowerUpLevel.DASHTIMELEVEL);
        timeStop = KyotoManager.Instance.GetLevel(PowerUpLevel.TIMESTOPLEVEL);
        radious = KyotoManager.Instance.GetLevel(PowerUpLevel.COINCOLLECTIONLEVEL);
        luck = KyotoManager.Instance.GetLevel(PowerUpLevel.LUCKLEVEL);
    }

   public int Dash()
    {


        Debug.Log(dashes);
        return dashes;
    }

    public float DashTime()
    {

        return dashTime;
    }

    public float TimeStop()
    {

        return timeStop;
    }

    public float CoinCollection()
    {

        return radious;
    }
    //Numero del uno al 100, siendo 100 asegurar recoger dos monedas, y 0 solo recoger 1
    public int Luck()
    {

        return luck;
    }
}
