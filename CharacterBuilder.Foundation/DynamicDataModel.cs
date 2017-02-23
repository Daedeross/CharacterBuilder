namespace CharacterBuilder.Foundation
{
    using System.ComponentModel;
    using System.Dynamic;

    public class DynamicDataModel : DynamicObject, INotifyPropertyChanged
    {
        protected string mName = "";
        public virtual string Name
        {
            get { return mName; }
            set
            {
                mName = value;
                RaisePropertyChanged("Name");
            }
        }

        #region Interface Implementations

        protected void RaisePropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected void BubblePropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            PropertyChanged?.Invoke(this, e);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion
    }
}
