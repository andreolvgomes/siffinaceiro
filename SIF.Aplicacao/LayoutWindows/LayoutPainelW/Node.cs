using SIF.Dao;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace SIF.Aplicacao.LayoutConfiguracaoW
{
    public class ChangedIsCheckedEventArgs : EventArgs
    {
        public Uscontrolesecao Permisoes { get; set; }

        public ChangedIsCheckedEventArgs(Uscontrolesecao permissoes)
        {
            this.Permisoes = permissoes;
        }
    }

    public struct CheckBoxId
    {
        public static string checkBoxId;
    }

    public class Node : INotifyPropertyChanged
    {
        public Node()
        {
            this.id = Guid.NewGuid().ToString();
        }

        private ObservableCollection<Node> children = new ObservableCollection<Node>();
        private ObservableCollection<Node> parent = new ObservableCollection<Node>();

        private string text;
        private string id;
        private bool? isChecked = true;
        private bool isExpanded;

        public ObservableCollection<Node> Children
        {
            get { return this.children; }
        }

        public ObservableCollection<Node> Parent
        {
            get { return this.parent; }
        }

        public Uscontrolesecao _Permissoes { get; set; }

        public Uscontrolesecao Permissoes
        {
            get { return this._Permissoes; }
            set
            {
                if (Permissoes != value)
                {
                    this._Permissoes = value;
                }
            }
        }

        public bool? IsChecked
        {
            get { return this.isChecked; }
            set
            {
                this.isChecked = value;
                OnIsCheckedEventArgs(new ChangedIsCheckedEventArgs(_Permissoes));
                RaisePropertyChanged("IsChecked");
            }
        }

        public string Text
        {
            get { return this.text; }
            set
            {
                this.text = value;
                RaisePropertyChanged("Text");
            }
        }

        public bool IsExpanded
        {
            get { return isExpanded; }
            set
            {
                isExpanded = value;
                RaisePropertyChanged("IsExpanded");
            }
        }

        public string Id
        {
            get { return this.id; }
            set
            {
                this.id = value;
            }
        }

        public event EventHandler<ChangedIsCheckedEventArgs> IsCheckedEventArgs;

        private void OnIsCheckedEventArgs(ChangedIsCheckedEventArgs e)
        {
            if (IsCheckedEventArgs != null)
            {
                switch (text)
                {
                    case "Incluir":
                        _Permissoes.Usc_incluir = (bool)isChecked;
                        break;
                    case "Alterar":
                        _Permissoes.Usc_editar = (bool)isChecked;
                        break;
                    case "Excluir":
                        _Permissoes.Usc_excluir = (bool)isChecked;
                        break;
                    case "Disponível no menu":
                        _Permissoes.Usc_disponivel = (bool)isChecked;
                        break;
                }

                IsCheckedEventArgs(this, e);
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void RaisePropertyChanged(string propertyName)
        {
            if (this.PropertyChanged != null)
                this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            int countCheck = 0;

            if (propertyName == "IsChecked")
            {
                if (this.Id == CheckBoxId.checkBoxId && this.Parent.Count == 0 && this.Children.Count != 0)
                {
                    CheckedTreeParent(this.Children, this.IsChecked);
                }
                if (this.Id == CheckBoxId.checkBoxId && this.Parent.Count > 0 && this.Children.Count > 0)
                {
                    CheckedTreeChildMiddle(this.Parent, this.Children, this.IsChecked);
                }
                if (this.Id == CheckBoxId.checkBoxId && this.Parent.Count > 0 && this.Children.Count == 0)
                {
                    CheckedTreeChild(this.Parent, countCheck);
                }
            }
        }

        private void CheckedTreeChildMiddle(ObservableCollection<Node> itemsParent, ObservableCollection<Node> itemsChild, bool? isChecked)
        {
            int countCheck = 0;
            CheckedTreeParent(itemsChild, isChecked);
            CheckedTreeChild(itemsParent, countCheck);
        }

        private void CheckedTreeParent(ObservableCollection<Node> items, bool? isChecked)
        {
            foreach (Node item in items)
            {
                item.IsChecked = isChecked;
                if (item.Children.Count != 0) CheckedTreeParent(item.Children, isChecked);
            }
        }

        public void CheckedTreeChildLoaded()
        {
            CheckedTreeChild(this.Parent, 0);
        }

        private void CheckedTreeChild(ObservableCollection<Node> items, int countCheck)
        {
            bool isNull = false;
            foreach (Node paren in items)
            {
                foreach (Node child in paren.Children)
                {
                    if (child.IsChecked == true || child.IsChecked == null)
                    {
                        countCheck++;
                        if (child.IsChecked == null)
                            isNull = true;
                    }
                }
                if (countCheck != paren.Children.Count && countCheck != 0) paren.IsChecked = null;
                else if (countCheck == 0) paren.IsChecked = false;
                else if (countCheck == paren.Children.Count && isNull) paren.IsChecked = null;
                else if (countCheck == paren.Children.Count && !isNull) paren.IsChecked = true;
                if (paren.Parent.Count != 0) CheckedTreeChild(paren.Parent, 0);
            }
        }
    }
}
