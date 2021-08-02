using System;
using UnityEngine;

[CreateAssetMenu(fileName = "Manual", menuName = "IKEA Document")]
public class IKEAManual : ScriptableObject
{
    [SerializeField]
    private string _name;

    [SerializeField]
    private string[] _twitchNames;

    [SerializeField]
    private Texture2D[] _pages;

    public string Name { get { return _name; } }

    public string[] TwitchNames { get { return _twitchNames; } }

    public Texture2D[] Pages { get { return _pages; } }
}
