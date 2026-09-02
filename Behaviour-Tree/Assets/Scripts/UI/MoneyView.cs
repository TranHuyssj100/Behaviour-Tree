using TMPro;
using UnityEngine;
using VContainer;

public class MoneyView : MonoBehaviour
{
    [SerializeField] TMP_Text label;
    [SerializeField] string format = "{0:N0} $";

    PlayerProfile profile;

    [Inject]
    public void Construct(PlayerProfile injected)
    {
        if (profile != null)
            profile.MoneyChanged -= Render;

        profile = injected;
        profile.MoneyChanged += Render;
        Render(profile.Money);
    }

    void OnDestroy()
    {
        if (profile != null)
            profile.MoneyChanged -= Render;
    }

    void Render(int money)
    {
        if (label != null)
            label.text = string.Format(format, money);
    }
}
