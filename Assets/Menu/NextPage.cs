using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(Button))]

// Provides animation of next button on level selection pages
public class NextPage : MonoBehaviour
{
    private Animator animator;
    private Button nextButton;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
        animator = GetComponent<Animator>();
        nextButton = GetComponent<Button>();
        SetNextAnim();
    }

    // Update is called once per frame
    private void OnEnable()
    {
        SetNextAnim();  
    }

    private void SetNextAnim()
    {
        if (nextButton)
            animator.SetBool("IsActive", nextButton.interactable);  
    }
}