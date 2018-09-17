using UnityEngine;
using System.Collections;
public class BeamShot : MonoBehaviour
{
	/// <summary>レーザーの飛距離</summary>
    [SerializeField] float m_range = 100f;
	[SerializeField] float m_lazerLife = 2f;
    /// <summary>explosion of enemy</summary>
    [SerializeField] GameObject m_eEnemy;
    /// <summary>explosion of asteroid</summary>
    [SerializeField] GameObject m_eAsteroid;
    /// <summary>レーザーフラグ</summary>
	private bool m_laserFlag = false;
    /// <summary>Coroutine's flag</summary>
	private bool m_isRunning = false;
    AudioSource m_audioS;

    Ray m_shotRay;
    RaycastHit m_shotHit;
    ParticleSystem m_beamParticle;
    LineRenderer m_lineRenderer;

    
    
    private void Shot()
    {
        //m_timer += Time.deltaTime;
        if(m_beamParticle.isPlaying == false)
        {
            m_beamParticle.Play();
        }
        

        m_lineRenderer.enabled = true;
        m_lineRenderer.SetPosition(0, transform.position);
        m_shotRay.origin = transform.position;
        m_shotRay.direction = transform.forward;
        
        if(Physics.Raycast(m_shotRay,out m_shotHit,m_range))
        {
            Debug.Log("Called Raycast : " + m_shotHit.collider.gameObject.name);
            //Raycastと接触したコライダのトランスフォーム情報を格納
            var transformInfo = m_shotHit.transform;
            //接触したコライダーが付いているオブジェクトのタグに応じて爆破エフェクトを変えてシーン上から消去する
            switch (m_shotHit.collider.tag)
            {
                case "Asteroid":
                    Instantiate(m_eAsteroid, transformInfo.position, transformInfo.rotation);
                    Destroy(m_shotHit.transform.gameObject);
                    break;
                case "Enemy":
                    Instantiate(m_eAsteroid, transformInfo.position, transformInfo.rotation);
                    Destroy(m_shotHit.transform.gameObject);
                    break;
                default:
                    break;
            }
        }
        m_lineRenderer.SetPosition(1, m_shotRay.origin + m_shotRay.direction * m_range);
    }

    /// <summary>Process to stop laser</summary>
    private void DisableEffect()
    {
        m_beamParticle.Stop();
        m_lineRenderer.enabled = false;
		//m_timer = 0f;
    }

    /// <summary>レーザーの発動制御コルーチン</summary>
    IEnumerator LaserCoroutine()
	{
        //レーザーを発動してから(m_lazerLife)秒経ったらレーザーを停止する。呼び出し元はUpdate()なのでコルーチンが処理中の間は重複実行させない様にフラグ管理している。
		if(m_isRunning)
        {
            yield break;
        }
        m_isRunning = true;
        m_laserFlag = true;
        yield return new WaitForSeconds(m_lazerLife);
        m_laserFlag = false;
        yield return new WaitForSeconds(1f);
        m_isRunning = false;
	}

    //Each initialization
    private void Awake()
    {
        m_beamParticle = GetComponent<ParticleSystem>();
        m_lineRenderer = GetComponent<LineRenderer>();
        m_audioS = GetComponent<AudioSource>();
        m_audioS.volume = 0.2f;
    }

    private void Update()
    {
        //シフト押したらレーザーフラグon
        if (Input.GetMouseButton(1))
        {
            StartCoroutine(LaserCoroutine());
        }

        //フラグonならレーザー始動、offの時レーザーがシーン上にいる場合レーザー停止
        if (m_laserFlag)
        {
            Shot();
        }
        else if (m_lineRenderer.enabled)
        {

            DisableEffect();
        }
    }
}