using H6_ChicBotique.DTOs;
using H6_ChicBotique.Services;
using Microsoft.AspNetCore.Mvc;

namespace H6_ChicBotique.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountInfoController : ControllerBase
    {
        private readonly IAccountInfoService _AccountInfoService; //Creating an instance of IAccountInfoService
        public AccountInfoController(IAccountInfoService AccountInfoService) //Dependency injection of AccountInforService
        {
            _AccountInfoService = AccountInfoService;
        }
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAll() //For getting all the entities of AccountInfo
        {
            try
            {
                List<AccountInfoResponse> AccountResponse = await _AccountInfoService.GetAll();
                if (AccountResponse == null)
                {
                    return Problem("Got no data, not even an empty list, this is unexpected");
                }
                if (AccountResponse.Count == 0)
                {
                    return NoContent();
                }
                return Ok(AccountResponse);
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);

            }


        }
      
        // https://localhost:5001/api/AccountInfo/derp
        [HttpGet("{userId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetUserGuid([FromRoute] int userId) //Getting UserGuidId by the userId
        {
            try
            {
                var userGuid = await _AccountInfoService.GetGuidIdByUserId(userId);
                //string guidToString = userGuid.ToString();
                if (userGuid == Guid.Empty)
                {
                    return NotFound();
                }

                return Ok(userGuid);
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }

        }



    }
}
