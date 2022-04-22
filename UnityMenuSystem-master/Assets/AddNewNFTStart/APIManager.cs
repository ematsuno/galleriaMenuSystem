using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;
using SimpleJSON;
using System.Linq;

public class APIManager : MonoBehaviour
{
    // Start is called before the first frame update
    private const string URL = "https://cirrus-api-ha.stratisplatform.com/api/SmartContracts/local-call";



    void Start()
    {

    }

    public void SendPostData(PostData postData, Action<GetData, string> callback)
    {
        StartCoroutine(RequestWebService(postData, callback));
    }

    IEnumerator RequestWebService(PostData body, Action<GetData, string> callback)
    {

        string getDataUrl = "https://cirrus-api-ha.stratisplatform.com/api/SmartContracts/local-call";
        Debug.Log("---------------- URL ----------------");
        Debug.Log(getDataUrl);

        var webData = new UnityWebRequest();
        webData.url = getDataUrl;
        webData.method = UnityWebRequest.kHttpVerbPOST;
        webData.downloadHandler = new DownloadHandlerBuffer();
        webData.uploadHandler = new UploadHandlerRaw(string.IsNullOrEmpty(JsonUtility.ToJson(body)) ? null : Encoding.UTF8.GetBytes(JsonUtility.ToJson(body)));
        webData.timeout = 60;

        webData.SetRequestHeader("Accept", "application/json");
        webData.SetRequestHeader("Content-Type", "application/json; charset=UTF-8");

        yield return webData.SendWebRequest();
        GetData getData = new GetData();
        string errorMessage = "";
        if (webData.isNetworkError)
        {
            Debug.Log("---------------- ERROR ----------------");
            Debug.Log(webData.error);
        }
        if (webData.isDone)
        {
            Debug.Log("---------------- Response Raw ----------------");
            Debug.Log(Encoding.UTF8.GetString(webData.downloadHandler.data));
            var jsonData = (Encoding.UTF8.GetString(webData.downloadHandler.data));

            if (jsonData == null)
            {
                Debug.Log("---------------- NO DATA ----------------");
            }
            else
            {
                Debug.Log("---------------- JSON DATA ----------------");
                //Debug.Log(jsonData);
                JSONNode node = JSON.Parse(jsonData);

                Debug.Log(node["errors"][0]["message"]);
                errorMessage = node["errors"][0]["message"];
                getData = JsonUtility.FromJson<GetData>(jsonData);
                Debug.Log(getData);

            }
        }
        callback?.Invoke(getData, errorMessage);


    }

    public void GetJsonData(string url, Action<string> callback)
    {
        StartCoroutine(GetDataURL(url, callback));
    }

    IEnumerator GetDataURL(string url, Action<string> callBack)
    {
        UnityWebRequest request = UnityWebRequest.Get(url);
        yield return request.SendWebRequest();

        if (request.isNetworkError || request.isHttpError)
        {
            Debug.Log(request.error);
        }
        else
        {
            Debug.Log(request.downloadHandler.text);
            var data = Encoding.UTF8.GetString(request.downloadHandler.data);
            callBack?.Invoke(data);
        }
    }

    //private void Update()
    //{
    //    if (Input.GetKeyDown(KeyCode.U))
    //    {
    //        StartCoroutine(GetDataURL("https://stratisphere.com/asset/CGcwcLoHF1RWVANXxMvds9osjPRaKWf71K/10", (data) =>
    //        {
    //            var source = WebUtility.HtmlDecode(data);

    //            Debug.Log(data);
    //        }));

    //    }
    //}
}


[Serializable]
public class PostData
{
    public string contractAddress = "CGcwcLoHF1RWVANXxMvds9osjPRaKWf71K";
    public string methodName = "TokenURI";
    public string amount = "0";
    public int gasPrice = 10000;
    public int gasLimit = 250000;
    public string sender = "CGcwcLoHF1RWVANXxMvds9osjPRaKWf71K";
    public List<string> parameters;
}

// Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
[Serializable]
public class GasConsumed
{
    public int value;
}

[Serializable]
public class GetData
{
    public List<string> internalTransfers;
    public GasConsumed gasConsumed;
    public bool revert;
    public string errorMessage;
    public string @return;
    public List<string> logs;
}



// Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
[Serializable]
public class Attribute
{
    public string trait_type;
    public string value;
    public object display_type;
}
[Serializable]
public class GetJsonData
{
    public string name;
    public string category;
    public string image;
    public string description;
    public object external_url;
    public List<Attribute> attributes;
}
