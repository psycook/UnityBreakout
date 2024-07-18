using UnityEngine;

public class LevelBehaviour : MonoBehaviour
{
    [SerializeField]
    private GameObject[] bricks = new GameObject[7];
    [SerializeField]
    private float brickWidth = 0.808f;
    [SerializeField]
    private float brickHeight = 0.404f;

    private int _levelCount { get; set; }
    private int _levelIndex;
    private int[,] _currentLevel;
    private int _levelBricksToBreakCount;

    private static int[,] level1 = new int[8, 20]
    {
       //1,2,3,4,5,6,7,8,9,0,1,2,3,4,5,6,7,8,9,0
        {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
        {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
        {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
        {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
        {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
        {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
        {2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2},
        {1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1}
    };

    private static int[,] level2 = new int[8, 20]
    {
       //1,2,3,4,5,6,7,8,9,0,1,2,3,4,5,6,7,8,9,0
        {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
        {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
        {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
        {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
        {2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2},
        {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
        {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
        {1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1}
    };

    private static int[,] level3 = new int[10, 20]
    {
       //1,2,3,4,5,6,7,8,9,0,1,2,3,4,5,6,7,8,9,0
        {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
        {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
        {0,1,1,1,1,1,1,1,1,0,0,1,1,1,1,1,1,1,1,0},
        {0,2,2,2,2,2,2,2,2,0,0,2,2,2,2,2,2,2,2,0},
        {0,3,3,3,3,3,3,3,3,0,0,3,3,3,3,3,3,3,3,0},
        {0,4,4,4,4,4,4,4,4,0,0,4,4,4,4,4,4,4,4,0},
        {0,4,4,4,4,4,4,4,4,0,0,4,4,4,4,4,4,4,4,0},
        {0,3,3,3,3,3,3,3,3,0,0,3,3,3,3,3,3,3,3,0},
        {0,2,2,2,2,2,2,2,2,0,0,2,2,2,2,2,2,2,2,0},
        {0,1,1,1,1,1,1,1,1,0,0,1,1,1,1,1,1,1,1,0},
    };

    private static int[,] level4 = new int[16, 20]
    {
        //1,2,3,4,5,6,7,8,9,0,1,2,3,4,5,6,7,8,9,0
        {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
        {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
        {0,0,0,0,0,0,0,2,0,0,0,0,2,0,0,0,0,0,0,0},
        {0,0,0,0,0,0,0,2,2,0,0,2,2,0,0,0,0,0,0,0},
        {0,0,0,0,0,0,0,0,2,0,0,2,0,0,0,0,0,0,0,0},
        {0,0,0,0,0,0,0,0,1,0,0,2,0,0,0,0,0,0,0,0},
        {0,0,0,0,0,0,0,1,1,1,1,1,1,0,0,0,0,0,0,0},
        {0,0,0,0,0,0,1,1,1,1,1,1,1,1,0,0,0,0,0,0},
        {0,0,0,0,0,1,1,1,6,1,1,6,1,1,1,0,0,0,0,0},
        {0,0,0,0,0,1,1,1,6,1,1,6,1,1,1,0,0,0,0,0},
        {0,0,0,0,0,1,1,1,1,1,1,1,1,1,1,0,0,0,0,0},
        {0,0,0,0,0,1,0,1,1,1,1,1,1,0,1,0,0,0,0,0},
        {0,0,0,0,0,1,0,1,0,0,0,0,1,0,1,0,0,0,0,0},
        {0,0,0,0,0,1,0,1,0,0,0,0,1,0,1,0,0,0,0,0},
        {0,0,0,0,0,0,0,1,1,0,0,1,1,0,0,0,0,0,0,0},
        {0,0,0,0,0,0,0,0,1,0,0,1,0,0,0,0,0,0,0,0}
    };

    private static int[,] level5 = new int[8, 20]
    {
       //1,2,3,4,5,6,7,8,9,0,1,2,3,4,5,6,7,8,9,0
        {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
        {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
        {6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6},
        {5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5},
        {4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4},
        {3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3},
        {2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2},
        {1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1}
    };


    private static int[,] levelBlank = new int[10, 20]
    {
       //1,2,3,4,5,6,7,8,9,0,1,2,3,4,5,6,7,8,9,0
        {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
        {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
        {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
        {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
        {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
        {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
        {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
        {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
        {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
        {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0}
    };

    private int[][,] levels = new int[][,] { level1, level2, level3, level4, level5 };

    LevelBehaviour()
    {
        newGame();
    }

    public void newGame()
    {
        _levelCount = levels.Length;
        _levelIndex = 0;
        _currentLevel = levels[_levelIndex];
    }

    public void setLevel(int newLevelIndex)
    {
        this._levelIndex = newLevelIndex;
    }

    public bool nextLevel()
    {
        if (++_levelIndex >= _levelCount)
        {
            // game finished
            return false;
        }
        _currentLevel = levels[_levelIndex];
        return true;
    }

    public int getDisplayLevel()
    {
        return _levelIndex + 1;
    }

    public bool startLevel()
    {
        int levelIndestrucableBrickCount = 0;
        for (int i = 0; i < _currentLevel.GetLength(0); i++) // Rows
        {
            for (int j = 0; j < _currentLevel.GetLength(1); j++) // Columns
            {
                int brickType = _currentLevel[i, j];
                if (brickType != 0)
                {
                    Vector2 position = new Vector2(j * brickWidth, i * -brickHeight);
                    GameObject brick = Instantiate(bricks[brickType - 1], position, Quaternion.identity);
                    brick.transform.SetParent(transform, false);
                    if (brickType == 7)
                    {
                        levelIndestrucableBrickCount++;
                    }
                }
            }
        }
        this._levelBricksToBreakCount = transform.childCount - levelIndestrucableBrickCount;
        return true;
    }

    public void decrementLevelBrickCount()
    {
        _levelBricksToBreakCount--;
    }

    public int getLevelBrickCount()
    {
        return _levelBricksToBreakCount;
    }
}