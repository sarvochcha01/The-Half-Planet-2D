using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.Video;

public class CinematicManager : MonoBehaviour
{
    [SerializeField] private double timer;
    [SerializeField] private double cinematicDuration;

    // Start is called before the first frame update
    void Start()
    {
        timer = 0;
        cinematicDuration = GetComponent<VideoPlayer>().clip.length;
        GetComponent<VideoPlayer>().targetTexture.Release();
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if (timer > cinematicDuration)
        {
            SceneManager.LoadScene("Menu");
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SceneManager.LoadScene("Menu");
            
        }
    }
}
