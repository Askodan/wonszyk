using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class InstructionInfo
{
    public SteeringEnum steeringEnum;
    public GameObject instructionScreen;
}
public class ShowInstruction : MonoBehaviour
{
    [SerializeField] private List<InstructionInfo> Instructions = new List<InstructionInfo>();
    private void OnEnable()
    {
        if (WonszykPlayerData.Instance)
        {
            for (int i = 0; i < Instructions.Count; i++)
            {
                Instructions[i].instructionScreen.SetActive(Instructions[i].steeringEnum == WonszykPlayerData.Instance.WonszSteering);
            }
        }
    }
}
