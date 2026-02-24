using AutoMapper;
using UserService.Core.DTO;
using UserService.Core.Entities;
using UserService.Core.RepositroyContracts;
using UserService.Core.ServiceContracts;

namespace UserService.Core.Services;

internal class UsersService : IUsersService
{
    private readonly IUsersRepository _usersRepository;
    private readonly IMapper _mapper;
    public UsersService(IUsersRepository usersRepository, IMapper mapper)
    {
        _usersRepository = usersRepository;
        _mapper = mapper;
    }

    public async Task<UserDTO> GetUserByUserID(Guid userID)
    {
        ApplicationUser? user = await _usersRepository.GetUserByUserID(userID);
        return _mapper.Map<UserDTO>(user);
    }


    public async Task<AuthenticationResponse?> Login(LoginRequest loginRequest)
    {
        ApplicationUser? user = await _usersRepository.GetUserByEmailAndPassword(loginRequest.Email, loginRequest.Password);

        if (user == null)
        {
            return null;
        }
        return _mapper.Map<AuthenticationResponse>(user)
            with{ Success = true, Token = "token"};
       
    }


    public async Task<AuthenticationResponse?> Register(RegisterRequest registerRequest)
    {
        //Create a new ApplicationUser object from RegisterRequest
        var user = _mapper.Map<ApplicationUser>(registerRequest);
        ApplicationUser? registeredUser = await _usersRepository.AddUser(user);
        if (registeredUser == null)
        {
            return null;
        }

        //Return success response
        return _mapper.Map<AuthenticationResponse>(registeredUser)
            with
        { Success = true, Token = "token" };
    }
}
