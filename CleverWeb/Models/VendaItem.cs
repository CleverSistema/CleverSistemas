﻿using System;
using System.Collections.Generic;

namespace CleverWeb.Models;

public partial class VendaItem
{
    public int Id { get; set; }

    public int? IdVenda { get; set; }

    public int? IdProduto { get; set; }

    public decimal? Quantidade { get; set; }

    public int? IdTabela { get; set; }

    public decimal? ValorProduto { get; set; }

    public decimal? ValorVenda { get; set; }

    public string? Informacao { get; set; }

    public int? TipoSaida { get; set; }

    public int? IdGrade { get; set; }

    public decimal? ValorCusto { get; set; }

    public decimal? QuantidadeEntregue { get; set; }

    public decimal? QuantidadeConferido { get; set; }
}
