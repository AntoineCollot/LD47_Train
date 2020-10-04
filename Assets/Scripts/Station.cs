using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Station : MonoBehaviour
{
    [SerializeField] float maxTimeBetweenTrainEasy = 40;
    [SerializeField] float maxTimeBetweenTrainHard = 20;
    float remainingTime;
    [SerializeField] Image clockImage = null;
    Vector2Int coords;

    public float MaxTimeBetweenTrain { get => Mathf.Lerp(maxTimeBetweenTrainEasy, maxTimeBetweenTrainHard, GameManager.difficulty); }

    // Start is called before the first frame update
    void Start()
    {
        remainingTime = 1;

        SoundManager.Instance.Play(7);
    }

    private void Update()
    {
        remainingTime -= Time.deltaTime / MaxTimeBetweenTrain;

        clockImage.fillAmount = remainingTime;

        if (remainingTime<0)
            GameManager.Instance.GameOver("Station");
    }

    void OnNewLocomotivePosition(Vector2Int coords)
    {
        //Hack to compare the station position to train's
        if(this.coords == coords)
            remainingTime =1;
    }

    public void Init(RailWalker train, Vector2Int coords)
    {
        this.coords = coords;
        train.onNewCoords.AddListener(OnNewLocomotivePosition);
    }
}
