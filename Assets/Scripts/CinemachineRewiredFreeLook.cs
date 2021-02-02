using System.Collections;
using UnityEngine;
using Cinemachine;
using Rewired;
 
[RequireComponent(typeof(CinemachineFreeLook))]
public class CinemachineRewiredFreeLook : MonoBehaviour
{
    [SerializeField] int m_PlayerId = 0;
 
    private void Reset() { OnValidate(); }
    private void OnValidate()
    {
        if (Application.isPlaying && ReInput.isReady)
            InitializeInput();
    }
 
    private IEnumerator Start()
    {
        yield return new WaitUntil(() => ReInput.isReady);
        InitializeInput();
    }
 
    private void InitializeInput()
    {
        if (ReInput.isReady)
        {
            Player input = ReInput.players.GetPlayer(m_PlayerId);
            CinemachineCore.GetInputAxis = input.GetAxis;
        }
    }
}
 
public class CinemachineRewiredOverrider
{
    [RuntimeInitializeOnLoadMethod]
    private static void InitializeSystemPlayer()
    {
        CinemachineCore.GetInputAxis = ReInput.players.SystemPlayer.GetAxis;
    }
}