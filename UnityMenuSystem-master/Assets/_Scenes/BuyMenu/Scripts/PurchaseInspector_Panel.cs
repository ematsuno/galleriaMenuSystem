using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PurchaseInspector_Panel : SimpleMenu<PurchaseInspector_Panel>
{
    private void Start()
    {
        //StartCoroutine(CheckSPace());
    }

    public void OnBuyAndBidPressed()
    {
        Hide();
        Confirmation_Panel.Show();
    }
    public void OnClosePressed()
    {
        Hide();
        Error_Panel.Show();
    }

    //public override void OnBackPressed()
    //{
    //    BG_Panel.Show();
    //}


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            Hide();
            Error_Panel.Show();
        }
    }

    //IEnumerator CheckSPace()
    //{
    //    while (true)
    //    {
    //        yield return null;
    //        if (Input.GetKeyDown(KeyCode.Space))
    //        {
    //            yield return new WaitForEndOfFrame();
    //            Hide();

    //            yield return new WaitForEndOfFrame();
    //            Error_Panel.Show();
    //            break;
    //        }
    //    }
    //}



}
