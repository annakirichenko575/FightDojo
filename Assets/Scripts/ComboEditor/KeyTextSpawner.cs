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

    public Vector2 GetTimeOffset(float time) => Vector2.right * (time * stripScale) + offset;
    
    public GameObject SpawnKeyText(int id, string action, 
      float time, string keyName, Transform parent,
      bool isInput = false)
    {
      Vector2 timeOffset = GetTimeOffset(time);
      GameObject keyGO = assetProvider.Instantiate(AssetPath.KeyTextPath, parent);
      StripItemView stripItem = keyGO.AddComponent<StripItemView>();
      stripItem.Initialize(id, timeOffset, keyName, action, isInput);
      return keyGO;
    }

    public float GetTimeByPosition(float x) =>
      (x - offset.x) / stripScale;
  }
}