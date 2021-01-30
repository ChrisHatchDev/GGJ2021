using UnityEngine;
using Normal.Realtime;

public class PlayerRealtimeManager : MonoBehaviour {
    private Realtime _realtime;

    [SerializeField]
    private GameObject _playerPrefab;

    private void Awake() {
        _realtime = GetComponent<Realtime>();

        // Notify us when Realtime successfully connects to the room
        _realtime.didConnectToRoom += DidConnectToRoom;
    }

    private void DidConnectToRoom(Realtime realtime) {
        // Instantiate the CubePlayer for this client once we've successfully connected to the room
        Realtime.Instantiate(_playerPrefab.name,                 // Prefab name
                            position: Vector3.up,          // Start 1 meter in the air
                            rotation: Quaternion.identity, // No rotation
                        ownedByClient: true,                // Make sure the RealtimeView on this prefab is owned by this client
            preventOwnershipTakeover: true,                // Prevent other clients from calling RequestOwnership() on the root RealtimeView.
                            useInstance: realtime);           // Use the instance of Realtime that fired the didConnectToRoom event.
    }
}