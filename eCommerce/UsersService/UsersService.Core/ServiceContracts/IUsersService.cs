using UserService.Core.DTO;

namespace UserService.Core.ServiceContracts;

public interface IUsersService
{
    Task<AuthenticationResponse?> Login(LoginRequest loginRequest);
    Task<AuthenticationResponse?> Register(RegisterRequest registerRequest);
    Task<UserDTO> GetUserByUserID(Guid userID);
}
