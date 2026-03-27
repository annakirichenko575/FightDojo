using FightDojo.Data;
using FightDojo.Data.AutoKeyboard;
using FightDojo.Database;
using Infrastructure.AssetManagement;
using Services;
using Services.Randomizer;
using UnityEngine;

namespace Infrastructure
{
  public class GameBootstrapper : MonoBehaviour, ICoroutineRunner
  {
    private AllServices _services = AllServices.Container;
    
    private void Awake()
    {
      RegisterServices();
      Debug.Log("Game bootstrapper started");
    }
    
    private void RegisterServices() 
    {
      _services.RegisterSingle<IRandomService>(new RandomService());
      _services.RegisterSingle<IAssetProvider>(new AssetProvider());
      _services.RegisterSingle<IDatabaseService>(new DatabaseService());
      
      JsonLoader jsonLoader = new JsonLoader();
      RecordedKeys recordedKeys = RecordDataAdapter.Adapt(jsonLoader.Load());
      _services.RegisterSingle<IRecordedKeysService>(recordedKeys);
      
    }
  }
}