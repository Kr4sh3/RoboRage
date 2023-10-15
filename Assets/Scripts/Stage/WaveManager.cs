using System.Collections;
using System.Collections.Generic;
using Packages.Rider.Editor.UnitTesting;
using UnityEngine;

public class WaveManager : MonoBehaviour
{

    private InnerStageController _currentStage;
    private InnerStageController _nextStage;

    [SerializeField] private GameObject Test;
    public float TimeToTransitionColor;
    public float TimeToTransitionTransform;

    public void SetNextStage(InnerStageController stageController)
    {
        if (_nextStage != null) //Get rid of ourselves if another stage is already set to transition onscreen 
        {
            Destroy(gameObject);
            return;
        }
        _nextStage = stageController;
        if (_currentStage != null) // Exit the current stage if there is one, otherwise immediately advance the stage
            _currentStage.ExitStage(InnerStageController.GetOppositeDirection(_nextStage.StartDirection));
        else
            AdvanceStage();
    }
    public void AdvanceStage()
    {
        if (_nextStage == null) //Dont advance if there is nothing to advance to
            return;
        _currentStage = _nextStage;
        _nextStage = null;
    }
    public InnerStageController GetCurrentStage()
    {
        return _currentStage;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            InnerStageController stage = Instantiate(Test, GameObject.FindGameObjectWithTag("Grid").transform).GetComponent<InnerStageController>();
            SetNextStage(stage);
        }
    }
}
