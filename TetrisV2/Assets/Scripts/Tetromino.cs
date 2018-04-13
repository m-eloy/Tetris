using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tetromino : MonoBehaviour {

    float fall = 0;
    public float fallSpeed = 1;

    public bool allowRotation = true;
    public bool limitRotation = false;

    private float continuousVerticalSpeed = 0.05f;
    private float continuousHorizontalSpeed = 0.1f;
    private float buttonDownWaitMax = 0.2f;

    private float verticalTimer = 0;
    private float horizontalTimer = 0;
    private float buttonDownWaitTimer = 0;

    private bool movedImmediateHorizontal = false;
    private bool moveImmediateVertical = false;

    public int individualScore = 100;
    private float individualScoreTime;

    private int touchSensitivityHorizontal = 8;
    private int touchSensitivityVertical = 4;
    Vector2 previousUnitPosition = Vector2.zero;
    Vector2 direction = Vector2.zero;
    bool moved = false;

    // Update is called once per frame
    void Update () {
        CheckUserInput();

        UpdateIndividualScore();

    }

    void UpdateIndividualScore()
    {
        if (individualScoreTime < 1)
        {
            individualScoreTime += Time.deltaTime;

        } else
        {
            individualScoreTime = 0;
            individualScore = Mathf.Max(individualScore - 10, 0);
        }
    }

    void CheckUserInput()
    {
#if UNITY_ANDROID
        if (Input.touchCount > 0)
        {
            Touch t = Input.GetTouch(0);

            if (t.phase == TouchPhase.Began)
            {
                previousUnitPosition = new Vector2(t.position.x, t.position.y);
            }
            else if (t.phase == TouchPhase.Moved)
            {
                Vector2 touchDeltaPosition = t.deltaPosition;
                direction = touchDeltaPosition.normalized;

                if (Mathf.Abs(t.position.x - previousUnitPosition.x) >= touchSensitivityHorizontal && direction.x < 0 && t.deltaPosition.y < 10)
                {
                    //- Move Left
                    MoveLeft();
                    previousUnitPosition = t.position;
                    moved = true;

                }
                else if (Mathf.Abs(t.position.x - previousUnitPosition.x) >= touchSensitivityHorizontal && direction.x > 0 && t.deltaPosition.y > -10 && t.deltaPosition.y < 10)
                {
                    //- Move Right
                    MoveRight();
                    previousUnitPosition = t.position;
                    moved = true;

                }
                else if (Mathf.Abs(t.position.y - previousUnitPosition.y) >= touchSensitivityVertical && direction.y < 0 && t.deltaPosition.x > -10 && t.deltaPosition.x < 10)
                {
                    //- Move Down
                    MoveDown();
                    previousUnitPosition = t.position;
                    moved = true;
                }
            }
            else if (t.phase == TouchPhase.Ended)
            {
                if (!moved && t.position.x > Screen.width / 4)
                {
                    Rotate();
                }
                moved = false;
            }
        }

        if(Time.time - fall >= fallSpeed)
        {
            MoveDown();
        }
#else

        if (Input.GetKeyUp(KeyCode.RightArrow) || Input.GetKeyUp(KeyCode.LeftArrow) || Input.GetKeyUp(KeyCode.DownArrow))
        {
            movedImmediateHorizontal = false;
            moveImmediateVertical = false;

            horizontalTimer = 0;
            verticalTimer = 0;
            buttonDownWaitTimer = 0;
        }

        if (Input.GetKey(KeyCode.RightArrow)){

            MoveRight();

        } else if (Input.GetKey(KeyCode.LeftArrow)){

            MoveLeft();

        } else if (Input.GetKeyDown(KeyCode.UpArrow)){
            Rotate();

        } else if (Input.GetKey(KeyCode.DownArrow) || Time.time - fall >= fallSpeed)
        {
            MoveDown();
        }

#endif
    }

    public void MoveRight()
    {
        if (movedImmediateHorizontal)
        {
            if (buttonDownWaitTimer < buttonDownWaitMax)
            {
                buttonDownWaitTimer += Time.deltaTime;
                return;
            }

            if (horizontalTimer < continuousHorizontalSpeed)
            {
                horizontalTimer += Time.deltaTime;
                return;
            }
        }

        if (!movedImmediateHorizontal)
        {
            movedImmediateHorizontal = true;
        }

        horizontalTimer = 0;

        transform.position += new Vector3(1, 0, 0);

        if (CheckIsValidPosition())
        {
            FindObjectOfType<Game>().UpdateGrid(this);

        }
        else
        {
            transform.position += new Vector3(-1, 0, 0);
        }
    }

    public void MoveLeft()
    {
        if (movedImmediateHorizontal)
        {
            if (buttonDownWaitTimer < buttonDownWaitMax)
            {
                buttonDownWaitTimer += Time.deltaTime;
                return;
            }

            if (horizontalTimer < continuousHorizontalSpeed)
            {
                horizontalTimer += Time.deltaTime;
                return;
            }
        }

        if (!movedImmediateHorizontal)
        {
            movedImmediateHorizontal = true;
        }

        horizontalTimer = 0;

        transform.position += new Vector3(-1, 0, 0);

        if (CheckIsValidPosition())
        {
            FindObjectOfType<Game>().UpdateGrid(this);
        }
        else
        {
            transform.position += new Vector3(1, 0, 0);
        }
    }

    public void MoveDown()
    {
        if (moveImmediateVertical)
        {
            if (buttonDownWaitTimer < buttonDownWaitMax)
            {
                buttonDownWaitTimer += Time.deltaTime;
                return;
            }

            if (verticalTimer < continuousVerticalSpeed)
            {
                verticalTimer += Time.deltaTime;
                return;
            }
        }

        if (!moveImmediateVertical)
        {
            moveImmediateVertical = true;
        }
        verticalTimer = 0;

        transform.position += new Vector3(0, -1, 0);

        if (CheckIsValidPosition())
        {
            FindObjectOfType<Game>().UpdateGrid(this);
        }
        else
        {
            transform.position += new Vector3(0, 1, 0);

            FindObjectOfType<Game>().DeleteRow();

            if (FindObjectOfType<Game>().CheckIsAboveGrid(this))
            {
                FindObjectOfType<Game>().GameOver();
            }

            FindObjectOfType<Game>().SpawnNextTetromino();

            Game.currentScore += individualScore;

            enabled = false;

        }

        fall = Time.time;
    }

    public void Rotate()
    {
        if (allowRotation)
        {
            if (limitRotation)
            {
                if (transform.rotation.eulerAngles.z >= 90)
                {
                    transform.Rotate(0, 0, -90);
                }
                else
                {
                    transform.Rotate(0, 0, 90);
                }

            }
            else
            {
                transform.Rotate(0, 0, 90);
            }

            if (CheckIsValidPosition())
            {
                FindObjectOfType<Game>().UpdateGrid(this);
            }
            else
            {
                if (limitRotation)
                {
                    if (transform.rotation.eulerAngles.z >= 90)
                    {
                        transform.Rotate(0, 0, -90);
                    }
                    else
                    {
                        transform.Rotate(0, 0, 90);
                    }
                }
                else
                {
                    transform.Rotate(0, 0, -90);
                }
            }
        }
    }


    bool CheckIsValidPosition()
    {
        foreach (Transform mino in transform)
        {
            Vector2 pos = FindObjectOfType<Game>().Round(mino.position);
            if (FindObjectOfType<Game>().CheckIsInsideGrid(pos) == false)
            {
                return false;
            }
            if(FindObjectOfType<Game>().GetTransformAtGridPosition(pos) != null && FindObjectOfType<Game>().GetTransformAtGridPosition(pos).parent != transform)
            {
                return false;
            }
        }
        return true;
    }
}
