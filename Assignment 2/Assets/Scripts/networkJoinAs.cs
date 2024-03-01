using UnityEngine;
using Unity.Netcode;
using UnityEngine.UI;

public class networkJoinAs : NetworkBehaviour
{
    [SerializeField] private Button hostButton;
    [SerializeField] private Button clientButton;   

    private void Awake()
    {
        hostButton.onClick.AddListener(() => NetworkManager.Singleton.StartHost());
        clientButton.onClick.AddListener(() => NetworkManager.Singleton.StartClient());
    }

}
