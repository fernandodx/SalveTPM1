using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Popups;
using System.Globalization; 

namespace SalveTPM1.ViewModel
{
    class PivotPageViewModel
    {
        public static string DB_PATH = string.Format(@"{0}\{1}", ApplicationData.Current.LocalFolder.Path, "SALVE_TPM_DB.sqlite");

        public ObservableCollection<Model.Mulher> listaMulheres { get; set; }
        public Model.Mulher mulherSelecionado { get; set; }
        public Model.Mulher mulherPrincipal
        {
            get
            {
                Model.Mulher escolhida = null;

                if (listaMulheres == null || listaMulheres.Count == 0)
                {
                    return null;
                }

                foreach (Model.Mulher mulher in listaMulheres)
                {
                    if (mulher.isPrincipal)
                    {
                        escolhida = mulher;
                    }
                }

                return escolhida;
            }
        }

        public String isMulherCadastrada
        {
            get
            {
                if (listaMulheres == null || listaMulheres.Count == 0)
                {
                    return "Collapsed";
                }
                return "Visible";
            }
        }


        public String caminhoImagemDicas
        {
            get
            {
                if (listaMulheres == null || listaMulheres.Count == 0)
                {
                    return "ms-appx:///Assets/fraseMulherNaoAdicionada.png";
                }
                return mulherPrincipal.caminhoImagemFraseTpm;
            }    
        }

        public ICommand abrirTelaCadastroAtualizaCommand { get; set; }
        public ICommand inserirOuAtualizarCommand { get; set; }
        public ICommand deletarCommand { get; set; }
        public ICommand EditarCommand { get; set; }
        public ICommand visualizarCommand { get; set; }


        public PivotPageViewModel()
        {
            using (var db = new SQLite.SQLiteConnection(DB_PATH))
            {
                db.CreateTable<Model.Mulher>();
            }

            listaMulheres = GetAllMulheres();

           abrirTelaCadastroAtualizaCommand = new ViewModel.DelegateCommand<Model.Mulher>(abriTelaCadastro);
           visualizarCommand = new ViewModel.DelegateCommand<Model.Mulher>(abrirTelaVisualizacao);
           deletarCommand = new ViewModel.DelegateCommand<Model.Mulher>(delete);
           EditarCommand = new ViewModel.DelegateCommand<Model.Mulher>(abriTelaCadastro);
        }


        public ObservableCollection<Model.Mulher> GetAllMulheres()
        {

            ObservableCollection<Model.Mulher> lista = new ObservableCollection<Model.Mulher>();
            using (var db = new SQLite.SQLiteConnection(DB_PATH))
            {
                lista = new ObservableCollection<Model.Mulher>(db.Table<Model.Mulher>());

                DateTime dataAtual = DateTime.Now;

                foreach (Model.Mulher mulher in lista)
                {
                    if (dataAtual >= mulher.dataTimeAproximadaMestruacao)
                    {
                        mulher.dataUltimaMestruacao = (DateTime)mulher.dataTimeAproximadaMestruacao;
                        db.InsertOrReplace(mulher);
                        criarNotificacao(mulher);
                    }
                }

            }

            return lista;
        }


        private void abriTelaCadastro(Model.Mulher mulher)
        {

            mulherSelecionado = mulher;
            this.inserirOuAtualizarCommand = new ViewModel.DelegateCommand<Model.Mulher>(insereOuAtualiza);

            Frame rootFrame = Window.Current.Content as Frame;
            rootFrame.Navigate(typeof(View.CadastroMulher), this);

        }

        private void abrirTelaVisualizacao(Model.Mulher mulher)
        {

            mulherSelecionado = mulher;

            Frame rootFrame = Window.Current.Content as Frame;
            rootFrame.Navigate(typeof(View.DetalheMulher), this);

        }

        private Boolean validaCamposObrigatorios(Model.Mulher mulher){
            
            StringBuilder camposObrigatorio = new StringBuilder();

            if (String.IsNullOrEmpty(mulher.nome))
            {
                camposObrigatorio.Append("Nome");
                camposObrigatorio.Append(", ");
            }

            if (mulher.dataUltimaMestruacao == null)
            {
                camposObrigatorio.Append("Data da última menstruação");
                camposObrigatorio.Append(", ");
            }

            if (String.IsNullOrEmpty(mulher.ligacao))
            {
                camposObrigatorio.Append("Ligação");
                camposObrigatorio.Append(", ");
            }

            if (camposObrigatorio.Length > 0)
            {
                String msg = camposObrigatorio.ToString().Substring(0, camposObrigatorio.Length-2) +" é de preenchimento obrigatório";

                alertPopUp(msg);
                return false;
            }
            return true;
        }

        private void insereOuAtualiza(Model.Mulher mulher) {


           Boolean ok = validaCamposObrigatorios(mulher);

           if (ok)
           {
               using (var db = new SQLite.SQLiteConnection(DB_PATH))
               {
                   int idMulher = db.InsertOrReplace(mulher);
                   if (idMulher > 0)
                   {
                       int index = this.listaMulheres.IndexOf(mulher);
                       if (index > -1)
                       {
                           this.listaMulheres[index] = mulher;
                       }
                       else
                       {
                           this.listaMulheres.Add(mulher);
                       }

                       criarNotificacao(mulher);

                   }
               }


               Frame rootFrame = Window.Current.Content as Frame;
               rootFrame.GoBack();
           }
            
        }

        private void criarNotificacao(Model.Mulher mulher)
        {
            ViewModel.NotificacaoUtil notificacao = new NotificacaoUtil();

            DateTime dataInicioTpm = DateTime.ParseExact(mulher.dataInicioTpm, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            DateTime dataProximoAcabar = DateTime.ParseExact(mulher.dataFimTpm, "dd/MM/yyyy", CultureInfo.InvariantCulture).AddDays(-3);
            DateTime dataFimTpm =  DateTime.ParseExact(mulher.dataFimTpm, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            DateTime dataAtual = DateTime.Now;

            if (dataInicioTpm.Ticks > dataAtual.Ticks)
            {
                notificacao.agendarNotificacao("Opa!, a TPM da " + mulher.nome + " Chegou!",
                mulher.caminhoImagemTpm, mulher.ID.ToString(), dataInicioTpm);
            }

            if (dataProximoAcabar.Ticks > dataAtual.Ticks)
            {
                notificacao.agendarNotificacao("Olha a TPM da " + mulher.nome + " Falta pouco para acabar, que tal fazer um agrado? Ela vai ficar feliz.",
                mulher.caminhoImagemTpm, mulher.ID.ToString(), dataProximoAcabar); 
            }

            if (dataFimTpm.Ticks > dataAtual.Ticks)
            {
                notificacao.agendarNotificacao("Ufa!, a TPM da " + mulher.nome + " Acabou!",
                mulher.caminhoImagemTpm, mulher.ID.ToString(), dataFimTpm);
            }
        }

        private void delete(Model.Mulher mulher)
        {
            using (var db = new SQLite.SQLiteConnection(DB_PATH))
            {
                if (db.Delete(mulher) > 0)
                {
                    listaMulheres.Remove(mulher);
                }
            }
        }

        public Model.Mulher consultarPorId(int id)
        {
            ObservableCollection<Model.Mulher> lista = GetAllMulheres();

            foreach(Model.Mulher m in lista){
                if(m.ID == id){
                    return m;
                }
            }
            return null;

        }

        private async void alertPopUp(String msg)
        {
            MessageDialog msgbox = new MessageDialog(msg);
            await msgbox.ShowAsync();
        }

       
    }
}
