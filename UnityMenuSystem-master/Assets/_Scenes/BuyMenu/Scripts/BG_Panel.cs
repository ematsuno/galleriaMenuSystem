using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BG_Panel : SimpleMenu<BG_Panel>
{
    public void OnBuyPressed()
    {
        PurchaseInspector_Panel.Show();
    }


    public void OnSharePressed()
    {
        Sharing_Panel.Show();
    }


    public void OnLogoPressed()
    {
        TransactionProcessing_Panel.Show();
    }
}
