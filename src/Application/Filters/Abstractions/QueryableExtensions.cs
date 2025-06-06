using System.Linq.Expressions;
using System.Reflection;

namespace Application.Filters.Abstractions;

/// <summary>
/// Provides internal extension methods for <see cref="IQueryable{T}"/> to support dynamic ordering based on property names.
/// </summary>
/// <remarks>
/// These extensions allow sorting operations (like OrderBy, ThenBy) to be applied using string representations of property names,
/// including support for nested properties via dot notation (e.g., "NavigationProperty.ChildProperty").
/// </remarks>
internal static class QueryableExtensions
{
	/// <summary>
	/// Applies a specified order to an <see cref="IQueryable{T}"/> source based on a property name and order type.
	/// </summary>
	/// <typeparam name="T">The type of the elements of <paramref name="source"/>.</typeparam>
	/// <param name="source">An <see cref="IQueryable{T}"/> to apply sorting to.</param>
	/// <param name="property">The name of the property to sort by. Supports dot notation for nested properties (e.g., "Customer.Address.City").</param>
	/// <param name="queryableOrderType">The type of ordering to apply (e.g., OrderBy, OrderByDescending, ThenBy, ThenByDescending).</param>
	/// <returns>An <see cref="IOrderedQueryable{T}"/> whose elements are sorted according to the specified property and order type.</returns>
	/// <exception cref="ArgumentException">Thrown if the <paramref name="property"/> does not exist on type <typeparamref name="T"/> or its nested path is invalid.</exception>
	public static IOrderedQueryable<T> NewOrder<T>(
		this IQueryable<T> source,
		string property,
		QueryableOrderType queryableOrderType)
		=> ApplyOrder(source, property, queryableOrderType.ToString());

	/// <summary>
	/// Sorts the elements of a sequence in ascending order according to a key specified by a property name.
	/// </summary>
	/// <typeparam name="T">The type of the elements of <paramref name="source"/>.</typeparam>
	/// <param name="source">An <see cref="IQueryable{T}"/> to sort.</param>
	/// <param name="property">The name of the property to sort by. Supports dot notation for nested properties.</param>
	/// <returns>An <see cref="IOrderedQueryable{T}"/> whose elements are sorted in ascending order according to the key.</returns>
	/// <seealso cref="NewOrder{T}(IQueryable{T}, string, QueryableOrderType)"/>
	public static IOrderedQueryable<T> OrderBy<T>(
		this IQueryable<T> source,
		string property)
		=> source.NewOrder(property, QueryableOrderType.OrderBy);

	/// <summary>
	/// Sorts the elements of a sequence in descending order according to a key specified by a property name.
	/// </summary>
	/// <typeparam name="T">The type of the elements of <paramref name="source"/>.</typeparam>
	/// <param name="source">An <see cref="IQueryable{T}"/> to sort.</param>
	/// <param name="property">The name of the property to sort by. Supports dot notation for nested properties.</param>
	/// <returns>An <see cref="IOrderedQueryable{T}"/> whose elements are sorted in descending order according to the key.</returns>
	/// <seealso cref="NewOrder{T}(IQueryable{T}, string, QueryableOrderType)"/>
	public static IOrderedQueryable<T> OrderByDescending<T>(
		this IQueryable<T> source,
		string property)
		=> source.NewOrder(property, QueryableOrderType.OrderByDescending);

	/// <summary>
	/// Performs a subsequent ordering of the elements in a sequence in ascending order according to a key specified by a property name.
	/// </summary>
	/// <typeparam name="T">The type of the elements of <paramref name="source"/>.</typeparam>
	/// <param name="source">An <see cref="IOrderedQueryable{T}"/> containing elements to sort.</param>
	/// <param name="property">The name of the property to sort by for the subsequent ordering. Supports dot notation for nested properties.</param>
	/// <returns>An <see cref="IOrderedQueryable{T}"/> whose elements are sorted in ascending order according to the key.</returns>
	/// <seealso cref="NewOrder{T}(IQueryable{T}, string, QueryableOrderType)"/>
	public static IOrderedQueryable<T> ThenBy<T>(
		this IOrderedQueryable<T> source,
		string property)
		=> source.NewOrder(property, QueryableOrderType.ThenBy);

	/// <summary>
	/// Performs a subsequent ordering of the elements in a sequence in descending order according to a key specified by a property name.
	/// </summary>
	/// <typeparam name="T">The type of the elements of <paramref name="source"/>.</typeparam>
	/// <param name="source">An <see cref="IOrderedQueryable{T}"/> containing elements to sort.</param>
	/// <param name="property">The name of the property to sort by for the subsequent ordering. Supports dot notation for nested properties.</param>
	/// <returns>An <see cref="IOrderedQueryable{T}"/> whose elements are sorted in descending order according to the key.</returns>
	/// <seealso cref="NewOrder{T}(IQueryable{T}, string, QueryableOrderType)"/>
	public static IOrderedQueryable<T> ThenByDescending<T>(
		this IOrderedQueryable<T> source,
		string property)
		=> source.NewOrder(property, QueryableOrderType.ThenByDescending);

	/// <summary>
	/// Dynamically applies an ordering method (e.g., "OrderBy", "ThenBy") to an <see cref="IQueryable{T}"/> source.
	/// </summary>
	/// <typeparam name="T">The type of the elements of <paramref name="source"/>.</typeparam>
	/// <param name="source">The <see cref="IQueryable{T}"/> to order.</param>
	/// <param name="property">The property path (dot-separated for nested properties) to order by.</param>
	/// <param name="methodName">The name of the <see cref="Queryable"/> ordering method (e.g., "OrderBy", "ThenByDescending").</param>
	/// <returns>An <see cref="IOrderedQueryable{T}"/> with the specified order applied.</returns>
	/// <exception cref="ArgumentException">Thrown if the property path is invalid or the property does not exist.</exception>
	private static IOrderedQueryable<T> ApplyOrder<T>(
		IQueryable<T> source,
		string property,
		string methodName)
	{
		string[] props = property.Split('.');
		Type type = typeof(T);
		ParameterExpression arg = Expression.Parameter(type, "x");
		Expression expr = arg;
		foreach (string prop in props)
		{
			// use reflection (not ComponentModel) to mirror LINQ
			PropertyInfo? pi = type.GetProperty(prop, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance) 
				?? throw new ArgumentException($"Property '{prop}' not found on type '{type.FullName}' in path '{property}'.");

			expr = Expression.Property(expr, pi);
			type = pi.PropertyType;
		}
		Type delegateType = typeof(Func<,>).MakeGenericType(typeof(T), type);
		LambdaExpression lambda = Expression.Lambda(delegateType, expr, arg);

		object? result = typeof(Queryable).GetMethods().Single(
				method => method.Name == methodName
						&& method.IsGenericMethodDefinition
						&& method.GetGenericArguments().Length == 2
						&& method.GetParameters().Length == 2)
				.MakeGenericMethod(typeof(T), type)
				.Invoke(null, [source, lambda]);

		if (result == null)
			throw new InvalidOperationException($"Failed to apply order '{methodName}' for property '{property}'.");

		return (IOrderedQueryable<T>)result;
	}
}