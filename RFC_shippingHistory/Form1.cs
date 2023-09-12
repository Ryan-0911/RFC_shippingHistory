using SAP.Middleware.Connector;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RFC_shippingHistory
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            RfcConfigParameters args = new RfcConfigParameters();
            args.Add(RfcConfigParameters.Name, "");
            args.Add(RfcConfigParameters.AppServerHost, "");
            args.Add(RfcConfigParameters.SystemNumber, "");
            args.Add(RfcConfigParameters.SystemID, "");
            args.Add(RfcConfigParameters.User, "");
            args.Add(RfcConfigParameters.Password, "");
            args.Add(RfcConfigParameters.Client, "");
            args.Add(RfcConfigParameters.Language, "ZF");

            RfcDestination dest = RfcDestinationManager.GetDestination(args);
            RfcRepository repo = dest.Repository;

            // 要連線的 RFC 名稱
            IRfcFunction func = repo.CreateFunction("IT_HEAD");
            func.Invoke(dest);

            // 要取得 SAP 的哪個表
            IRfcTable dataTable = func.GetTable("IT_HEAD");

            List<IT_HEAD> list_IT_HEAD = new List<IT_HEAD>();
            for(int i = 0; i < dataTable.Count; i++)
            {
                list_IT_HEAD.Add(new IT_HEAD()
                {
                    ID = dataTable[i].GetString("ID"),
                    Name = dataTable[i].GetString("Name"),
                });
                MessageBox.Show($"{dataTable[i].GetString("ID")} {dataTable[i].GetString("NAME")}");
            }
        }

        public class IT_HEAD
        {
            public string ID { get; set; }
            public string Name { get; set; }
        }
    }
}
