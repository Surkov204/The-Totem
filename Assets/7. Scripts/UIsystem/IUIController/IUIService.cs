namespace JS 
{
    public interface IUIService
    {
        void Show<T>() where T : UIBase;
        void Hide<T>() where T : UIBase;
        bool IsVisible<T>() where T : UIBase;
        void HideAll();
        void Back();
    }
}
