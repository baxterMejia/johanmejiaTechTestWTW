namespace Repository.Interfaces
{
    /// <summary>
    /// Repository for product CRUD operations.
    /// All operations return JSON as a string.
    /// </summary>
    public interface IProductRepository
    {
        /// <summary>
        /// Retrieves a product by its identifier.
        /// </summary>
        /// <param name="id">Product identifier.</param>
        /// <returns>JSON with the product data or an empty object if not found.</returns>
        Task<string> GetProductById(int id);

        /// <summary>
        /// Retrieves a paginated list of products, optionally filtered by name.
        /// The filter applies a "contains" match.
        /// </summary>
        /// <param name="name">Text to filter by name (optional).</param>
        /// <param name="page">Page number (1-based).</param>
        /// <param name="pageSize">Number of records per page.</param>
        /// <returns>JSON containing the paginated list of products and pagination metadata.</returns>
        Task<string> GetProductsPaged(string? name, int page, int pageSize);

        /// <summary>
        /// Creates a new product.
        /// The JSON must contain the required fields according to domain validations.
        /// </summary>
        /// <param name="productJson">JSON containing the product information.</param>
        /// <returns>JSON with the created product or validation errors.</returns>
        Task<string> CreateProduct(string productJson);

        /// <summary>
        /// Updates an existing product.
        /// </summary>
        /// <param name="id">Identifier of the product to update.</param>
        /// <param name="productJson">JSON with the updated product values.</param>
        /// <returns>JSON with the updated product or an error message.</returns>
        Task<string> UpdateProduct(int id, string productJson);

        /// <summary>
        /// Deletes a product.
        /// Must fail (409 or 400 depending on implementation) if the product is referenced in orders.
        /// </summary>
        /// <param name="id">Identifier of the product to delete.</param>
        /// <returns>JSON with the result of the operation.</returns>
        Task<string> DeleteProduct(int id);

    }
}
