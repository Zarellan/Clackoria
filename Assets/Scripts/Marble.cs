using UnityEngine;
using PrimeTween;
public class Marble : MonoBehaviour
{
    public string marbleName = "Marble";
    public float cooldownNumber = 0.1f;
    public System.Numerics.BigInteger value = 1;
    public float movementDelta = 0.1f;

    bool mouseClicked = false;
    bool isOverTheBall;
    bool beenOverTheBall = false;
    Rigidbody2D rb;
    
    Tween cooldown;

    Camera cam;

    [SerializeField] GameObject particleHolder;
    [SerializeField] ParticleSystem particle;
    [SerializeField] GameObject trail;
    
    SpriteRenderer spr;

    public Color colorSprite;

    public ClackerData clackDat;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        cam = Camera.main;
        spr = GetComponent<SpriteRenderer>();
        if (colorSprite == default)
            colorSprite = new Color(1,1,1,1);
        spr.color = colorSprite;
        trail.GetComponent<TrailRenderer>().startColor = new Color(spr.color.r,spr.color.g,spr.color.b);
        trail.GetComponent<TrailRenderer>().endColor = new Color(spr.color.r,spr.color.g,spr.color.b,0);
    }

    // Update is called once per frame
    void Update()
    {
        // if (Input.GetMouseButtonDown(0))
        // {
        //     mouseClicked = true;
        // }
        // else if (Input.GetMouseButtonUp(0))
        // {
        //     mouseClicked = false;
        // }

        if (isOverTheBall)
        {
            if (Input.GetMouseButtonDown(0))
            {
                mouseClicked = true;
            }
            MarbleManager.instance.ball_collided = true;
            MarbleManager.instance.SetMarbleName(marbleName);

            if (Input.GetKeyDown(KeyCode.E) && GameObject.FindGameObjectsWithTag("Marble").Length > 2)
            {
                MarbleManager.instance.money += clackDat.sellPrice;
                
                Destroy(gameObject);
            }
        }
        else
        {
            BallExit();
        }

        if (Input.GetMouseButtonUp(0) && mouseClicked)
        {
            mouseClicked = false;
        }
    }

    void FixedUpdate()
    {
        if (mouseClicked)
        {
            Vector3 mousePos = Input.mousePosition;
            mousePos.z = cam.WorldToScreenPoint(transform.position).z;
            mousePos = new Vector3(Mathf.Clamp(mousePos.x,0,Screen.width),Mathf.Clamp(mousePos.y,0,Screen.height),cam.WorldToScreenPoint(transform.position).z);
            Vector3 target = cam.ScreenToWorldPoint(mousePos);
            
            Vector3 direction = target - transform.position;
            direction = Vector3.ClampMagnitude(direction, MarbleManager.instance.distanceCap);
            rb.AddForce(direction * MarbleManager.instance.force);

        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Marble") && !cooldown.isAlive && rb.linearVelocity.magnitude > movementDelta)
        {
            // MarbleManager.instance.money += value * Mathf.Clamp(Mathf.FloorToInt(rb.linearVelocity.magnitude / 12f), 1, 3);
            float multiplier = Mathf.Clamp(1f + rb.linearVelocity.magnitude / 20f, 1f, 5f);
            MarbleManager.instance.AddValue(BigIntegerHandler.MultiplyFloat(value,multiplier));
            if (rb.linearVelocity.magnitude > movementDelta * 3)
                mouseClicked = false;
            Vector3 direction = collision.transform.position - transform.position;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            particleHolder.transform.rotation = Quaternion.Euler(0, 0, angle - 90);
            int count = Mathf.FloorToInt((rb.linearVelocity.magnitude + 1f) * 2f);
            particle.Emit(count);
            PlayBallSound(rb.linearVelocity.magnitude);
            NumberSpawner.instance.SpawnNumber(transform,
                    BigIntegerHandler.MultiplyFloat(value,multiplier) * MarbleManager.instance.multiply, multiplier);
            cooldown = Tween.Delay(cooldownNumber);
        }
    }

    void PlayBallSound(float magnitude)
    {
        float volume = Mathf.Clamp(magnitude / 10f, 0.25f, 1f);
        if (magnitude > 6)
        {
            GlobalAudio.instance.PlayAudioPitch("ball_hit_1", Random.Range(0.95f,1.1f), volume);
        }
        else if (magnitude > 3)
        {
            GlobalAudio.instance.PlayAudioPitchUnsame("ball_hit_2", Random.Range(0.95f,1.1f), volume);
        }
        else
        {
            GlobalAudio.instance.PlayAudioPitchUnsame("ball_hit_3", Random.Range(0.95f,1.1f), volume);
        }
    }

    void OnMouseEnter()
    {
        isOverTheBall = true;
        beenOverTheBall = true;
    }
    void OnMouseExit()
    {
        isOverTheBall = false;
    }

    void BallExit()
    {
        if (mouseClicked || !beenOverTheBall) return ;
        
        MarbleManager.instance.ball_collided = false;
        MarbleManager.instance.SetMarbleName("");
        beenOverTheBall = false;
    }
    bool MouseOutsideScreen()
    {
        Vector3 m = Input.mousePosition;

        return m.x < 0 || 
            m.y < 0 || 
            m.x > Screen.width || 
            m.y > Screen.height;
    }

    public void SetMarble(string name , ClackerData clack, System.Numerics.BigInteger val, Color col)
    {
        marbleName = name;
        value = val;
        clackDat = clack;
        colorSprite = col;
    }
    public void TweenMarble()
    {
        float circlePlace = Random.Range(0, Mathf.PI * 2);
        transform.position = new Vector2(0,0) + new Vector2(Mathf.Cos(circlePlace), Mathf.Sin(circlePlace)) * 10;
        Vector2 currentPos = transform.position;

        float clampedX = Mathf.Clamp(currentPos.x, MarbleManager.instance.clampPlace.minX, MarbleManager.instance.clampPlace.maxX);
        float clampedY = Mathf.Clamp(currentPos.y, MarbleManager.instance.clampPlace.minY, MarbleManager.instance.clampPlace.maxY);

        Vector2 targetPos = new Vector2(clampedX, clampedY);
        Vector2 startPos = transform.position;
        float duration = 0.4f;
        Tween.Position(transform, targetPos,duration,Ease.Linear).OnComplete(() =>
        {
            GetComponent<Rigidbody2D>().linearVelocity = (targetPos - startPos) / duration;
        });

    }
}
