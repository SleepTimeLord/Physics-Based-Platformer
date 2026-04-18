using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class WeaponRotation : MonoBehaviour
{
    public Transform weaponReferencePoint;
    public InputAction mousePosition;
    public CharacterController characterController;
    public SpriteRenderer weaponSprite;

    private Vector2 mousePos;
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    void OnEnable()
    {
        mousePosition.Enable();
    }
    void OnDisable()
    {
        mousePosition.Disable();
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (characterController.isFacingRight)
        {
            mousePos = mousePosition.ReadValue<Vector2>();
            Vector3 worldMousePos = Camera.main.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y, 0));
            Vector2 direction = worldMousePos - weaponReferencePoint.position;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            weaponReferencePoint.rotation = Quaternion.Euler(0, 0, angle);

            // 
            if (angle > 90 || angle < -90)
            {
                weaponReferencePoint.localScale = new Vector3(1, -1, 1);
            }
            else
            {
                weaponReferencePoint.localScale = new Vector3(1, 1, 1);
            }
        }
        else
        {
            mousePos = mousePosition.ReadValue<Vector2>();
            Vector3 worldMousePos = Camera.main.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y, 0));
            Vector2 direction = worldMousePos - weaponReferencePoint.position;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            weaponReferencePoint.rotation = Quaternion.Euler(0, 180, -angle);

            // 
            if (angle > 90 || angle < -90)
            {
                weaponReferencePoint.localScale = new Vector3(1, -1, 1);
            }
            else
            {
                weaponReferencePoint.localScale = new Vector3(1, 1, 1);
            }
        }


    }
}
