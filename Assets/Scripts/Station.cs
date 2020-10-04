using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Station : MonoBehaviour
{
    [SerializeField] float maxTimeBetweenTrain = 30;
    float remainingTime;
    [SerializeField] Image clockImage = null;

    // Start is called before the first frame update
    void Start()
    {
        GameObject.FindGameObjectWithTag("Locomotive").GetComponent<RailWalkerLeader>().onNewCoords.AddListener(OnNewLocomotivePosition);
        remainingTime = 1;
    }

    private void Update()
    {
        remainingTime -= Time.deltaTime / maxTimeBetweenTrain;

        clockImage.fillAmount = remainingTime;

        if (remainingTime<0)
            GameManager.Instance.GameOver("Station");
    }

    void OnNewLocomotivePosition(Vector2Int coords)
    {
        //Hack to compare the station position to train's
        if(Vector2.Distance(TileMap.Instance.GetTile(coords).transform.position, transform.position)<0.1f)
            remainingTime =1;
    }
}
