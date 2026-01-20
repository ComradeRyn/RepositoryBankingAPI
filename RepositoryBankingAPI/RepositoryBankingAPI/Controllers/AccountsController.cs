using System.Net;
using Microsoft.AspNetCore.Mvc;
using RepositoryBankingAPI.Models;
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
        [ProducesResponseType(typeof(AccountResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<AccountResponse>> PostAccount(CreationRequest request)
        {
            var response = await _service.CreateAccount(request);

            if (!response.ValidateSuccessfulCode())
            {
                return NotFound(response.ErrorMessage);
            }

            return Ok(response.Response);
        }
        
        /// <summary>
        /// Retrieves an account based off a given ID
        /// </summary>
        /// <param name="id">The unique identification for the requested account</param>
        /// <returns>The account information corresponding to the id</returns>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(Account), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Account>> GetAccount(string id)
        {
            var response = await _service.GetAccount(id);

            if (!response.ValidateSuccessfulCode())
            {
                return NotFound(response.ErrorMessage);
            }

            return Ok(response.Response);
        }
        
        /// <summary>
        /// Adds an entered money amount to a requested account
        /// </summary>
        /// <param name="id">The unique identification for the requested account</param>
        /// <param name="request">A record which contains a decimal Amount that will be deposited</param>
        /// <returns>A response object containing the new account balance</returns>
        [HttpPost("{id}/deposits")]
        [ProducesResponseType(typeof(ChangeBalanceResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<ChangeBalanceResponse>> PostDeposit(string id, ChangeBalanceRequest request)
        {
            var response = await _service.Deposit(new ApiRequest<ChangeBalanceRequest>(id, request));

            if (!response.ValidateSuccessfulCode())
            {
                switch (response.StatusCode)
                {
                    case HttpStatusCode.NotFound:
                        return NotFound(response.ErrorMessage);
                    case HttpStatusCode.BadRequest:
                        return BadRequest(response.ErrorMessage);
                }
            }

            return Ok(response.Response);
        }
        
        /// <summary>
        /// Subtracts an entered money amount to a requested account
        /// </summary>
        /// <param name="id">The unique identification for the requested account</param>
        /// <param name="request">A record which contains a decimal Amount that will be withdrawn</param>
        /// <returns>A response object containing the new account balance</returns>
        [HttpPost("{id}/withdraws")]
        [ProducesResponseType(typeof(ChangeBalanceResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<ChangeBalanceResponse>> PostWithdraw(string id, ChangeBalanceRequest request)
        {
            var response = await _service.Withdraw(new ApiRequest<ChangeBalanceRequest>(id, request));

            if (!response.ValidateSuccessfulCode())
            {
                switch (response.StatusCode)
                {
                    case HttpStatusCode.NotFound:
                        return NotFound(response.ErrorMessage);
                    case HttpStatusCode.BadRequest:
                        return BadRequest(response.ErrorMessage);
                }
            }

            return Ok(response.Response);
        }

        /// <summary>
        /// Takes money from one account and moves it to another
        /// </summary>
        /// <param name="request">A record that contains an id for the sending account, an id for the receiving
        /// account, along with the decimal amount that will be transferred</param>
        /// <returns>A response object containing the receiver's new account balance</returns>
        [HttpPost("transfers")]
        [ProducesResponseType(typeof(ChangeBalanceResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<ChangeBalanceResponse>> PostTransfer(TransferRequest request)
        {
            var response = await _service.Transfer(request);
            
            if (!response.ValidateSuccessfulCode())
            {
                switch (response.StatusCode)
                {
                    case HttpStatusCode.NotFound:
                        return NotFound(response.ErrorMessage);
                    case HttpStatusCode.BadRequest:
                        return BadRequest(response.ErrorMessage);
                }
            }

            return Ok(response.Response);
        }
        // TODO: add xml comment explaining functionality
        [HttpGet("{id}/convert")]
        [ProducesResponseType(typeof(CurrencyApiResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<CurrencyApiResponse>> GetConversion(string id, ConversionRequest request)
        {
            var response = await _service.Convert(new ApiRequest<ConversionRequest>(id, request));

            if (!response.ValidateSuccessfulCode())
            {
                return BadRequest(response.ErrorMessage);
            }
            
            return Ok(response.Response);
        }
    }
}