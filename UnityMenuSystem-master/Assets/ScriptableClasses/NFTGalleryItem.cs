using System.Collections;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum NFTStandardType { SRC721, SRC1155, Other };
public enum ClickableTypes { NFTItem, UnityAsset, PolycarbonFree, PolycarbonBasic, PolycarbonPremium, User, Bot, Advertisement, Portal }

public enum UserTypes { Unregistered, Registered, WalletConnected, BasicSubscriber, PremiumSubscriber, Administrator, StoreAdministrator, StoreFloorAdministrator, MasterFloorAdministrator, MasterAdministrator }

public interface iClickable
{
    string Name { get; set; }
    string Description { get; set; }
    ClickableTypes ClickableType { get; }

}
[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/NFTGalleryItem", order = 1)]

public class NFTGalleryItem : ScriptableObject , iClickable
{
    public SerializedNFTMetadataModel NFTItem;
    public string ContractHash;
    public string MarketURL;
    public int TokenID;
    public NFTStandardType NftType;
    public string OwnerAddress;
    public string Collection;
    public float Price;
    public string Currency;
    public string TransactionCode;
    public string Name
    {
        get
        {
            return NFTItem.name;
        }

        set
        {
            NFTItem.name = value;
        }
    }

    public string Description
    {
        get
        {
            return NFTItem.description;
        }

        set
        {
            NFTItem.description = value;
        }
    }

    public HistoryItem[] History;

    public ClickableTypes ClickableType
    {
        get { return ClickableTypes.NFTItem; }
    }
}


[System.Serializable]
public class HistoryItem
{
    public string Event;
    public string SellerAddress;
    public string BuyerAdddress;
    public float Price;
    public string Currency;
}

[System.Serializable]
public class SerializedNFTMetadataModel
{
    public string description;
    public string externalUrl;
    public string image;
    public string name;
}
