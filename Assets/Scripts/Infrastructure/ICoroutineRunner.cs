using System.Collections;
using UnityEngine;

namespace FightDojo.Infrastructure
{
  public interface ICoroutineRunner
  {
    Coroutine StartCoroutine(IEnumerator coroutine);
  }
}