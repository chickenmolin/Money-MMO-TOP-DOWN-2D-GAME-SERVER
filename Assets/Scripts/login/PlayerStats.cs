using Unity.Netcode;
using UnityEngine;

public class PlayerStats : NetworkBehaviour
{
    public NetworkVariable<int> Point = new NetworkVariable<int>(
        0, // Giá trị ban đầu
        NetworkVariableReadPermission.Everyone, // Ai cũng đọc được
        NetworkVariableWritePermission.Server   // Chỉ Server được ghi
    );

    public override void OnNetworkSpawn()
    {
        if (IsServer)
        {
            // Test: Cộng 10 điểm sau 2 giây
            InvokeRepeating(nameof(AddTestPoint), 2f, 2f);
        }
    }

    void AddTestPoint()
    {
        Point.Value += 10;
    }
}
