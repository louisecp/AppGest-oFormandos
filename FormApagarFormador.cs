﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GestaoFormandos
{
    public partial class FormApagarFormador : Form
    {
        DBConnect ligacao = new DBConnect();
        public FormApagarFormador()
        {
            InitializeComponent();
        }

        private void FormApagarFormador_Load(object sender, EventArgs e)
        {
            txtNome.ReadOnly = true;
            txtNIF.ReadOnly = true;
            mtxtDataNascimento.ReadOnly = true;
            txtIDArea.ReadOnly = true;
            txtIDUtilizador.ReadOnly = true;

            btnEliminar.Enabled = false;

            this.AcceptButton = this.btnPesquisa;
        }

        private void btnPesquisa_Click(object sender, EventArgs e)
        {
            string nome = "", nif = "", dataNascimento = "", idArea = "", idUtilizador = "";

            if (ligacao.PesquisaFormador(nudID.Value.ToString(), ref nome, ref nif, ref dataNascimento, ref idArea, ref idUtilizador))
            {
                txtNome.Text = nome;
                txtNIF.Text = nif;
                mtxtDataNascimento.Text = dataNascimento;
                txtIDArea.Text = ligacao.DevolverIDArea(idArea);
                txtIDUtilizador.Text = ligacao.DevolverIDUtilizador(idUtilizador);
                btnEliminar.Enabled = true;
            }
            else
            {
                MessageBox.Show("Formador não encontrado.");
                Limpar();
            }
        }
        private void Limpar()
        {
            nudID.Value = 0;
            txtNome.Text = string.Empty;
            txtNIF.Text = string.Empty;
            dateTimePicker1.Value = DateTime.Now;
            mtxtDataNascimento.Clear();
            txtIDArea.Text = string.Empty;
            txtIDUtilizador.Text = string.Empty;

        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            groupBox1.Enabled = true;
            btnEliminar.Enabled = false;
            nudID.Focus();
            Limpar();
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            string id = nudID.Value.ToString();

            if (MessageBox.Show("Tem certeza de que deseja excluir esta área?", "Confirmar Exclusão", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                if (ligacao.DeleteFormador(id))
                {
                    MessageBox.Show("Área excluída com sucesso.");
                    Limpar();
                    btnEliminar.Enabled = false;
                }
                else
                {
                    MessageBox.Show("Falha ao excluir a área.");
                }
            }
        }
    }
}
