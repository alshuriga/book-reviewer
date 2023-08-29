namespace BookReviewer.InentityProvider.DTO;

public record CreateUserDTO(string Email, string Password);

public record SignInUserDTO(string Email, string Password);

public record UserDTO(Guid UserId, string Email, IEnumerable<string> Roles);