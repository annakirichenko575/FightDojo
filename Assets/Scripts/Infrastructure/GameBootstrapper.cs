using System.Collections.Generic;
using FightDojo.ComboEditor.AudioService;
using FightDojo.ComboEditor.Data;
using FightDojo.Database;
using FightDojo.Infrastructure.AssetManagement;
using FightDojo.Services;
using FightDojo.Services.Randomizer;
using UnityEngine;
using System.Globalization;
using FightDojo.ComboEditor;

namespace FightDojo.Infrastructure
{
  public class GameBootstrapper : MonoBehaviour, ICoroutineRunner
  {
    private AllServices _services = AllServices.Container;
    
    private void Awake()
    {
      CultureInfo.DefaultThreadCurrentCulture = CultureInfo.InvariantCulture;
      
      RegisterServices();
      Debug.Log("Game bootstrapper started");
    }
    
    private void RegisterServices() 
    {
      _services.RegisterSingle<IRandomService>(new RandomService());
      _services.RegisterSingle<IAssetProvider>(new AssetProvider());
      _services.RegisterSingle<IDatabaseService>(new DatabaseService());
      _services.RegisterSingle<IRecordedKeysService>(
        new RecordedKeys(new List<KeyData>()));
      _services.RegisterSingle<IAudioMasterService>(
        new AudioMasterService(_services.Single<IAssetProvider>()));
      _services.RegisterSingle<ICurrentComboInfoService>(
        new CurrentComboInfoService());
    }
  }
}