using System.Collections;
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
    [SerializeField]
    private PreviewPrefab entity;

    [SerializeField]
    private Building buildingData;

    // 건물 지어지는 이펙트
    public GameObject BuildVFX;

    public bool isBuildMode = false;

    // 내부 변수
    private Vector3 buildPos;
    private int x, y, z;
    private sbyte[] availableRot = new sbyte[4];
    private Vector3 tempPos;
    private sbyte tempRot;
    private sbyte currentRot;

    private void LateUpdate()
    {
        if (!isBuildMode) return;

        UpdatePos();
    }

    public void ChangeBuildMode()
    {
        isBuildMode = !isBuildMode;
    }

    public void UpdatePos()
    {
        if (tempPos != entity.blockPointer.position.GetXYZRound())
        {
            tempPos = entity.blockPointer.position.GetXYZRound(out x, out y, out z);
            buildPos = tempPos + buildingData.buildOffset;
            Debug.Log($"현재 블럭 포인터 위치 : {tempPos}");
            BuildCheck();
        }

        if (GridSystem.Instance.CheckCanCraft(x, y, z))
            Debug.DrawLine(buildPos, Vector3.up, Color.green);
        else
            Debug.DrawLine(buildPos, Vector3.up, Color.red);
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
        if (tempRot == 0)
        {
            Debug.Log("구조물을 돌릴 수 없습니다.");
            return;

        }

        for (sbyte i = 0; i < availableRot.Length - 1; i++)
        {
            if (availableRot[i] == 0) continue;

            if (currentRot < i)
            {
                entity.transform.rotation = Quaternion.Euler(new Vector3(0, 90f * i, 0));
                currentRot = i;
                break;
            }
            else if (currentRot >= i)
            {
                currentRot = -1;
                continue;
            }
        }
    }

    public void Construct()
    {
        StartCoroutine(ConstructWithEffect());
    }
    protected void ConstructionFinish()
    {
        Instantiate(Resources.Load<GameObject>(buildingData.buildPrefabPath), transform.position, transform.rotation);
    }

    private IEnumerator ConstructWithEffect()
    {
        var wait = new WaitForSeconds(0.5f);

        for (int i = 0; i < 6; i++)
        {
            float x = Random.Range(-0.5f, 0.5f);
            float y = Random.Range(-0.5f, 0.5f);
            float z = Random.Range(-0.5f, 0.5f);
            GameObject obj = Instantiate(BuildVFX, new Vector3(x, y, z), Quaternion.identity);
            Destroy(obj, 4f);
            yield return wait;
        }

        ConstructionFinish();
        Destroy(gameObject);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(buildPos, 0.1f);
    }
}
