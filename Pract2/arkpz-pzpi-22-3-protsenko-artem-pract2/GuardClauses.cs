//Код до рефакторингу

public async Task<LoginUserResponse> Login(string email, string password)
{
    var loginUserResult = new LoginUserResponse();

    var candidate = await _userRepository.GetByEmail(email);

    if (candidate != null)
    {
        var passwordVerifyResult = _passwordHasher.Verify(password, candidate.Password);

        if (passwordVerifyResult)
        {
            var (accessToken, refreshToken) = await _tokenService.GenerateTokens(candidate);

            loginUserResult.AccessToken = accessToken;
            loginUserResult.RefreshToken = refreshToken;

            return loginUserResult;
        }
        else
        {
            throw new ApiException("Incorrect password", 400);
        }
    }
    else
    {
        throw new ApiException($"User with email {email} wasn't found", 404);
    }
}

// Код після рефакторингу

public async Task<LoginUserResponse> Login(string email, string password)
{
    var loginUserResult = new LoginUserResponse();

    var candidate = await _userRepository.GetByEmail(email);
    if (candidate == null)
    {
        throw new ApiException($"User with email {email} wasn't found", 404);
    }

    var passwordVerifyResult = _passwordHasher.Verify(password, candidate.Password);
    if (!passwordVerifyResult)
    {
        throw new ApiException("Incorrect password", 400);
    }

    var (accessToken, refreshToken) = await _tokenService.GenerateTokens(candidate);
    loginUserResult.AccessToken = accessToken;
    loginUserResult.RefreshToken = refreshToken;

    return loginUserResult;
}