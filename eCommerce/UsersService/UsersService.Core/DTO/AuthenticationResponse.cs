

namespace UserService.Core.DTO
{
    public record AuthenticationResponse(
  Guid UserID = default!,
  string? Email = default!,
  string? PersonName = default!,
  string? Gender = default!,
  string? Token = default!,
  bool Success = default!
  );
}
