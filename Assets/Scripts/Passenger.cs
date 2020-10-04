using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Passenger : MonoBehaviour
{
    Vector2Int coords;


    void OnNewTrainCoords(Vector2Int coords)
    {
        if(coords == this.coords)
        {
            Destroy(gameObject);
            PassengerManager.Instance.PassengerSaved(coords);
            PassengerSavedSmoke.Instance.Play(transform.position);
        }
    }

    public void Init(RailWalker train, Vector2Int coords)
    {
        train.onNewCoords.AddListener(OnNewTrainCoords);
        this.coords = coords;
    }
}
