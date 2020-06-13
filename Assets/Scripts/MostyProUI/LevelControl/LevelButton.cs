using UnityEngine.UI;

[System.Serializable]
public class LevelButton
{
    public Button button;
    public int levelToOpen = 0;

    public LevelButton(Button button)
    {
        this.button = button;
    }

}
