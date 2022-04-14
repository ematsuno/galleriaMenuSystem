using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GalleryItemType { TwoDimension, ThreeDimension, Other };


[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/UnityAsset", order = 2)]

public class UnityAsset : ScriptableObject, iClickable
{
    public GalleryItemType itemType;
    public string Key;
    public string Title;
    public string Summary;
    [TextArea(15, 20)]
    public string Details;
    public string Publisher;
    public string Author;
    public string URL;
    public string VideoURL;
    public string [] VideoURLs;
    public string SketchFabURL;
    public string DemoURL;
    public string AffiliateID;
    public float Price;
    public float SalePrice;
    public PreviewItem[] galleryItemPreviewObjs;

    string iClickable.Name { get =>  Title; set => throw new System.NotImplementedException(); }
    string iClickable.Description { get => Details; set => throw new System.NotImplementedException(); }

    ClickableTypes iClickable.ClickableType
    {
        get
        {
            return ClickableTypes.UnityAsset;
        }
    }
}

[System.Serializable]
public class PreviewItem
{
    public string Name;
    public GameObject Item;
    public int scaleFactor = 200;
    public Vector3 relativePosition;
    public bool isTouchEnabled = false;
}
