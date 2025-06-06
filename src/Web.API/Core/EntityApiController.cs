using AutoMapper;
using Domain.Abstractions;

namespace Web.API.Core;

/// <summary>
/// Provides an abstract base class for API controllers designed to manage a specific domain entity type via a corresponding entity service.
/// </summary>
/// <typeparam name="TEntityService">
/// The type of the service used to perform operations on the managed entity.
/// This service type is typically an interface (e.g., `IUserService`) that defines methods for CRUD operations and other business logic related to an entity.
/// While not explicitly constrained here, <typeparamref name="TEntityService"/> usually works with entities implementing <see cref="IBaseEntity{TId}"/>.
/// </typeparam>
/// <remarks>
/// This class inherits from <see cref="ApiController"/>, thereby gaining common API controller functionalities like authorization.
/// It initializes protected fields for <see cref="AutoMapper.IMapper"/> and the <typeparamref name="TEntityService"/>,
/// making them readily available to derived controller classes for handling requests.
/// </remarks>
/// <param name="mapper">The AutoMapper instance, used for mapping between domain entities and DTOs (Data Transfer Objects).</param>
/// <param name="entityService">The service instance responsible for business logic and data operations for the managed entity type.</param>
public abstract class EntityApiController<TEntityService>(
	IMapper mapper,
	TEntityService entityService) : ApiController
{
	/// <summary>
	/// Gets the <see cref="AutoMapper.IMapper"/> instance used for object-to-object mapping (e.g., entities to DTOs).
	/// </summary>
	/// <value>The AutoMapper instance provided during construction.</value>
	protected readonly IMapper _mapper = mapper;

	/// <summary>
	/// Gets the instance of the entity service used for performing operations on the managed entity.
	/// </summary>
	/// <value>The entity service instance of type <typeparamref name="TEntityService"/> provided during construction.</value>
	protected readonly TEntityService _entityService = entityService;
}