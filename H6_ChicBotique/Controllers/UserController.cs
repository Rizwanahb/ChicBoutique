
using H6_ChicBotique.DTOs;
using H6_ChicBotique.Helpers;
using H6_ChicBotique.Repositories;
using H6_ChicBotique.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace H6_ChicBotique.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : Controller
    {
        private readonly IUserService _userService;  ////Creating an instance of IUserService
        private readonly IUserRepository _userRepository;
        public UserController(IUserService userService)
        {
            _userService = userService;

        }

        //[Authorize(Role.Administrator)] // only admins are allowed entry to this endpoint
        [AllowAnonymous]
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAll()
        {
            //Getting all users
            try
            {

                List<UserResponse> users = await _userService.GetAll();  //Getting all users info from the UserService by using IUserService instance

                if (users == null)
                {
                    return Problem("Got no data, not even an empty list, this is unexpected");
                }

                if (users.Count == 0)
                {
                    return NoContent();
                }

                return Ok(users);
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }


        [AllowAnonymous]
        [HttpPost("register")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Register([FromBody] UserRegisterRequest newUser)
        {
            try
            {

                // The email is already in use; return an error response

                UserResponse user = await _userService.Register(newUser);
                return Ok(user);

            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }

        [AllowAnonymous]
        [HttpPost("guestRegister")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> guestRegister([FromBody] GuestRequest newGuest)
        {
            try
            {

                GuestResponse user = await _userService.Register_Guest(newGuest);
                return Ok(user);

            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }
        // Authenticate User
        [AllowAnonymous]
        [HttpPost("authenticate")]
            [ProducesResponseType(StatusCodes.Status200OK)]
            [ProducesResponseType(StatusCodes.Status400BadRequest)]
            [ProducesResponseType(StatusCodes.Status401Unauthorized)]
            [ProducesResponseType(StatusCodes.Status500InternalServerError)]
            public async Task<IActionResult> Authenticate(LoginRequest login)
            {
                try
                {
                    // Attempt to authenticate user credentials
                    LoginResponse response = await _userService.Authenticate(login);

                    if (response == null)
                    {
                        return Unauthorized(); // Return 401 if authentication fails
                    }

                    return Ok(response); // Return token if authentication is successful
                }
                catch (Exception ex)
                {
                    return Problem(ex.Message); // Return 500 if an unexpected error occurs
                }
            }

        // Get User ID by Email
        [AllowAnonymous]
        [HttpGet("{email}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> GetUserByEmail([FromRoute] string email)
        {
            try
            {
                // Retrieve user information by email
                UserResponse user = await _userService.GetIdByEmail(email);

                if (user == null)
                {
                    return NotFound(); // Return 404 if user not found
                }

                return Ok(user); // Return user information if found
            }
            catch (Exception ex)
            {
                return Problem(ex.Message); // Return 500 if an unexpected error occurs
            }
        }

        // Update User Information
        //[Authorize(Roles = "Member,Administrator")]
        [AllowAnonymous]
        [HttpPut("{userId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
            public async Task<IActionResult> Update([FromRoute] int userId, [FromBody] UserUpdateRequest updateUser)
            {
                try
                {
                    // Update user information
                    UserResponse user = await _userService.Update(userId, updateUser);

                    if (user == null)
                    {
                        return Problem("User record didn't get updated, something went wrong.");
                    }

                    return Ok(user); // Return updated user information
                }
                catch (Exception ex)
                {
                    return Problem(ex.Message); // Return 500 if an unexpected error occurs
                }
            }


            // Change User Password
            [AllowAnonymous]
            [HttpPost("changepassword")]
            [ProducesResponseType(StatusCodes.Status200OK)]
            [ProducesResponseType(StatusCodes.Status400BadRequest)]
            [ProducesResponseType(StatusCodes.Status401Unauthorized)]
            [ProducesResponseType(StatusCodes.Status500InternalServerError)]
            public async Task<IActionResult> ChangePassword([FromBody] PasswordEntityRequest passwordEntityRequest)
            {
                try
                {
                    // Validate model state
                    if (!ModelState.IsValid)
                    {
                        return BadRequest(ModelState);
                    }

                    // Change user password
                    bool response = await _userService.UpdatePassword(passwordEntityRequest);

                    if (!response)
                    {
                        return Problem("User record didn't get updated, something went wrong.");
                    }

                    return Ok("Password Changed Successfully");
                }
                catch (Exception ex)
                {
                    return Problem(ex.Message); // Return 500 if an unexpected error occurs
                }
            }

           
        }




    }

