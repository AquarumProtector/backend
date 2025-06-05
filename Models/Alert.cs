namespace backend.Models
{
    /// <summary>
    /// Representa um alerta genérico do sistema.
    /// </summary>
    public class Alert
    {
        /// <summary>
        /// Identificador único do alerta.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Título principal do alerta.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Descrição detalhada do alerta.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Caminho ou nome do ícone associado ao alerta.
        /// </summary>
        public string Icon { get; set; }

        /// <summary>
        /// Indica se o alerta está ativo (true) ou inativo (false).
        /// </summary>
        public bool IsActive { get; set; }
    }
}
