using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBaseScript : MonoBehaviour {

    [Header("Configuration")]
    public int playerNumber;
    [Tooltip("Determina qual o player contrário")]
    public GameObject playerAgainst;
    [Tooltip("Determina qual direção do personagem")]
    public int lookDirection;
    [Tooltip("Determina qual animator do personagem")]
    public Animator anim;
    [Tooltip("Determina qual Rigidbody do personagem")]
    public Rigidbody rig;
    [Tooltip("Determina a velocidade de deslocamento atual")]
    public float speed;
    [Tooltip("Determina a velocidade de deslocamento durante animação")]
    public float animationWalkSpeed;
    [Tooltip("Determina a altura do pulo")]
    public float jumpForce;
    [Tooltip("Determina o tempo de invunerabilidade")]
    public float invunerableTime;
    [Tooltip("Determina o drag quando está no chão")]
    public float dragOnGround;
    [Tooltip("Determina a herança mais alta do objeto")]
    public GameObject father;
    [Tooltip("Determina a cordenada invertida para girar o personagem")]
    public Vector3 mirrorCordenate;
    public AudioClip groundAudio;
    float direction; // Determina a direção do Axis
    float cloackA, cloackB, cloackX, cloackY, cloackHit;

    [Header("Animation Controls")]
    [Tooltip("Determina se é possivel controlar o personagem durante a ação")]
    public bool canControl;
    [Tooltip("Determina que o personagem irá para durante a ação")]
    public bool walkFoward;
    [Tooltip("Determina que o personagem irá para trás durante a ação")]
    public bool walkBackward;
    [Tooltip("Determina que o personagem pulará durante a ação")]
    public bool jump;
    [Tooltip("Determina se é possivel deslocar-se com o personagem durante a ação")]
    public bool canWalk;
    [Tooltip("Determina a skin do personagem")]
    public GameObject mesh;
    [Tooltip("Determina a skin do personagem")]
    public int skin;
    [Tooltip("Determina a face do personagem")]
    public bool hurtFaceActivate;
    [Tooltip("Determina a textura com expressão padrão")]
    public Material [] normalFace;
    [Tooltip("Determina a textura com expressão padrão")]
    public Material[] hurtFace;
    [Tooltip("Determina as instancias a serem criadas")]
    public GameObject[] instances;
    [Tooltip("Determina a instancia a ser criadas")]
    public int instanceID;
    [Tooltip("Determina a criação da instância")]
    public bool instanciate;
    [Tooltip("Determina se está morto")]
    public bool death;
    [Tooltip("Determina se venceu")]
    public bool win;

    [Header("Stats Controls")]
    [Tooltip("Determina se está no chão)")]
    public bool grounded;
    [Tooltip("Determina se está pulando)")]
    public bool jumping;
    [Tooltip("Determina se é possivel ser atingido ou não(desligado pode ser atingido)")]
    public bool invunerable;

    private void Awake()
    {
        GameObject GC = GameObject.FindGameObjectWithTag("GameController");
        GC.SendMessage("PlayerReference" + playerNumber, gameObject);
        GameObject Cam = GameObject.FindGameObjectWithTag("MainCamera");
        Cam.SendMessage("GetTarget" + playerNumber, transform);
    }
    void Start () {
        if (transform.localScale.x > 0) lookDirection = 1; else lookDirection = -1;
        PlayerAgainst();
        mesh.GetComponent<Renderer>().material = normalFace[skin];
        grounded = false; // Retirar depois, somente para teste
	}
	
	void Update () {
        if (!win)
        {
            if (!invunerable)
            {
                Controls();
            }
            Walk();
        }
        AnimationControlls();
        anim.SetBool("Grounded", grounded);

        if (rig.velocity.y != 0 || grounded == false) { gameObject.layer = 9; }
        else gameObject.layer = 8;
        Physics.IgnoreLayerCollision(9, 9);
        Physics.IgnoreLayerCollision(9, 8);
    }
    void Walk()
    {
        direction = Input.GetAxisRaw("Horizontal" + playerNumber);

        if (canWalk)
        {
            rig.velocity = (new Vector3(speed * direction, rig.velocity.y, 0));
        }
        else if(!invunerable) rig.velocity = (new Vector3(0, rig.velocity.y, 0));

        if (direction > 0 && grounded && !invunerable)
        {
            anim.SetBool("Walk F", true);
        }
        else anim.SetBool("Walk F", false);
        if (direction < 0 && grounded && !invunerable)
        {
            anim.SetBool("Walk B", true);
        }
        else anim.SetBool("Walk B", false);

        if (invunerable)
        {
            cloackHit += Time.deltaTime;
            if(cloackHit >= invunerableTime)
            {
                invunerable = false;
                cloackHit = 0;
            }
        }

        if (grounded)
        {
            if(playerAgainst.transform.position.x > transform.position.x && transform.localScale* lookDirection != transform.localScale
                || playerAgainst.transform.position.x < transform.position.x && transform.localScale * lookDirection == transform.localScale)
            {
                transform.localScale = new Vector3(transform.localScale.x * mirrorCordenate.x,
                    transform.localScale.y * mirrorCordenate.y, transform.localScale.z * mirrorCordenate.z);
                lookDirection *= -1;
            }
        }

        if (grounded && rig.velocity.y != 0)
        {
            grounded = false;
            anim.SetBool("Grounded", false);
        }
    }
    void AnimationControlls()
    {
        if(hurtFaceActivate && mesh.GetComponent<Renderer>().material != hurtFace[skin])
        {
            mesh.GetComponent<Renderer>().material = hurtFace[skin];
        } else mesh.GetComponent<Renderer>().material = normalFace[skin];

        anim.SetFloat("Velocity Y", rig.velocity.y);
        if (walkFoward)
        {
            rig.velocity = (new Vector3(animationWalkSpeed * lookDirection, rig.velocity.y, 0));
        }
        if (walkBackward)
        {
            rig.velocity = (new Vector3(animationWalkSpeed * -1 * lookDirection, rig.velocity.y, 0));
        }
        if (instanciate)
        {
            instances[instanceID].SetActive(true);
        }
        if (anim.GetBool("A"))
        {
            cloackA += Time.deltaTime;
            if (cloackA >= 0.3f)
            {
                anim.SetBool("A", false);
                cloackA = 0;
            }
        }
        if (anim.GetBool("B"))
        {
            cloackB += Time.deltaTime;
            if (cloackB >= 0.3f)
            {
                anim.SetBool("B", false);
                cloackB = 0;
            }
        }
        if (anim.GetBool("X"))
        {
            cloackX += Time.deltaTime;
            if (cloackX >= 0.3f)
            {
                anim.SetBool("X", false);
                cloackX = 0;
            }
        }
        if (anim.GetBool("Y"))
        {
            cloackY += Time.deltaTime;
            if (cloackY >= 0.3f)
            {
                anim.SetBool("Y", false);
                cloackY = 0;
            }
        }
    }
    void Jump(float force)
    {
            jumping = true;
            rig.AddForce(new Vector3(0, force, 0));
        grounded = false;
            rig.drag = 0;
    }
    void Controls()
    {
        if (Input.GetButtonDown("A" + playerNumber))
        {
            anim.SetBool("A", true);
        }
        if (Input.GetButtonDown("B" + playerNumber))
        {
            anim.SetBool("B", true);
        }
        if (Input.GetButtonDown("X" + playerNumber))
        {
            anim.SetBool("X", true);
        }
        if (Input.GetButtonDown("Y" + playerNumber))
        {
            anim.SetBool("Y", true);
        }
        if (Input.GetAxis("LT" + playerNumber) < 0) anim.SetBool("LT", true);
        else anim.SetBool("LT", false);
        if (Input.GetAxis("Vertical" + playerNumber) >= 0.5f && grounded && !invunerable)
        {
            rig.drag = 0;
            anim.SetBool("Jump", true);
        }
    }
    void Hit(float damage)
    {
        if (!anim.GetBool("Walk B"))
        {
            GameObject Gc = GameObject.FindGameObjectWithTag("GameController");
            Gc.SendMessage("HitP" + playerNumber, damage);
            Gc.SendMessage("MpP" + playerNumber, damage / 2000);
            rig.drag = 0;
            invunerable = true;
        }
    }
    void Death()
    {
        if (!death)
        {
            death = true;
            anim.SetTrigger("Hit Jump");
            anim.SetBool("Death",true);
        }
    }
    void Win()
    {
        win = true;
    }
    void Special(int specialNumber)
    {
        GameObject GC = GameObject.FindGameObjectWithTag("GameController");
        GC.SendMessage("SP"+specialNumber, playerNumber);
    }
    void PlayAudio(AudioClip audio)
    {
        GetComponent<AudioSource>().PlayOneShot(audio);
    }
    void PlayerAgainst()
    {
        GameObject GC = GameObject.FindGameObjectWithTag("GameController");
        GC.SendMessage("PlayerReference" + playerNumber, gameObject);
        GameObject Cam = GameObject.FindGameObjectWithTag("MainCamera");
        Cam.SendMessage("GetTarget" + playerNumber, transform);
        if (playerNumber == 1) playerAgainst = GameControllerScript.player2;
        else playerAgainst = GameControllerScript.player1;
    }
    private void OnCollisionStay(Collision col)
    {
        if(!grounded && col.gameObject.tag == "Ground" && rig.velocity.y <= 0)
        {
            PlayAudio(groundAudio);
            anim.SetBool("Jump", false);
            jumping = false;
            grounded = true;
            rig.drag = dragOnGround;
            Debug.Log("Está no chão");
        }
    }
}
