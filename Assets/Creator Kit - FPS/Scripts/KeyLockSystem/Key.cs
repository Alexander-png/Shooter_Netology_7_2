using Lesson_7_4.LocalizationSystem.Base;
using Lesson_7_4.LocalizationSystem.Providers.Base;
using UnityEngine;
using UnityEngine.UI;

public class Key : LocalizationProvider
{
    public string keyType;
    public Text KeyNameText;

    private void Awake() =>
        UpdateText();

    private void OnValidate() =>
        UpdateText();

    private void UpdateText()
    {
        KeyNameText.text = LocalizationCore.GetTerm(keyType, null);
    }

    protected override void UpdateValue()
    {
        UpdateText();
    }

    private void OnTriggerEnter(Collider other)
    {
        var keychain = other.GetComponent<Keychain>();

        if (keychain != null)
        {
            keychain.GrabbedKey(keyType);
            Destroy(gameObject);
        }
    }
}
