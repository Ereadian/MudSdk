namespace Ereadian.MudSdk.Sdk
{
    using System.Collections.Generic;

    public class ActionableObjectManager
    {
        /// <summary>
        /// first item
        /// </summary>
        private ActionableObject first = null; 

        /// <summary>
        /// last item
        /// </summary>
        private ActionableObject last = null;

        /// <summary>
        /// items to add
        /// </summary>
        private Queue<ActionableObject> itemsToAdd = new Queue<ActionableObject>(100);

        /// <summary>
        /// items to remove
        /// </summary>
        private Queue<ActionableObject> itemsToRemove = new Queue<ActionableObject>(100);

        /// <summary>
        /// run each item
        /// </summary>
        public void Run()
        {
            var item = this.first;
            while (item != null)
            {
                if (!item.IsActive)
                {
                    this.Remove(item);
                }
                else
                {
                    item.Run();
                }
            }

            lock (this)
            {
                while (this.itemsToRemove.Count > 0)
                {
                    item = this.itemsToRemove.Dequeue();
                    this.RemoveItem(item);
                }

                while (this.itemsToAdd.Count > 0)
                {
                    item = this.itemsToAdd.Dequeue();
                    this.AddItem(item);
                }
            }
        }

        /// <summary>
        /// Add item
        /// </summary>
        /// <param name="item">item to add</param>
        public void Add(ActionableObject item)
        {
            lock (this)
            {
                this.itemsToAdd.Enqueue(item);
            }
        }

        /// <summary>
        /// Remove item
        /// </summary>
        /// <param name="item">item to remove</param>
        public void Remove(ActionableObject item)
        {
            lock (this)
            {
                this.itemsToRemove.Enqueue(item);
            }
        }

        /// <summary>
        /// Remove item
        /// </summary>
        /// <param name="item">item to remove</param>
        private void RemoveItem(ActionableObject item)
        {
            if (item.IsActive)
            {
                if (item.Next == null)
                {
                    this.last = item.Previous;
                }
                else
                {
                    item.Next.Previous = item.Previous;
                }

                if (item.Previous == null)
                {
                    this.first = item.Next;
                }
                else
                {
                    item.Previous.Next = item.Next;
                }

                item.IsActive = false;
            }
        }

        /// <summary>
        /// Add item
        /// </summary>
        /// <param name="item">item to add</param>
        private void AddItem(ActionableObject item)
        {
            if (!item.IsActive)
            {
                if (this.last == null)
                {
                    this.first = item;
                    this.last = item;
                }
                else
                {
                    this.last.Next = item;
                }

                item.IsActive = true;
            }
        }
    }
}
