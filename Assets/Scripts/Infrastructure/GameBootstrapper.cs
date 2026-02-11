using Infrastructure.AssetManagement;
using Services;
using Services.Randomizer;
using UnityEngine;

namespace Infrastructure
{
  public class GameBootstrapper : MonoBehaviour, ICoroutineRunner
  {
    private readonly AllServices _services;
    
    private void Awake()
    {

    }
    
    private void RegisterServices() 
    {
      _services.RegisterSingle<IRandomService>(new RandomService());
      _services.RegisterSingle<IAssetProvider>(new AssetProvider());
      
    }
  }
}