using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public class MushoodMenuManager : MonoBehaviour
{
    public BG_Panel bG_PanelPrefab;
    public Confirmation_Panel confirmation_PanelPrefab;
    public Error_Panel error_PanelPrefab;
    public PurchaseInspector_Panel purchaseInspector_PanelPrefab;
    public Sharing_Panel sharing_PanelPrefab;
    public TransactionProcessing_Panel transactionProcessing_PanelPrefab;

    private Stack<BaseMenu> menuStack = new Stack<BaseMenu>();

    public static MushoodMenuManager Instance { get; set; }

    private void Awake()
    {
        Instance = this;

        BG_Panel.Show();
    }

    private void OnDestroy()
    {
        Instance = null;
    }

    public void CreateInstance<T>() where T : BaseMenu
    {
        var prefab = GetPrefab<T>();

        Instantiate(prefab, transform);
    }

    public void OpenMenu(BaseMenu instance)
    {
        // De-activate top menu
        if (menuStack.Count > 0)
        {
            if (instance.DisableMenusUnderneath)
            {
                foreach (var menu in menuStack)
                {
                    menu.gameObject.SetActive(false);

                    if (menu.DisableMenusUnderneath)
                        break;
                }
            }

            var topCanvas = instance.GetComponent<Canvas>();
            var previousCanvas = menuStack.Peek().GetComponent<Canvas>();
            topCanvas.sortingOrder = previousCanvas.sortingOrder + 1;
        }

        menuStack.Push(instance);
    }

    private T GetPrefab<T>() where T : BaseMenu
    {
        // Get prefab dynamically, based on public fields set from Unity
        // You can use private fields with SerializeField attribute too
        var fields = this.GetType().GetFields(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly);
        foreach (var field in fields)
        {
            var prefab = field.GetValue(this) as T;
            if (prefab != null)
            {
                return prefab;
            }
        }

        throw new MissingReferenceException("Prefab not found for type " + typeof(T));
    }

    public void CloseMenu(BaseMenu menu)
    {
        Debug.Log("menuStack " + menuStack.Count);

        foreach (var m in menuStack)
        {
            Debug.Log("Menu " + menu);
        }

        if (menuStack.Count == 0)
        {
            Debug.LogErrorFormat(menu, "{0} cannot be closed because menu stack is empty", menu.GetType());
            return;
        }



        if (menuStack.Peek() != menu)
        {
            Debug.LogErrorFormat(menu, "{0} cannot be closed because it is not on top of stack", menu.GetType());
            return;
        }


        CloseTopMenu();
    }

    public void CloseTopMenu()
    {
        var instance = menuStack.Pop();

        if (instance.DestroyWhenClosed)
            Destroy(instance.gameObject);
        else
            instance.gameObject.SetActive(false);

        // Re-activate top menu
        // If a re-activated menu is an overlay we need to activate the menu under it
        foreach (var menu in menuStack)
        {
            menu.gameObject.SetActive(true);

            if (menu.DisableMenusUnderneath)
                break;
        }
    }

    private void Update()
    {
        // On Android the back button is sent as Esc
        if (Input.GetKeyDown(KeyCode.Escape) && menuStack.Count > 0)
        {
            menuStack.Peek().OnBackPressed();
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {

        }
    }
}

