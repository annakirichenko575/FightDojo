using FightDojo.Data;
using Services;
using UnityEngine;

public class SaveComboJsonButton : MonoBehaviour
{
    private ComboDataProvider _comboDataProvider;
    private IRecordedKeysService _recordedKeys;

    private void Awake()
    {
        _comboDataProvider = FindAnyObjectByType<ComboDataProvider>();
        _recordedKeys = AllServices.Container.Single<IRecordedKeysService>();
    }

    public void SaveCombo()
    {
        string json = _recordedKeys.ToJson();
        _comboDataProvider.UpdateComboJson(json);
        Debug.Log(json);
    }
}