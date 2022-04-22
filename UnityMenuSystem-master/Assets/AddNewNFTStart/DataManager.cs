using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Net;
using UnityEngine.UI;
using System.Linq;

public class DataManager : MonoBehaviour
{
    public GameObject NFTPanel;
    public GameObject PopMsgObj;
    public TMPro.TMP_InputField externalURLInput;
    public TMPro.TMP_InputField contactIdInput;
    public TMPro.TMP_InputField tokenIDInput;

    public TMPro.TextMeshProUGUI addBtnText;
    public TMPro.TextMeshProUGUI popUpText;

    APIManager aPIManager;

    public event Action<string> getJsonDataAction;

    //private 

    private string _tokenID;
    private string _contactID;
    private List<Enteries> enteries;

    private void Start()
    {
        aPIManager = FindObjectOfType<APIManager>();
        getJsonDataAction += DataManager_getJsonDataAction;
        tokenIDInput.contentType = TMPro.TMP_InputField.ContentType.IntegerNumber;
    }



    public void OnSubmitClick()
    {

        _tokenID = "";
        _contactID = "";

        if (!string.IsNullOrEmpty(tokenIDInput.text) && !string.IsNullOrEmpty(contactIdInput.text))
        {
            if (PlayerPrefs.HasKey(contactIdInput.text))
            {
                Debug.Log("Already Added");
                ShowPopMsg("Already Added");
                return;
            }

            PostData(contactIdInput.text, tokenIDInput.text);
        }
        else if (!string.IsNullOrEmpty(externalURLInput.text))
        {
            var split = externalURLInput.text.Split(new[] { '/' }, StringSplitOptions.RemoveEmptyEntries);
            Uri uri = new Uri(externalURLInput.text);
            if (split.Length > 2)
            {
                Debug.Log(" " + split[split.Length - 2] + " " + split[split.Length - 1]);
                tokenIDInput.text = split[split.Length - 1];
                contactIdInput.text = split[split.Length - 2];
                if (PlayerPrefs.HasKey(split[split.Length - 1]))
                {
                    Debug.Log("Already Added");
                    ShowPopMsg("Already Added");
                    return;
                }
                PostData(split[split.Length - 2], split[split.Length - 1]);

            }
        }
    }

    private void PostData(string contactID, string tokenID)
    {
        PostData postData = new PostData();
        postData.sender = contactID.Trim();
        postData.contractAddress = contactID.Trim();

        string id = "12#" + tokenID.Trim();
        postData.parameters = new List<string>();
        postData.parameters.Add(id);

        _tokenID = tokenID.Trim();
        _contactID = contactID.Trim();

        aPIManager.SendPostData(postData, (GetData, errorMessage) =>
        {
            if (!string.IsNullOrEmpty(GetData.@return))
            {
                aPIManager.GetJsonData(GetData.@return, getJsonDataAction);
            }
            else
            {
                ShowPopMsg("Credential Error");
                Debug.Log("Error " + GetData.errorMessage + " " + errorMessage);
            }
        });
    }

    private void DataManager_getJsonDataAction(string obj)
    {
        Debug.Log("Data " + obj);
        GetJsonData data = JsonUtility.FromJson<GetJsonData>(obj);
        CreateScriptableObject(data);

        ////pop Message
        ShowPopMsg("NFT Added Successfully");

        addBtnText.text = "Add NFT";

        PlayerPrefs.SetString(_contactID, _contactID);


    }


    public void CreateScriptableObject(GetJsonData data)
    {
        var obj = ScriptableObject.CreateInstance<AddingNFT>();
        obj.data = data;
        AssetDatabase.CreateAsset(obj, "Assets/AddNewNFTStart/NFTs/" + data.name + ".asset");
        //AssetDatabase.getas
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        EditorUtility.FocusProjectWindow();
        Selection.activeObject = obj;


    }


    public void OpenAndCloseNFTPanel(bool open)
    {
        addBtnText.text = "Submit";
        externalURLInput.text = "";
        contactIdInput.text = "";
        tokenIDInput.text = "";

        NFTPanel.SetActive(open);
    }

    public void GetPath()
    {
        var paths = AssetDatabase.GetAllAssetPaths();
        string[] folder = new string[] { "Assets/AddNewNFTStart/NFTs" };
        var assets = AssetDatabase.FindAssets("Guba Bear", folder);
        for (int i = 0; i < assets.Length; i++)
        {
            Debug.Log(AssetDatabase.GUIDToAssetPath(assets[i]));
            var id = AssetDatabase.GUIDFromAssetPath("");
            //AssetDatabase.
            //AssetDatabase.Contains()
            //Debug.Log(assets[i]);
        }
    }


    public void ShowPopMsg(string msg)
    {
        StartCoroutine(ShowPopUpCor(msg));
    }

    IEnumerator ShowPopUpCor(string msg)
    {
        popUpText.text = msg;
        PopMsgObj.SetActive(true);
        yield return new WaitForSeconds(2f);
        PopMsgObj.SetActive(false);
    }

    //private void Update()
    //{
    //    if (Input.GetKeyDown(KeyCode.K))
    //    {
    //        GetPath();
    //    }
    //}

}

public class Params
{
    public string contactID;
    public string tokenID;
}

[Serializable]
public class Enteries
{
    public string contactID;
    public string tokenID;

}
