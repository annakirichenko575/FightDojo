using FightDojo.Data;
using UnityEngine;

namespace FightDojo
{
    public class TestJson : MonoBehaviour
    {
        private void Start()
        {
            JsonLoader jsonLoader = new JsonLoader();
            jsonLoader.Load();
        }
    }
}
