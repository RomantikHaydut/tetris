using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShapeController : MonoBehaviour
{
    public List<Transform> listPiece = new List<Transform>();

    bool isMoveableHorizontal = true;

    public bool canRotate;

    float delay;
    float timer;

    private void Start()
    {
        delay = GameManager.Instance.gameSpeed;
        timer = delay;
        // StartCoroutine(MoveDown());
       // StartCoroutine(MoveDownnn());
    }

    private void Update()
    {
        HorizontalMovement();
        MoveDownn();
    }

    /*IEnumerator MoveDownnn()
    {
        while (true)
        {
            var isMoveable = GameManager.Instance.isInside(NextPointVertical());
            if (isMoveable)
            {
                var delay = GameManager.Instance.gameSpeed;
                yield return new WaitForSecondsRealtime(delay);
                transform.position += new Vector3(0, -1, 0);

            }
            else
            {
                break;
            }

        }
    }*/

   /* IEnumerator MoveDown()
    {
        while (true)
        {
            var isMoveable = GameManager.Instance.isInside(NextPointVertical());

            if (isMoveable)
            {
                var delay = GameManager.Instance.gameSpeed;
                if (Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S))
                {
                    yield return new WaitForSecondsRealtime(delay/3);
                }
                else
                {
                    yield return new WaitForSecondsRealtime(delay);
                }
                
                transform.position += new Vector3(0, -1, 0);
            }
            else
            {
                FindObjectOfType<SpawnManager>().canSpawn = true;
                foreach (var piece in listPiece)
                {
                    int x = Mathf.RoundToInt(piece.transform.position.x);
                    int y = Mathf.RoundToInt(piece.transform.position.y);

                    GameManager.Instance.Grid[x, y] = true;
                    
                }
                GameManager.Instance.RemovePiecesController();
                SoundManager.Instance.HitSound();
                enabled = false;
                break;
            }
           
        }
    }*/

    void MoveDownn()
    {
        if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S))
        {
            for (int i = 0; i < 100; i++)
            {
                var isMoveable = GameManager.Instance.isInside(NextPointVertical());
                if (isMoveable)
                {
                    transform.position += new Vector3(0, -1, 0);
                }
                else
                {
                    FindObjectOfType<SpawnManager>().canSpawn = true;
                    foreach (var piece in listPiece)
                    {
                        int x = Mathf.RoundToInt(piece.transform.position.x);
                        int y = Mathf.RoundToInt(piece.transform.position.y);

                        GameManager.Instance.Grid[x, y] = true;

                    }
                    GameManager.Instance.RemovePiecesController();
                    SoundManager.Instance.HitSound();
                    enabled = false;
                    return;
                }
            }
        }
        else
        {
            var isMoveable = GameManager.Instance.isInside(NextPointVertical());
            if (isMoveable)
            {
               
                if (timer <= 0)
                {
                    transform.position += new Vector3(0, -1, 0);
                    timer = delay;
                }
                else
                {
                    timer -= Time.deltaTime;
                }
                
            }
            else
            {
                FindObjectOfType<SpawnManager>().canSpawn = true;
                foreach (var piece in listPiece)
                {
                    int x = Mathf.RoundToInt(piece.transform.position.x);
                    int y = Mathf.RoundToInt(piece.transform.position.y);

                    GameManager.Instance.Grid[x, y] = true;

                }
                GameManager.Instance.RemovePiecesController();
                SoundManager.Instance.HitSound();
                enabled = false;
                return;
            }
        }
        
    }

    private List<Vector2> NextPointVertical()
    {
        var result = new List<Vector2>();
        foreach (var piece in listPiece)
        {
            var position = piece.position;
            position.y--;
            result.Add(position);
        }

        return result;
    }

    private List<Vector2> NextPointHorizontal(int value)   
    {
        var result = new List<Vector2>();
        foreach (var piece in listPiece)
        {
            var position = piece.position;
            if (value == -1)
            {
                position.x--;
            }
            else if (value == 1)
            {
                position.x++;
            }

            result.Add(position);
        }

        return result;
    }

    private List<Vector2> NextPointRotate()
    {
        var result = new List<Vector2>();
        var pivot = transform.position;
        foreach (var piece in listPiece)
        {
            var position = piece.position;
            position -= pivot;
            position = new Vector3(position.y, -position.x, 0);
            position += pivot;
            result.Add(position);
        }

        return result;
    }
    void HorizontalMovement()
    {
        if (SpawnManager.gameOver)
        {
            enabled = false;
        }
        bool isPressedLeft = Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow);
        bool isPressedRight = Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow);
        bool isPressedUp=Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow);
        float moveValue;
        if (isPressedLeft)
        {
            moveValue = -1;
            isMoveableHorizontal = GameManager.Instance.isInside(NextPointHorizontal(-1));

        }
        else if (isPressedRight)
        {
            moveValue = 1;
            isMoveableHorizontal = GameManager.Instance.isInside(NextPointHorizontal(1));
        }
        else if (isPressedUp)
        {
            moveValue = 0;
            var isRotatable = GameManager.Instance.isInside(NextPointRotate());
            if (isRotatable)
            {
                Rotate();
            }
        }
        else
        {
            moveValue = 0;
        }

        if (isMoveableHorizontal)
        {
            transform.position += new Vector3(moveValue, 0, 0);
        }

    }
    void Rotate()
    {
        if (canRotate)
        {
            transform.eulerAngles -= new Vector3(0, 0, 90);
            SoundManager.Instance.RotateSound();

        }
    }


}
