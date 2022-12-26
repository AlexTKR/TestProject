using UnityEngine;

namespace Controllers
{
    public interface IGetScreenBounds
    {
        ref Vector2 ScreenBounds { get; }
    }

    public class CameraController : ControllersBase , IGetScreenBounds
    {
        private Camera _mainCamera;
        private Vector2 _screenBounds;

        public ref Vector2 ScreenBounds => ref _screenBounds;
        
        public CameraController()
        {
            _mainCamera = Camera.main;
            
            var  screenBounds =  _mainCamera.ScreenToWorldPoint(new Vector3(Screen.width,  _mainCamera.transform.position.y,
                Screen.height));
            _screenBounds = new Vector2(Mathf.Abs(screenBounds.x), Mathf.Abs(screenBounds.z));
        }
    }
}