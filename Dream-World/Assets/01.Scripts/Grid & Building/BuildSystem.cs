using System.Collections;
using System.Linq;
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
    Building buildingData => Managers.Instance.Player.interaction.currentBuilding;

    // 건물 지어지는 이펙트
    public GameObject BuildVFX;

    public bool isBuildMode = false;

    // 내부 변수
    private Vector3 buildPos;
    private int x, y, z;
    private int[] availableRot = new int[4]; // sbyte로 바꾸다가 형변환 이슈때문에 다시 바꿈
    private Vector3 tempPos;
    private Vector3 entity_Rot;
    private float tempRot;
    private int currentRot;
    private Material tempMat;
    private bool buildCheck = false;

    private bool ladderCheck = false;
    private Ladder tempLadder;

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

    public Vector3 GetBuildPos()
    {
        entity.blockPointer.position.GetXYZFloor(out x, out y, out z);
        y = Mathf.RoundToInt(entity.blockPointer.position.y);

        return new Vector3(x, y, z);
    }

    public void UpdatePos()
    {
        if (tempPos != GetBuildPos())
        {
            tempPos = GetBuildPos();
            buildPos = tempPos + buildingData.buildOffset;
            entity.Preview.transform.position = buildPos;
            Debug.Log($"현재 블럭 포인터 위치 : {tempPos}");

            buildCheck = BuildCheck();

            if (ladderCheck == false)
            {
                InitRotateBuilding();
            }
        }

        if (GridSystem.Instance.CheckCanCraft(x, y, z) && buildCheck == true)
        {
            buildCheck = true;
            entity_Renderer.material.color = Color.green;
        }
        else
        {
            /* 다 없어져야 할 저주받은 코드 */
            /* ladderCheck를 죽여라 */

            if (entity.Preview.name == "Ladder(Clone)" || entity.Preview.name == "Ladder")
            {
                Collider[] colliders = Physics.OverlapBox(new Vector3(x + 0.5f, y + 0.5f, z + 0.5f), new Vector3(0.40f, 0.40f, 0.40f));

                for (int i = 0; i < colliders.Length; i++)
                {
                    if (colliders[i].name == "Ladder(Clone)" || colliders[i].name == "Ladder")
                    {
                        if (colliders[i] == entity.Preview.gameObject) return;

                        ladderCheck = true;
                        tempLadder = colliders[i].GetComponent<Ladder>();
                        entity_Renderer.material.color = Color.green;
                        entity.Preview.transform.position = colliders[i].transform.position + new Vector3(0f, tempLadder.ReachHeight - 0.1f, 0f);
                        entity.Preview.transform.rotation = colliders[i].transform.rotation;
                    }
                    else if(buildCheck == false)
                    {
                        ladderCheck = false;
                        tempLadder = null;
                        entity_Renderer.material.color = Color.red;
                    }
                }
            }
            else
            {
                buildCheck = false;
                ladderCheck = false;
                tempLadder = null;
                entity_Renderer.material.color = Color.red;
            }
        }


        
    }

    public bool BuildCheck()
    {
        tempRot = 0;
        for (int i = 0; i < availableRot.Length - 1; i++)
        {
            availableRot[i] = 0;
        }

        switch (buildingData.buildCondition)
        {
            case BuildCondition.Top:

                if (GridSystem.Instance.StageGrid.GetGridObject(x, y - 1, z).GetGridObjectData().isConstructableTop)
                    availableRot[2] = 1;

                return true;

            case BuildCondition.Side:
                if (GridSystem.Instance.StageGrid.GetGridObject(x + 1, y, z) != null)
                {
                    availableRot[3] = GridSystem.Instance.StageGrid.GetGridObject(x + 1, y, z).GetGridObjectData().isConstructableSide ? 1 : 0;
                }
                if (GridSystem.Instance.StageGrid.GetGridObject(x - 1, y, z) != null)
                {
                    availableRot[1] = GridSystem.Instance.StageGrid.GetGridObject(x - 1, y, z).GetGridObjectData().isConstructableSide ? 1 : 0;
                }
                if (GridSystem.Instance.StageGrid.GetGridObject(x, y, z + 1) != null)
                {
                    availableRot[2] = GridSystem.Instance.StageGrid.GetGridObject(x, y, z + 1).GetGridObjectData().isConstructableSide ? 1 : 0;
                }
                if (GridSystem.Instance.StageGrid.GetGridObject(x, y, z - 1) != null)
                {
                    availableRot[0] = GridSystem.Instance.StageGrid.GetGridObject(x, y, z - 1).GetGridObjectData().isConstructableSide ? 1 : 0;
                }
                for (int i = 0; i < availableRot.Length; i++)
                {
                    tempRot += availableRot[i];
                }
                Debug.Log($"빌드체크 완료 : 돌릴 수 있는 방향 개수 {tempRot}개");
                if (tempRot != 0) return true;
                break;
        }

        return false;
    }

    // 2. 설치할 GridObject의 Data에 있는 조건들을 가져와서
    // 해당 위치에 건물을 지을 수 있는지 확인

    // 3. 오버랩

    // 조건 처리

    public void InitRotateBuilding()
    {
        Debug.Log("회전 초기화");
        currentRot = -1;
        RotateBuilding();
    }

    public void RotateBuilding()
    {
        if (availableRot.Contains(1) == false)
        {
            Debug.Log("돌릴 수 있는 방향이 없습니다.");
            return;
        }

        for (int i = 0; i < availableRot.Length + 1; i++)
        {
            if (currentRot < i)
            {
                currentRot = i;

                if (currentRot == availableRot.Length)
                {
                    // 배열 초기화
                    InitRotateBuilding();
                    break;
                }

                if (availableRot[i] == 0)
                {
                    continue;
                }

                entity.Preview.transform.rotation = Quaternion.Euler(new Vector3(0, 90f * currentRot, 0));
                Debug.Log($"{currentRot}번째 회전, 건물의 각도 : {90f * i}");
                break;
            }
        }

        // entity.Preview.transform.rotation *= Quaternion.Euler(new Vector3(0, 90f, 0));
    }

    public void Construct()
    {
        if(ladderCheck == true)
        {
            Managers.Inventory.OnChangeBuilding?.Invoke(Managers.Inventory.currentBuildingSlot);
            tempLadder.AddLadder();
            Destroy(entity.Preview);

            Building temp = new Building(buildingData);
            temp.itemCount = 1;
            Managers.Inventory.RemoveItem(temp);
            Managers.UI.DrawItemSlots();

            isBuildMode = false;
            return;
        }

        if (buildCheck == false) return;

        Managers.Inventory.OnChangeBuilding?.Invoke(Managers.Inventory.currentBuildingSlot);
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
        Managers.Inventory.RemoveItem(temp);
        Managers.UI.DrawItemSlots();

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
