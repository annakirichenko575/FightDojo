using System;
using Infrastructure.AssetManagement;
using Services;
using TMPro;
using UnityEngine;

namespace FightDojo
{
  public class KeyTextSpawner
  {
    private readonly float stripScale;
    private readonly Vector2 offset;
    private readonly IAssetProvider assetProvider;

    public KeyTextSpawner(float stripScale, Vector2 offset, IAssetProvider assetProvider)
    {
      this.stripScale = stripScale;
      this.offset = offset;
      this.assetProvider = assetProvider;
    }

    public GameObject SpawnKeyText(int id, string action, float time, string keyName, Transform parent)
    {
      Vector2 right = Vector2.right * (time * stripScale) + offset;
      GameObject keyGO = assetProvider.Instantiate(AssetPath.KeyTextPath, parent);

      RectTransform keyRect = keyGO.GetComponent<RectTransform>();
      keyRect.anchoredPosition = new Vector2(right.x, right.y);

      TMP_Text keyText = keyGO.GetComponent<TMP_Text>();
      keyText.text = keyName;

      TMP_Text text = keyGO.GetComponent<TMP_Text>();
      if (text != null)
      {
        text.color = action == Constants.Press
          ? new Color(0.0f, 0.0f, 0.7f, 1f)
          : new Color(0.0f, 0.0f, 0.7f, 0.6f);
      }

      StripItemView stripItem = keyGO.AddComponent<StripItemView>();
      stripItem.Initialize(id);
      return keyGO;
    }

    public float GetTimeByPosition(float x) =>
      (x - offset.x) / stripScale;
  }
}