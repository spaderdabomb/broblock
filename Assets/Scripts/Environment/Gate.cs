using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gate : MonoBehaviour
{
    public GateType gateType;
    public Key keySO;

    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Start()
    {
        spriteRenderer.color = keySO.color;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<InteractTrigger>() == null) 
            return;

        if (CanOpenGate())
        {
            RemoveGate();
        }
    }

    private bool CanOpenGate()
    {
        KeyType gateKeyType = GlobalData.gateToKeyDict[gateType];
        return (GameManager.Instance.keyDictionary[gateKeyType] > 0) ? true : false;
    }

    private void RemoveGate() 
    {
        KeyType gateKeyType = GlobalData.gateToKeyDict[gateType];
        GameManager.Instance.keyDictionary[gateKeyType] -= 1;
        UIManager.Instance.uiGameSceneMain.SetKeyUIToEmpty(gateKeyType);
        Destroy(gameObject);
    }
}

public enum GateType
{
    GateRed,
    GateGreen,
    GateBlue,
    GatePurple,
    GateYellow
}
