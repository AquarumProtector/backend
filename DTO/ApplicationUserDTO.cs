namespace backend.DTO
{
    /// <summary>
    /// DTO para criação de um novo usuário.
    /// </summary>
    public class ApplicationUserDTOCreate
    {
        /// <summary>
        /// Nome completo do usuário.
        /// </summary>
        public required string Name { get; set; }

        /// <summary>
        /// E-mail do usuário. Deve ser único no sistema.
        /// </summary>
        public required string Email { get; set; }

        /// <summary>
        /// Senha para acesso do usuário.
        /// </summary>
        public required string Password { get; set; }
    }
}
