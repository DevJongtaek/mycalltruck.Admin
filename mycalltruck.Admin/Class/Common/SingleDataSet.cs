using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//using System.Threading.Tasks;
using mycalltruck.Admin.CMDataSetTableAdapters;
using System.Windows.Forms;
using mycalltruck.Admin.DataSets;

namespace mycalltruck.Admin.Class.Common
{
    class SingleDataSet 
    {
        private static SingleDataSet _Instance = new SingleDataSet();
        public static SingleDataSet Instance { get { return _Instance; } }
        void _Instance_Disposed(object sender, EventArgs e)
        {
            _Instance = null;
        }
        private CMDataSet _InnerDataSet = new CMDataSet();
        private BaseDataSet _BInnerDataSet = new BaseDataSet();
        private SingleDataSet()
        {
            try
            {
                AddressReferencesTableAdapter addressReferencesTableAdapter = new AddressReferencesTableAdapter();
                addressReferencesTableAdapter.Fill(_InnerDataSet.AddressReferences);
                StaticOptionsTableAdapter staticOptionsTableAdapter = new StaticOptionsTableAdapter();
                staticOptionsTableAdapter.Fill(_InnerDataSet.StaticOptions);
                FPISOptionsTableAdapter fPISOptionsTableAdapter = new FPISOptionsTableAdapter();
                fPISOptionsTableAdapter.Fill(_InnerDataSet.FPISOptions);

                DataSets.BaseDataSetTableAdapters.UserAuthorityTableAdapter userAuthorityTableAdapter = new DataSets.BaseDataSetTableAdapters.UserAuthorityTableAdapter();
                userAuthorityTableAdapter.Fill(_BInnerDataSet.UserAuthority);

            }
            catch
            {
                Application.Exit();
            }
        }
        public CMDataSet.AddressReferencesDataTable AddressReferences
        {
            get { return _InnerDataSet.AddressReferences; }
        }
        public CMDataSet.StaticOptionsDataTable StaticOptions
        {
            get { return _InnerDataSet.StaticOptions; }
        }
        public CMDataSet.FPISOptionsDataTable FPISOptions
        {
            get { return _InnerDataSet.FPISOptions; }
        }
        public BaseDataSet.UserAuthorityDataTable UserAuthority
        {
            get { return _BInnerDataSet.UserAuthority; }
        }
    }
}
