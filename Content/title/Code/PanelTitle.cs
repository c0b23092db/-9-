using UnityEngine;
using UnityEngine.SceneManagement;
public class PanelTitle : classbasePanel
{
    protected override void Update()
    {
        base.Update();
        Active_EventMenu(ManagerGameSystem.Instance.isClickTitle);
        Active_Cursor(ManagerGameSystem.Instance.isClickTitle);
    }
    private void OnGUI()
    {
        if (ManagerDebug.DebugPlay)
        { // 変数をGUIで表示 //
            GUI.Label(new Rect(10, 400, 200, 20), $"isClickTitle : {ManagerGameSystem.Instance.isClickTitle}");
            GUI.Label(new Rect(10, 410, 200, 20), $"Panel Cursor {num_menu}");
        }
    }
    protected override void Input_EventMenu()
    {
        base.Input_EventMenu();
    }
    // メニュー選択コモン//
    public override void HandleCursorSelect(int menuIndex)
    {
        switch (menuIndex)
        { // メニュー項目 //
            case 1: // 上 // 続きから //
                Game_Load();
                break;
            case 2: // 左 // 最初から //
                Game_NewGame();
                break;
            case 3: // 右 // オプション //
                // commonOption.Game_Option();
                break;
            case 4: // 下 // 終了 //
                Game_Exit();
                break;
        }
    }
    private void Game_Load()
    {
        Debug.Log("Game Continue");
    }
    private void Game_NewGame()
    {
        SceneManager.LoadScene("GameRun");
    }
    private void Game_Exit()
    {
        Application.Quit();
    }
}