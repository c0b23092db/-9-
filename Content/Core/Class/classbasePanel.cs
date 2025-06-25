using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public abstract class classbasePanel : MonoBehaviour
{
    [SerializeField] private GameObject pauseGUI;
    [SerializeField] private AudioPanel AudioCard;
    [SerializeField] private Button[] Button;
    [SerializeField] private GameObject Cursor;
    public static classbasePanel ActivePanel { get; private set; }
    public static int num_menu = 0;
    private Color normalColor;
    private Color highlightedColor;
    protected virtual void OnEnable()
    {
        ActivePanel = this;
    }
    protected virtual void OnDisable()
    {
        if (ActivePanel == this) ActivePanel = null;
    }
    private void Start()
    {
        normalColor = Button[0].colors.normalColor;
        highlightedColor = Button[0].colors.highlightedColor;
    }

    protected virtual void Update()
    {
        Input_EventMenu();
        Update_EventMenu();
        Update_ButtonColors();
        Update_CursorPosition();
    }
    protected virtual void Update_EventMenu() { }
    public virtual void Active_EventMenu(bool show = false)
    {
        pauseGUI.SetActive(show);
        // TODO ボタンを動かしながら表示させたい。
        // if (show == true)
        // {
        //     pauseGUI.SetActive(true);
        //     for (int i = 0; i < Button.Length; i++)
        //     {

        //     }
        // }
        // else
        // {
        //     pauseGUI.SetActive(false);
        //     for (int i = 0; i < Button.Length; i++)
        //     {

        //     }
        // }
    }
    public virtual void Active_Cursor(bool show = false)
    {
        if (num_menu == 0) show = false;
        Cursor.SetActive(show);
    }
    private int CursorMove()
    {
        // カーソル移動 //
        if(Input.anyKey)
            AudioCard.Play("クリック");
        if (Keyboard.current.upArrowKey.wasPressedThisFrame || Keyboard.current.wKey.wasPressedThisFrame)
                num_menu = 1;
        if (Keyboard.current.leftArrowKey.wasPressedThisFrame || Keyboard.current.aKey.wasPressedThisFrame)
            num_menu = 2;
        if (Keyboard.current.rightArrowKey.wasPressedThisFrame || Keyboard.current.dKey.wasPressedThisFrame)
            num_menu = 3;
        if (Keyboard.current.downArrowKey.wasPressedThisFrame || Keyboard.current.sKey.wasPressedThisFrame)
            num_menu = 4;
        return num_menu;
    }
    private void Update_CursorPosition()
    {
        int index = num_menu - 1;
        if (index < 0 || Button.Length <= index) return;
        Cursor.transform.position = Button[index].transform.position;
    }
    private void Update_ButtonColors()
    {
        for (int count = 0; count < Button.Length; count++)
        {
            ColorBlock colors = Button[count].colors;
            if (count == num_menu - 1)
            {
                colors.normalColor = highlightedColor;
                colors.highlightedColor = highlightedColor;
            }
            else
            {
                colors.normalColor = normalColor;
                colors.highlightedColor = normalColor;
            }
            Button[count].colors = colors; ;
        }
    }
    protected virtual void Input_EventMenu()
    {
        num_menu = CursorMove();
        if (Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            AudioCard.Play("決定");
            HandleCursorSelect(num_menu);
        }
    }
    public abstract void HandleCursorSelect(int menuIndex);
}