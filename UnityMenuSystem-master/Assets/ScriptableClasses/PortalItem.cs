using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/PortalItem", order = 3)]

public class PortalItem : ScriptableObject, iClickable
{
    [SerializeField] string _name;
    [SerializeField] string _description;
     public string Name { get { return _name; } set { _name = value; } }
     public string Description { get { return _description; } set { _description = value; } }

    public ClickableTypes ClickableType
    {
        get { return ClickableTypes.Portal; }
    }
}
