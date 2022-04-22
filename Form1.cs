using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using PersonasClinica;
using MySql.Data.MySqlClient;
using System.Net;
using System.Net.Http;
using System.Net.Mail;


namespace ClinicaDental
{
    public partial class Form1 : Form
    {

        static string connectionString = "datasource=localhost;port=3306;username=root;password=;database=dentist;";
        static Doctor doctor;
        static Employee employee = null;
        static Patient patient;
        private static string connection;
        private static readonly HttpClient cl = new HttpClient();
        private static List<TextBox> textBoxes;
        private static List<RichTextBox> richTextBoxes;
        private static ArrayList tb;

        public Form1()
        {

            InitializeComponent();

            comboBox1.DataSource = Enum.GetValues(typeof(EnumServices));
            comboBox2.DataSource = Enum.GetValues(typeof(EnumPosition));
            makeArrays();

            buttonDeleteDoctor.Name = "doctor";
            buttonDeleteEmployee.Name = "employee";
            buttonDeletePatient.Name = "patient";




        }

        private void makeArrays()
        {
            tb = new ArrayList()
            {

                textBoxPName, textBoxPSurname, textBoxPDNI, textBoxPEmail, textBoxPTel, textBoxPAddress, textBoxPImport,
                textBoxDName, textBoxDSurname, textBoxDDNI,

                textBoxDEmail, textBoxDTel, textBoxDAddress, textBoxDSalary, textBoxEName, textBoxESurname, textBoxEDNI,
                textBoxEEmail, textBoxETel, textBoxEAddress, textBoxESalary
            };
        }



        private void buttonCreatePatient_Click(object sender, EventArgs e)
        {

            EnumServices brackets = EnumServices.Brackets;
            EnumServices mouthCleaning = EnumServices.MouthCleaning;
            EnumServices cavities = EnumServices.CavitiesFilling;
            EnumServices revision = EnumServices.Revision;

            switch (comboBox1.SelectedItem.ToString())
            {
                case "Brackets":
                    patient = new Patient(textBoxPName.Text, textBoxPSurname.Text, textBoxPDNI.Text,
                        textBoxPEmail.Text, textBoxPTel.Text, textBoxPAddress.Text, 3500, brackets);
                    break;
                case "MouthCleaning":
                    patient = new Patient(textBoxPName.Text, textBoxPSurname.Text, textBoxPDNI.Text,
                        textBoxPEmail.Text, textBoxPTel.Text, textBoxPAddress.Text, 30, mouthCleaning);
                    break;
                case "CavitiesFilling":
                    patient = new Patient(textBoxPName.Text, textBoxPSurname.Text, textBoxPDNI.Text,
                        textBoxPEmail.Text, textBoxPTel.Text, textBoxPAddress.Text, 80, cavities);
                    break;
                case "Revision":
                    patient = new Patient(textBoxPName.Text, textBoxPSurname.Text, textBoxPDNI.Text,
                        textBoxPEmail.Text, textBoxPTel.Text, textBoxPAddress.Text, 50, revision);
                    break;
            }

            insertPatient();
            richTextBoxPatient.Text = patient.ToString();


        }

        private void buttonCreateEmployee_Click(object sender, EventArgs e)
        {



            EnumPosition accounting = EnumPosition.Accounting;
            EnumPosition cleaner = EnumPosition.Cleaner;
            EnumPosition receptionist = EnumPosition.Receptionist;


            switch (comboBox2.SelectedItem.ToString())
            {
                case "Accounting":
                    employee = new Employee(textBoxEName.Text, textBoxESurname.Text, textBoxEDNI.Text,
                        textBoxEEmail.Text, textBoxETel.Text, textBoxEAddress.Text, float.Parse(textBoxESalary.Text),
                        accounting);
                    break;
                case "Cleaner":
                    employee = new Employee(textBoxEName.Text, textBoxESurname.Text, textBoxEDNI.Text,
                        textBoxEEmail.Text, textBoxETel.Text, textBoxEAddress.Text, float.Parse(textBoxESalary.Text),
                        cleaner);
                    break;
                case "Receptionist":
                    employee = new Employee(textBoxEName.Text, textBoxESurname.Text, textBoxEDNI.Text,
                        textBoxEEmail.Text, textBoxETel.Text, textBoxEAddress.Text, float.Parse(textBoxESalary.Text),
                        receptionist);
                    break;
            }

            richTextBoxEmployee.Text = employee.ToString();
            insertEmployee();

        }

        private void buttonCreateDoctor_Click(object sender, EventArgs e)
        {
            doctor = new Doctor(textBoxDName.Text, textBoxDSurname.Text, textBoxDDNI.Text,
                textBoxDEmail.Text, textBoxDTel.Text, textBoxDAddress.Text, float.Parse(textBoxDSalary.Text));
            richTextBoxDoctor.Text = doctor.ToString();
            insertDoctor();
        }


        private void connect()
        {

            connection = "http://172.17.41.191/PHP/m7.php";
            string result = new WebClient().DownloadString(connection);
            richTextBoxDB.Text = result;
        }

        private async void insertDoctor()
        {

            connection = "http://172.17.41.191/PHP/m7.php";

            var client = new WebClient();
            var values = new NameValueCollection();

            var doctorDictionary = new Dictionary<string, string>
            {
                {"mode", "2"},
                {"name", doctor.name},
                {"surname", doctor.surname},
                {"dni", doctor.dni},
                {"email", doctor.email},
                {"tel", doctor.tel},
                {"address", doctor.address},
                {"salary", doctor.Salary.ToString()}
            };

            var content = new FormUrlEncodedContent(doctorDictionary);

            var r = await cl.PostAsync(connection, content);

            var responseString = await r.Content.ReadAsStringAsync();
            richTextBoxDB.Text = responseString;
        }

        private async void insertEmployee()
        {
            connection = "http://172.17.41.191/PHP/m7.php";

            var employeeDictionary = new Dictionary<string, string>
            {
                {"mode", "3"},
                {"name", employee.name},
                {"surname", employee.surname},
                {"dni", employee.dni},
                {"email", employee.email},
                {"tel", employee.tel},
                {"address", employee.address},
                {"salary", employee.Salary.ToString()},
                {"position", employee.position.ToString()}
            };

            var content = new FormUrlEncodedContent(employeeDictionary);

            var r = await cl.PostAsync(connection, content);

            var responseString = await r.Content.ReadAsStringAsync();
            richTextBoxDB.Text = responseString;


        }

        private async void insertPatient()
        {
            // connect();

            connection = "http://172.17.41.191/PHP/m7.php";

            var client = new WebClient();
            var values = new NameValueCollection();

            var data = new Dictionary<string, string>
            {
                {"mode", "1"},
                {"name", patient.name},
                {"surname", patient.surname},
                {"dni", patient.dni},
                {"email", patient.email},
                {"tel", patient.tel},
                {"address", patient.address},
                {"import", patient.Import.ToString()},
                {"service", patient.ServiceType.ToString()}
            };

            var content = new FormUrlEncodedContent(data);

            var r = await cl.PostAsync(connection, content);

            var responseString = await r.Content.ReadAsStringAsync();
            richTextBoxDB.Text = responseString;
            /*
            values["name"] = patient.name;
            values["surname"] = patient.surname;
            values["dni"] = patient.dni;
            values["email"] = patient.email;
            values["tel"] = patient.tel;
            values["address"] = patient.address;
            values["service"] = patient.ServiceType.ToString();
            var response = client.UploadValues(connection,"POST" , values);
            var responseString = Encoding.Default.GetString(response);*/
        }

        private async void buttonView_Click(object sender, EventArgs e)
        {
            connection = "http://172.17.41.191/PHP/m7.php?mode=select";
            string result = new WebClient().DownloadString(connection);
            foreach (TextBox t in tb)
            {
                t.ResetText();
                t.ReadOnly = true;
            }



            buttonView.Text = ">>";

            connection = "http://172.17.41.191/PHP/m7.php";

            var selectLoadDoctor = new Dictionary<string, string>
                {{"mode", "4"},};


            var content = new FormUrlEncodedContent(selectLoadDoctor);

            var responseString = await cl.PostAsync(connection, content);
            string doctorData = responseString.ToString();

            String[] arrayDoctor = result.Split(',');

            string nameD = arrayDoctor[0];
            string surnameD = arrayDoctor[1];
            string dniD = arrayDoctor[2];
            string emailD = arrayDoctor[3];
            string telD = arrayDoctor[4];
            string addressD = arrayDoctor[5];
            string salaryD = arrayDoctor[6];
            string nameE = arrayDoctor[7];
            string surnameE = arrayDoctor[8];
            string dniE = arrayDoctor[9];
            string emailE = arrayDoctor[10];
            string telE = arrayDoctor[11];
            string addressE = arrayDoctor[12];
            string salaryE = arrayDoctor[13];
            string positionE = arrayDoctor[14];
            string nameP = arrayDoctor[15];
            string surnameP = arrayDoctor[16];
            string dniP = arrayDoctor[17];
            string emailP = arrayDoctor[18];
            string telP = arrayDoctor[19];
            string addressP = arrayDoctor[20];
            string import = arrayDoctor[21];
            string service = arrayDoctor[22];


            textBoxEName.Text = nameE;
            textBoxESurname.Text = surnameE;
            textBoxEDNI.Text = dniE;
            textBoxEEmail.Text = emailE;
            textBoxETel.Text = telE;
            textBoxEAddress.Text = addressE;
            textBoxESalary.Text = salaryE;
            comboBox2.SelectedItem = positionE;
            textBoxDName.Text = nameD;
            textBoxDSurname.Text = surnameD;
            textBoxDDNI.Text = dniD;
            textBoxDEmail.Text = emailD;
            textBoxDTel.Text = telD;
            textBoxDAddress.Text = addressD;
            textBoxDSalary.Text = salaryD;
            textBoxPName.Text = nameP;
            textBoxPSurname.Text = surnameP;
            textBoxPDNI.Text = dniP;
            textBoxPEmail.Text = emailP;
            textBoxPTel.Text = telP;
            textBoxPAddress.Text = addressP;
            textBoxPImport.Text = import;
            comboBox1.SelectedItem = service;
        }

        private async void buttonDelete_Click(object sender, EventArgs e)
        {
            connection = "http://172.17.41.191/PHP/m7.php";
            var b = (Button)sender;


            switch (b.AccessibleName)
            {
                case "doctor":
                    var doctorDictionary = new Dictionary<string, string>
                        {{"mode", "7"}, {"dni", textBoxDDNI.Text}};
                    var content = new FormUrlEncodedContent(doctorDictionary);

                    var r = await cl.PostAsync(connection, content);

                    var responseString = await r.Content.ReadAsStringAsync();
                    break;
                case "employee":
                    var employeeDictionary = new Dictionary<string, string>
                        {{"mode", "8"}, {"dni", textBoxEDNI.Text}};
                    var cnt = new FormUrlEncodedContent(employeeDictionary);

                    var res = await cl.PostAsync(connection, cnt);

                    var response = await res.Content.ReadAsStringAsync();
                    break;
                case "patient":
                    var patientDictionary = new Dictionary<string, string>
                        {{"mode", "9"}, {"dni", textBoxPDNI.Text}};
                    var cont = new FormUrlEncodedContent(patientDictionary);

                    var rsp = await cl.PostAsync(connection, cont);

                    var rspString = await rsp.Content.ReadAsStringAsync();
                    break;
            }

        }

        private async void buttonModify_Click(object sender, EventArgs e)
        {
            connection = "http://172.17.41.191/PHP/m7.php";
            var b = (Button)sender;

            switch (b.AccessibleName)
            {

                case "doctor":
                    var doctorDictionary = new Dictionary<string, string>
                    {
                        {"mode", "10"}, {"name", textBoxDName.Text},
                        {"surname", textBoxDSurname.Text},
                        {"dni", textBoxDDNI.Text},
                        {"email", textBoxDEmail.Text},
                        {"tel", textBoxDTel.Text},
                        {"address", textBoxDAddress.Text},
                        {"salary", textBoxDSalary.Text}
                    };
                    var content = new FormUrlEncodedContent(doctorDictionary);

                    var r = await cl.PostAsync(connection, content);

                    var responseString = await r.Content.ReadAsStringAsync();
                    break;
                case "employee":
                    var employeeDictionary = new Dictionary<string, string>
                    {
                        {"mode", "11"}, {"name", textBoxEName.Text},
                        {"surname", textBoxESurname.Text},
                        {"dni", textBoxEDNI.Text},
                        {"email", textBoxEEmail.Text},
                        {"tel", textBoxETel.Text},
                        {"address", textBoxEAddress.Text},
                        {"salary", textBoxESalary.Text}
                    };
                    var cnt = new FormUrlEncodedContent(employeeDictionary);

                    var res = await cl.PostAsync(connection, cnt);

                    var response = await res.Content.ReadAsStringAsync();
                    break;
                case "patient":



                    var patientDictionary = new Dictionary<string, string>
                    {
                        {"mode", "12"},
                        {"name", textBoxPName.Text},
                        {"surname", textBoxPSurname.Text},
                        {"dni", textBoxPDNI.Text},
                        {"email", textBoxPEmail.Text},
                        {"tel", textBoxPTel.Text},
                        {"address", textBoxPAddress.Text}
                    };
                    var cont = new FormUrlEncodedContent(patientDictionary);

                    var rsp = await cl.PostAsync(connection, cont);

                    var rspString = await rsp.Content.ReadAsStringAsync();
                    break;
            }

        }

        private async void buttonIncome_Click(object sender, EventArgs e)
        {
            connection = "http://172.17.41.191/PHP/m7.php";
            var incomeDictionary = new Dictionary<string, string>
                {{"mode", "13"}};
            var cont = new FormUrlEncodedContent(incomeDictionary);

            var rsp = await cl.PostAsync(connection, cont);

            var rspString = await rsp.Content.ReadAsStringAsync();
            string incomeData = rspString.ToString();


        }

        private void buttonSend_Click(object sender, EventArgs e)
        {
            Email email = new Email(textBoxTo.Text, textBoxCC.Text, textBoxCCO.Text, textBoxSubject.Text,
                richTextBoxMessage.Text);

            MailMessage mail = new MailMessage();


            mail.To.Add(email.to);
            mail.From = new MailAddress("mariohv007@gmail.com");
            mail.CC.Add(email.cc);
            //mail.Bcc.Add(email.cco);
            mail.Subject = email.subject;
            mail.Body = email.body;
            SmtpClient smtp = new SmtpClient();
            smtp.Host = "smtp.gmail.com";
            smtp.Port = 587;
            smtp.UseDefaultCredentials = false;
            smtp.Credentials = new System.Net.NetworkCredential("mariomonlau@gmail.com", "Mariohv007!");
            smtp.EnableSsl = true;


            smtp.Send(mail);
            MessageBox.Show("Done", "Email sent", MessageBoxButtons.OK, MessageBoxIcon.Information,
                MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
        }

        private void buttonEmailSpam_Click(object sender, EventArgs e)
        {
            string[] nameData = namePicker();


            connection = "http://172.17.41.191/PHP/m7.php?mod=email";

            string result = new WebClient().DownloadString(connection);
            MessageBox.Show(result);

            string[] emailData = result.Split(',');
            int i = 0;
            foreach (var email in emailData)
            {
                MailMessage mail = new MailMessage();

                richTextBoxMessage.Text = emailData[i] + "\n";

                //mail.To.Add(emailData[i]);
                mail.From = new MailAddress("mariomonlau@gmail.com");
                //mail.Bcc.Add(email.cco);
                mail.Subject = "Feliz Semana Santa";
                mail.Body = "Feliz Semana Santa " + nameData[i];
                SmtpClient smtp = new SmtpClient();
                smtp.Host = "smtp.gmail.com";
                smtp.Port = 587;
                smtp.UseDefaultCredentials = false;
                smtp.Credentials = new System.Net.NetworkCredential("mariomonlau@gmail.com", "Mariohv007!");
                smtp.EnableSsl = true;


                // smtp.Send(mail);
                i++;
            }


        }

        private String[] namePicker()
        {

            connection = "http://172.17.41.191/PHP/m7.php?mod=name";

            string result = new WebClient().DownloadString(connection);





            String[] nameData = result.Split(',');

            return nameData;




        }
    }
}




/*
 * string query = "SELECT * FROM user";
 * MySqlCommand commandDatabase = new MySqlCommand(query, databaseConnection);
 * MySqlDataReader reader;
 *
 * string query = "INSERT INTO user(`id`, `first_name`,
 * `last_name`, `address`) VALUES (NULL, '"+textBox1.Text+ "', '" + textBox2.Text + "', '" + textBox3.Text + "')";
 *
 *
 *
 *
 *
 *  
        comboBox2.SelectedItem = positionE;
   
   
  
 *
 *
 *
 */



