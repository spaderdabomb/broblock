using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lava : MonoBehaviour
{
    [SerializeField] float baseSpeed;
    private Level currentLevel;
    void Start()
    {
        currentLevel = GameManager.Instance.GetCurrentLevelData();
        baseSpeed = currentLevel.lavaSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += new Vector3(0f, baseSpeed * Time.deltaTime, 0f);
    }
}
