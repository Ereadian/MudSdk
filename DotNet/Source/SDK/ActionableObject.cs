namespace Ereadian.MudSdk.Sdk
{
    public abstract class ActionableObject
    {
        public ActionableObject(ActionableObjectManager manager)
        {
            this.Manager = manager;
        }

        public ActionableObjectManager Manager { get; private set; }

        public ActionableObject Previous { get; set; }
        public ActionableObject Next { get; set; }

        public bool IsActive { get; set; }

        public void Remove()
        {
            this.Manager.Remove(this);
        }

        public void Add(ActionableObject item)
        {
            this.Manager.Add(item);
        }

        public abstract void Run();
    }
}
