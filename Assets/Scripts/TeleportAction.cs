using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TeleportAction", menuName = "Data/Actions/Teleport", order = 1)]
public class TeleportAction : BaseAction
{
    public Int2 ChunkCoord;
    public string TargetName;


    public bool IsInsideHouse;

    public override void ExecuteAction()
    {
        if (IsInsideHouse)
        {
            Vector3 playerPos = GameManager.Instance.Player.transform.position;
            Vector3 targetPos = GameManager.Instance.House.Entry.position;

            GameManager.Instance.Player.transform.position = new Vector3(targetPos.x, targetPos.y, playerPos.z);
        }
        else
        {
            MapChunk chunk = GameManager.Instance.MapInstances[ChunkCoord];
            if (chunk == null)
            {
                Debug.Log("Chunk not found for " + ChunkCoord);
                return;
            }

            TeleportTarget target = chunk.TeleportTargets.Find(x => x.Name.Equals(TargetName));

            if (target == null)
            {
                Debug.Log("Teleport target " + TargetName + " does not exist");
                return;
            }

            Vector3 playerPos = GameManager.Instance.Player.transform.position;
            Vector3 targetPos = target.Target.transform.position;

            GameManager.Instance.Player.transform.position = new Vector3(targetPos.x, targetPos.y, playerPos.z);
        }
    }
}
