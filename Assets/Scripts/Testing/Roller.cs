using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Roller : MonoBehaviour
{
    Rigidbody _myBody;
    Vector3 torque;
    Vector3 initialPosition;
    private void Awake()
    {
        _myBody = GetComponent<Rigidbody>();
    }
    private void Start()
    {
        initialPosition = transform.position;
        Roll();
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Roll();
        }
    }
    private void Roll()
    {
        ResetDice();
        torque.x = Random.Range(0, 500);
        torque.y = Random.Range(0, 100);
        torque.z = Random.Range(0, 50);
        _myBody.AddTorque(torque);
    }
    void ResetDice()
    {
        transform.position = initialPosition;
    }
    void ReRoll()
    {
        transform.position = initialPosition;
        torque.x = Random.Range(0, 500);
        torque.y = Random.Range(0, 500);
        torque.z = Random.Range(0, 500);
        _myBody.AddTorque(torque);
    }
}
