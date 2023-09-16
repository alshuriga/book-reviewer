namespace BookReviewer.IdentityProvider.DTO;

public record CreateUserDTO(string Email, string Password);

public record SignInUserDTO(string Email, string Password);

public record UserRoleManagementDTO(Guid UserId, List<string> Roles);

public record UserDTO(Guid UserId, string Email, IEnumerable<RoleDTO> Roles);

public record RoleDTO(Guid id, string Name);