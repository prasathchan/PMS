using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using PMS_Library.Models.DataModel;
using PMS_Library.Models.CustomModel;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Identity;
using NuGet.Protocol;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System.Security.Policy;

namespace PMS_Api.Controllers.Token
{
    [ApiController]
    public class TokenController(IConfiguration config, PMS_Entities context) : ControllerBase
    {
        private readonly IConfiguration configuration = config;
        private readonly PMS_Entities _context = context;


		//Get All User Details
		[ApiExplorerSettings(IgnoreApi = true)]
		[HttpGet]
		[Route("api/Auth/")]
		public async Task<IActionResult> GetAuthDetails()
		{
			var user = await _context.User_Auth.ToListAsync();
			if (user == null) { return NotFound(); }
			else { return Ok(user); }
		}


		//Get the User Details based on the Email ID
		[ApiExplorerSettings (IgnoreApi = true)]
        [HttpGet]
        [Route("api/Auth/{email}")]
        public async Task<IActionResult> GetAuthByEmail(string email)
        {
			var user = await _context.User_Auth.FirstOrDefaultAsync(x => x.EmailID.ToUpper().Equals(email.ToUpper()));
			if (user == null) { return NotFound(); }
			else { return Ok(user); }
		}



        //Create a Session Access Token for the User based on Authentication
        [AllowAnonymous]
        [Route("api/Auth")]
        [HttpPost]
        public async Task<IActionResult> PostLogin([FromBody]LoginModel lm)
        {
            try
            {  
               
                //Check if the User Exists in the User_Auth Table
                var user = await _context.User_Auth.FirstOrDefaultAsync(x => x.EmailID.Equals(lm.EmailID));

                if (user != null) //if the User Exists, then Auth Successful
                {
                    //Check if the Password is Valid
                    bool passVerify = Cryptography.VerifyHashedPassword(lm.Password, user.Password);

                    if (passVerify == false) { return BadRequest("Invalid Password"); }
                    else
                    {
                        //Check if the User Email has the Access Token in the User_Token Table
                        var result = await _context.User_Token.FirstOrDefaultAsync(x => x.EmailID.ToUpper().Equals(user.EmailID.ToUpper()));

                        if (result == null) //if he doesn't have the Access Token, then Create a New Record
                        {
                            //Create the Claims for the User
                            var authClaims = new List<Claim>
                            {
                            new(ClaimTypes.Name, user.EmailID),
                            new(ClaimTypes.Role, user.Role),
                            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                            };

                            //Create the Access Token and Refresh Token
                            var token = GenerateJSONAccessToken(authClaims);
                            var refreshToken = GenerateJSONRefreshToken();

                            //Fetch the Refresh Token Validity in Days
                            _ = int.TryParse(configuration["JWT:RefreshTokenValidityInDays"], out int refreshTokenValidityInDays);

                            //Create the Content for the Response
                            var _userData = (new User_Token
                            {
                                EmailID = lm.EmailID,
                                AccessToken = new JwtSecurityTokenHandler().WriteToken(token),
                                RefreshToken = refreshToken,
                                TokenExpiration = token.ValidTo,
                                Role = user.Role
                            });

                            _userData.AccessID = "AccessID_" + Guid.NewGuid();
                            _context.Entry(_userData).State = EntityState.Added;
                            await _context.SaveChangesAsync();
                            return Ok(_userData);
                        }
                        else if (result.TokenExpiration >= DateTime.UtcNow.AddMinutes(330)) //User already has an entry in User_Token context, then Check if the Access Token is Valid
                        {
                            //if the Access Token is Valid, then Return the Access Token
                            return Content(result.ToJson());
                        }
                        else
                        {
                            //if the Access Token is Invalid, then Revoke the Access Token
                            _context.User_Token.Where(x => x.AccessID.Equals(result.AccessID)).ExecuteDelete();
                            var deltoken = await _context.SaveChangesAsync();
							System.Threading.Thread.Sleep(3000);
							if (deltoken == 0) { return BadRequest("Error in Deleting the Token"); }
                            else
                            {
								result.AccessID = "AccessID_" + Guid.NewGuid();
								_context.Entry(result).State = EntityState.Added;
								await _context.SaveChangesAsync();
								return Ok(result);
							}                           
                        }
                    }
                }
                else
                {
                    return Unauthorized();
                }
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
                throw;
            }  
        }



        //Refresh the Access Token based on the Refresh Token
        [HttpPost]
        [Route("api/Auth/RefreshToken")]
        public async Task<IActionResult> RefreshToken([FromBody]RefreshTokenModel rtm)
        {
            //Check if the Email ID and Refresh Token is not Null
            if (rtm.AccessToken == null || rtm.RefreshToken == null)
            {
                return BadRequest("Invalid client request");
            }
            else
            {
                //Fetch the User Token Details from the User_Token Table using accessToken and Refresh Token
                var _userData = await _context.User_Token.FirstOrDefaultAsync(x => x.AccessToken  == rtm.AccessToken);
                if(_userData!=null) //If the user has a entry in the User_Token Table
                {
                    //Check if the Access Token is Valid or not
                    var principal = GetPrincipalFromExpiredToken(_userData.AccessToken);
                    if (principal is not null)
                    {
                        if (principal.Identity is not null)
                        {
                            //Get the Email ID from the Access Token
                            string? dbemail = principal.Identity.Name;

                            //Check if the Email ID is not Null and convert it to Upper Case
                            if (!string.IsNullOrEmpty(dbemail))
                            { dbemail = dbemail.ToUpper(); }
                            else //If the Email ID is Null, then return Bad Request
                            { return BadRequest("Invalid access token or refresh token"); }

                            //Validated the Email ID from the Access Token and the Email ID from the User_Token Table
                            bool isEmail = dbemail.Equals(_userData.EmailID.ToUpper(), StringComparison.InvariantCultureIgnoreCase);
                            if (isEmail == true)
                            {
                                if (_userData.RefreshToken != rtm.RefreshToken) //Check if the Refresh Token is Valid or not
                                {
                                    return BadRequest("Invalid refresh token");
                                }
                                else
                                {
                                    var newAccessToken = GenerateJSONAccessToken(principal.Claims.ToList());
                                    var newRefreshToken = GenerateJSONRefreshToken();

                                    _userData.AccessToken = new JwtSecurityTokenHandler().WriteToken(newAccessToken);
                                    _userData.RefreshToken = newRefreshToken;
                                    _userData.TokenExpiration = newAccessToken.ValidTo;
                                    _context.Entry(_userData).State = EntityState.Modified;
                                    await _context.SaveChangesAsync();
                                    return Ok(_userData.ToJson());
                                }
                            }
                            else
                            {
                                return BadRequest("Invalid User Access to the Token");
                            }
                        }
                        else
                        {
                            return BadRequest("Invalid/Expired access token or refresh token");

                        }
                    }
                    else
                    {
                        return BadRequest("Invalid access token or refresh token");
                    }
                }
                else { return BadRequest("Error in Fetching the User Token"); }
                
            }
        }




        //Revoke the Access Token based on the Email ID and Delete the Record from the User_Token Table
        [HttpPost]
        [Route("api/Auth/Revoke/{email}")]
        public async Task<IActionResult> RevokeByEmail(string email)
        {
            try
            {
				var _userdata = await _context.User_Token.FirstOrDefaultAsync(x => x.EmailID.ToUpper().Equals(email.ToUpper()));
				if (_userdata == null) { return NotFound("User Not Found"); }
				else
				{
					_context.Entry(_userdata).State = EntityState.Deleted;
					_context.User_Token.Where(x => x.AccessID.Equals(_userdata.AccessID)).ExecuteDelete();
					return NoContent();
				}
			}
            catch
            {
                throw;
            }
           
        }




        //Revoke all the Access Tokens and Delete the Records from the User_Token Table
        [HttpPost]
        [Route("api/Auth/RevokeAll")]
        public async Task<IActionResult> RevokeAll()
        {
            try
            {
                var users = await _context.User_Token.ToListAsync();
                foreach (var _userdata in users)
                {
                    _context.Entry(_userdata).State = EntityState.Deleted;
                    _context.User_Token.Where(x => x.AccessID.Equals(_userdata.AccessID)).ExecuteDelete();
                }
                return Ok();
            }
            catch(Exception e)
            {
                return BadRequest(e.Message);
                throw;
            }
            
        }


        //Register User to the database
        [AllowAnonymous]
        [Route("api/Auth/Register")]
        [HttpPost]
        public async Task<IActionResult> Register([FromBody]User_Auth user)
        {
            try
            {
                if (user != null)
                {
                    //Check if the User already exists in the User_Auth Table
                    var result = await _context.User_Auth.FirstOrDefaultAsync(x => x.EmailID.ToUpper().Equals(user.EmailID.ToUpper()));
                    if (result == null) //If the User doesn't exist, then Create a New Record
                    {
                        user.LoginId = "Login_" + Guid.NewGuid();
                        user.EmailID = user.EmailID.ToUpper();
                        user.Password = Cryptography.HashPassword(user.Password);
                        user.ClientName = user.ClientName.ToUpper();
                        _context.Entry(user).State = EntityState.Added;
                        await _context.SaveChangesAsync();
                        return Ok(user.ToJson());
                    }
                    else //If the User already exists, then return Bad Request
                    {
                        return BadRequest("User Already Exists");
                    }
                }
                else
                {
                    return BadRequest("User is Null");
                }
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
                throw;
            }
        }

        [HttpDelete]
        [Route("api/User/Delete/{email}")]
        public async Task<IActionResult> DeleteLogin(string email)
        {
			try
            {
				var user = await _context.User_Auth.FirstOrDefaultAsync(x => x.EmailID.ToUpper().Equals(email.ToUpper()));
				if (user == null) { return NotFound(); }
				else
                {
					_context.Entry(user).State = EntityState.Deleted;
					_context.User_Auth.Where(x => x.LoginId.Equals(user.LoginId)).ExecuteDelete();
					return NoContent();
				}
			}
			catch (Exception e)
            {
				return BadRequest(e.Message);
				throw;
			}
		}

        [HttpPut]
        [Route("api/User/Update/{email}")]
        public async Task<IActionResult> UpdateLogin(string email, [FromBody]User_Auth user)
        {
            try
            {
				if (user != null)
				{
					var result = await _context.User_Auth.FirstOrDefaultAsync(x => x.EmailID.ToUpper().Equals(email.ToUpper()));
					if (result == null) { return NotFound(); }
					else
					{
						result.ClientName = user.ClientName.ToUpper();
						result.Password = Cryptography.HashPassword(user.Password);
						result.Role = user.Role;
						_context.Entry(result).State = EntityState.Modified;
						await _context.SaveChangesAsync();
						return Ok(result.ToJson());
					}
				}
				else
				{
					return BadRequest("User is Null");
				}
			}
			catch (Exception e)
			{
				return BadRequest(e.Message);
				throw;
			}
		}

        //-----------------------------------------------------------Private Methods-----------------------------------------------------------//
        //Create Access Token Method
        private JwtSecurityToken GenerateJSONAccessToken(List<Claim> authClaims)
        {
            var key = configuration["Jwt:Key"];
            var token = new JwtSecurityToken();
            if(key != null)
            {
                _ = int.TryParse(configuration["JWT:TokenValidityInMinutes"], out int tokenValidityInMinutes);

                token = new JwtSecurityToken(
                    issuer: configuration["JWT:Issuer"],
                    audience: configuration["JWT:Audience"],
                    expires: DateTime.UtcNow.AddMinutes(tokenValidityInMinutes+330),
                    claims: authClaims,
                    signingCredentials: new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key)), SecurityAlgorithms.HmacSha512Signature)
                );
            }
            else
            {
                return token;
            }
            return token;
        }

        //Create Refresh Token Method
        private static string GenerateJSONRefreshToken()
        {
            var randomNumber = new byte[64];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }

        //Claim Principal Method
        private ClaimsPrincipal? GetPrincipalFromExpiredToken(string token)
        {
            var key = configuration["Jwt:Key"];
            if(key!=null)
            {
                var tokenValidationParameters = new TokenValidationParameters
                {
                    ValidateAudience = false,
                    ValidateIssuer = false,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key)),
                    ValidateLifetime = false
                };

                var tokenHandler = new JwtSecurityTokenHandler();
                var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out SecurityToken securityToken);
                if (securityToken is not JwtSecurityToken jwtSecurityToken || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha512Signature, StringComparison.InvariantCultureIgnoreCase))
                    throw new SecurityTokenException("Invalid token");
                return principal;
            }
            else
            {
                return null;
            }
        }

        //-----------------------------------------------------------Private Methods-----------------------------------------------------------//

    }
}
