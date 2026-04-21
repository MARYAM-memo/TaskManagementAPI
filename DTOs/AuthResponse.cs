using System;

namespace TodoAPI.DTOs;

public class AuthResponse
{

      public required string AccessToken { get; set; }
      public UserSimple? User { get; set; }
}

public class UserSimple
{
      public int Id { get; set; }
      public required string Email { get; set; }
      public required string Role { get; set; }
}