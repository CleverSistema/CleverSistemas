﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using Microsoft.Reporting.WinForms;
using System.Text;
using System.Windows.Forms;
using Sistema.BLL;
using Sistema.DTO;
using Sistema.UTIL;

namespace Sistema.UI
{
    public partial class UI_CPagar : Sistema.UI.UI_Modelo_Financeiro
    {
        public UI_CPagar()
        {
            InitializeComponent();
        }

        #region VARIAVEIS DE CLASSE
        BLL_PlanoConta BLL_PlanoConta;
        BLL_FluxoCaixa BLL_FluxoCaixa;
        BLL_CPagar BLL_CPagar;
        BLL_Pessoa BLL_Pessoa;
        #endregion

        #region VARIAVEIS DIVERSAS
        int obj;
        string Conta;

        DataRow DR;

        DataTable DT;

        int Parcela;

        DateTime ValidaData;

        bool Seleciona;
        #endregion

        #region ESTRUTURA
        DTO_PlanoConta PlanoConta;
        DTO_FluxoCaixa FluxoCaixa;
        DTO_CPagar CPagar;
        DTO_Pessoa Pessoa;
        DTO_Pagamento_Lanca Pagamento_Lanca;
        #endregion

        #region PROPRIEDADES
        public Tipo_Financeiro Tipo { get; set; }
        #endregion

        #region ROTINAS
        private void Inicia_Form()
        {
            this.Text = "CONTAS À PAGAR";

            bt_Imprime.Visible = false;
            bt_Visualiza.Visible = false;
            bt_Proximo.Visible = true;
            bt_Anterior.Visible = true;

            switch (Tipo)
            {
                case Tipo_Financeiro.Consulta:
                    tabctl.TabPages.Remove(TabPage1);
                    tabctl.SelectedTab = TabPage2;

                    bt_Exclui.Visible = false;
                    bt_Novo.Visible = false;
                    bt_Grava.Visible = false;
                    bt_Edita.Visible = false;

                    gb_Baixa.Visible = false;
                    break;

                case Tipo_Financeiro.Lancamento_Baixa:
                    DG.AutoGenerateColumns = false;
                    dg_Cheque.AutoGenerateColumns = false;
                    dg_ResumoPagto.AutoGenerateColumns = false;
                    tabctl.SelectedTab = TabPage2;
                    break;
            }
            Limpa_Campo();
        }

        private void Limpa_Campo()
        {
            util_dados.LimpaCampos(this, gb_Cadastro);
            mk_Conta.Text = "00000000";
            mk_ContaP.Text = "00000000";
            mk_DataInicial.Text = Convert.ToString(DateTime.Now.AddDays(-30));
            mk_DataFinal.Text = Convert.ToString(DateTime.Now.AddDays(30));
            mk_Emissao.Text = Convert.ToString(DateTime.Now);
            mk_Vencimento.Text = Convert.ToString(DateTime.Now);

            txt_QuantidadeParcela.Text = "1";
            txt_Parcela.Text = "1";
            txt_ParcelaFinal.Text = "1";
            txt_Controle.Text = "0";
            txt_Periodo.Text = "30";

            txt_Valor.Text = "0,00";
            txt_Acrescimo.Text = "0,00";
            txt_Desconto.Text = "0,00";
            txt_Total.Text = "0,00";

            cb_TipoPessoa.SelectedValue = 3;
            cb_Situacao.SelectedIndex = 0;
            cb_Periodo.SelectedIndex = 2;
            cb_TipoPessoa.Focus();
        }

        private void CarregaConta()
        {
            BLL_PlanoConta = new BLL_PlanoConta();
            PlanoConta = new DTO_PlanoConta();

            DataTable _DT = new DataTable();

            int Nivel = util_dados.VerificaNivel(mk_Conta.Text);

            Conta = util_dados.ConsultaCodigoPai(mk_Conta.Text, Nivel);
            //Nivel 1

            if (Conta.Length == 2)
            {
                PlanoConta.Nivel = Nivel;
                PlanoConta.CodigoPai = util_dados.ConsultaCodigoPai(mk_Conta.Text, Nivel);

                _DT = BLL_PlanoConta.Busca(PlanoConta);

                string Descricao = string.Empty;
                for (int i = 0; i <= _DT.Rows.Count - 1; i++)
                {
                    Descricao += _DT.Rows[i]["CodigoDescritivo"].ToString() + " - " + _DT.Rows[i]["DescricaoConta"].ToString() + Environment.NewLine;
                    if (i == _DT.Rows.Count - 1)
                        txt_ID_Conta.Text = _DT.Rows[i]["ID"].ToString();
                }
                txt_DescricaoConta.Text = Descricao;

                if (_DT.Rows.Count == 0)
                {
                    txt_ID_Conta.Text = string.Empty;
                    txt_DescricaoConta.Text = string.Empty;
                    return;
                }
                return;
            }
            //Nivel 2
            if (Conta.Length == 4)
            {
                PlanoConta.Nivel = Nivel;
                PlanoConta.CodigoPai = util_dados.ConsultaCodigoPai(mk_Conta.Text, Nivel);

                _DT = BLL_PlanoConta.Busca(PlanoConta);

                string Descricao = string.Empty;

                if (_DT.Rows.Count <= 1)
                {
                    txt_ID_Conta.Text = string.Empty;
                    txt_DescricaoConta.Text = string.Empty;
                    return;
                }
                else
                {
                    for (int i = 0; i <= _DT.Rows.Count - 1; i++)
                    {
                        Descricao += _DT.Rows[i]["CodigoDescritivo"].ToString() + " - " + _DT.Rows[i]["DescricaoConta"].ToString() + Environment.NewLine;
                        if (i == _DT.Rows.Count - 1)
                            txt_ID_Conta.Text = _DT.Rows[i]["ID"].ToString();
                    }

                    txt_DescricaoConta.Text = Descricao;
                }
                return;
            }
            //Nivel 3
            if (Conta.Length == 6)
            {
                PlanoConta.Nivel = Nivel;
                PlanoConta.CodigoPai = util_dados.ConsultaCodigoPai(mk_Conta.Text, Nivel);

                _DT = BLL_PlanoConta.Busca(PlanoConta);

                string Descricao = string.Empty;

                if (_DT.Rows.Count <= 2)
                {
                    txt_ID_Conta.Text = string.Empty;
                    txt_DescricaoConta.Text = string.Empty;
                    return;
                }
                else
                {
                    for (int i = 0; i <= _DT.Rows.Count - 1; i++)
                    {
                        Descricao += _DT.Rows[i]["CodigoDescritivo"].ToString() + " - " + _DT.Rows[i]["DescricaoConta"].ToString() + Environment.NewLine;
                        if (i == _DT.Rows.Count - 1)
                            txt_ID_Conta.Text = _DT.Rows[i]["ID"].ToString();
                    }
                    txt_DescricaoConta.Text = Descricao;
                }
                return;
            }
            //Nivel 4
            if (Conta.Length == 8)
            {
                PlanoConta.Nivel = Nivel;
                PlanoConta.CodigoPai = util_dados.ConsultaCodigoPai(mk_Conta.Text, Nivel);

                _DT = BLL_PlanoConta.Busca(PlanoConta);

                string Descricao = string.Empty;

                if (_DT.Rows.Count <= 3)
                {
                    txt_ID_Conta.Text = string.Empty;
                    txt_DescricaoConta.Text = string.Empty;
                    return;
                }
                else
                {
                    for (int i = 0; i <= _DT.Rows.Count - 1; i++)
                    {
                        Descricao += _DT.Rows[i]["CodigoDescritivo"].ToString() + " - " + _DT.Rows[i]["DescricaoConta"].ToString() + Environment.NewLine;
                        if (i == _DT.Rows.Count - 1)
                            txt_ID_Conta.Text = _DT.Rows[i]["ID"].ToString();
                    }
                    txt_DescricaoConta.Text = Descricao;
                }
                return;
            }

        }

        private void CarregaCB()
        {
            DataTable _DT = util_Param.Tipo_Pessoa();
            util_dados.CarregaCombo(_DT, "Descricao", "ID", cb_TipoPessoa);


            _DT = util_Param.Tipo_Pessoa();
            util_dados.CarregaCombo(_DT, "Descricao", "ID", cb_TipoPessoaP);
            cb_TipoPessoaP.SelectedIndex = -1;

            List<string> aux = new List<string> { "TODOS", "EM ABERTO", "PAGO" };

            _DT = util_dados.CarregaComboDinamico(0, aux);
            util_dados.CarregaCombo(_DT, "Descricao", "ID", cb_SituacaoP);
            cb_SituacaoP.SelectedIndex = 1;

            aux = new List<string> { "EM ABERTO", "PAGO" };

            _DT = util_dados.CarregaComboDinamico(1, aux);
            util_dados.CarregaCombo(_DT, "Descricao", "ID", cb_Situacao);
            cb_Situacao.SelectedIndex = 0;
        }

        private void Carrega_Lancamento()
        {
            string strIDCaixa = string.Empty;

            dg_ResumoPagto.DataSource = null;
            dg_Cheque.DataSource = null;

            BLL_FluxoCaixa = new BLL_FluxoCaixa();
            FluxoCaixa = new DTO_FluxoCaixa();

            FluxoCaixa.ID = 0;
            FluxoCaixa.ID_CPagar = Convert.ToInt32(txt_ID.Text);
            FluxoCaixa.ID_CReceber = 0;
            FluxoCaixa.Planejamento = true;

            //VERIFICA SE TEM ALGUM CHEQUE VINCULADO A CONTA
            DataTable _DT = new DataTable();
            _DT = BLL_FluxoCaixa.Busca_Controle(FluxoCaixa);

            if (_DT != null && _DT.Rows.Count > 0)
            {
                for (int i = 0; i <= _DT.Rows.Count - 1; i++)
                {
                    DR = _DT.Rows[i];
                    strIDCaixa = strIDCaixa + DR["ID_FluxoCaixa"];
                    if (i != _DT.Rows.Count - 1)
                        strIDCaixa += ", ";
                }

                _DT = new DataTable();
                FluxoCaixa.ListaID = strIDCaixa;
                FluxoCaixa.SomenteCheque = false;
                _DT = BLL_FluxoCaixa.Busca_Resumo(FluxoCaixa);
                if (_DT.Rows.Count > 0)
                {
                    lb_ResumoPagto.Visible = true;
                    dg_ResumoPagto.Visible = true;
                    dg_ResumoPagto.DataSource = _DT;
                }
                else
                {
                    lb_ResumoPagto.Visible = false;
                    dg_ResumoPagto.Visible = false;
                }

                _DT = new DataTable();
                FluxoCaixa.SomenteCheque = true;
                _DT = BLL_FluxoCaixa.Busca_Resumo(FluxoCaixa);

                if (_DT.Rows.Count > 0)
                {
                    lb_Cheque.Visible = true;
                    dg_Cheque.Visible = true;
                    dg_Cheque.DataSource = _DT;
                }
                else
                {
                    lb_Cheque.Visible = false;
                    dg_Cheque.Visible = false;
                }
            }
        }

        private void CarregaPessoa(int intTipoPessoa)
        {
            try
            {
                BLL_Pessoa = new BLL_Pessoa();
                Pessoa = new DTO_Pessoa();
                switch (intTipoPessoa)
                {
                    case 1:
                        Pessoa.TipoPessoa = Convert.ToInt32(cb_TipoPessoa.SelectedValue);
                        Pessoa.ID_Empresa = Parametro_Empresa.ID_Empresa_Ativa;

                        Pessoa.FiltraSituacao = true;
                        Pessoa.Situacao = true;

                        DT = new DataTable();
                        DT = BLL_Pessoa.Busca_Nome(Pessoa);
                        util_dados.CarregaCombo(DT, "Descricao", "ID", cb_ID_Pessoa);
                        cb_ID_Pessoa.SelectedIndex = -1;
                        break;
                    case 2:
                        Pessoa.TipoPessoa = Convert.ToInt32(cb_TipoPessoaP.SelectedValue);
                        Pessoa.ID_Empresa = Parametro_Empresa.ID_Empresa_Ativa;

                        DT = new DataTable();
                        DT = BLL_Pessoa.Busca_Nome(Pessoa);
                        util_dados.CarregaCombo(DT, "Descricao", "ID", cb_ID_PessoaP);
                        cb_ID_PessoaP.SelectedIndex = -1;
                        break;
                }
                //  if (util_dados.Verifica_int(txt_ID_Pessoa.Text) > 0)
                // cb_ID_Pessoa.SelectedValue = util_dados.Verifica_int(txt_ID_Pessoa.Text);
            }
            catch (Exception)
            {
            }
        }

        private void Config_DG()
        {
            DateTime DataDG;
            DateTime DataPC = DateTime.Now;
            for (int i = 0; i <= DG.Rows.Count - 1; i++)
            {
                DataDG = Convert.ToDateTime(DG.Rows[i].Cells["col_Vencimento"].Value);

                if (DataDG < DataPC)
                    DG.Rows[i].DefaultCellStyle.ForeColor = Color.Red;

                if (DataDG.ToShortDateString() == DataPC.ToShortDateString())
                    DG.Rows[i].DefaultCellStyle.ForeColor = Color.Blue;

                if (Convert.ToString(DG.Rows[i].Cells["col_Situacao"].Value) == "PAGO")
                    DG.Rows[i].DefaultCellStyle.ForeColor = Color.Gray;
            }
        }

        private void CalculaValor()
        {
            int aux = 0;

            double Total = 0;
            double Total_Baixa = 0;
            double Total_Desconto = 0;
            double Total_Acrescimo = 0;

            for (int i = 0; i <= DG.Rows.Count - 1; i++)
            {
                Total = Total + (Convert.ToDouble(DG.Rows[i].Cells["col_Valor"].Value) + Convert.ToDouble(DG.Rows[i].Cells["col_Acrescimo"].Value)) - Convert.ToDouble(DG.Rows[i].Cells["col_Desconto"].Value);
                if (Convert.ToBoolean(DG.Rows[i].Cells["col_Seleciona"].Value) == true)
                {
                    aux++;
                    Total_Baixa = Total_Baixa + Convert.ToDouble(DG.Rows[i].Cells["col_Valor"].Value);
                    Total_Desconto = Total_Desconto + Convert.ToDouble(DG.Rows[i].Cells["col_Desconto"].Value);
                    Total_Acrescimo = Total_Acrescimo + Convert.ToDouble(DG.Rows[i].Cells["col_Acrescimo"].Value);
                }
            }
            if (aux <= 1)
            {
                txt_AcrescimoBaixa.Enabled = true;
                txt_DescontoBaixa.Enabled = true;
                ck_PagamentoParcial.Enabled = true;
            }
            else
            {
                txt_AcrescimoBaixa.Enabled = false;
                txt_DescontoBaixa.Enabled = false;
                ck_PagamentoParcial.Checked = false;
                ck_PagamentoParcial.Enabled = false;
            }

            txt_AcrescimoBaixa.Text = util_dados.ConfigNumDecimal(Total_Acrescimo, 2);
            txt_DescontoBaixa.Text = util_dados.ConfigNumDecimal(Total_Desconto, 2);
            txt_ValorBaixa.Text = util_dados.ConfigNumDecimal((Total_Baixa + Total_Acrescimo) - Total_Desconto, 2);
            txt_Soma.Text = util_dados.ConfigNumDecimal(Total, 2);
        }

        private void Consulta_Pessoa()
        {
            UI_Pessoa_Consulta UI_Pessoa_Consulta = new UI_Pessoa_Consulta();
            UI_Pessoa_Consulta.OcultaNovoCadastro = true;
            UI_Pessoa_Consulta.TipoPessoa = Convert.ToInt32(cb_TipoPessoa.SelectedValue);

            UI_Pessoa_Consulta.ShowDialog();

            if (UI_Pessoa_Consulta.ID_Pessoa == 0)
                return;

            if (tabctl.SelectedTab == TabPage1)
            {
                cb_TipoPessoa.SelectedValue = UI_Pessoa_Consulta.TipoPessoa;

                CarregaPessoa(UI_Pessoa_Consulta.TipoPessoa);
                cb_ID_Pessoa.SelectedValue = UI_Pessoa_Consulta.ID_Pessoa;
                cb_ID_Pessoa.Focus();
            }

            if (tabctl.SelectedTab == TabPage2)
            {
                cb_TipoPessoaP.SelectedValue = UI_Pessoa_Consulta.TipoPessoa;

                CarregaPessoa(UI_Pessoa_Consulta.TipoPessoa);
                cb_ID_PessoaP.SelectedValue = UI_Pessoa_Consulta.ID_Pessoa;
                cb_ID_PessoaP.Focus();
            }
        }
        #endregion

        #region MODIFICADORES
        public override void Pesquisa()
        {
            try
            {
                BLL_CPagar = new BLL_CPagar();
                CPagar = new DTO_CPagar();

                CPagar.ID = util_dados.Verifica_int(txt_ID_P.Text);
                CPagar.ID_Empresa = Parametro_Empresa.ID_Empresa_Ativa;
                CPagar.GrupoConta = mk_ContaP.Text;
                CPagar.Documento = txt_DocumentoP.Text;
                CPagar.TipoPessoa = Convert.ToInt32(cb_TipoPessoaP.SelectedValue);
                CPagar.ID_Pessoa = Convert.ToInt32(cb_ID_PessoaP.SelectedValue);
                CPagar.Situacao = Convert.ToInt32(cb_SituacaoP.SelectedValue);
                CPagar.Ordena_Por = 2;

                if (cb_Periodo.SelectedIndex == 0)
                {
                    CPagar.Consulta_Baixa.Filtra = true;
                    CPagar.Consulta_Baixa.Inicial = Convert.ToDateTime(mk_DataInicial.Text);
                    CPagar.Consulta_Baixa.Final = Convert.ToDateTime(mk_DataFinal.Text);
                }

                if (cb_Periodo.SelectedIndex == 1)
                {
                    CPagar.Consulta_Emissao.Filtra = true;
                    CPagar.Consulta_Emissao.Inicial = Convert.ToDateTime(mk_DataInicial.Text);
                    CPagar.Consulta_Emissao.Final = Convert.ToDateTime(mk_DataFinal.Text);
                }

                if (cb_Periodo.SelectedIndex == 2)
                {
                    CPagar.Consulta_Vencimento.Filtra = true;
                    CPagar.Consulta_Vencimento.Inicial = Convert.ToDateTime(mk_DataInicial.Text);
                    CPagar.Consulta_Vencimento.Final = Convert.ToDateTime(mk_DataFinal.Text);
                }

                DataTable _DT = new DataTable();
                _DT = BLL_CPagar.Busca(CPagar);
                DG.DataSource = _DT;
                util_dados.CarregaCampo(this, _DT, gb_Cadastro);

                CalculaValor();
            }
            catch (Exception ex)
            {
                MessageBox.Show(util_msg.msg_Erro + ex.Message, this.Text);
            }
        }

        public override void Gravar()
        {
            try
            {
                #region LANÇAMENTO
                if (util_dados.Verifica_int(txt_ID.Text) == 0)
                {
                    BLL_CPagar = new BLL_CPagar();
                    CPagar = new DTO_CPagar();

                    CPagar.ID = util_dados.Verifica_int(txt_ID.Text);
                    CPagar.ID_Empresa = Parametro_Empresa.ID_Empresa_Ativa;
                    CPagar.ID_Conta = util_dados.Verifica_int(txt_ID_Conta.Text);
                    CPagar.Situacao = Convert.ToInt32(cb_Situacao.SelectedValue);
                    CPagar.Documento = txt_Documento.Text;
                    CPagar.Emissao = Convert.ToDateTime(mk_Emissao.Text);
                    CPagar.Vencimento = Convert.ToDateTime(mk_Vencimento.Text);
                    CPagar.TipoPessoa = Convert.ToInt32(cb_TipoPessoa.SelectedValue);
                    CPagar.ID_Pessoa = Convert.ToInt32(cb_ID_Pessoa.SelectedValue);
                    CPagar.Valor = Convert.ToDouble(txt_Valor.Text);
                    CPagar.QuantidadeParcela = Convert.ToInt32(txt_QuantidadeParcela.Text);
                    CPagar.Parcela = Convert.ToInt32(txt_Parcela.Text);
                    CPagar.Descricao = txt_Descricao.Text;
                    CPagar.Controle = Convert.ToInt32(txt_Controle.Text);
                    CPagar.Desconto = Convert.ToDouble(txt_Desconto.Text);
                    CPagar.Acrescimo = Convert.ToDouble(txt_Acrescimo.Text);

                    #region LANÇAMENTO ÚNICO
                    if (util_dados.Verifica_int(txt_QuantidadeParcela.Text) == 1)
                    {
                        CPagar.Vencimento = Convert.ToDateTime(mk_Vencimento.Text);
                        CPagar.Parcela = Convert.ToInt32(txt_Parcela.Text);
                        obj = BLL_CPagar.Grava(CPagar);
                    }
                    #endregion

                    #region LANÇAMENTO PARCELADO
                    DateTime Vencimento;
                    DateTime Vencimento_Temp;

                    if (util_dados.Verifica_int(txt_QuantidadeParcela.Text) > 1)
                    {
                        Parcela = int.Parse(txt_Parcela.Text);
                        Vencimento = Convert.ToDateTime(mk_Vencimento.Text);
                        Vencimento_Temp = Convert.ToDateTime(mk_Vencimento.Text);

                        if (Vencimento.Day == 31)
                            Vencimento = Vencimento.AddDays(-1);

                        int aux = 0;

                        for (int i = int.Parse(txt_Parcela.Text); i <= int.Parse(txt_ParcelaFinal.Text); i++)
                        {
                            Vencimento_Temp = Vencimento;

                            if (Vencimento_Temp.Day == 30 &&
                                Vencimento_Temp.Month == 2)
                                Vencimento_Temp = Vencimento_Temp.AddDays(-2);

                            if (Vencimento_Temp.Day == 29 &&
                                Vencimento_Temp.Month == 2)
                                Vencimento_Temp = Vencimento_Temp.AddDays(-1);

                            CPagar.Vencimento = Vencimento_Temp;
                            CPagar.Parcela = Parcela;
                            CPagar.ID = 0;
                            obj = BLL_CPagar.Grava(CPagar);

                            #region AJUSTA DATA DE VENCIMENTO E PARCELA
                            Parcela = Parcela + 1;

                            if (int.Parse(txt_Periodo.Text) == 30)
                                Vencimento = Vencimento.AddMonths(1);
                            else
                                Vencimento = Vencimento.AddDays(int.Parse(txt_Periodo.Text));
                            #endregion
                        }
                    }
                    #endregion

                    if (obj > 0)
                    {
                        Config(StatusForm.Consulta);
                        txt_ID.Text = obj.ToString();
                        MessageBox.Show(util_msg.msg_Grava, this.Text);
                        return;
                    }
                }
                #endregion

                #region ALTERAÇÃO
                else
                {
                    int aux = 0;
                    //VERIFICA SE TEM MAIS DE UM REGISTRO MARCADO
                    for (int i = 0; i <= DG.Rows.Count - 1; i++)
                        if (Convert.ToBoolean(DG.Rows[i].Cells["col_Seleciona"].Value) == true)
                            aux++;

                    if (aux == 0)
                    {
                        MessageBox.Show(util_msg.msg_RegistroNull, this.Text);
                        return;
                    }

                    DialogResult msgbox = MessageBox.Show(util_msg.msg_Confirma_Alteracao, this.Text, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
                    if (msgbox == DialogResult.No)
                        return;

                    //EXECUTA SE FOR ALTERAÇÃO
                    if (aux == 1)
                    {
                        BLL_CPagar = new BLL_CPagar();
                        CPagar = new DTO_CPagar();

                        CPagar.ID = util_dados.Verifica_int(txt_ID.Text);
                        CPagar.ID_Empresa = Parametro_Empresa.ID_Empresa_Ativa;
                        CPagar.ID_Conta = util_dados.Verifica_int(txt_ID_Conta.Text);
                        CPagar.GrupoConta = mk_Conta.Text;
                        CPagar.Situacao = Convert.ToInt32(cb_Situacao.SelectedValue);
                        CPagar.Documento = txt_Documento.Text;
                        CPagar.Emissao = Convert.ToDateTime(mk_Emissao.Text);
                        CPagar.Vencimento = Convert.ToDateTime(mk_Vencimento.Text);
                        CPagar.TipoPessoa = Convert.ToInt32(cb_TipoPessoa.SelectedValue);
                        CPagar.ID_Pessoa = Convert.ToInt32(cb_ID_Pessoa.SelectedValue);
                        CPagar.Valor = Convert.ToDouble(txt_Valor.Text);
                        CPagar.QuantidadeParcela = Convert.ToInt32(txt_QuantidadeParcela.Text);
                        CPagar.Parcela = Convert.ToInt32(txt_Parcela.Text);
                        CPagar.Descricao = txt_Descricao.Text;
                        CPagar.Controle = Convert.ToInt32(txt_Controle.Text);
                        CPagar.Desconto = Convert.ToDouble(txt_Desconto.Text);
                        CPagar.Acrescimo = Convert.ToDouble(txt_Acrescimo.Text);

                        obj = BLL_CPagar.Grava(CPagar);

                        if (obj > 0)
                        {
                            Config(StatusForm.Consulta);
                            txt_ID.Text = obj.ToString();
                            MessageBox.Show(util_msg.msg_Grava, this.Text);
                            return;
                        }
                    }
                    else if (aux > 1)
                    {
                        if (ck_AlteraGrupo.Checked == false &&
                            ck_AlteraPessoa.Checked == false &&
                            ck_AlteraValor.Checked == false &&
                            ck_Data.Checked == false)
                        {
                            MessageBox.Show("Selecione uma das opções para alteração!", this.Text);
                            return;
                        }

                        for (int i = 0; i <= DG.Rows.Count - 1; i++)
                            if (Convert.ToBoolean(DG.Rows[i].Cells["col_Seleciona"].Value) == true)
                            {
                                BLL_CPagar = new BLL_CPagar();
                                CPagar = new DTO_CPagar();

                                CPagar.ID = Convert.ToInt32(DG.Rows[i].Cells["col_ID"].Value);

                                if (ck_AlteraGrupo.Checked == true)
                                {
                                    CPagar.ID_Conta = util_dados.Verifica_int(txt_ID_Conta.Text);
                                    CPagar.Altera_Registro = 1;

                                    BLL_CPagar.Altera(CPagar);
                                }

                                if (ck_AlteraValor.Checked == true)
                                {
                                    CPagar.Valor = Convert.ToDouble(txt_Valor.Text);
                                    CPagar.Desconto = Convert.ToDouble(txt_Desconto.Text);
                                    CPagar.Acrescimo = Convert.ToDouble(txt_Acrescimo.Text);
                                    CPagar.Altera_Registro = 3;

                                    BLL_CPagar.Altera(CPagar);
                                }

                                if (ck_AlteraPessoa.Checked == true)
                                {
                                    CPagar.TipoPessoa = Convert.ToInt32(cb_TipoPessoa.SelectedValue);
                                    CPagar.ID_Pessoa = Convert.ToInt32(cb_ID_Pessoa.SelectedValue);
                                    CPagar.Altera_Registro = 2;

                                    BLL_CPagar.Altera(CPagar);
                                }

                                if (ck_Data.Checked == true)
                                {
                                    CPagar.Vencimento = Convert.ToDateTime(mk_Vencimento.Text);
                                    CPagar.Altera_Registro = 6;

                                    BLL_CPagar.Altera(CPagar);
                                }
                            }
                        Config(StatusForm.Consulta);
                        MessageBox.Show(util_msg.msg_Grava, this.Text);
                        return;
                    }
                }
                #endregion

                Pesquisa();
                CalculaValor();
            }
            catch (Exception ex)
            {
                MessageBox.Show(util_msg.msg_Erro + ex.Message, this.Text);
            }
        }

        public override void Excluir()
        {
            DialogResult msgbox = MessageBox.Show(util_msg.msg_ConfirmaExclusao, this.Text, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
            if (msgbox == DialogResult.No)
                return;

            int aux = 0;
            //VERIFICA SE TEM MAIS DE UM REGISTRO MARCADO
            for (int i = 0; i <= DG.Rows.Count - 1; i++)
                if (Convert.ToBoolean(DG.Rows[i].Cells["col_Seleciona"].Value) == true)
                    aux++;

            if (aux == 0)
            {
                MessageBox.Show(util_msg.msg_RegistroNull, this.Text);
                return;
            }

            try
            {
                for (int i = 0; i <= DG.Rows.Count - 1; i++)
                {
                    if (Convert.ToBoolean(DG.Rows[i].Cells["col_Seleciona"].Value) == true)
                    {
                        BLL_CPagar = new BLL_CPagar();
                        CPagar = new DTO_CPagar();

                        CPagar.ID = Convert.ToInt32(DG.Rows[i].Cells["col_ID"].Value);
                        BLL_CPagar.Exclui(CPagar);
                    }
                }
                MessageBox.Show(util_msg.msg_Exclui, this.Text);
                Pesquisa();
            }
            catch (Exception ex)
            {
                MessageBox.Show(util_msg.msg_Erro + ex.Message, this.Text);
            }
        }

        public override void ExibeRegistro()
        {
            tabctl.SelectedTab = TabPage1;
        }

        public override void Novo()
        {
            Limpa_Campo();
            tabctl.SelectedTab = TabPage1;
            cb_TipoPessoa.Focus();
        }
        #endregion

        #region COMBOBOX
        private void cb_TipoPessoa_SelectedValueChanged(object sender, EventArgs e)
        {
            CarregaPessoa(1);
        }

        private void cb_TipoPessoaP_SelectedValueChanged(object sender, EventArgs e)
        {
            CarregaPessoa(2);
        }

        private void cb_TipoPessoaP_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete)
                cb_TipoPessoaP.SelectedIndex = -1;
        }

        private void cb_ID_PessoaP_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete)
                cb_ID_PessoaP.SelectedIndex = -1;
        }
        #endregion

        #region FORM
        private void UI_CPagar_Load(object sender, EventArgs e)
        {
            CarregaCB();
            Inicia_Form();
        }

        private void UI_CPagar_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (txt_Acrescimo.Focused == true ||
               txt_Desconto.Focused == true ||
               txt_Valor.Focused ||
               txt_AcrescimoBaixa.Focused == true ||
               txt_DescontoBaixa.Focused == true ||
               txt_ValorPagoParcial.Focused == true)
            {
                short KeyAscii = (short)e.KeyChar;
                KeyAscii = Convert.ToInt16(util_dados.NumDecimal(KeyAscii));
                if (KeyAscii == 0)
                {
                    e.Handled = true;
                }
            }

            if (txt_QuantidadeParcela.Focused == true ||
                txt_Parcela.Focused == true ||
                txt_Periodo.Focused == true)
            {
                short KeyAscii = (short)e.KeyChar;
                KeyAscii = Convert.ToInt16(util_dados.NumInteiro(KeyAscii));
                if (KeyAscii == 0)
                {
                    e.Handled = true;
                }
            }
        }

        private void UI_CPagar_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F5 &&
                tabctl.SelectedTab == TabPage2)
            {
                Pesquisa();
                Status = StatusForm.Consulta;
                Config_Botao();
            }

            if (e.KeyCode == Keys.F7)
                Consulta_Pessoa();
        }
        #endregion

        #region TEXTBOX
        private void txt_Acrescimo_Leave(object sender, EventArgs e)
        {
            if (txt_Acrescimo.Text == string.Empty)
                txt_Acrescimo.Text = "0";

            txt_Acrescimo.Text = util_dados.ConfigNumDecimal(txt_Acrescimo.Text, 2);
            txt_Total.Text = util_dados.ConfigNumDecimal((double.Parse(txt_Valor.Text) + double.Parse(txt_Acrescimo.Text)) - double.Parse(txt_Desconto.Text), 2);
        }

        private void txt_Desconto_Leave(object sender, EventArgs e)
        {
            if (txt_Desconto.Text == string.Empty)
                txt_Desconto.Text = "0";

            txt_Desconto.Text = util_dados.ConfigNumDecimal(txt_Desconto.Text, 2);
            txt_Total.Text = util_dados.ConfigNumDecimal((double.Parse(txt_Valor.Text) + double.Parse(txt_Acrescimo.Text)) - double.Parse(txt_Desconto.Text), 2);
        }

        private void txt_Valor_Leave(object sender, EventArgs e)
        {
            if (txt_Valor.Text == string.Empty)
                txt_Valor.Text = "0";

            txt_Valor.Text = util_dados.ConfigNumDecimal(txt_Valor.Text, 2);
            txt_Total.Text = util_dados.ConfigNumDecimal((double.Parse(txt_Valor.Text) + double.Parse(txt_Acrescimo.Text)) - double.Parse(txt_Desconto.Text), 2);
        }

        private void txt_AcrescimoBaixa_Leave(object sender, EventArgs e)
        {
            if (txt_AcrescimoBaixa.Text == string.Empty)
                txt_AcrescimoBaixa.Text = "0";
            int aux = 0;
            double Total = 0;

            txt_AcrescimoBaixa.Text = util_dados.ConfigNumDecimal(txt_AcrescimoBaixa.Text, 2);

            for (int i = 0; i <= DG.Rows.Count - 1; i++)
                if (Convert.ToBoolean(DG.Rows[i].Cells["col_Seleciona"].Value) == true)
                {
                    aux++;
                    Total = Convert.ToDouble(DG.Rows[i].Cells["col_Valor"].Value);
                }

            if (aux == 1)
                txt_ValorBaixa.Text = util_dados.ConfigNumDecimal((Total + double.Parse(txt_AcrescimoBaixa.Text)) - double.Parse(txt_DescontoBaixa.Text), 2);

        }

        private void txt_DescontoBaixa_Leave(object sender, EventArgs e)
        {
            if (txt_DescontoBaixa.Text == string.Empty)
                txt_DescontoBaixa.Text = "0";
            int aux = 0;
            double Total = 0;

            txt_DescontoBaixa.Text = util_dados.ConfigNumDecimal(txt_DescontoBaixa.Text, 2);

            for (int i = 0; i <= DG.Rows.Count - 1; i++)
                if (Convert.ToBoolean(DG.Rows[i].Cells["col_Seleciona"].Value) == true)
                {
                    aux++;
                    Total = Convert.ToDouble(DG.Rows[i].Cells["col_Valor"].Value);
                }

            if (aux == 1)
                txt_ValorBaixa.Text = util_dados.ConfigNumDecimal((Total + double.Parse(txt_AcrescimoBaixa.Text)) - double.Parse(txt_DescontoBaixa.Text), 2);
        }

        private void txt_ID_Pessoa_TextChanged(object sender, EventArgs e)
        {
            if (util_dados.Verifica_int(txt_ID_Pessoa.Text) > 0)
                cb_ID_Pessoa.SelectedValue = int.Parse(txt_ID_Pessoa.Text);
        }

        private void txt_ValorPagoParcial_Leave(object sender, EventArgs e)
        {
            if (double.Parse(txt_ValorPagoParcial.Text) >= double.Parse(txt_ValorBaixa.Text))
            {
                MessageBox.Show("Valor Parcial Inválido!");

                txt_ValorPagoParcial.Text = "0";
                txt_ValorPagoParcial.Text = util_dados.ConfigNumDecimal(txt_ValorPagoParcial.Text, 2);
                return;
            }
            if (txt_ValorPagoParcial.Text == string.Empty)
                txt_ValorPagoParcial.Text = "0";

            txt_ValorPagoParcial.Text = util_dados.ConfigNumDecimal(txt_ValorPagoParcial.Text, 2);
            txt_ValorRestante.Text = util_dados.ConfigNumDecimal((double.Parse(txt_ValorBaixa.Text) - double.Parse(txt_ValorPagoParcial.Text)), 2);
        }

        private void txt_ID_TextChanged(object sender, EventArgs e)
        {
            //VERIFICA SE TEM ALGUM REGISTRO SELECIONADO
            if (util_dados.Verifica_int(txt_ID.Text) > 0)
            {
                lb_Periodo.Visible = false;
                txt_Periodo.Visible = false;
                gb_Calendario.Visible = false;

                Config(StatusForm.Consulta);

                Carrega_Lancamento();
            }
            else
            {
                lb_Periodo.Visible = true;
                txt_Periodo.Visible = true;
                gb_Calendario.Visible = true;

                lb_Cheque.Visible = false;
                dg_Cheque.Visible = false;

                lb_ResumoPagto.Visible = false;
                dg_ResumoPagto.Visible = false;
            }
        }

        private void txt_QuantidadeParcela_Leave(object sender, EventArgs e)
        {
            if (util_dados.Verifica_int(txt_ID.Text) == 0)
            {
                txt_Parcela.Text = "1";
                txt_ParcelaFinal.Text = txt_QuantidadeParcela.Text;
            }
        }
        #endregion

        #region CHECKBOX
        private void ck_PagamentoParcial_CheckedChanged(object sender, EventArgs e)
        {
            if (ck_PagamentoParcial.Checked == true)
            {
                gb_Parcial.Enabled = true;
                for (int i = 0; i <= DG.Rows.Count - 1; i++)
                    if (Convert.ToBoolean(DG.Rows[i].Cells["col_Seleciona"].Value) == true)
                        mk_DataProrrogacao.Text = Convert.ToDateTime(DG.Rows[i].Cells["col_Vencimento"].Value).ToString("dd/MM/yyyy");
            }
            else
                gb_Parcial.Enabled = false;
        }
        #endregion

        #region MASKEDBOX
        private void mk_Conta_TextChanged(object sender, EventArgs e)
        {
            CarregaConta();
        }

        private void mk_Emissao_Leave(object sender, EventArgs e)
        {
            DateTime.TryParseExact(mk_Emissao.Text, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out ValidaData);
            if (ValidaData == DateTime.MinValue)
            {
                MessageBox.Show("Data Inválida!", "Erro");
                mk_Emissao.Text = Convert.ToString(DateTime.Now);
                mk_Emissao.Focus();
            }
           
        }

        private void mk_Vencimento_Leave(object sender, EventArgs e)
        {
            DateTime.TryParseExact(mk_Vencimento.Text, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out ValidaData);
            if (ValidaData == DateTime.MinValue)
            {
                MessageBox.Show("Data Inválida!", "Erro");
                mk_Vencimento.Text = Convert.ToString(DateTime.Now);
                mk_Vencimento.Focus();
            }
           
        }

        private void mk_DataInicial_Leave(object sender, EventArgs e)
        {
            if (mk_DataInicial.Text.Replace(@"/", "").Trim() == string.Empty)
                return;

            DateTime.TryParseExact(mk_DataInicial.Text, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out ValidaData);
            if (ValidaData == DateTime.MinValue)
            {
                MessageBox.Show("Data Inválida!", "Erro");
                mk_DataInicial.Text = Convert.ToString(DateTime.Now);
                mk_DataInicial.Focus();
            }
           
        }

        private void mk_DataFinal_Leave(object sender, EventArgs e)
        {
            if (mk_DataFinal.Text.Replace(@"/", "").Trim() == string.Empty)
                return;

            DateTime.TryParseExact(mk_DataFinal.Text, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out ValidaData);
            if (ValidaData == DateTime.MinValue)
            {
                MessageBox.Show("Data Inválida!", "Erro");
                mk_DataFinal.Text = Convert.ToString(DateTime.Now);
                mk_DataFinal.Focus();
            }
           
        }

        private void mk_DataProrrogacao_Leave(object sender, EventArgs e)
        {
            DateTime.TryParseExact(mk_DataProrrogacao.Text, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out ValidaData);
            if (ValidaData == DateTime.MinValue)
            {
                MessageBox.Show("Data Inválida!", "Erro");
                mk_DataProrrogacao.Text = Convert.ToString(DateTime.Now);
                mk_DataProrrogacao.Focus();
            }
           
        }
        #endregion

        #region DATAGRIDVIEW
        private void DG_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            CalculaValor();
        }

        private void DG_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            Config_DG();
        }

        private void DG_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.ColumnIndex == 0)
            {
                Seleciona = !Seleciona;
                for (int i = 0; i <= DG.Rows.Count - 1; i++)
                    DG.Rows[i].Cells[0].Value = Seleciona;
            }
            CalculaValor();
            txt_ValorBaixa.Focus();
        }

        private void DG_DoubleClick(object sender, EventArgs e)
        {
            if (DG.Rows.Count == 0)
                return;

            if (Convert.ToBoolean(DG.Rows[DG.CurrentRow.Index].Cells["col_Seleciona"].Value) == true)
            {
                DG.Rows[DG.CurrentRow.Index].Cells["col_Seleciona"].Value = false;
                return;
            }

            if (Convert.ToBoolean(DG.Rows[DG.CurrentRow.Index].Cells["col_Seleciona"].Value) == false)
                DG.Rows[DG.CurrentRow.Index].Cells["col_Seleciona"].Value = true;

            CalculaValor();
        }

        private void DG_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            if (e.RowIndex == -1 && e.ColumnIndex == 0)
            {
                try
                {
                    using (Brush backColorBrush = new SolidBrush(e.CellStyle.BackColor))
                    {
                        e.Graphics.FillRectangle(backColorBrush, e.CellBounds);
                    }

                    e.Graphics.DrawLine(Pens.DarkGray, e.CellBounds.Left, e.CellBounds.Bottom - 1, e.CellBounds.Right, e.CellBounds.Bottom - 1);
                    e.Graphics.DrawLine(Pens.DarkGray, e.CellBounds.Left, e.CellBounds.Top, e.CellBounds.Right, e.CellBounds.Top);
                    e.Graphics.DrawLine(Pens.White, e.CellBounds.Left, e.CellBounds.Top + 1, e.CellBounds.Right, e.CellBounds.Top + 1);
                    e.Graphics.DrawLine(Pens.DarkGray, e.CellBounds.Right - 1, e.CellBounds.Top, e.CellBounds.Right - 1, e.CellBounds.Bottom);
                    e.Graphics.DrawLine(Pens.White, e.CellBounds.Left, e.CellBounds.Top, e.CellBounds.Left, e.CellBounds.Bottom);

                    Image imgChecked = (Image)Sistema.UI.Properties.Resources._checked;
                    Image imgUnchecked = (Image)Sistema.UI.Properties.Resources._unchecked;

                    int X = e.CellBounds.Left + ((e.CellBounds.Width - imgChecked.Width) / 2) - 1;
                    int Y = e.CellBounds.Top + ((e.CellBounds.Height - imgChecked.Height) / 2) - 1;

                    if (Seleciona)
                        e.Graphics.DrawImage(imgChecked, X, Y);
                    else
                        e.Graphics.DrawImage(imgUnchecked, X, Y);

                    e.Handled = true;
                }
                catch
                {
                }
            }
        }
        #endregion

        #region BUTTON
        private void bt_PesquisaConta_P_Click(object sender, EventArgs e)
        {
            UI_PlanoConta_Consulta frm = new UI_PlanoConta_Consulta();

            frm.ShowDialog();
            mk_ContaP.Text = frm.Cod_Conta;
        }

        private void bt_PesquisaConta_Click(object sender, EventArgs e)
        {
            UI_PlanoConta_Consulta frm = new UI_PlanoConta_Consulta();

            frm.ShowDialog();
            mk_Conta.Text = frm.Cod_Conta;
            CarregaConta();
        }

        private void bt_Baixa_Click(object sender, EventArgs e)
        {
            int aux = 0;
            try
            {
                //VERIFICA SE EXISTE ALGUM REGISTRO QUE JÁ FOI BAIXADO
                for (int i = 0; i <= DG.Rows.Count - 1; i++)
                    if (Convert.ToBoolean(DG.Rows[i].Cells["col_Seleciona"].Value) == true)
                    {
                        aux++;//FAZ CONTAGEM DE QUANTOS REGISTROS ESTÃO MARCADOS
                        if (DG.Rows[i].Cells["col_Situacao"].Value.ToString() == "PAGO")
                        {
                            MessageBox.Show("Existem registros já baixados!\nSelecione novamente!", this.Text);
                            return;
                        }
                    }
                //FINALIZA CONSULTA

                if (aux == 0)
                {
                    MessageBox.Show("Nenhum Registro Selecionado!");
                    return;
                }

                //ROTINAS PARA PAGAMENTO PARCIAL
                #region PAGAMENTO PARCIAL
                if (ck_PagamentoParcial.Checked == true)
                {
                    double ValorParcial = 0;
                    int row = 0;

                    for (int i = 0; i <= DG.Rows.Count - 1; i++)
                        if (Convert.ToBoolean(DG.Rows[i].Cells["col_Seleciona"].Value) == true)
                        {
                            ValorParcial = double.Parse(txt_ValorRestante.Text);
                            row = i;
                        }

                    //SOLICITA CONFIRMAÇÃO PARA NOVO LANÇAMENTO
                    DialogResult msgbox = MessageBox.Show("Confirma novo lançamento de R$ " + util_dados.ConfigNumDecimal(ValorParcial, 2), this.Text, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
                    if (msgbox == DialogResult.No)
                        return;

                    //ALTERA O VALOR DO REGISTRO ATUAL
                    BLL_CPagar = new BLL_CPagar();
                    CPagar = new DTO_CPagar();

                    CPagar.ID = Convert.ToInt32(DG.Rows[row].Cells["col_ID"].Value);
                    CPagar.Valor = double.Parse(txt_ValorPagoParcial.Text);
                    CPagar.Desconto = Convert.ToInt32(DG.Rows[row].Cells["col_Desconto"].Value);
                    CPagar.Acrescimo = Convert.ToInt32(DG.Rows[row].Cells["col_Acrescimo"].Value);
                    CPagar.Altera_Registro = 3;

                    BLL_CPagar.Altera(CPagar);

                    //DG.Rows[row].Cells["col_Valor"].Value = util_dados.ConfigNumDecimal(txt_ValorPagoParcial.Text, 8);

                    //CRIA NOVO LANÇAMENTO COM DIFERENÇA DO VALOR
                    BLL_CPagar = new BLL_CPagar();
                    CPagar = new DTO_CPagar();

                    CPagar.ID = 0;
                    CPagar.ID_Empresa = Parametro_Empresa.ID_Empresa_Ativa;
                    CPagar.ID_Conta = util_dados.Verifica_int(txt_ID_Conta.Text);
                    CPagar.Situacao = Convert.ToInt32(cb_Situacao.SelectedValue);
                    CPagar.Documento = txt_Documento.Text;
                    CPagar.Emissao = Convert.ToDateTime(mk_Emissao.Text);
                    CPagar.TipoPessoa = Convert.ToInt32(cb_TipoPessoa.SelectedValue);
                    CPagar.ID_Pessoa = Convert.ToInt32(cb_ID_Pessoa.SelectedValue);
                    CPagar.Valor = double.Parse(txt_ValorRestante.Text);
                    CPagar.QuantidadeParcela = Convert.ToInt32(txt_QuantidadeParcela.Text);
                    CPagar.Parcela = Convert.ToInt32(txt_Parcela.Text);
                    CPagar.Descricao = txt_Descricao.Text;
                    CPagar.Controle = Convert.ToInt32(txt_Controle.Text) + 1;
                    CPagar.Desconto = 0;
                    CPagar.Acrescimo = 0;

                    CPagar.Vencimento = Convert.ToDateTime(mk_DataProrrogacao.Text);

                    BLL_CPagar.Grava(CPagar);
                }
                #endregion

                UI_CPagar_Lanca frm = new UI_CPagar_Lanca();

                if (ck_PagamentoParcial.Checked == true)
                    frm.Valor = double.Parse(txt_ValorPagoParcial.Text);
                else
                    frm.Valor = double.Parse(txt_ValorBaixa.Text);

                frm.lst_CPagar = new List<DTO_CPagar>();

                string _Doc = string.Empty;
                for (int i = 0; i <= DG.Rows.Count - 1; i++)
                    if (Convert.ToBoolean(DG.Rows[i].Cells["col_Seleciona"].Value) == true)
                    {
                        CPagar = new DTO_CPagar();

                        CPagar.ID = (int)DG.Rows[i].Cells["col_ID"].Value;
                        CPagar.ID_Conta = (int)DG.Rows[i].Cells["col_ID_Conta"].Value;
                        _Doc += "(" + DG.Rows[i].Cells["col_ID"].Value + ")";

                        if (aux > 1)
                        {
                            CPagar.Desconto = double.Parse(DG.Rows[i].Cells["col_Desconto"].Value.ToString());
                            CPagar.Acrescimo = double.Parse(DG.Rows[i].Cells["col_Acrescimo"].Value.ToString());
                        }
                        else
                        {
                            CPagar.Desconto = double.Parse(txt_DescontoBaixa.Text);
                            CPagar.Acrescimo = double.Parse(txt_AcrescimoBaixa.Text);
                        }
                        frm.Documento = _Doc;
                        frm.lst_CPagar.Add(CPagar);
                    }
                frm.ShowDialog();

                if (frm.Concluido == false)
                    return;

                MessageBox.Show(util_msg.msg_Baixa, this.Text);
                Pesquisa();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, this.Text);

            }
        }

        private void bt_Anterior_Click(object sender, EventArgs e)
        {
            if (DG.Rows.Count == 0 || DG.Rows.Count == 1)
                return;
            if (DG.CurrentRow == null)
                DG.Rows[0].Cells[0].Selected = true;

            int aux = DG.CurrentRow.Index - 1;
            if (aux >= 0)
            {
                DG.Rows[DG.CurrentRow.Index].Cells[0].Selected = false;
                DG.Rows[aux].Cells[0].Selected = true;
            }
        }

        private void bt_Proximo_Click(object sender, EventArgs e)
        {
            if (DG.Rows.Count == 0 || DG.Rows.Count == 1)
                return;
            if (DG.CurrentRow == null)
                DG.Rows[0].Cells[0].Selected = true;

            int aux = DG.CurrentRow.Index + 1;
            if (aux < DG.Rows.Count)
            {
                DG.Rows[DG.CurrentRow.Index].Cells[0].Selected = false;
                DG.Rows[aux].Cells[0].Selected = true;
            }
        }

        private void bt_Edita_Click(object sender, EventArgs e)
        {
            if (Status == StatusForm.Edita)
                tabctl.SelectedTab = TabPage1;
        }

        private void bt_Recibo_Click(object sender, EventArgs e)
        {
            string strPessoa = "";
            int aux = 0;
            int ID_Pessoa = 0;
            int TipoPessoa = 0;
            string Documento = "";
            DataTable _DT;

            for (int i = 0; i <= DG.Rows.Count - 1; i++)
                if (Convert.ToBoolean(DG.Rows[i].Cells["col_Seleciona"].Value) == true)
                {
                    if (aux >= 1)
                        if (strPessoa != DG.Rows[i].Cells["col_Pessoa"].Value.ToString())
                        {
                            MessageBox.Show("ATENÇÃO, Recibo Somente com Mesma Pessoa!");
                            return;
                        }

                    strPessoa = DG.Rows[i].Cells["col_Pessoa"].Value.ToString();
                    ID_Pessoa = Convert.ToInt32(DG.Rows[i].Cells["col_ID_Pessoa"].Value);
                    TipoPessoa = Convert.ToInt32(DG.Rows[i].Cells["col_TipoPessoa"].Value);
                    aux++;
                }

            if (aux == 0)
            {
                MessageBox.Show("Nenhum Registro Selecionado!");
                return;
            }

            BLL_Pessoa = new BLL_Pessoa();
            Pessoa = new DTO_Pessoa();

            _DT = new DataTable();

            Pessoa.TipoPessoa = TipoPessoa;
            Pessoa.ID = ID_Pessoa;
            Pessoa.ID_Empresa = Parametro_Empresa.ID_Empresa_Ativa;
            _DT = BLL_Pessoa.Busca(Pessoa);

            if (_DT.Rows.Count == 1)
                Documento = _DT.Rows[0]["CNPJ_CPF"].ToString();

            UI_Visualiza_Relatorio rpt = new UI_Visualiza_Relatorio();
            rpt.Show();

            string rpt_Nome = "";

            if (ck_2Vias.Checked == true)
                rpt_Nome = "rpt_Pagamento2.rdlc";
            else
                rpt_Nome = "rpt_Pagamento.rdlc";

            string Caminhorpt = Parametro_Sistema.Caminho_Relatorio + rpt_Nome;
            rpt.rpt_Viewer.LocalReport.ReportPath = Caminhorpt;

            BLL_Pessoa = new BLL_Pessoa();
            Pessoa = new DTO_Pessoa();

            Pessoa.ID = Parametro_Empresa.ID_Empresa_Ativa;

            _DT = new DataTable();

            _DT = BLL_Pessoa.Busca_Info_Relatorio(Pessoa);

            ReportDataSource ds_Empresa = new ReportDataSource("ds_Emitente", _DT);
            string Informacao = "";

            for (int i = 0; i <= DG.Rows.Count - 1; i++)
                if (Convert.ToBoolean(DG.Rows[i].Cells["col_Seleciona"].Value) == true)
                {
                    Informacao = Informacao + Environment.NewLine + "R$ ";
                    Informacao += util_dados.ConfigNumDecimal(DG.Rows[i].Cells["col_Valor"].Value, 2) + " - ";
                    Informacao += DG.Rows[i].Cells["col_DescricaoConta"].Value + " - ";
                    Informacao += DG.Rows[i].Cells["col_Informacao"].Value + " - " + DG.Rows[i].Cells["col_Parcela"].Value;
                }
            if (Convert.ToDouble(txt_AcrescimoBaixa.Text) > 0)
                Informacao += Environment.NewLine + "ACRÉSCIMO R$ " + util_dados.ConfigNumDecimal(txt_AcrescimoBaixa.Text, 2);

            if (Convert.ToDouble(txt_DescontoBaixa.Text) > 0)
                Informacao += Environment.NewLine + "DESCONTO R$ " + util_dados.ConfigNumDecimal(txt_DescontoBaixa.Text, 2);

            string numExtenso = util_dados.ValorExtenso(Convert.ToDecimal(txt_ValorBaixa.Text));
            ReportParameter p1 = new ReportParameter("Valor", util_dados.ConfigNumDecimal(txt_ValorBaixa.Text, 2));
            ReportParameter p2 = new ReportParameter("ValorExtenso", numExtenso.ToUpper());
            ReportParameter p3 = new ReportParameter("Pessoa", strPessoa);
            ReportParameter p4 = new ReportParameter("Informacao", Informacao);
            ReportParameter p5 = new ReportParameter("Data", Convert.ToString(util_dados.Config_Data(DateTime.Now, 4)).ToUpper());
            ReportParameter p6 = new ReportParameter("Documento", Documento);

            rpt.rpt_Viewer.LocalReport.DataSources.Add(ds_Empresa);
            rpt.rpt_Viewer.LocalReport.SetParameters(new ReportParameter[] { p1, p2, p3, p4, p5, p6 });

            rpt.rpt_Viewer.RefreshReport();
         }
        #endregion

        #region TABPAGE
        private void TabPage1_Enter(object sender, EventArgs e)
        {
            if (util_dados.Verifica_int(txt_ID.Text) > 0)
            {
                int aux = 0;

                for (int i = 0; i <= DG.Rows.Count - 1; i++)
                    if (Convert.ToBoolean(DG.Rows[i].Cells["col_Seleciona"].Value) == true)
                        aux++;
                if (aux > 1)
                    gb_Alterar.Visible = true;
                else
                    gb_Alterar.Visible = false;
            }
            else
                gb_Alterar.Visible = false;
        }
        #endregion

        #region CALENDARIO
        private void cal_Calendario_DateChanged(object sender, DateRangeEventArgs e)
        {
            txt_qtDias.Text = util_dados.Calcula_dias_Data(Convert.ToDateTime(mk_Vencimento.Text), Convert.ToDateTime(cal_Calendario.SelectionEnd)).ToString();
        }
        #endregion

  
    }
}
