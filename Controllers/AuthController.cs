using backend.DTO;
using backend.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace backend.Controllers
{
    /// <summary>
    /// Controller responsável pela autenticação, registro e renovação de tokens de usuários.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        /// <summary>
        /// Autentica um usuário existente utilizando e-mail e senha.
        /// </summary>
        /// <param name="authDTO">DTO contendo e-mail e senha do usuário.</param>
        /// <returns>Token de acesso e refresh token em caso de sucesso.</returns>
        [HttpPost("login")]
        [Consumes("application/json")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(object), StatusCodes.Status200OK)]     
        [ProducesResponseType(StatusCodes.Status400BadRequest)]           
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]           
        public async Task<IActionResult> Login([FromBody] AuthDTO authDTO)
        {
            return await _authService.Authenticate(authDTO);
        }

        /// <summary>
        /// Registra um novo usuário no sistema.
        /// </summary>
        /// <param name="registerDTO">DTO contendo e-mail, senha e nome do novo usuário.</param>
        /// <returns>Detalhes do usuário criado e tokens de acesso em caso de sucesso.</returns>
        [HttpPost("register")]
        [Consumes("application/json")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(object), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Register([FromBody] RegisterDTO registerDTO)
        {
            return await _authService.Register(registerDTO);
        }

        /// <summary>
        /// Renova o token de acesso usando um refresh token válido.
        /// </summary>
        /// <param name="refreshDTO">DTO contendo o token de acesso expirado e o refresh token.</param>
        /// <returns>Novo token de acesso e refresh token em caso de sucesso.</returns>
        [HttpPost("refresh")]
        [Consumes("application/json")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(object), StatusCodes.Status200OK)]    
        [ProducesResponseType(StatusCodes.Status400BadRequest)]   
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> RefreshToken([FromBody] RefreshDTO refreshDTO)
        {
            return await _authService.Authenticate(refreshDTO);
        }
    }
}
