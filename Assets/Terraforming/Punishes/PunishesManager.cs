using Events;
using UnityEngine;

public class PunishesManager : MonoBehaviour
{
    [SerializeField] private GameObject acidRainPrefab;
    private TriangularGrid grid;
    private bool inStandBy = false;
    public int punishCount = 0;

    private void Awake()
    {
        EventManager.AddListener(ENUM_DominoeEvent.punishEvent, TriggerRandomPunish);
        EventManager.AddListener(ENUM_DominoeEvent.selectDoneEvent, SetStandBy);
        EventManager.AddListener(ENUM_DominoeEvent.finishPunishEvent, QuitStandBy);
    }

    private void OnDestroy()
    {
        EventManager.RemoveListener(ENUM_DominoeEvent.punishEvent, TriggerRandomPunish);
        EventManager.RemoveListener(ENUM_DominoeEvent.selectDoneEvent, SetStandBy);
        EventManager.RemoveListener(ENUM_DominoeEvent.finishPunishEvent, QuitStandBy);
    }
    private void TriggerRandomPunish()
    {
        if(inStandBy)
            return;
        // TODO: Add more random punishes
        punishCount++;
        AcidRainPunish();
        EventManager.Dispatch(ENUM_DominoeEvent.selectDoneEvent);
    }
    private void Start()
    {
        grid = TriangularGrid.FindTriangularGrid();
    }
    private void AcidRainPunish()
    {
        (Vector3? position, Quaternion? rotation, Vector3Int? center) selectedGridCell = grid.GetRandomFreeCell();
        if (selectedGridCell.position == null || selectedGridCell.rotation == null || selectedGridCell.center == null)
            return;

        DropTile gridTile = grid.FindDropTileByIntCenter((Vector3Int)selectedGridCell.center);
        // Instantiate the replacement prefab at the position of the selected Cell Grid.
        Instantiate(acidRainPrefab, (Vector3)selectedGridCell.position, (Quaternion)selectedGridCell.rotation, gridTile.transform);
        // Trigger the selectDone event or perform any other desired actions.
        grid.OccupyCell((Vector3Int)selectedGridCell.center);
        gridTile.meshCollider.enabled = false;
        EventManager.Dispatch(ENUM_DominoeEvent.spawnedAcidRainEvent); 
    }

    public void SetStandBy() => inStandBy =true;
    public void QuitStandBy() => inStandBy = false;
}