﻿using iTextSharp.text.pdf;
using iTextSharp.text;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GestaoFormandos
{
    public partial class FormListarFormandos : Form
    {
        DBConnect ligacao = new DBConnect();
        public FormListarFormandos()
        {
            InitializeComponent();
        }

        private void FormListarFormandos_Load(object sender, EventArgs e)
        {
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells; //Tamanho automaticamente ajustado
            dataGridView1.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells; //Linhas e colunas ajustaveis
            dataGridView1.AllowUserToAddRows = false; //não permite o utilizador add linhas
            dataGridView1.AllowUserToDeleteRows = false; //não permite o utilizador deletar linhas
            dataGridView1.EditMode = DataGridViewEditMode.EditProgrammatically; //o dataGridView só pode ser alterado a partir da programação
            dataGridView1.Columns.Add("codID", "ID"); //cabeçalho do dataGridView
            dataGridView1.Columns.Add("Nome", "Nome");
            dataGridView1.Columns.Add("Iban", "Iban");
            dataGridView1.Columns.Add("Nacionalidade", "Nacionalidade");
            dataGridView1.Columns.Add("ID_Nac.", "ID_Nac.");
            dataGridView1.Columns.Add("Genero", "Genero");

            ligacao.PreencherDataGriedViewFormandos(ref dataGridView1, "", "", "", "", "", Genero());

            ligacao.PreencherComboNacionalidade(ref cmbNacionalidade);

            lblRegistos.Text = "Nº Registos: " + dataGridView1.RowCount.ToString();

            rbTodos.Checked = true;
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            dataGridView1.Rows.Clear();
            txtNome.Text = "";
            rbTodos.Checked = true;

            ligacao.PreencherDataGriedViewFormandos(ref dataGridView1, "", "", "","","", Genero());

            lblRegistos.Text = "Nº Registos: " + dataGridView1.RowCount.ToString();

           
        }

        private void btnPesquisa_Click(object sender, EventArgs e)
        {
            string id_nacionalidade = "";

            dataGridView1.Rows.Clear();

            if (cmbNacionalidade.SelectedIndex != -1)
            {
                id_nacionalidade = cmbNacionalidade.Text.Substring(
                    cmbNacionalidade.Text.LastIndexOf(' ') + 1);
            }
                txtNome.Text = Geral.TirarEspacos(txtNome.Text);

            ligacao.PreencherDataGriedViewFormandos(ref dataGridView1, "", txtNome.Text, "", "", id_nacionalidade, Genero());

            lblRegistos.Text = "Nº Registos: " + dataGridView1.RowCount.ToString();
        }
        private char Genero()
        {
            char genero = 'T';

            if (rbFeminino.Checked)
            {
                genero = 'F';
            }
            else if (rbMasculino.Checked)
            {
                genero = 'M';
            }
            else if (rbOutro.Checked)
            {
                genero = 'O';
            }
            return genero;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (dataGridView1.Rows.Count > 0)
            {
                SaveFileDialog sfd = new SaveFileDialog();
                sfd.Filter = "PDF (*.pdf)|*.pdf";
                sfd.FileName = "Formandos.pdf";
                bool fileError = false;
                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    if (File.Exists(sfd.FileName))
                    {
                        try
                        {
                            File.Delete(sfd.FileName);
                        }
                        catch (IOException ex)
                        {
                            fileError = true;
                            MessageBox.Show("Impossível de apagar o ficheiro!");
                        }
                    }
                    //if (!fileError == true)
                    //if (fileError == false)
                    if (!fileError)
                    {
                        try
                        {
                            PdfPTable pdfPTable = new PdfPTable(dataGridView1.Columns.Count);
                            pdfPTable.DefaultCell.Padding = 3;
                            pdfPTable.WidthPercentage = 100;
                            pdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;

                            foreach (DataGridViewColumn column in dataGridView1.Columns)
                            {
                                PdfPCell cell = new PdfPCell(new Phrase(column.HeaderText));
                                pdfPTable.AddCell(cell);
                            }

                            foreach (DataGridViewRow row in dataGridView1.Rows)
                            {
                                foreach (DataGridViewCell cell in row.Cells)
                                {
                                    pdfPTable.AddCell(cell.Value.ToString());
                                }
                            }

                            //using (FileStream stream = new FileStream(sfd.FileName, FileMode.Create))

                            FileStream stream = new FileStream(sfd.FileName, FileMode.Create);
                            //{
                            Document pdfDoc = new Document(PageSize.A4, 10f, 20f, 20f, 10f);
                            PdfWriter.GetInstance(pdfDoc, stream);
                            pdfDoc.Open();
                            pdfDoc.Add(pdfPTable);
                            pdfDoc.Close();
                            stream.Close();
                            //}

                            MessageBox.Show("Imprimiu com sucesso!");
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("ERROR: " + ex.Message);
                        }
                    }
                }
            }
            else
            {
                MessageBox.Show("Não existe registos!");
            }
        }
    }
    
}
