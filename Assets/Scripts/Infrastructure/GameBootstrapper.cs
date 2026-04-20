using System.Collections.Generic;
using FightDojo.AudioService;
using FightDojo.Data;
using FightDojo.Database;
using Infrastructure.AssetManagement;
using Services;
using Services.Randomizer;
using UnityEngine;
using System.Globalization;

namespace Infrastructure
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
    }
  }
}