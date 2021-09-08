using com.snake.framework.custom;
using UnityEngine;

public class NetworkSample : MonoBehaviour
{
    private NetworkImplement _networkImplement;
    private void Awake()
    {
        this._networkImplement = new NetworkImplement();
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
