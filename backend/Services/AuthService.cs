using backend.Data;
using backend.DTOs;
using backend.Models;
using Microsoft.EntityFrameworkCore;

namespace backend.Services;

public class AuthService
{
    private readonly AppDbContext _db;
    private readonly JwtService _jwt;

    public AuthService(AppDbContext db, JwtService jwt)
    {
        _db = db;
        _jwt = jwt;
    }

    public async Task<string> Register(RegisterRequest request)
    {
        if (await _db.Users.AnyAsync(u => u.Username == request.Username))
            throw new Exception("Username already taken");

        if (await _db.Users.AnyAsync(u => u.Email == request.Email))
            throw new Exception("Email already in use");

        var user = new User
        {
            Username = request.Username,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password),
            Email = request.Email,
            Afm = request.Afm,
            BusinessName = request.BusinessName
        };

        _db.Users.Add(user);
        await _db.SaveChangesAsync();

        return _jwt.GenerateToken(user.Username);
    }

    public async Task<string> Login(LoginRequest request)
    {
        var user = await _db.Users.FirstOrDefaultAsync(u => u.Username == request.Username);

        if (user == null || !BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
            throw new Exception("Invalid username or password");

        return _jwt.GenerateToken(user.Username);
    }
}
