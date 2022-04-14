using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneInventory : MonoBehaviour
{
    public ScriptableObject[] GalleryItems;

    private int _currentIndex = 0;
    public int CurrentIndex
    {
        get { return _currentIndex; }
    }

   
    public ScriptableObject CurrentItem
    {
        get
        {
            return GalleryItems[CurrentIndex];
        }
    }

    public ScriptableObject SetCurrentIndex(int index)
    {
        if ((index < 0) || (index >= GalleryItems.Length))
        {
            Debug.LogError("index out of range");
            _currentIndex = 0;
        }
        _currentIndex = index;
        return GalleryItems[_currentIndex];
    }
    public ScriptableObject GetItemAtIndex(int index)
    {

        if ((index < 0) || (index >= GalleryItems.Length))
        {

            Debug.LogError("index out of range");
            index = 0;
        }

        return GalleryItems[index];
    }

    public ScriptableObject GetNext()
    {
        if (++_currentIndex >= GalleryItems.Length)
            _currentIndex = 0;

        return CurrentItem;
    }
    public ScriptableObject GetPrior()
    {
        if (--_currentIndex < 0)
            _currentIndex = GalleryItems.Length - 1;

        return CurrentItem;
    }
}
