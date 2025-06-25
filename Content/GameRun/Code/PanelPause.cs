using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PanelPause : classbasePanel
{
    protected override void Update()
    {
        base.Update();
        Active_EventMenu(ManagerPause.IsPaused);
        Active_Cursor(ManagerPause.IsPaused);
    }
    private void OnGUI()
    {
        if (ManagerDebug.DebugPlay)
        { // 変数をGUIで表示 //
            GUI.Label(new Rect(10, 400, 200, 20), $"Pause : {ManagerPause.IsPaused}");
            GUI.Label(new Rect(10, 410, 200, 20), $"Panel Cursor {num_menu}");
        }
    }
    protected override void Input_EventMenu()
    {
        if (Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            ManagerPause.Instance.PauseOrResume();
            num_menu = 0;
        }
        if (ManagerPause.IsPaused)
            base.Input_EventMenu(); // 通常の入力処理へ
    }
    // メニュー選択コモン//
    public override void HandleCursorSelect(int menuIndex)
    {
        switch (menuIndex)
        {  // メニュー項目 //
            case 1: // 上 // シーンを再開する //
                Game_Resume();
                break;
            case 2: // 左 // シーンをやり直す //
                Game_Restart();
                break;
            case 3: // 右 // オプション //
                // commonOption.Game_Option();
                break;
            case 4: // 下 // シーンを終える //
                Scene_Exit();
                break;
        }
    }
    private void Game_Resume()
    {
        ManagerPause.Instance.Resume();
    }
    private void Game_Restart()
    {
        ManagerPause.Instance.Resume();
        SceneManager.LoadScene("GameRun");
    }
        private void Scene_Exit()
    {
        ManagerPause.Instance.Resume();
        SceneManager.LoadScene("Title");
    }
}