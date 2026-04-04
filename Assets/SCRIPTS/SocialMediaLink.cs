using UnityEngine;

public class SocialMediaLink : MonoBehaviour
{
    
    [SerializeField] private string facebookURL = "https://www.facebook.com/tuantinh.04";

    public void OpenFacebook()
    {
        
        if (!string.IsNullOrEmpty(facebookURL))
        {
            Application.OpenURL(facebookURL);
            Debug.Log("Đang mở Facebook: " + facebookURL);
        }
        else
        {
            Debug.LogWarning("Chưa nhập link Facebook trong Inspector!");
        }
    }
}