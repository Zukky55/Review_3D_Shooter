using UnityEngine;
using System.Collections;
public class BeamShot : MonoBehaviour
{
	/// <summary>レーザーの飛距離</summary>
    [SerializeField] float m_range = 200f;
    /// <summary>Laser's life time</summary>
	[SerializeField] float m_laserLife = 2f;
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
    public LineRenderer m_lineRenderer;
    /// <summary>it's the value of ParticleSystem.main.startSize.constant</summary>
    private float m_constant = 0f;
    private Vector3 m_colPos;
    ObjectController m_oc;
    /// <summary>Information on the "Transform" of the object hited Raycast</summary>
    private Transform m_transformInfo;
    /// <summary>Attack power of laser</summary>
    [SerializeField] private int m_laserAtk = 3;
    ParticleSystem.MainModule m_parMain;




    private void Shot()
    {
        if (m_laserFlag)
        {
            //m_timer += Time.deltaTime;
            if (m_beamParticle.isPlaying == false)
            {
                m_beamParticle.Play();
            }
            m_lineRenderer.enabled = true;                                      //lineRenderer ON
            m_lineRenderer.SetPosition(0, transform.position);                  //Set first position
            m_shotRay.origin = transform.position;                          
            m_shotRay.direction = transform.forward;
        
            if(Physics.Raycast(m_shotRay,out m_shotHit,m_range))                //Raycast.仮想のレーザーがcolliderに当たった時の処理
            {
                ProcessingRaycast(m_shotHit);　//　ここに書くとネストしすぎる為メソッドで別に呼ぶ
            }
            m_lineRenderer.SetPosition(1, m_shotRay.origin + m_shotRay.direction * m_range);


            if (m_lineRenderer.widthMultiplier <= 2f)                           //滑らかに起動させる。(少しずつレーザーの幅を広げる)
            {
                m_lineRenderer.widthMultiplier += Time.deltaTime / 0.5f;
                m_constant += Time.deltaTime / 0.5f;
            }
            else if (m_lineRenderer.widthMultiplier > 2f)
            {
                m_lineRenderer.widthMultiplier = 2f;
                m_constant = 2f;
            }

            m_parMain.startSize = new ParticleSystem.MinMaxCurve(m_constant);
        }
        else if(m_lineRenderer.enabled == true)
        {

            m_lineRenderer.SetPosition(0, transform.position);                  //Beamを滑らかに停止する迄(少しずつレーザーの幅を狭める)LineRendererを表示させた儘にする
            m_shotRay.origin = transform.position;
            m_shotRay.direction = transform.forward;
            m_lineRenderer.SetPosition(1, m_shotRay.origin + m_shotRay.direction * m_range);



            if (m_lineRenderer.widthMultiplier > 0f)
            {
                m_lineRenderer.widthMultiplier -= Time.deltaTime / 0.5f;
                m_constant -= Time.deltaTime / 0.5f;
            }
            else if (m_lineRenderer.widthMultiplier < 0f)
            {
                m_lineRenderer.widthMultiplier = 0f;
                m_constant = 0f;
            }

            m_parMain.startSize = new ParticleSystem.MinMaxCurve(m_constant);
        }

    }

    /// <summary>Shotの</summary>
    /// <param name="m_shotHit"></param>
    void ProcessingRaycast(RaycastHit m_shotHit)
    {
        m_oc = m_shotHit.collider.gameObject.GetComponent<ObjectController>();
        m_transformInfo = m_shotHit.transform;                              //Raycastと接触したコライダのトランスフォーム情報を格納

        switch (m_oc.m_type)
        {
            case Type.Planet:
                if (m_oc.m_myStatus.hitPoint > m_laserAtk) //HPが攻撃力より高い場合ダメージを与える。HP < Damageの場合破壊
                {
                    m_oc.m_myStatus.hitPoint -= m_laserAtk;
                    m_oc.GetAnimator().SetTrigger("Damage");

                }
                else
                {
                    Destroy(Instantiate(m_eAsteroid, m_transformInfo.transform.position, m_transformInfo.transform.rotation), m_eAsteroid.GetComponent<ParticleSystem>().main.duration);//爆破エフェクト
                    GameManager.AddScore(m_oc.m_myStatus.point);
                    Destroy(m_transformInfo.transform.gameObject);
                }
                break;
            case Type.Asteroid:
                if (m_oc.m_myStatus.hitPoint > m_laserAtk)
                {
                    m_oc.m_myStatus.hitPoint -= m_laserAtk;
                    m_oc.GetAnimator().SetTrigger("Damage");

                }
                else
                {
                    Destroy(Instantiate(m_eAsteroid, m_transformInfo.transform.position, m_transformInfo.transform.rotation), m_eAsteroid.GetComponent<ParticleSystem>().main.duration);//爆破エフェクト
                    GameManager.AddScore(m_oc.m_myStatus.point);
                    Destroy(m_transformInfo.transform.gameObject);
                }
                break;
            case Type.WeakEnemy:
            case Type.NormalEnemy:
            case Type.StrongEnemy:
                if (m_oc.m_myStatus.hitPoint > m_laserAtk)
                {
                    var enemyCon = m_transformInfo.GetComponent<EnemyController>(); //ダメージを受けた時色を変えるアニメーション
                    enemyCon.GetAnimator().SetTrigger("Damage");
                    m_oc.m_myStatus.hitPoint -= m_laserAtk;
                }
                else
                {
                    Destroy(Instantiate(m_eEnemy, m_transformInfo.transform.position, m_transformInfo.transform.rotation), m_eEnemy.GetComponent<ParticleSystem>().main.duration);//爆破エフェクト
                    GameManager.AddScore(m_oc.m_myStatus.point);
                    Destroy(m_transformInfo.transform.gameObject);
                }
                break;
            case Type.MotherShip:
            case Type.Other:
            default:
                break;
        }
    }

    /// <summary>Process to stop laser</summary>
    private void DisableEffect()
    {
        if(m_lineRenderer.widthMultiplier == 0f)
        {
            m_lineRenderer.enabled = false;
            m_beamParticle.Stop();
        }
    }

    /// <summary>レーザーの発動制御コルーチン</summary>
    IEnumerator LaserCoroutine()
	{
        //レーザーを発動してから(m_laserLife)秒経ったらレーザーを停止する。呼び出し元はUpdate()なのでコルーチンが処理中の間は重複実行させない様にフラグ管理している。
		if(m_isRunning)
        {
            yield break;
        }
        m_isRunning = true;
        m_laserFlag = true;
        yield return new WaitForSeconds(m_laserLife);
        m_laserFlag = false;
        yield return new WaitUntil(() => m_lineRenderer.widthMultiplier == 0f);
        DisableEffect();
        yield return new WaitForSeconds(1f);
        m_isRunning = false;
	}

    //Each initialization
    private void Start()
    {
        m_beamParticle = GetComponent<ParticleSystem>();
        m_lineRenderer = GetComponent<LineRenderer>();
        m_audioS = GetComponent<AudioSource>();
        m_audioS.volume = 0.2f;
        m_parMain = m_beamParticle.main;                      //This is Component of ParticleSystem.main of Beam
    }

    private void Update()
    {
        if (GameManager.m_startFlag)
        {
            StartCoroutine(LaserCoroutine());
        }

        Shot();

    }
}