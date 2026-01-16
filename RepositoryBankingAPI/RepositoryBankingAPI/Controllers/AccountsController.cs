using Microsoft.AspNetCore.Mvc;
using RepositoryBankingAPI.Models;
using RepositoryBankingAPI.Models.Records;
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
            // try
            // {
            //     return Ok(await _service.CreateAccount(request));
            // }
            // catch(ArgumentException e)
            // {
            //     return BadRequest(e.Message);
            // }

            return null;
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
            // try
            // {
            //     return Ok(await _service.GetAccount(id));
            // }
            // catch (AccountNotFoundException e)
            // {
            //     return NotFound(e.Message);
            // }

            return null;
        }
        
        /// <summary>
        /// Adds an entered money amount to a requested account
        /// </summary>
        /// <param name="id">The unique identification for the requested account</param>
        /// <param name="request">A record which contains a decimal Amount that will be deposited</param>
        /// <returns>A response object containing the new account balance</returns>
        [HttpPost("{id}/deposits")]
        [ProducesResponseType(typeof(UpdateBalanceResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<UpdateBalanceResponse>> PostDeposit(string id, ChangeBalanceRequest request)
        {
            // try
            // {
            //     return Ok(await _service.Deposit(id, request));
            // }
            // catch (AccountNotFoundException e)
            // {
            //     return NotFound(e.Message);
            // }
            // catch (NegativeAmountException e)
            // {
            //     return BadRequest(e.Message);
            // }

            return null;
        }
        
        /// <summary>
        /// Subtracts an entered money amount to a requested account
        /// </summary>
        /// <param name="id">The unique identification for the requested account</param>
        /// <param name="request">A record which contains a decimal Amount that will be withdrawn</param>
        /// <returns>A response object containing the new account balance</returns>
        [HttpPost("{id}/withdraws")]
        [ProducesResponseType(typeof(UpdateBalanceResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<UpdateBalanceResponse>> PostWithdraw(string id, ChangeBalanceRequest request)
        {
            // try
            // {
            //     return Ok(await _service.Withdraw(id, request));
            // }
            // catch (AccountNotFoundException e)
            // {
            //     return NotFound(e.Message);
            // }
            // catch (Exception e) when (e is InsufficientFundsException or NegativeAmountException)
            // {
            //     return BadRequest(e.Message);
            // }

            return null;
        }

        /// <summary>
        /// Takes money from one account and moves it to another
        /// </summary>
        /// <param name="request">A record that contains an id for the sending account, an id for the receiving
        /// account, along with the decimal amount that will be transferred</param>
        /// <returns>A response object containing the receiver's new account balance</returns>
        [HttpPost("transfers")]
        [ProducesResponseType(typeof(UpdateBalanceResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<UpdateBalanceResponse>> PostTransfer(TransferRequest request)
        {
            // try
            // {
            //     return Ok(await _service.Transfer(request));
            // }
            // catch (AccountNotFoundException e)
            // {
            //     return NotFound(e.Message);
            // }
            // catch (Exception e) when (e is InsufficientFundsException or NegativeAmountException)
            // {
            //     return BadRequest(e.Message);
            // }

            return null;
        }
    }
}