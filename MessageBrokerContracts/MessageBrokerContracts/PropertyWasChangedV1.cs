using System;

namespace MessageBrokerContracts
{
    /// <summary>
    /// Message for broker when Property changed
    /// </summary>
    public class PropertyWasCreatedV1
    {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>
        public Guid Id { get; set; }
        /// <summary>
        /// Gets or sets the name of the property.
        /// </summary>
        /// <value>
        /// The name of the property.
        /// </value>
        public string PropertyName { get; set; }
        /// <summary>
        /// Gets or sets the created.
        /// </summary>
        /// <value>
        /// The created.
        /// </value>
        public DateTimeOffset Created { get; set; }
    }
}
