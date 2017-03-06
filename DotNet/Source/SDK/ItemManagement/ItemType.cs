namespace Ereadian.MudSdk.Sdk.ItemManagement
{
    using System.Collections.Generic;
    using Ereadian.MudSdk.Sdk.ActionManagement;
    using Ereadian.MudSdk.Sdk.ContentManagement;

    public class ItemType
    {
        /// <summary>
        /// Category id
        /// </summary>
        public int CategoryId { get; set; }

        /// <summary>
        /// Gets item type name
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Gets item type id
        /// </summary>
        public int Id { get; private set; }

        /// <summary>
        /// Gets item descriptions
        /// </summary>
        public IReadOnlyList<Text> Descriptions { get; private set; }

        /// <summary>
        /// Item commands
        /// </summary>
        public IReadOnlyDictionary<string, Command> Commands { get; private set; }
    }
}
