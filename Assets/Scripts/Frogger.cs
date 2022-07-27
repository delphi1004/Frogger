using System.Collections;
using UnityEngine;

public class Frogger : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    private Vector3 spawnPosition;
    private float farthestRow;

    public Sprite idleSprite;
    public Sprite leapSprite;
    public Sprite deadSprite;

    private void Awake() 
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        spawnPosition = transform.position;
    }

    private void Move(Vector3 direction)
    {
            Vector3 destination = transform.position + direction;

            Collider2D wall = Physics2D.OverlapBox(destination,Vector2.zero,0f, LayerMask.GetMask("Wall"));
            Collider2D platform = Physics2D.OverlapBox(destination,Vector2.zero,0f, LayerMask.GetMask("Platform"));
            Collider2D obstacle = Physics2D.OverlapBox(destination,Vector2.zero,0f, LayerMask.GetMask("Obstacle"));
            
            if(wall != null){
                return;
            }

            if(platform != null){
                transform.SetParent(platform.transform);
            }else{
                transform.SetParent(null);
            }

            if(obstacle != null && platform == null){
                transform.position = destination;
                Death();
            }else{
                if(destination.y > farthestRow){
                    farthestRow = destination.y;
                    FindObjectOfType<Manager>().AdvancedRow();
                }
                StartCoroutine(Leap(destination));
            }
    }

    private IEnumerator Leap(Vector3 destination)
    {
            Vector3 startPosition = transform.position;
            float ellipsedTime = 0f;
            float duration = 0.12f;

            spriteRenderer.sprite = leapSprite;

            while(ellipsedTime < duration){
                float t = ellipsedTime/duration;
                transform.position = Vector3.Lerp(startPosition,destination,t);
                ellipsedTime += Time.deltaTime;
                yield return null;
            }

            transform.position = destination;
            spriteRenderer.sprite = idleSprite;
    }

    private void OnTriggerEnter2D(Collider2D other) 
    {
        if(enabled && other.gameObject.layer == LayerMask.NameToLayer("Obstacle") && transform.parent == null) {
            Death();
        }   
    }

    private void Update() 
    {
        if(Input.GetKeyDown(KeyCode.UpArrow)) {
            transform.rotation = Quaternion.Euler(0f,0f,0f);
            Move(Vector3.up);
        } else if(Input.GetKeyDown(KeyCode.DownArrow)) {
            transform.rotation = Quaternion.Euler(0f,0f,180f);
            Move(Vector3.down);
        } else if(Input.GetKeyDown(KeyCode.LeftArrow)) {
            transform.rotation = Quaternion.Euler(0f,0f,90f);
            Move(Vector3.left);
        } else if(Input.GetKeyDown(KeyCode.RightArrow)) {
            transform.rotation = Quaternion.Euler(0f,0f,-90f);
            Move(Vector3.right);
        }
    }

    public void Death()
    {
        StopAllCoroutines();
        transform.rotation = Quaternion.identity;
        spriteRenderer.sprite = deadSprite;
        enabled = false;
        FindObjectOfType<Manager>().Died();
    }

    public void Respawn()
    {
        farthestRow = spawnPosition.y;
        StopAllCoroutines();
        transform.rotation = Quaternion.identity;
        transform.position = spawnPosition;
        spriteRenderer.sprite = idleSprite;
        gameObject.SetActive(true);
        enabled = true;
    }
}
