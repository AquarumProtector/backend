// DTOs/WaterSourceDtos.cs
using System;
using backend.Models;

namespace backend.Controllers.DTOs
{
    public class WaterSourceCreateDto
    {
        /// <summary>
        /// Nome da fonte de água.
        /// </summary>
        public string Nome { get; set; }

        /// <summary>
        /// Descrição da fonte de água.
        /// </summary>
        public string Descricao { get; set; }

        /// <summary>
        /// Localização textual (endereço, ponto de referência etc.).
        /// </summary>
        public string Localizacao { get; set; }

        /// <summary>
        /// Latitude geográfica.
        /// </summary>
        public double Latitude { get; set; }

        /// <summary>
        /// Longitude geográfica.
        /// </summary>
        public double Longitude { get; set; }

        /// <summary>
        /// Tipo da fonte de água (Poço, Fonte, Rio, Lago, Reservatório).
        /// </summary>
        public WaterSourceType Type { get; set; }

        /// <summary>
        /// ID do usuário que criou a fonte de água.
        /// </summary>
        public int CreatedById { get; set; }

        /// <summary>
        /// Data da última inspeção prévia ao cadastro.
        /// </summary>
        public DateTime LastInspected { get; set; }

        /// <summary>
        /// Status inicial da fonte (Potável ou Contaminada).
        /// </summary>
        public WaterSourceStatus Status { get; set; }
    }

    public class WaterSourceUpdateDto
    {
        /// <summary>
        /// Nome atualizado da fonte de água.
        /// </summary>
        public string Nome { get; set; }

        /// <summary>
        /// Descrição atualizada da fonte de água.
        /// </summary>
        public string Descricao { get; set; }

        /// <summary>
        /// Localização textual atualizada.
        /// </summary>
        public string Localizacao { get; set; }

        /// <summary>
        /// Latitude atualizada.
        /// </summary>
        public double Latitude { get; set; }

        /// <summary>
        /// Longitude atualizada.
        /// </summary>
        public double Longitude { get; set; }

        /// <summary>
        /// Tipo de fonte atualizado (Poço, Fonte, Rio, Lago, Reservatório).
        /// </summary>
        public WaterSourceType Type { get; set; }

        /// <summary>
        /// Data da última inspeção atualizada.
        /// </summary>
        public DateTime LastInspected { get; set; }

        /// <summary>
        /// Novo status da fonte (Potável ou Contaminada).
        /// </summary>
        public WaterSourceStatus Status { get; set; }

        /// <summary>
        /// Define se a fonte continua ativa ou não.
        /// </summary>
        public bool IsActive { get; set; }

        /// <summary>
        /// Descrição do motivo/observação para criar o registro de atualização.
        /// </summary>
        public string UpdateDescricao { get; set; }
    }
}
