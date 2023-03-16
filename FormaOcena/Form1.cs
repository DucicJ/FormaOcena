using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace FormaOcena
{
    public partial class Form1 : Form
    {
        DataTable TabelaUcenik, TabelaPredmet, TabelaOcena;
        int redni = 0;
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            IspisiUcenik();
            IspisiPredmet();
            IspisiOcenu();
            Popuni();
        }

        public void Popuni()
        {
            comboBox1.SelectedValue = TabelaOcena.Rows[redni]["ucenik_id"].ToString(); //ovo uzimam iz ocene jer 
            comboBox2.SelectedValue = TabelaOcena.Rows[redni]["predmet_id"].ToString();
            comboBox3.SelectedValue = TabelaOcena.Rows[redni]["ocena"].ToString();
        }

        private void IspisiUcenik()
        {
            SqlConnection veza = Konekcija.connect();
            SqlDataAdapter ada = new SqlDataAdapter("SELECT id, ime + ' ' + prezime as Identitet FROM osoba WHERE uloga = 1", veza); //komanda za sql koja vraca tabelu sa listom ucenika
            TabelaUcenik = new DataTable(); //kreiramo novu tabelu. mislim da ovo moze da se stavi van metode
            ada.Fill(TabelaUcenik); //popnjavamo tabelu
            comboBox1.DataSource = TabelaUcenik; //popunjavamo combobox sa odgovarajucim atributima
            comboBox1.ValueMember = "id";
            comboBox1.DisplayMember = "Identitet";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string naredba = "UPDATE Ocena SET ";
            naredba += "ucenik_id=" + comboBox1.SelectedValue.ToString();
            naredba += ", predmet_id=" + comboBox2.SelectedValue.ToString();
            naredba += ", ocena=" + comboBox3.SelectedValue.ToString() + " WHERE id=" + TabelaOcena.Rows[redni]["id"].ToString();
            SqlConnection veza = Konekcija.connect();
            SqlCommand komanda = new SqlCommand(naredba, veza);
            veza.Open();
            komanda.ExecuteNonQuery();
            veza.Close();
            IspisiOcenu();
            redni = 0;
            Popuni();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string naredba = "INSERT INTO Ocena (ucenik_id, predmet_id, ocena) VALUES(";
            naredba += comboBox1.SelectedValue.ToString() + ", ";
            naredba += comboBox2.SelectedValue.ToString() + ", ";
            naredba += comboBox3.SelectedValue.ToString() + ")";
            SqlConnection veza = Konekcija.connect();
            SqlCommand komanda = new SqlCommand(naredba, veza);
            veza.Open();
            komanda.ExecuteNonQuery();
            veza.Close();
            IspisiOcenu();
            redni = 0;
            Popuni();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            string naredba = "DELETE FROM Ocena WHERE id=" + TabelaOcena.Rows[redni]["id"].ToString();
            SqlConnection veza = Konekcija.connect();
            SqlCommand komanda = new SqlCommand(naredba, veza);
            veza.Open();
            komanda.ExecuteNonQuery();
            veza.Close();
            IspisiOcenu();
            redni = 0;
            Popuni();
        }

        private void dataGridView1_CurrentCellChanged(object sender, EventArgs e)
        {
            if (dataGridView1.Focused && dataGridView1.CurrentRow != null)
                redni = dataGridView1.CurrentRow.Index;
            Popuni();
        }

        private void IspisiPredmet()
        {
            SqlConnection veza = Konekcija.connect();
            SqlDataAdapter adapter = new SqlDataAdapter("SELECT * FROM predmet", veza);
            TabelaPredmet = new DataTable();
            adapter.Fill(TabelaPredmet);
            comboBox2.DataSource = TabelaPredmet;//ista prica
            comboBox2.ValueMember = "id";
            comboBox2.DisplayMember = "naziv";
        }
        private void IspisiOcenu()
        {
            SqlConnection veza = Konekcija.connect();
            string naredba = "select Ocena.id, ucenik_id, Predmet.id, ime + ' ' + prezime as Identitet, naziv, ocena from Ocena join Osoba on Osoba.id = ucenik_id join Predmet on Predmet.id = predmet_id";
            SqlDataAdapter adapter = new SqlDataAdapter(naredba, veza);
            TabelaOcena = new DataTable();
            adapter.Fill(TabelaOcena);
            dataGridView1.DataSource = TabelaOcena;
            dataGridView1.ReadOnly = true;
            dataGridView1.AllowUserToAddRows = false;
            dataGridView1.Columns["ucenik_id"].Visible = false;
            dataGridView1.Columns["id"].Visible = false;
            dataGridView1.Columns["predmet_id"].Visible = false;
            dataGridView1.AllowUserToDeleteRows = false;
        }
    }
}
