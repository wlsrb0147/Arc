using System;
using System.Collections;
using System.Collections.Generic;
using MyFolder.Script;
using UnityEngine;
using UnityEngine.Animations;

public class CharacterManager : MonoBehaviour
{
    public static CharacterManager instance;
    public List<CharacterStat> characterInfo = new();
    public CharacterInfo[] info = new CharacterInfo[9];
    
    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);
    }
    private void Start()
    {
        characterInfo[0] = new("Ai", CharacterType.Ai)
        {
            charBase = new Attribute
            {
                Str = 1,
                Agi = 2,
                Int = 3,
                Luk = 4,
                Vit = 5
            }
        };
    }

    
    
    
}
