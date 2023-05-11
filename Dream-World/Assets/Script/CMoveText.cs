using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class CMoveText : MonoBehaviour
{
    public RectTransform ins_traTitle = null;
    [Header("이동 속도")]
    [SerializeField]
    private int ins_nMoveSpeed = 150;
    [Header("이동 방향")]
    [SerializeField]
    private bool ins_bRight = true;

    //
    private RectTransform _rtaBg;
    private Vector2 _vStartPos;
    private Vector2 _vInitPos = new Vector2(97,0);
    private Vector2 _vDirection = Vector2.right;
    private float _fEndPosX;

    private void OnEnable()
    {
        _rtaBg = transform.GetComponent<RectTransform>();
                
        //ins_traTitle 가변 사이즈일 수 있기 때문에.
        LayoutRebuilder.ForceRebuildLayoutImmediate(ins_traTitle);

        float _fTexthalf = ins_traTitle.rect.width / 2 + (_rtaBg.rect.width / 2);
        _fEndPosX = ins_traTitle.anchoredPosition.x;

        if (ins_bRight)
        {
            _vDirection = Vector2.right;
            _fEndPosX += _fTexthalf;
        }
        else
        {
            _vDirection = Vector2.left;
            _fEndPosX -= _fTexthalf;
        }
        _vStartPos = new Vector2(-_fEndPosX, ins_traTitle.anchoredPosition.y);
        ins_traTitle.anchoredPosition = _vStartPos;


        StartCoroutine(CorMoveText());
    }

    public void InitPos()
    {
        //float _fTexthalf = ins_traTitle.rect.width / 2 + (_rtaBg.rect.width / 2);
        //_fEndPosX = ins_traTitle.anchoredPosition.x;


        //_vStartPos = new Vector2(-_fEndPosX, ins_traTitle.anchoredPosition.y);
        //ins_traTitle.anchoredPosition = _vStartPos;
        ins_traTitle.anchoredPosition = _vInitPos;

    }

    private IEnumerator CorMoveText()
    {
        while (true)
        {
            ins_traTitle.Translate(_vDirection * ins_nMoveSpeed * Time.unscaledDeltaTime);
            if (IsEndPos())
            {
                ins_traTitle.anchoredPosition = _vStartPos;
            }
            yield return null;
        }
    }

    private bool IsEndPos()
    {
        if (ins_bRight)
            return _fEndPosX < ins_traTitle.anchoredPosition.x;
        else
            return _fEndPosX > ins_traTitle.anchoredPosition.x;
    }

}