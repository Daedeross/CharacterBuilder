namespace CharacterBuilder.Foundation
{
    using System.ComponentModel;
    using System.Dynamic;

    public abstract class DynamicDataModel : DynamicObject, INotifyPropertyChanged
    {
        protected string _name = "";
        public virtual string Name
        {
            get { return _name; }
            set
            {
                if (!Equals(_name, value))
                {
                    _name = value;
                    RaisePropertyChanged(nameof(Name));
                }
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
