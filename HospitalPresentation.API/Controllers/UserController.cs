using HospitalBusinessLayer.Core;
using HospitalDataLayer.Infrastructure.DTOs.User;
using Microsoft.AspNetCore.Mvc;

namespace HospitalPresentation.API.Controllers
{
    [ApiController]
    [Route("api/users")]
    public class UserController : ControllerBase
    {
        private readonly clsUser _userService;
        private readonly ILogger<UserController> _logger;

        public UserController(clsUser userService, ILogger<UserController> logger)
        {
            _userService = userService;
            _logger = logger;
        }



        [HttpPost("CreateUser", Name = "CreateUser")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<int>> CreateUserAsync([FromBody] CreateUserDTO createUserDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest("Invalid data.");
                }

                int userId = await _userService.CreateUserAsync(createUserDto);

                if (userId == 0)
                {
                    return StatusCode(StatusCodes.Status400BadRequest);
                }

                return CreatedAtRoute("GetUserById", new { UserId = userId }, userId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while creating user.");
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while processing your request.");
            }
        }



        [HttpDelete("DeleteUser/{userId}", Name = "DeleteUser")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<bool>> DeleteUserAsync(int userId)
        {
            try
            {
                if (userId <= 0)
                {
                    return BadRequest("Invalid User ID.");
                }

                bool isDeleted = await _userService.DeleteUserAsync(userId);

                if (isDeleted)
                {
                    return Ok(true);
                }
                else
                {
                    return NotFound("User not found.");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while deleting user.");
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while processing your request.");
            }
        }



        [HttpGet("GetAllUsers", Name = "GetAllUsers")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<UserDTO>>> GetAllUsersAsync()
        {

            try
            {
                IEnumerable<UserDTO> UsersList = await _userService.GetAllUsersAsync();
                if (UsersList == null || !UsersList.Any())
                {
                    return NotFound("No Users Found!");
                }
                return Ok(UsersList);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching users.");
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while processing your request.");
            }

        }



        [HttpGet("GetUserById", Name = "GetUserById")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]

        public async Task<ActionResult<UserDTO>> GetUserByIdAsync(int userId)
        {

            try
            {
                UserDTO user = await _userService.GetUserByIdAsync(userId);
                if (user == null)
                {
                    return NotFound($"User with Id {userId} NOT FOUND!");
                }
                return Ok(user);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching user.");
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while processing your request.");
            }

        }



        [HttpGet("GetUserByUsername", Name = "GetUserByUsername")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]

        public async Task<ActionResult<UserDTO>> GetUserByUsernameAsync(string username)
        {

            try
            {
                UserDTO user = await _userService.GetUserByUsernameAsync(username);
                if (user == null)
                {
                    return NotFound($"User with username {username} NOT FOUND!");
                }
                return Ok(user);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching user.");
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while processing your request.");
            }

        }



        [HttpPut("UpdateUser/{userId}", Name = "UpdateUser")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<bool>> UpdateUserAsync(int userId, [FromBody] UpdateUserDTO UpdateUserDto)
        {
            try
            {
                if (userId <= 0 || !ModelState.IsValid)
                {
                    return BadRequest("Invalid User ID.");
                }

                bool isUpdated = await _userService.UpdateUserAsync(userId, UpdateUserDto);

                if (isUpdated)
                {
                    return Ok(true);
                }
                else
                {
                    return NotFound("User not found or could not be updated.");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while updating user.");
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while processing your request.");
            }
        }



    }
}