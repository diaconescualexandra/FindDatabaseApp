using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevExpress.Utils.Menu;
using DevExpress.XtraEditors;
using System.Reflection;
using FindDatabaseApp;


namespace FindDatabaseApp
{
    public partial class Form1 : Form
    {
        Result result = new Result();
        DatabaseManagement obj = new DatabaseManagement(); 
        FileRW fileObj = new FileRW(); 
        List<FileData> filedtc = new List<FileData>();
        DataSet ds = new DataSet();
        public Form1()
        {
            InitializeComponent();
            LoadFromFile();

        }

        private void LoadFromFile()
        {
            filedtc = FileOperations.readFromFile(fileObj);
            if (filedtc != null && filedtc.Count() != 0)
            {
                foreach (var fileInfo in filedtc)
                {

                    comboBox2.Items.Add(fileInfo.serverName);
                    foreach (var key in fileInfo.UserPassDict.Keys)
                    {
                        comboBox3.Items.Add(key);
                    }

                }
            }
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            //se face conexiunea cu baza de date selectata
            obj.databaseName = comboBox1.Text;
            obj.valueLookedAfter = (string)textEdit2.EditValue;

            DatabaseManagementMethods.ConnectToSelectedDatabase(obj.serverName, obj.databaseName, obj.userName,obj.pass, obj.valueLookedAfter);
            result.DisplayResults(obj.valueLookedAfter);
            result.Show();
        }



        private void simpleButton2_Click(object sender, EventArgs e)
        {
            //se preiau toate bazele de date ale serverului
            obj.serverName = (string)comboBox2.Text;
            obj.userName = (string)comboBox3.Text;
            obj.pass = (string)textEdit1.EditValue;
            
            obj.dataBaseList = DatabaseManagementMethods.GetDatabaseList(obj.serverName, obj.databaseNameDefault,obj.userName, obj.pass);
            foreach (var st in obj.dataBaseList)
            {
                comboBox1.Items.Add(st);

            }
            
            //primul elem din lista = primul elem din dropdown
            comboBox1.Text = obj.dataBaseList.ElementAt(0);
        }
        


        private void checkButton1_CheckedChanged(object sender, EventArgs e)
        {
            
            if (textEdit1.Properties.PasswordChar == '*')
                textEdit1.Properties.PasswordChar = '\0';
            else
                textEdit1.Properties.PasswordChar = '*'; 
        }

        private void comboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            
            if (comboBox3.SelectedItem != null)
            {
                string selectedItem = (string)comboBox3.SelectedItem;
                foreach (var fileInfo in filedtc)
                {
                    if( fileInfo.UserPassDict.ContainsKey(selectedItem))
                    {
                        string cryptedPassword = fileInfo.UserPassDict[selectedItem];
                        string decrytedPassword = StringUtil.Decrypt(cryptedPassword);
                        textEdit1.Text= decrytedPassword;
                    }
                    else
                    {
                        textEdit1.Text = string.Empty;
                    }

                }
            }
        }
    }
}
