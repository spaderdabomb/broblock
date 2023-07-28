using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataContainer : MonoBehaviour
{

    private void Awake()
    {
        // build your config
        var config = new FBPPConfig()
        {
            SaveFileName = "broblock-data.txt",
            AutoSaveData = false,
            ScrambleSaveData = false,
            // EncryptionSecret = "spadersecretcode",
            // SaveFilePath = "C:/Repositories/unity-2d-builtin-template"
        };
        // pass it to FBPP
        FBPP.Start(config);
    }
}
