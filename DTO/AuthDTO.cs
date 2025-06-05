namespace backend.DTO
{
    /// <summary>
    /// DTO para autenticação de usuário.
    /// </summary>
    public class AuthDTO
    {
        /// <summary>
        /// E-mail do usuário para autenticação.
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// Senha do usuário para autenticação.
        /// </summary>
        public string Password { get; set; }
    }

    /// <summary>
    /// DTO para registro de novo usuário.
    /// </summary>
    public class RegisterDTO
    {
        /// <summary>
        /// E-mail do novo usuário.
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// Senha escolhida pelo novo usuário.
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// Nome completo do novo usuário.
        /// </summary>
        public string Name { get; set; }
    }

    /// <summary>
    /// DTO para renovação de token de autenticação.
    /// </summary>
    public class RefreshDTO
    {
        /// <summary>
        /// Token de acesso atual.
        /// </summary>
        public string Token { get; set; }

        /// <summary>
        /// Refresh token utilizado para obter um novo token de acesso.
        /// </summary>
        public string RefreshToken { get; set; }
    }
}
