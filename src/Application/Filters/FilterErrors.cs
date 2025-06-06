using Application.Results;

namespace Application.Filters;

/// <summary>
/// Provides static factory methods for creating <see cref="Error"/> objects specific to filtering and ordering operations.
/// </summary>
/// <remarks>
/// This class centralizes the definition of errors that can occur during the process of applying filters,
/// sorting, and pagination to queries, ensuring consistent error codes and messages.
/// </remarks>
public static class FilterErrors
{
	/// <summary>
	/// Creates an error indicating that a specified property name is not valid for ordering.
	/// </summary>
	/// <param name="propertyName">The name of the property that was attempted to be used for ordering.</param>
	/// <returns>An <see cref="Error"/> object with <see cref="ErrorType.BadRequest"/>.</returns>
	public static Error InvalidOrderField(string propertyName) =>
		Error.BadRequest($"Filter.{nameof(InvalidOrderField)}", "The field '{0}' is not a valid order field.", propertyName);

	/// <summary>
	/// Creates an error indicating that an ordering directive for a specified property has already been added.
	/// </summary>
	/// <param name="propertyName">The name of the property for which an ordering directive already exists.</param>
	/// <returns>An <see cref="Error"/> object with <see cref="ErrorType.BadRequest"/>.</returns>
	public static Error OrderFieldAlreadyExists(string propertyName) =>
		Error.BadRequest($"Filter.{nameof(OrderFieldAlreadyExists)}", "The field '{0}' is already added as an order field.", propertyName);

	/// <summary>
	/// Creates an error indicating that the input provided for ordering (e.g., field and type pairing) is incorrect.
	/// </summary>
	/// <returns>An <see cref="Error"/> object with <see cref="ErrorType.BadRequest"/>.</returns>
	public static Error InvalidOrderInput =>
		Error.BadRequest($"Filter.{nameof(InvalidOrderInput)}", "The order input is incorrect.");

	/// <summary>
	/// Creates an error indicating that the number of specified order fields does not match the number of specified order types.
	/// </summary>
	/// <returns>An <see cref="Error"/> object with <see cref="ErrorType.BadRequest"/>.</returns>
	public static Error OrderFieldCountMismatch =>
		Error.BadRequest($"Filter.{nameof(OrderFieldCountMismatch)}", "The number of order fields must match the number of order types.");

	/// <summary>
	/// Creates an error indicating an invalid sort order sequence, such as using 'ThenBy' or 'ThenByDescending' without a preceding 'OrderBy' or 'OrderByDescending'.
	/// </summary>
	/// <returns>An <see cref="Error"/> object with <see cref="ErrorType.BadRequest"/>.</returns>
	public static Error InvalidSortOrder =>
		Error.BadRequest($"Filter.{nameof(InvalidSortOrder)}", "The sort order is invalid. You must add at least one order field before using ThenBy or ThenByDescending.");
}