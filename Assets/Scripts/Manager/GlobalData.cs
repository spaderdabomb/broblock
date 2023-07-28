using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GlobalData
{
    //
    public static int numAchievements = 12;
    public static int numAchievements2 = 12;

    // Object relationships
    public static int maxKeyStack;
    public static Dictionary<KeyType, GateType> keyToGateDict = new();
    public static Dictionary<GateType, KeyType> gateToKeyDict = new();

    static GlobalData()
    {
        maxKeyStack = 1;
        keyToGateDict.Add(KeyType.Green, GateType.GateGreen);
        keyToGateDict.Add(KeyType.Blue, GateType.GateBlue);
        keyToGateDict.Add(KeyType.Red, GateType.GateRed);
        keyToGateDict.Add(KeyType.Yellow, GateType.GateYellow);
        keyToGateDict.Add(KeyType.Purple, GateType.GatePurple);

        gateToKeyDict.Add(GateType.GateGreen, KeyType.Green);
        gateToKeyDict.Add(GateType.GateBlue, KeyType.Blue);
        gateToKeyDict.Add(GateType.GateRed, KeyType.Red);
        gateToKeyDict.Add(GateType.GateYellow, KeyType.Yellow);
        gateToKeyDict.Add(GateType.GatePurple, KeyType.Purple);
    }
}
