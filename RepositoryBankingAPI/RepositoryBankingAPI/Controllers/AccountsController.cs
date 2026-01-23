using System.Net;
using Microsoft.AspNetCore.Mvc;
using RepositoryBankingAPI.Models.DTOs.Requests;
using RepositoryBankingAPI.Models.DTOs.Responses;
using RepositoryBankingAPI.Services;

namespace RepositoryBankingAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountsController : ControllerBase
    {
        private readonly AccountsService _service;

        public AccountsController(AccountsService service)
        {
            _service = service;
        }

        /// <summary>
        /// Creates a new Account with the provided name given the name follows the required naming convention
        /// </summary>
        /// <param name="request">A record which contains a string Name for the new account</param>
        /// <returns>The information of the generated account</returns>
        [HttpPost]
        [ProducesResponseType(typeof(Account), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Account>> PostAccount(CreationRequest request)
        {
            var response = await _service.CreateAccount(request);
            if (!response.IsSuccess)
            {
                return StatusCode((int)response.StatusCode, response.ErrorMessage);
            }

            return Ok(response.Content);
        }
        
        /// <summary>
        /// Retrieves an account based off a given ID
        /// </summary>
        /// <param name="id">The unique identification for the requested account</param>
        /// <returns>The account information corresponding to the ID</returns>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(Account), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Account>> GetAccount(string id)
        {
            var response = await _service.GetAccount(id);
            if (!response.IsSuccess)
            {
                return StatusCode((int)response.StatusCode, response.ErrorMessage);
            }

            return Ok(response.Content);
        }
        
        /// <summary>
        /// Adds an entered money amount to a requested account
        /// </summary>
        /// <param name="id">The unique identification for the requested account</param>
        /// <param name="request">A record which contains a decimal Amount that will be deposited</param>
        /// <returns>The updated account details</returns>
        [HttpPost("{id}/deposits")]
        [ProducesResponseType(typeof(Account), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Account>> PostDeposit(string id, ChangeBalanceRequest request)
        {
            var response = await _service.Deposit(new AccountRequest<ChangeBalanceRequest>(id, request));
            if (!response.IsSuccess)
            {
                return StatusCode((int)response.StatusCode, response.ErrorMessage);
            }

            return Ok(response.Content);
        }
        
        /// <summary>
        /// Subtracts an entered money amount to a requested account
        /// </summary>
        /// <param name="id">The unique identification for the requested account</param>
        /// <param name="request">A record which contains a decimal Amount that will be withdrawn</param>
        /// <returns>The updated account details</returns>
        [HttpPost("{id}/withdraws")]
        [ProducesResponseType(typeof(Account), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Account>> PostWithdraw(string id, ChangeBalanceRequest request)
        {
            var response = await _service.Withdraw(new AccountRequest<ChangeBalanceRequest>(id, request));
            if (!response.IsSuccess)
            {
                return StatusCode((int)response.StatusCode, response.ErrorMessage);
            }

            return Ok(response.Content);
        }

        /// <summary>
        /// Takes money from one account and moves it to another
        /// </summary>
        /// <param name="request">A record that contains an id for the sending account, an id for the receiving
        /// account, along with the decimal amount that will be transferred</param>
        /// <returns>The sender's new account details</returns>
        [HttpPost("transfers")]
        [ProducesResponseType(typeof(Account), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Account>> PostTransfer(TransferRequest request)
        {
            var response = await _service.Transfer(request);
            if (!response.IsSuccess)
            {
                return StatusCode((int)response.StatusCode, response.ErrorMessage);
            }

            return Ok(response.Content);
        }
        
        /// <summary>
        /// Returns the balance of a provided account converted into the requested currencies
        /// </summary>
        /// <param name="id">The unique identification for the requested account</param>
        /// <param name="request">A record which contains a string holding comma separated currencies</param>
        /// <returns>A record containing a dictionary where the keys are the currency type and the values
        /// are the balance converted to the respective value</returns>
        [HttpGet("{id}/convert")]
        [ProducesResponseType(typeof(ConversionResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(string), StatusCodes.Status422UnprocessableEntity)]
        public async Task<ActionResult<ConversionResponse>> GetConversion(string id, ConversionRequest request)
        {
            var response = await _service.Convert(new AccountRequest<ConversionRequest>(id, request));
            if (!response.IsSuccess)
            {
                return StatusCode((int)response.StatusCode, response.ErrorMessage);
            }

            return Ok(response.Content);
        }
    }
}