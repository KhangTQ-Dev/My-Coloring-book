using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadingManager : MonoBehaviour
{
    public void OnLoading(TypeLoading typeLoading)
    {

    }
}

public enum TypeLoading
{
    LoadingInitialGame,
    LoadingLobby,
    LoadingInGame
}
