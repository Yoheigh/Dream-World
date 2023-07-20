using System.Collections;
using System.Net.Http.Headers;
using UnityEditor;
using UnityEngine;

[System.Serializable]
public class BuildSystem : MonoBehaviour
{
    // 건물 지을 위치
    [SerializeField]
    private Vector3 buildPositionOffset;

    // 플레이어 캐릭터의 앞쪽 벡터값

    // 제작할 건물의 Preview 오브젝트
    public PreviewPrefab entity;

    // 이거 언제 리팩토링함 하하
    Building buildingData => Manager.Instance.Player.interaction.currentBuilding;

    // 건물 지어지는 이펙트
    public GameObject BuildVFX;

    public bool isBuildMode = false;

    // 내부 변수
    private Vector3 buildPos;
    private int x, y, z;
    private sbyte[] availableRot = new sbyte[4];
    private Vector3 tempPos;
    private Vector3 entity_Rot;
    private float tempRot;
    private sbyte currentRot;
    private Material tempMat;

    private Collider entity_Collider;
    private Renderer entity_Renderer;

    public void Setup()
    {
        entity = FindObjectOfType<PreviewPrefab>();
    }

    private void LateUpdate()
    {
        if (!isBuildMode) return;

        UpdatePos();
    }

    public void ChangeBuildMode()
    {
        if (buildingData == null) return;

        isBuildMode = !isBuildMode;
        Debug.Log($"빌드 모드 : {isBuildMode}");
        // 으악 이게 뭐야
        tempMat = Resources.Load<GameObject>(buildingData.buildPrefabPath).GetComponentInChildren<Renderer>().material;

        switch (isBuildMode)
        {
            case true:
                entity.Preview = Instantiate(Resources.Load<GameObject>(buildingData.buildPrefabPath));
                if (entity.Preview.TryGetComponent(out entity_Collider) == true)
                {
                    entity.Preview.TryGetComponent(out entity_Collider);
                }
                else
                {
                    entity_Collider = entity.Preview.GetComponentInChildren<Collider>();
                }
                entity_Collider.enabled = !isBuildMode;
                if (entity.Preview.TryGetComponent(out entity_Renderer) == true)
                {
                    entity.Preview.TryGetComponent(out entity_Renderer);
                }
                else
                {
                    entity_Renderer = entity.Preview.GetComponentInChildren<Renderer>();
                }
                entity_Renderer.material = entity.previewMaterial;
                break;
            case false:
                Destroy(entity.Preview);
                break;
        }
    }

    public void UpdatePos()
    {
        if (tempPos != entity.blockPointer.position.GetXYZRound())
        {
            tempPos = entity.blockPointer.position.GetXYZRound(out x, out y, out z);
            buildPos = tempPos + buildingData.buildOffset;
            entity.Preview.transform.position = buildPos;
            Debug.Log($"현재 블럭 포인터 위치 : {tempPos}");
            BuildCheck();
        }

        if (GridSystem.Instance.CheckCanCraft(x, y, z))
            entity_Renderer.material.color = Color.green;
        else
            entity_Renderer.material.color = Color.red;
    }

    public void BuildCheck()
    {
        tempRot = 0;

        switch (buildingData.buildCondition)
        {
            case BuildCondition.Top:

                if (GridSystem.Instance.StageGrid.GetGridObject(x, y - 1, z).GetGridObjectData().isConstructableTop)
                    availableRot[2] = 1;

                break;

            case BuildCondition.Side:
                if (GridSystem.Instance.StageGrid.GetGridObject(x + 1, y, z).GetGridObjectData().isConstructableSide)
                    availableRot[0] = 1;
                if (GridSystem.Instance.StageGrid.GetGridObject(x - 1, y, z).GetGridObjectData().isConstructableSide)
                    availableRot[1] = 1;
                if (GridSystem.Instance.StageGrid.GetGridObject(x, y, z + 1).GetGridObjectData().isConstructableSide)
                    availableRot[2] = 1;
                if (GridSystem.Instance.StageGrid.GetGridObject(x, y, z - 1).GetGridObjectData().isConstructableSide)
                    availableRot[3] = 1;

                for (int i = 0; i < availableRot.Length - 1; i++)
                {
                    tempRot += availableRot[i];
                }

                if (tempRot == 0) return;

                break;
        }
    }

    // 2. 설치할 GridObject의 Data에 있는 조건들을 가져와서
    // 해당 위치에 건물을 지을 수 있는지 확인

    // 3. 오버랩

    // 조건 처리
    

    public void RotateBuilding()
    {
        //if (tempRot == 0)
        //{
        //    Debug.Log("구조물을 돌릴 수 없습니다.");
        //    return;

        //}

        //for (sbyte i = 0; i < availableRot.Length - 1; i++)
        //{
        //    if (availableRot[i] == 0) continue;

        //    if (currentRot < i)
        //    {
        //        entity.Preview.transform.rotation = Quaternion.Euler(new Vector3(0, 90f * i, 0));
        //        currentRot = i;
        //        break;
        //    }
        //    else if (currentRot >= i)
        //    {
        //        currentRot = -1;
        //        continue;
        //    }
        //}

        entity.Preview.transform.rotation *= Quaternion.Euler(new Vector3(0, 90f, 0));
    }

    public void Construct()
    {
        Manager.Instance.Inventory.OnChangeBuilding?.Invoke(Manager.Instance.Inventory.currentBuildingSlot);
        entity_Rot = entity.Preview.transform.rotation.eulerAngles;
        Destroy(entity.Preview);
        StartCoroutine(ConstructWithEffect());
        isBuildMode = false;
    }

    protected void ConstructionFinish()
    {
        GameObject obj = Instantiate(Resources.Load<GameObject>(buildingData.buildPrefabPath), buildPos, Quaternion.Euler(entity_Rot));
        //if (obj.TryGetComponent(out Renderer renderer) == true)
        //{
        //    renderer.material = tempMat;
        //}
        //else
        //{
        //    Renderer renderer2 = obj.GetComponentInChildren<Renderer>();
        //    renderer2.material = tempMat;
        //}
        tempMat = null;
    }

    private IEnumerator ConstructWithEffect()
    {
        Building temp = new Building(buildingData);
        temp.itemCount = 1;
        Manager.Instance.Inventory.RemoveItem(temp);
        Manager.Instance.UI.DrawItemSlots();

        var wait = new WaitForSeconds(0.3f);

        for (int i = 0; i < 3; i++)
        {
            float x = Random.Range(-0.5f, 0.5f);
            float y = Random.Range(-0.5f, 0.5f);
            float z = Random.Range(-0.5f, 0.5f);
            GameObject obj = Instantiate(BuildVFX, buildPos + new Vector3(x, y, z), Quaternion.Euler(new Vector3(0, 180f, 0f)));
            Destroy(obj, 4f);
            yield return wait;
        }

        ConstructionFinish();
        // entity.gameObject.SetActive(false);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(buildPos, 0.1f);
    }
}
