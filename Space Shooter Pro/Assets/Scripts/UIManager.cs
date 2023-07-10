using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    // Start is called before the first frame update
    // handle to Text
    [SerializeField]
    private Text _scoreText;

    [SerializeField]
    private Image _livesImg;

    [SerializeField]
    private Sprite[] _liveSprties;

    [SerializeField]
    private Text _gameOverText;

    [SerializeField]
    private Text _restartLevelText;

    private GameManager _gameManager;

    void Start()
    {
        _scoreText.text = "Score: " + 0;
        _gameManager = GameObject.Find("Game_Manager").GetComponent<GameManager>();

        if(_gameManager == null)
        {
            Debug.LogError("Game manager is null");
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateScoreUI(int score)
    {
        _scoreText.text = "Score: " + score;
    }

    public void UpdateLives(int currentLives)
    {
        if (currentLives <=3 && currentLives>=0)
        {
            _livesImg.sprite = _liveSprties[currentLives]; 
        }
        if(currentLives < 1)
        {
            GameOverSequence();    
        }
    }

    void GameOverSequence()
    {
        _gameManager.GameOver();
        StartCoroutine(FlickerGameOver());
        _restartLevelText.gameObject.SetActive(true);
    }

    IEnumerator FlickerGameOver()
    {
        while(true)
        {
            yield return new WaitForSeconds(0.5f);
            if(_gameOverText.gameObject.activeSelf)
            {
                _gameOverText.gameObject.SetActive(false);
            }
            else
            {
                _gameOverText.gameObject.SetActive(true);
            }
        }
    }
}
