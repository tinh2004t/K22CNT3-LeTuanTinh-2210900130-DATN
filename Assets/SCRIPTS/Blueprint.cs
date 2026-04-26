using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Blueprint  
{
    public string itemName;
    public int numOfRequriments;

    public string Req1;
    public int Req1amount;

    public string Req2;
    public int Req2amount;

    public string Req3;
    public int Req3amount;

    public string Req4;
    public int Req4amount;

    public int numberOfItemsProduced;


    public Blueprint(string name,int producedItems, int reqNum, string R1, int R1num, string R2, int R2num, string R3, int R3num, string R4, int R4num)
    {
        itemName = name;
        numberOfItemsProduced = producedItems;
        numOfRequriments = reqNum;
        Req1 = R1;
        Req1amount = R1num;
        Req2 = R2;
        Req2amount = R2num;
        Req3 = R3;
        Req3amount = R3num;
        Req4 = R4;
        Req4amount = R4num;
    }
}