using LimitOrders15minGUI.Models;
using Prism.Mvvm;

namespace LimitOrders15minGUI.ViewModels
{
    class AppViewModel : BindableBase
    {
        // Models
        private AppModel appModel;
   
        #region AppViewModel
        public AppViewModel()
        {
            appModel = new AppModel
            {

            };
        }
        #endregion

        #region Getter/Setter for Application Model - Binding
        public AppModel AppModel
        {
            get { return appModel; }
            set { SetProperty(ref appModel, value); }
        }
        #endregion
    }
}
