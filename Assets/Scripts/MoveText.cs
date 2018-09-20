using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class MoveText : MonoBehaviour
{
    /// <summary>テキストを動かす先の座標</summary>
    [SerializeField] Vector3 m_moveToAnchor;
    /// <summary>テキストが動く時間</summary>
    [SerializeField] float m_movingTime = 5.0f;
    private RectTransform m_rect;
    private Sequence m_sequence;

    private void Move()
    {
        //iTween.MoveTo(gameObject, m_moveToAnchor, m_movingTime);
        m_rect.DOMove(m_moveToAnchor, m_movingTime);
    }

    private void Start()
    {
        m_rect = GetComponent<RectTransform>();
        Move();
    }
}
