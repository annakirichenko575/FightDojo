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
    }
    
    private void RegisterServices() 
    {
      _services.RegisterSingle<IRandomService>(new RandomService());
      _services.RegisterSingle<IAssetProvider>(new AssetProvider());
      _services.RegisterSingle<IDatabaseService>(new DatabaseService());
      
    }
  }
}