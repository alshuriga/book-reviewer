namespace BookReviewer.IdentityProvider.DTO;

public record CreateUserDTO(string Email, string Password);

public record SignInUserDTO(string Email, string Password);

public record UserRoleManagementDTO(Guid UserId);

public record UserDTO(Guid UserId, string Email, IEnumerable<string> Roles);